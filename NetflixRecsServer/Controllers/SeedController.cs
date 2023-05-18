using CsvHelper.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetflixModel;
using NetflixRecsServer.Data;
using System.Globalization;
using CsvReader = CsvHelper.CsvReader;

namespace NetflixRecsServer.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class SeedController : ControllerBase {
        private readonly UserManager<NetflixRecsUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly NetflixRecsContext _context;
        private string _pathName;

        public SeedController(UserManager<NetflixRecsUser> userManager, RoleManager<IdentityRole> roleManager,
            IConfiguration configuration, NetflixRecsContext context, IHostEnvironment environment) {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _context = context;
            _pathName = Path.Combine(environment.ContentRootPath, "Data/BestShowsNetflix.csv");
        }

        // POST: api/Seed
        [HttpPost("Genres")]
        public async Task<IActionResult> ImportGenres() {
            Dictionary<string, Genre> genresByName = _context.Genres
                .AsNoTracking().ToDictionary(x => x.Genre1, StringComparer.OrdinalIgnoreCase);

            CsvConfiguration config = new(CultureInfo.InvariantCulture) {
                HasHeaderRecord = true,
                HeaderValidated = null
            };

            using StreamReader reader = new(_pathName);
            using CsvReader csv = new(reader, config);

            IEnumerable<BestShowsNetflixCsv>? records = csv.GetRecords<BestShowsNetflixCsv>();
            foreach (BestShowsNetflixCsv record in records) {
                if (genresByName.ContainsKey(record.genre)) {
                    continue;
                }

                Genre genre = new() {
                    Genre1 = record.genre,
                };
                await _context.Genres.AddAsync(genre);
                genresByName.Add(record.genre, genre);
            }

            await _context.SaveChangesAsync();

            return new JsonResult(genresByName.Count);
        }

        [HttpPost("Users")]
        public async Task<IActionResult> ImportUsers() {
            const string roleUser = "RegisteredUser";
            const string roleAdmin = "Administrator";

            if (await _roleManager.FindByNameAsync(roleUser) is null) {
                await _roleManager.CreateAsync(new IdentityRole(roleUser));
            }
            if (await _roleManager.FindByNameAsync(roleAdmin) is null) {
                await _roleManager.CreateAsync(new IdentityRole(roleAdmin));
            }

            List<NetflixRecsUser> addedUserList = new();
            (string name, string email) = ("admin", "admin@email.com");

            if (await _userManager.FindByNameAsync(name) is null) {
                NetflixRecsUser userAdmin = new() {
                    UserName = name,
                    Email = email,
                    SecurityStamp = Guid.NewGuid().ToString()
                };
                await _userManager.CreateAsync(userAdmin, _configuration["DefaultPasswords:Administrator"]
                    ?? throw new InvalidOperationException());
                await _userManager.AddToRolesAsync(userAdmin, new[] { roleUser, roleAdmin });
                userAdmin.EmailConfirmed = true;
                userAdmin.LockoutEnabled = false;
                addedUserList.Add(userAdmin);
            }

            (string name, string email) registered = ("user", "user@email.com");

            if (await _userManager.FindByNameAsync(registered.name) is null) {
                NetflixRecsUser user = new() {
                    UserName = registered.name,
                    Email = registered.email,
                    SecurityStamp = Guid.NewGuid().ToString()
                };
                await _userManager.CreateAsync(user, _configuration["DefaultPasswords:RegisteredUser"]
                    ?? throw new InvalidOperationException());
                await _userManager.AddToRoleAsync(user, roleUser);
                user.EmailConfirmed = true;
                user.LockoutEnabled = false;
                addedUserList.Add(user);
            }

            if (addedUserList.Count > 0) {
                await _context.SaveChangesAsync();
            }

            return new JsonResult(new {
                addedUserList.Count,
                Users = addedUserList
            });

        }
    }
}
