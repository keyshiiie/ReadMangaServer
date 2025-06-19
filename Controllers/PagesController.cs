using Microsoft.AspNetCore.Mvc;
using ReadMangaWS.Models;
using ReadMangaWS.Repository;

namespace ReadMangaWS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PagesController : ControllerBase
    {
        private readonly IPagesRepository _pagesRepository;

        public PagesController(IPagesRepository pagesRepository)
        {
            _pagesRepository = pagesRepository;
        }

        // GET api/pages/chapter/10
        [HttpGet("chapter/{chapterId}")]
        public ActionResult<List<MangaPage>> GetAllPagesByChapter(int chapterId)
        {
            try
            {
                var pages = _pagesRepository.GetAllPagesByChapter(chapterId);
                if (pages == null || pages.Count == 0)
                    return NotFound($"Страниц для главы с id={chapterId} не найдено.");
                return Ok(pages);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка сервера: {ex.Message}");
            }
        }
    }
}
