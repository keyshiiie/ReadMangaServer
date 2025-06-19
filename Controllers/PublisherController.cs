using Microsoft.AspNetCore.Mvc;
using ReadMangaWS.Models;
using ReadMangaWS.Repository;
using System.Collections.Generic;

namespace ReadMangaWS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PublisherController : ControllerBase
    {
        private readonly IPublisherRepository _publisherRepository;

        public PublisherController(IPublisherRepository publisherRepository)
        {
            _publisherRepository = publisherRepository;
        }

        // GET api/publisher/all
        [HttpGet("all")]
        public ActionResult<List<Publisher>> GetAllPublisher()
        {
            try
            {
                var publishers = _publisherRepository.GetAllPublisher();
                return Ok(publishers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка сервера: {ex.Message}");
            }
        }

        // GET api/publisher/by-manga
        [HttpGet("by-manga")]
        public ActionResult<Dictionary<int, List<Publisher>>> GetAllPublishersByAllManga()
        {
            try
            {
                var dict = _publisherRepository.GetAllPublishersByAllManga();
                return Ok(dict);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка сервера: {ex.Message}");
            }
        }
    }
}
