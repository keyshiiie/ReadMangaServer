using Microsoft.AspNetCore.Mvc;
using ReadMangaWS.Models;
using ReadMangaWS.Repository;

namespace ReadMangaWS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TegController : ControllerBase
    {
        private readonly ITegRepository _tegRepository;
        public TegController(ITegRepository tegRepository)
        {
            _tegRepository = tegRepository;
        }

        [HttpGet("all")]
        public ActionResult<List<Genre>> GetAllGenres()
        {
            var tegs = _tegRepository.GetAllTegs();
            return Ok(tegs);
        }

        [HttpGet("by-manga")]
        public ActionResult<Dictionary<int, List<Genre>>> GetGenresByManga()
        {
            var tegsByManga = _tegRepository.GetAllTegsByAllManga();
            return Ok(tegsByManga);
        }
    }
}
    
