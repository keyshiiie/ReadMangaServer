using Microsoft.AspNetCore.Mvc;
using ReadMangaWS.Repository;

namespace ReadMangaWS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MangaScoreController : ControllerBase
    {
        private readonly IMangaScoreRepository _scoreRepository;

        public MangaScoreController(IMangaScoreRepository scoreRepository)
        {
            _scoreRepository = scoreRepository;
        }

        // GET api/mangascore/averages
        [HttpGet("averages")]
        public ActionResult<Dictionary<int, decimal>> GetAllAverageScores()
        {
            try
            {
                var scores = _scoreRepository.GetAllAverageScores();
                return Ok(scores);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка сервера: {ex.Message}");
            }
        }

        public class UpdateScoreRequest
        {
            public int IdUser { get; set; }
            public int IdManga { get; set; }
            public decimal Score { get; set; }
        }

        // POST api/mangascore/update
        [HttpPost("update")]
        public ActionResult<string> UpdateScore([FromBody] UpdateScoreRequest request)
        {
            if (request == null || request.Score < 0 || request.Score > 10)
                return BadRequest("Некорректные данные.");

            try
            {
                string? result = _scoreRepository.UpdateScore(request.IdUser, request.IdManga, request.Score);
                if (result == null)
                    return BadRequest("Не удалось обновить оценку.");
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка сервера: {ex.Message}");
            }
        }
    }
}
