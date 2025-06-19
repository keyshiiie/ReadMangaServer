using ReadMangaWS.DataAccess;
using ReadMangaWS.Models;
using System.Data;

namespace ReadMangaWS.Repository
{
    public interface IGenreRepository
    {
        List<Genre> GetAllGenres();
        Dictionary<int, List<Genre>> GetAllGenresByAllManga();
    }
    public class GenreRepository : IGenreRepository
    {
        private readonly DBConnection _dbConnection;

        public GenreRepository(IConfiguration configuration)
        {
            string? connectionString = configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException("Строка подключения не может быть пустой или равной null.", nameof(connectionString));
            }

            _dbConnection = new DBConnection(connectionString);
        }


        // получение списка жанров
        public List<Genre> GetAllGenres()
        {
            var genres = new List<Genre>();
            string query = @"SELECT * FROM Genre";
            try
            {
                DataTable dataTable = _dbConnection.ExecuteReader(query);
                foreach(DataRow row in dataTable.Rows)
                {
                    var genre = new Genre(
                    (int)row["id_genre_manga"],
                    (string)row["name_genre"]
                    );
                    genres.Add(genre);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка при получении списка жанров:", ex);
            }
            return genres;
        }

        // получение списка жанров для манги
        public Dictionary<int, List<Genre>> GetAllGenresByAllManga()
        {
            var genresByManga = new Dictionary<int, List<Genre>>();
            string query = @"
            SELECT gm.id_manga, g.id_genre_manga, g.name_genre
            FROM MangaGenres gm
            JOIN Genre g ON gm.id_genre_manga = g.id_genre_manga";
            try
            {
                DataTable dataTable = _dbConnection.ExecuteReader(query);
                foreach (DataRow row in dataTable.Rows)
                {
                    int mangaId = (int)row["id_manga"];
                    var genre = new Genre(
                        (int)row["id_genre_manga"],
                        (string)row["name_genre"]
                    );
                    if (!genresByManga.ContainsKey(mangaId))
                        genresByManga[mangaId] = new List<Genre>();
                    genresByManga[mangaId].Add(genre);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка при получения списка жанров для манги:", ex);
            }
            return genresByManga;
        }
    }
}
