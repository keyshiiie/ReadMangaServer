using Microsoft.AspNetCore.Mvc;
using ReadMangaWS.Models;
using ReadMangaWS.Repository;

namespace ReadMangaWS.Controllers
{ 
    [ApiController]
    [Route("api/[controller]")]
    public class GenreController : ControllerBase
    {
        private readonly IGenreRepository _genreRepository;
        public GenreController(IGenreRepository genreRepository)
        {
            _genreRepository = genreRepository;
        }

        [HttpGet("all")]
        public ActionResult<List<Genre>> GetAllGenres()
        {
            var genres = _genreRepository.GetAllGenres();
            return Ok(genres);
        }

        [HttpGet("by-manga")]
        public ActionResult<Dictionary<int, List<Genre>>> GetGenresByManga()
        {
            var genresByManga = _genreRepository.GetAllGenresByAllManga();
            return Ok(genresByManga);
        }
    }

}
