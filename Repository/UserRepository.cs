using ReadMangaWS.Models;
using Npgsql;
using ReadMangaWS.DataAccess;
using System.Data;

namespace ReadMangaWS.Repository
{
    public interface IUserRepository
    {
        List<User> AuthorizationUser(string username, string passwordHash);
    }
    public class UserRepository : IUserRepository
    {
        private readonly DBConnection _dbConnection;

        public UserRepository(IConfiguration configuration)
        {
            string? connectionString = configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException("Строка подключения не может быть пустой или равной null.", nameof(connectionString));
            }

            _dbConnection = new DBConnection(connectionString);
        }

        // авторизация пользователя (получение данных пользователя по никнейму и паролю)
        public List<User> AuthorizationUser(string username, string passwordHash)
        {
            var users = new List<User>();
            string query = @"SELECT * FROM Users WHERE username = @username AND password_hash = @passwordHash";
            var parameters = new[]
            {
                new NpgsqlParameter(nameof(username), username),
                new NpgsqlParameter(nameof(passwordHash), passwordHash)
            };
            try
            {
                DataTable dataTable = _dbConnection.ExecuteReader(query, parameters);
                foreach (DataRow row in dataTable.Rows)
                {
                    var user = new User(
                        (int)row["id_user"],
                        (string)row["username"],
                        (string)row["password_hash"],
                        (string)row["email"],
                        (DateTime)row["created_at"]
                    );
                    users.Add(user);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка при авторизации:", ex);
            }
            return users;
        }
    }
}
