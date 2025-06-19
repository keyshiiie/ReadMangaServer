using ReadMangaWS.DataAccess;
using ReadMangaWS.Models;
using System.Data;

namespace ReadMangaWS.Repository
{
    public interface IPublisherRepository
    {
        Dictionary<int, List<Publisher>> GetAllPublishersByAllManga();
        List<Publisher> GetAllPublisher();
    }
    public class PublisherRepository : IPublisherRepository
    {
        private readonly DBConnection _dbConnection;

        public PublisherRepository(IConfiguration configuration)
        {
            string? connectionString = configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException("Строка подключения не может быть пустой или равной null.", nameof(connectionString));
            }

            _dbConnection = new DBConnection(connectionString);
        }
        // получение списка издательсв для всей манги
        public Dictionary<int, List<Publisher>> GetAllPublishersByAllManga()
        {
            var publishersByManga = new Dictionary<int, List<Publisher>>();
            string query = @"
            SELECT mp.id_manga, p.id_publisher_manga, p.name_publisher
            FROM MangaPublishers mp
            JOIN Publisher p ON mp.id_publisher_manga = p.id_publisher_manga";
            try
            {
                DataTable dataTable = _dbConnection.ExecuteReader(query);
                foreach (DataRow row in dataTable.Rows)
                {
                    int mangaId = (int)row["id_manga"];
                    var publisher = new Publisher(
                        (int)row["id_publisher_manga"],
                        (string)row["name_publisher"]
                    );

                    if (!publishersByManga.ContainsKey(mangaId))
                        publishersByManga[mangaId] = new List<Publisher>();

                    publishersByManga[mangaId].Add(publisher);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка при получении списка издательств для манги:", ex);
            }
            return publishersByManga;
        }
        // получения списка издательств для сортировки
        public List<Publisher> GetAllPublisher()
        {
            var publishers = new List<Publisher>();
            string query = @"SELECT * FROM Publisher";
            try
            {
                DataTable dataTable = _dbConnection.ExecuteReader(query);
                foreach (DataRow row in dataTable.Rows)
                {
                    var publisher = new Publisher(
                    (int)row["id_publisher_manga"],
                    (string)row["name_publisher"]
                    );
                    publishers.Add(publisher);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка при получении списка издательств:", ex);
            }
            return publishers;
        }
    }
}
