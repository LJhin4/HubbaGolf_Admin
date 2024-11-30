#pragma warning disable
using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace HubbaGolfAdmin.Database
{
    public class DapperHelper
    {
        private string _conn;
        public DapperHelper(IConfiguration configuration)
        {
             _conn = configuration.GetConnectionString("Default");
        }

        public long Insert<T>(T entityToInsert) where T : class
        {
            using (var connection = new SqlConnection(_conn))
            {
                connection.Open();
                long re = 0;

                try
                {
                    re = connection.Insert(entityToInsert);
                }
                catch (Exception)
                {
                    throw;
                }

                return re;
            }
        }

        public bool Update<T>(T entityToUpdate) where T : class
        {
            using (var connection = new SqlConnection(_conn))
            {
                connection.Open();
                var result = false;
                try
                {
                    result = connection.Update(entityToUpdate);
                }
                catch (Exception)
                {
                    throw;
                }

                return result;
            }
        }

        public bool Delete<T>(T entityToDelete) where T : class
        {
            using (var connection = new SqlConnection(_conn))
            {
                connection.Open();
                bool success = false;

                try
                {
                    success = connection.Delete(entityToDelete);
                }
                catch (Exception)
                {
                    throw;
                }

                return success;
            }
        }

        public DataTable ExecProcedureDataAsDataTable(string ProcedureName, object parametter = null)
        {
            using (var connection = new SqlConnection(_conn))
            {
                connection.Open();
                DataTable table = new DataTable();

                try
                {
                    var reader = connection.ExecuteReader(ProcedureName, param: parametter, commandType: CommandType.StoredProcedure);

                    table.Load(reader);
                }
                catch (Exception)
                {
                    throw;
                }

                return table;
            }
        }

        public async Task<DataTable> ExecProcedureDataAsyncAsDataTable(string ProcedureName, object parametter = null)
        {
            return await WithConnection(async c =>
            {
                var reader = await c.ExecuteReaderAsync(ProcedureName, param: parametter, commandType: CommandType.StoredProcedure);
                DataTable table = new DataTable();
                table.Load(reader);
                return table;
            });
        }

        public DataTable ExecQueryDataAsDataTable(string T_SQL, object parametter = null)
        {
            using (var connection = new SqlConnection(_conn))
            {
                connection.Open();
                var reader = connection.ExecuteReader(T_SQL, param: parametter, commandType: CommandType.Text);
                DataTable table = new DataTable();
                table.Load(reader);
                return table;
            }
        }

        public async Task<DataTable> ExecQueryDataAsyncAsDataTable(string T_SQL, object parametter = null)
        {
            return await WithConnection(async c =>
            {
                var reader = await c.ExecuteReaderAsync(T_SQL, param: parametter, commandType: CommandType.Text);
                DataTable table = new DataTable();
                table.Load(reader);
                return table;
            });
        }

        public IEnumerable<T> ExecProcedureData<T>(string ProcedureName, object parametter = null)
        {
            using (var connection = new SqlConnection(_conn))
            {
                connection.Open();
                return connection.Query<T>(ProcedureName, param: parametter, commandType: CommandType.StoredProcedure);
            }
        }

        public T ExecProcedureDataFistOrDefault<T>(string ProcedureName, object parametter = null)
        {
            using (var connection = new SqlConnection(_conn))
            {
                connection.Open();
                return connection.QueryFirstOrDefault<T>(ProcedureName, parametter, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<IEnumerable<T>> ExecProcedureDataAsync<T>(string ProcedureName, object parametter = null)
        {
            return await WithConnection(async c =>
            {
                return await c.QueryAsync<T>(ProcedureName, parametter, commandType: CommandType.StoredProcedure);
            });
        }

        public async Task<T> ExecProcedureDataFirstOrDefaultAsync<T>(string ProcedureName, object parametter = null)
        {
            using (var connection = new SqlConnection(_conn))
            {
                connection.Open();
                return await connection.QueryFirstOrDefaultAsync<T>(ProcedureName, parametter, commandType: CommandType.StoredProcedure);
            }
        }

        public int ExecProcedureNonData(string ProcedureName, object parametter = null)
        {
            using (var connection = new SqlConnection(_conn))
            {
                connection.Open();
                //return affectedRows
                return connection.Execute(ProcedureName, parametter, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<int> ExecProcedureNonDataAsync(string ProcedureName, object parametter = null)
        {
            using (var connection = new SqlConnection(_conn))
            {
                connection.Open();
                //return affectedRows
                return await connection.ExecuteAsync(ProcedureName, parametter, commandType: CommandType.StoredProcedure);
            }
        }

        public IEnumerable<T> ExecQueryData<T>(string T_SQL, object parametter = null)
        {
            using (var connection = new SqlConnection(_conn))
            {
                connection.Open();
                return connection.Query<T>(T_SQL, parametter, commandType: CommandType.Text);
            }
        }

        public T ExecQueryDataFirstOrDefault<T>(string T_SQL, object parametter = null)
        {
            using (var connection = new SqlConnection(_conn))
            {
                connection.Open();
                return connection.QueryFirstOrDefault<T>(T_SQL, parametter, commandType: CommandType.Text);
            }
        }

        public async Task<IEnumerable<T>> ExecQueryDataAsync<T>(string T_SQL, object parametter = null)
        {
            return await WithConnection(async c =>
            {
                return await c.QueryAsync<T>(T_SQL, parametter, commandType: CommandType.Text);
            });
        }

        public async Task<T> ExecQueryDataFirstOrDefaultAsync<T>(string T_SQL, object parametter = null)
        {
            using (var connection = new SqlConnection(_conn))
            {
                connection.Open();
                return await connection.QueryFirstOrDefaultAsync<T>(T_SQL, parametter, commandType: CommandType.Text);
            }
        }

        public int ExecQueryNonData(string T_SQL, object? parametter = null)
        {
            using (var connection = new SqlConnection(_conn))
            {
                connection.Open();
                return connection.Execute(T_SQL, parametter, commandType: CommandType.Text);
            }
        }

        public async Task<int> ExecQueryNonDataAsync(string T_SQL, object? parametter = null)
        {
            return await WithConnection(async c =>
            {
                return await c.ExecuteAsync(T_SQL, parametter, commandType: CommandType.Text);
            });
        }

        public async Task<T> WithConnection<T>(Func<IDbConnection, Task<T>> getData)
        {
            using (var connection = new SqlConnection(_conn))
            {
                await connection.OpenAsync(); // Asynchronously open a connection to the database
                return await getData(connection); // Asynchronously execute getData, which has been passed in as a Func<IDBConnection, Task<T>>
            }
        }

        public object ExecProcedureSacalar(string ProcedureName, object? parametter = null)
        {
            using (var connection = new SqlConnection(_conn))
            {
                connection.Open();
                return connection.ExecuteScalar<object>(ProcedureName, parametter, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<object> ExecProcedureSacalarAsync(string ProcedureName, object? parametter = null)
        {
            using (var connection = new SqlConnection(_conn))
            {
                connection.Open();
                return await connection.ExecuteScalarAsync<object>(ProcedureName, parametter, commandType: CommandType.StoredProcedure);
            }
        }

        public object ExecQuerySacalar(string T_SQL, object? parametter = null)
        {
            using (var connection = new SqlConnection(_conn))
            {
                connection.Open();
                return connection.ExecuteScalar<object>(T_SQL, parametter, commandType: CommandType.Text);
            }
        }

        public async Task<object> ExecQuerySacalarAsync(string T_SQL, object? parametter = null)
        {
            using var connection = new SqlConnection(_conn);
            connection.Open();
            return await connection.ExecuteScalarAsync<object>(T_SQL, parametter, commandType: CommandType.Text);
        }

        public List<T> ExecProcedureDataAsList<T>(string procedureName, object? parameters = null)
        {
            using (IDbConnection connection = new SqlConnection(_conn))
            {
                connection.Open();
                var result = connection.Query<T>(procedureName, parameters, commandType: CommandType.StoredProcedure);
                return result.AsList();
            }
        }

        public async Task<List<T>> ExecProcedureDataAsListAsync<T>(string procedureName, object? parameters = null)
        {
            using (IDbConnection connection = new SqlConnection(_conn))
            {
                connection.Open();
                var result = await connection.QueryAsync<T>(procedureName, parameters, commandType: CommandType.StoredProcedure);
                return result.AsList();
            }
        }

        public async Task<string> ExecuteAndGetOutputValueAsync<T>(string procedureName, string type)
        {
            using (IDbConnection connection = new SqlConnection(_conn))
            {
                connection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@Module", type, DbType.String, ParameterDirection.Input);
                dynamicParameters.Add("@NewValue", dbType: DbType.String, size: 20, direction: ParameterDirection.Output);

                await connection.ExecuteAsync(procedureName, dynamicParameters, commandType: CommandType.StoredProcedure);

                // Get output value from parameters
                var outputValue = dynamicParameters.Get<string>("@NewValue");

                return outputValue;
            }
        }
    }
}
