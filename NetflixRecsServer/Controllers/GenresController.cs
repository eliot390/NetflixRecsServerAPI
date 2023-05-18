using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetflixModel;
using NetflixRecsServer.Dtos;

namespace NetflixRecsServer.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase {
        private readonly NetflixRecsContext _context;

        public GenresController(NetflixRecsContext context) {
            _context = context;
        }

        // GET: api/Genres
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Genre>>> GetGenres(){
          if (_context.Genres == null){
              return NotFound();
          }
            return await _context.Genres.ToListAsync();
        }

        // GET: api/Genres/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Genre>> GetGenre(int id) {
          Genre? genre = await _context.Genres.FindAsync(id);
            return genre is null ? NotFound() : Ok(genre);
        }

        [HttpGet("Shows/{id}")]
        public async Task<ActionResult<IEnumerable<GenreShows>>> GetGenreShows(int id) {
            var genreShows = await _context.Shows
                .Join(_context.Genres, show => show.GenreID, genre => genre.Id, (show, genre) => new { show, genre })
                .Where(joined => joined.genre.Id == id)
                .Select(joined => new GenreShows {
                    Id = joined.show.Id,
                    Title = joined.show.Title,
                    Score = joined.show.Score,
                    Votes = joined.show.Votes,
                    Genre1 = joined.genre.Genre1
                })
                .ToListAsync();

            if (genreShows == null || genreShows.Count == 0) {
                return NotFound();
            }

            return genreShows;
        }

        // PUT: api/Genres/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGenre(int id, Genre genre) {
            if (id != genre.Id) {
                return BadRequest();
            }

            _context.Entry(genre).State = EntityState.Modified;

            try {
                await _context.SaveChangesAsync();
            } catch (DbUpdateConcurrencyException) {
                if (!GenreExists(id)) {
                    return NotFound();
                } else {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Genres
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Genre>> PostGenre(Genre genre) {
          if (_context.Genres == null) {
              return Problem("Entity set 'NetflixRecsContext.Genres'  is null.");
          }
            _context.Genres.Add(genre);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGenre", new { id = genre.Id }, genre);
        }

        // DELETE: api/Genres/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGenre(int id) {
            if (_context.Genres == null) {
                return NotFound();
            }
            var genre = await _context.Genres.FindAsync(id);
            if (genre == null) {
                return NotFound();
            }

            _context.Genres.Remove(genre);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GenreExists(int id) {
            return (_context.Genres?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
