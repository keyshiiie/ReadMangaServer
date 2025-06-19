using Microsoft.AspNetCore.Mvc;
using ReadMangaWS.Models;
using ReadMangaWS.Repository;

namespace ReadMangaWS.Controllers
{ 
    [ApiController]
    [Route("api/[controller]")]
    public class MangaController : ControllerBase
    {
        // обрабатывает HTTP-запросы и использует репозиторий для получения данных

        private readonly IMangaRepository _mangaRepository;

        // Конструктор принимает интерфейс IMangaRepository, который определяет методы для работы с данными манги
        public MangaController(IMangaRepository mangaRepository)
        {
            _mangaRepository = mangaRepository;
        }

        // Метод Get возвращает список манги в формате JSON

        [HttpGet]
        public ActionResult<List<Manga>> Get()
        {
            var mangas = _mangaRepository.GetAllManga();
            return Ok(mangas);
        }
    }
}
    