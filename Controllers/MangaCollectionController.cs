using Microsoft.AspNetCore.Mvc;
using ReadMangaWS.Models;
using ReadMangaWS.Repository;

namespace ReadMangaWS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MangaCollectionController : ControllerBase
    {
        private readonly IMangaCollection _mangaCollectionRepository;

        public MangaCollectionController(IMangaCollection mangaCollectionRepository)
        {
            _mangaCollectionRepository = mangaCollectionRepository;
        }

        // GET /MangaCollection/collections-by-manga
        // Получение коллекций манги для текущего пользователя из сессии
        [HttpGet("collections-by-manga")]
        public ActionResult<Dictionary<int, string>> GetCollectionsByMangaForUser()
        {
            var currentUser = UserSession.Instance.CurrentUser;
            if (currentUser == null)
                return Unauthorized("Пользователь не авторизован");

            try
            {
                var result = _mangaCollectionRepository.GetCollectionsByMangaForUser(currentUser.Id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка сервера: {ex.Message}");
            }
        }

        // GET /MangaCollection/all-collections
        // Получение всех коллекций для текущего пользователя
        [HttpGet("all-collections")]
        public ActionResult<List<MangaCollection>> GetAllCollectionsByUser()
        {
            var currentUser = UserSession.Instance.CurrentUser;
            if (currentUser == null)
                return Unauthorized("Пользователь не авторизован");

            try
            {
                // В репозитории ожидается объект User, передаём текущего пользователя
                var collections = _mangaCollectionRepository.GetAllCollectionsByUser(currentUser.Id, currentUser);
                return Ok(collections);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка сервера: {ex.Message}");
            }
        }

        // POST /MangaCollection/update
        // Обновить или добавить мангу в коллекцию для текущего пользователя
        [HttpPost("update")]
        public ActionResult<string> UpdateMangasCollection([FromBody] UpdateCollectionRequest request)
        {
            if (request == null)
                return BadRequest("Некорректные данные запроса");

            var currentUser = UserSession.Instance.CurrentUser;
            if (currentUser == null)
                return Unauthorized("Пользователь не авторизован");

            if (request.MangaId <= 0 || request.CollectionId <= 0)
                return BadRequest("Некорректные идентификаторы манги или коллекции");

            try
            {
                var result = _mangaCollectionRepository.UpdateMangasCollection(currentUser.Id, request.MangaId, request.CollectionId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка сервера: {ex.Message}");
            }
        }

        // Модель запроса для обновления коллекции
        public class UpdateCollectionRequest
        {
            public int MangaId { get; set; }
            public int CollectionId { get; set; }
        }
    }
}
