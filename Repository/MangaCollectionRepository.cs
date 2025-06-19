﻿using ReadMangaWS.Models;
using Npgsql;
using ReadMangaWS.DataAccess;
using System.Data;

namespace ReadMangaWS.Repository
{
    public interface IMangaCollection
    {
        Dictionary<int, string> GetCollectionsByMangaForUser(int userId);
        List<MangaCollection> GetAllCollectionsByUser(int userId, User user);
        string? UpdateMangasCollection(int userId, int mangaId, int collectionId);
    }
    public class MangaCollectionRepository : IMangaCollection
    {
        private readonly DBConnection _dbConnection;
        public MangaCollectionRepository(IConfiguration configuration) 
        {
            string? connectionString = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException("Строка подключения не может быть пустой или равной null.", nameof(connectionString));
            }

            _dbConnection = new DBConnection(connectionString);
        }
        // получение списка коллекций манги для пользователя
        public Dictionary<int, string> GetCollectionsByMangaForUser(int userId)
        {
            var collectionByManga = new Dictionary<int, string>();
            string query = @"SELECT mc.id_manga, c.title
                     FROM MangaCollection mc
                     JOIN Collection c ON mc.id_collection = c.id_collection
                     WHERE mc.id_user = @userId";

            var parameters = new NpgsqlParameter[]
            {
                new NpgsqlParameter(nameof(userId), userId)
            };
            try
            {
                DataTable dataTable = _dbConnection.ExecuteReader(query, parameters);
                foreach (DataRow row in dataTable.Rows)
                {
                    int mangaId = (int)row["id_manga"];
                    string collectionTitle = (string)row["title"];
                    collectionByManga[mangaId] = collectionTitle;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка при получении списка коллекций для манги из базы данных", ex);
            }
            return collectionByManga;
        }

        // получение списка коллекций для пользователя (для комбо бокса)
        public List<MangaCollection> GetAllCollectionsByUser(int userId, User user)
        {
            var collections = new List<MangaCollection>();
            string query = @"SELECT id_collection, title FROM Collection WHERE id_user = @userId";
            var parameters = new[]
            {
                new NpgsqlParameter(nameof(userId), userId)
            };
            try
            {
                DataTable dataTable = _dbConnection.ExecuteReader(query, parameters);
                foreach (DataRow row in dataTable.Rows)
                {
                    var collection = new MangaCollection
                        (
                        (int)row["id_collection"],
                        (string)row["title"],
                        user
                        );
                    collections.Add(collection);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка при получении списка коллекций из базы данных", ex);
            }
            return collections;
        }

        // добавление или обновление коллекции для выбранной манги
        public string? UpdateMangasCollection(int userId, int mangaId, int collectionId)
        {
            string query = @"SELECT add_or_update_manga_in_collection(@userId, @mangaId, @collectionId)";
            var parameters = new[]
            {
                new NpgsqlParameter(nameof(userId), userId),
                new NpgsqlParameter(nameof(mangaId), mangaId),
                new NpgsqlParameter(nameof(collectionId), collectionId)
            };
            try
            {
                var result = _dbConnection.ExecuteScalar(query, parameters);
                return result?.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка при обновлении коллекции манги", ex);
            }
        }
    }
}
