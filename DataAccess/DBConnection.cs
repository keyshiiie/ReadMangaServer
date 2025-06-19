using Npgsql;
using System.Data;
namespace ReadMangaWS.DataAccess
{
    public class DBConnection
    {
        private readonly string _connectionString;
        public DBConnection(string connectionString)
        {
            _connectionString = connectionString;
        }
        // Вспомогательный метод для добавления параметров к команде во избежание дублирования кода
        private void AddParameters(NpgsqlCommand command, NpgsqlParameter[] parameters)
        {
            if (parameters != null && parameters.Length > 0)
            {
                command.Parameters.AddRange(parameters);
            }
        }
        // Выполняет запрос и возвращает данные в виде DataTable
        public DataTable ExecuteReader(string query, params NpgsqlParameter[] parameters)
        {
            // Создаем соединение с базой данных
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                // Создаем команду с запросом и соединением
                using (var command = new NpgsqlCommand(query, connection))
                {
                    // Добавляем параметры к команде
                    AddParameters(command, parameters);
                    // Заполняем DataTable
                    var dataTable = new DataTable();
                    using (var reader = command.ExecuteReader())
                    {
                        dataTable.Load(reader);
                    }
                    return dataTable; // Возвращаем заполненный DataTable
                }
            }
        }
        // Выполняет запрос и возвращает одно значение
        public object? ExecuteScalar(string query, params NpgsqlParameter[] parameters)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(query, connection))
                {
                    // Добавляем параметры к команде с помощью вспомогательного метода
                    AddParameters(command, parameters);
                    return command.ExecuteScalar();
                }    
            }
        }
        // Выполняет команды, которые не возвращают данных (например, INSERT, UPDATE, DELETE)
        public int ExecuteNonQuery(string query, params NpgsqlParameter[] parameters)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(query, connection))
                {
                    AddParameters(command, parameters);
                    return command.ExecuteNonQuery();
                }
            }
        }

    }
}
