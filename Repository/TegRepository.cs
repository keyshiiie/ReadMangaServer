using ReadMangaWS.DataAccess;
using ReadMangaWS.Models;
using System.Data;

namespace ReadMangaWS.Repository
{
    public interface ITegRepository
    {
        List<Teg> GetAllTegs();
        Dictionary<int, List<Teg>> GetAllTegsByAllManga();
    }
    public class TegRepository : ITegRepository
    {
        private readonly DBConnection _dbConnection;

        public TegRepository(IConfiguration configuration)
        {
            string? connectionString = configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException("Строка подключения не может быть пустой или равной null.", nameof(connectionString));
            }

            _dbConnection = new DBConnection(connectionString);
        }
        // получение списка тегов
        public List<Teg> GetAllTegs()
        {
            var tegs = new List<Teg>();
            string query = @"SELECT * FROM Teg";
            try
            {
                DataTable dataTable = _dbConnection.ExecuteReader(query);
                foreach (DataRow row in dataTable.Rows) 
                {
                    var teg = new Teg(
                            (int)row["id_teg_manga"],
                            (string)row["name_teg"]
                            );
                    tegs.Add(teg);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка при получении списка тегов:", ex);
            }
            return tegs;
        }
        // получение списка тегов для манги всей
        public Dictionary<int, List<Teg>> GetAllTegsByAllManga()
        {
            var tegsByManga = new Dictionary<int, List<Teg>>();
            string query = @"
            SELECT tm.id_manga, t.id_teg_manga, t.name_teg
            FROM MangaTegs tm
            JOIN Teg t ON tm.id_teg_manga = t.id_teg_manga";
            try
            {
                DataTable dataTable = _dbConnection.ExecuteReader(query);
                foreach (DataRow row in dataTable.Rows)
                {
                    int mangaId = (int)row["id_manga"];
                    var teg = new Teg(
                        (int)row["id_teg_manga"],
                        (string)row["name_teg"]
                    );
                    if (!tegsByManga.ContainsKey(mangaId))
                        tegsByManga[mangaId] = new List<Teg>();
                    tegsByManga[mangaId].Add(teg);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка при получении списка тегов для манги:", ex);
            }
            return tegsByManga;
        }
    }
}
