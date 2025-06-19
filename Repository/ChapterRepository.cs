using Npgsql;
using ReadMangaWS.DataAccess;
using ReadMangaWS.Models;
using System.Data;

namespace ReadMangaWS.Repository
{
    public interface IChapterRepository
    {
        List<Chapter> GetAllChapter(int idManga);
    }
    public class ChapterRepository : IChapterRepository
    {
        private readonly DBConnection _dbConnection;

        public ChapterRepository(IConfiguration configuration)
        {
            string? connectionString = configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException("Строка подключения не может быть пустой или равной null.", nameof(connectionString));
            }

            _dbConnection = new DBConnection(connectionString);
        }

        // получение всех глав для манги с сортировкой по номеру главы
        public List<Chapter> GetAllChapter(int idManga)
        {
            var chapters = new List<Chapter>();
            string query = @"SELECT ch.id_chapter, ch.chapter_title, ch.date_published, ch.number_chapter
                     FROM Chapter ch WHERE ch.id_manga = @IdManga  ORDER BY ch.number_chapter";
            // Создаем параметр для запроса
            var parameters = new[]
            {
                new NpgsqlParameter("@IdManga", idManga)
            };
            try
            {
                DataTable dataTable = _dbConnection.ExecuteReader(query, parameters);
                foreach (DataRow row in dataTable.Rows)
                {
                    var chapter = new Chapter(
                        (int)row["id_chapter"],
                        null,
                        (string)row["chapter_title"],
                        (DateTime)row["date_published"],
                        (int)row["number_chapter"]
                    );
                    chapters.Add(chapter);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка при получении списка глав:", ex);
            }
            return chapters;
        }
    }
}
