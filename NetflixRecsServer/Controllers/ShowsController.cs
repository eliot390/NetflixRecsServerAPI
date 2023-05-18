using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetflixModel;
using NetflixRecsServer.Dtos;

namespace NetflixRecsServer.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class ShowsController : ControllerBase {
        private readonly NetflixRecsContext _context;

        public ShowsController(NetflixRecsContext context) {
            _context = context;
        }

        // GET: api/Shows
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Show>>> GetShows() {
            if (_context.Shows == null) {
                return NotFound();
            }
            return await _context.Shows.ToListAsync();
        }

        // GET: api/Shows/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Show>> GetShow(int id) {
            Show? show = await _context.Shows.FindAsync(id);
            return show is null ? NotFound() : Ok(show);
        }

        [HttpGet("Shows/{id}")]
                public async Task<ActionResult<Shows>> GetClassShows(int id) {
                    Shows? classShows = await _context.Shows
                        .Where(c => c.Id == id)
                        .Select(c => new Shows {
                            Id = c.Id,
                            Title = c.Title,
                            Score = c.Score,
                            Votes = c.Votes,
                            GenreID = c.GenreID
                        }).SingleOrDefaultAsync();
                    if (classShows == null) {
                        return NotFound();
                    }
                    return classShows;
                }


        // PUT: api/Shows/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutShow(int id, Show show) {
            if (id != show.Id) {
                return BadRequest();
            }

            _context.Entry(show).State = EntityState.Modified;

            try {
                await _context.SaveChangesAsync();
            } catch (DbUpdateConcurrencyException) {
                if (!ShowExists(id)) {
                    return NotFound();
                } else {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Shows
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Show>> PostShow(Show show) {
            if (_context.Shows == null) {
                return Problem("Entity set 'NetflixRecsContext.Shows'  is null.");
            }
            _context.Shows.Add(show);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetShow", new { id = show.Id }, show);
        }

        // DELETE: api/Shows/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShow(int id) {
            if (_context.Shows == null) {
                return NotFound();
            }
            var show = await _context.Shows.FindAsync(id);
            if (show == null) {
                return NotFound();
            }

            _context.Shows.Remove(show);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ShowExists(int id) {
            return (_context.Shows?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}