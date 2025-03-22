using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Ideageek.Subscribly.Core.Repositories
{
    public interface IBaseRepository<T> where T : class
    {
        Task<T?> GetById(Guid id);
        Task<IEnumerable<T>> GetAll();
        Task<T?> GetByConditionsAsync(object conditions);
        Task<IEnumerable<T>> GetAllByConditionsAsync(object conditions);
        Task<int> Add(T entity);
        Task<int> Update(T entity);
        Task<int> Delete(Guid id);
        Task<T> ExecuteSingleStoredProcedure(string procedureName, object parameters = null);
        Task<IEnumerable<T>> ExecuteListStoredProcedure(string procedureName, object parameters = null);
        Task<IEnumerable<T>> ExecuteCustomQuery(string query, object parameters = null);
    }

    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly string _connectionString;

        public BaseRepository(string connectionString)
        {
            _connectionString = connectionString;
            if (Connection.State != ConnectionState.Open)
                Connection.Open();
        }

        private IDbConnection Connection => new SqlConnection(_connectionString);

        public async Task<T?> GetById(Guid id)
        {
            using (var db = Connection)
            {
                return (await db.QueryAsync<T>($"SELECT * FROM {typeof(T).Name} WHERE Id = @Id", new { Id = id })).FirstOrDefault();
            }
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            using (var db = Connection)
            {
                return await db.QueryAsync<T>("SELECT * FROM " + typeof(T).Name);
            }
        }

        public async Task<T?> GetByConditionsAsync(object conditions)
        {
            using (var db = Connection)
            {
                var query = $"SELECT * FROM {typeof(T).Name} WHERE 1=1";
                var parameters = new DynamicParameters();

                foreach (var property in conditions.GetType().GetProperties())
                {
                    var value = property.GetValue(conditions);
                    if (value != null)
                    {
                        query += $" AND {property.Name} = @{property.Name}";
                        parameters.Add($"@{property.Name}", value);
                    }
                }
                return (await db.QueryAsync<T>(query, parameters)).FirstOrDefault();
            }
        }

        public async Task<IEnumerable<T>> GetAllByConditionsAsync(object conditions)
        {
            using (var db = Connection)
            {
                var query = $"SELECT * FROM {typeof(T).Name} WHERE 1=1";
                var parameters = new DynamicParameters();

                foreach (var property in conditions.GetType().GetProperties())
                {
                    var value = property.GetValue(conditions);
                    if (value != null)
                    {
                        query += $" AND {property.Name} = @{property.Name}";
                        parameters.Add($"@{property.Name}", value);
                    }
                }
                return await db.QueryAsync<T>(query, parameters);
            }
        }

        public async Task<int> Add(T entity)
        {
            using (var db = Connection)
            {
                string insertQuery = GenerateInsertQuery();
                return await db.ExecuteAsync(insertQuery, entity);
            }
        }

        public async Task<int> Update(T entity)
        {
            using (var db = Connection)
            {
                string updateQuery = GenerateUpdateQuery();
                return await db.ExecuteAsync(updateQuery, entity);
            }
        }

        public async Task<int> Delete(Guid id)
        {
            using (var db = Connection)
            {
                return await db.ExecuteAsync("DELETE FROM " + typeof(T).Name + " WHERE Id = @Id", new { Id = id });
            }
        }

        public async Task<T> ExecuteSingleStoredProcedure(string procedureName, object parameters = null)
        {
            using (var db = Connection)
            {
                return await db.QueryFirstAsync<T>(procedureName, parameters, commandType: CommandType.StoredProcedure);
            }
        }
        public async Task<IEnumerable<T>> ExecuteListStoredProcedure(string procedureName, object parameters = null)
        {
            using (var db = Connection)
            {
                return await db.QueryAsync<T>(procedureName, parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<IEnumerable<T>> ExecuteCustomQuery(string query, object parameters = null)
        {
            using (var db = Connection)
            {
                return await db.QueryAsync<T>(query, parameters);
            }
        }

        private string GenerateInsertQuery()
        {
            var type = typeof(T);
            //var properties = type.GetProperties().Where(p => p.Name.ToLower() != "id").ToList();
            var properties = type.GetProperties().ToList();
            var columnNames = string.Join(", ", properties.Select(p => p.Name));
            var paramNames = string.Join(", ", properties.Select(p => "@" + p.Name));

            return $"INSERT INTO {type.Name} ({columnNames}) VALUES ({paramNames})";
        }

        private string GenerateUpdateQuery()
        {
            var type = typeof(T);
            var properties = type.GetProperties().Where(p => p.Name.ToLower() != "id").ToList();
            var setClause = string.Join(", ", properties.Select(p => $"{p.Name} = @{p.Name}"));

            return $"UPDATE {type.Name} SET {setClause} WHERE Id = @Id";
        }
    }
}
