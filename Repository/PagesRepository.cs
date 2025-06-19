using Npgsql;
using ReadMangaWS.DataAccess;
using ReadMangaWS.Models;
using System.Data;

namespace ReadMangaWS.Repository
{
    public interface IPagesRepository
    {
        List<MangaPage> GetAllPagesByChapter(int chapterId);
    }
    public class PagesRepository : IPagesRepository
    {
        private readonly DBConnection _dbConnection;

        public PagesRepository(IConfiguration configuration)
        {
            string? connectionString = configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException("Строка подключения не может быть пустой или равной null.", nameof(connectionString));
            }

            _dbConnection = new DBConnection(connectionString);
        }

        // Получение списка страниц для главы
        public List<MangaPage> GetAllPagesByChapter(int chapterId)
        {
            var pages = new List<MangaPage>();
            string query = @"SELECT p.id_page, p.id_chapter, p.page_number, p.page_content_url
                     FROM Page p 
                     WHERE p.id_chapter = @chapterId
                     ORDER BY p.page_number"; // Добавлена сортировка по page_number
            // Создаем параметр для запроса
            var parameters = new[]
            {
                new NpgsqlParameter(nameof(chapterId), chapterId)
            };
            // Используем ExecuteReader с параметрами
            try
            {
                DataTable dataTable = _dbConnection.ExecuteReader(query, parameters);
                foreach (DataRow row in dataTable.Rows)
                {
                    var page = new MangaPage(
                        (int)row["id_page"],
                        null,
                        (int)row["page_number"],
                        (string)row["page_content_url"]
                    );
                    pages.Add(page);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка при получении списка страниц для главы:", ex);
            }
            return pages;
        }
    }
}
