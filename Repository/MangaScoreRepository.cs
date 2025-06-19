using Npgsql;
using ReadMangaWS.DataAccess;
using ReadMangaWS.Models;
using System.Data;

namespace ReadMangaWS.Repository
{
    public interface IMangaScoreRepository
    {
        Dictionary<int, decimal> GetAllAverageScores();
        string? UpdateScore(int idUser, int idManga, decimal score);
    }
    public class MangaScoreRepository : IMangaScoreRepository
    {
        private readonly DBConnection _dbConnection;

        public MangaScoreRepository(IConfiguration configuration)
        {
            string? connectionString = configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException("Строка подключения не может быть пустой или равной null.", nameof(connectionString));
            }

            _dbConnection = new DBConnection(connectionString);
        }

        // получение средней оценки для всей манги
        public Dictionary<int, decimal> GetAllAverageScores()
        {
            var scoresByManga = new Dictionary<int, decimal>();
            string query = @"
            SELECT id_manga, AVG(score) as average_score
            FROM Manga_scores
            GROUP BY id_manga";
            try
            {
                DataTable dataTable = _dbConnection.ExecuteReader(query);
                foreach (DataRow row in dataTable.Rows)
                {
                    int mangaId = (int)row["id_manga"];
                    decimal averageScore = row["average_score"] == DBNull.Value ? 0 : Convert.ToDecimal(row["average_score"]);
                    scoresByManga[mangaId] = averageScore;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка при получении средней оценки:", ex);
            }
            return scoresByManga;
        }

        // добавление или обновление оценки для выбранной манги
        public string? UpdateScore(int idUser, int idManga, decimal score)
        {
            string query = @"SELECT add_manga_score(@IdUser, @IdManga, @Score)";
            var parameters = new[]
            {
                new NpgsqlParameter("@IdUser", idUser),
                new NpgsqlParameter("@IdManga", idManga),
                new NpgsqlParameter("@Score", score)
            };
            try
            {
                var result = _dbConnection.ExecuteScalar(query, parameters);
                return result?.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка:", ex);
            }
        }
    }
}

