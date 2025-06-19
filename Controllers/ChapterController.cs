using Microsoft.AspNetCore.Mvc;
using ReadMangaWS.Models;
using ReadMangaWS.Repository;

namespace ReadMangaWS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChapterController : ControllerBase
    {
        private readonly IChapterRepository _chapterRepository;

        public ChapterController(IChapterRepository chapterRepository)
        {
            _chapterRepository = chapterRepository;
        }

        // GET api/chapter/manga/5
        [HttpGet("manga/{idManga}")]
        public ActionResult<List<Chapter>> GetAllChapters(int idManga)
        {
            try
            {
                var chapters = _chapterRepository.GetAllChapter(idManga);
                if (chapters == null || chapters.Count == 0)
                    return NotFound($"Глав для манги с id={idManga} не найдено.");
                return Ok(chapters);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка сервера: {ex.Message}");
            }
        }
    }
}
