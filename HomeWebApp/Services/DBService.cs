using HomeWebApp.Models;
using MySqlConnector;

namespace HomeWebApp.Services
{
    public class DBService
    {
        private readonly string _connectionString = "";
        private readonly string _tablePrefix = "homeapp_";

        public DBService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DS-Server") ?? _connectionString;
            _tablePrefix = configuration["DatabaseTablePrefix"] ?? _tablePrefix;
        }

        public async Task<IEnumerable<RadioStation>> GetRadioStations()
        {
            var radioStations = new List<RadioStation>();

            var sql = $"""
                SELECT
                    radio_station_id
                    , radio_station_name
                    , radio_station_url
                FROM
                    {_tablePrefix}radio_stations
                WHERE
                    radio_station_deleted_at IS NULL
            """;

            using (var connection = new MySqlConnection(_connectionString))
            using (var command = new MySqlCommand(sql, connection))
            {
                connection.Open();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        radioStations.Add(new RadioStation()
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Url = reader.GetString(2)
                        });
                    }
                }
                connection.Close();
            }

            return radioStations;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            var users = new List<User>();

            var sql = $"""
                SELECT
                   user_id
                   , user_name
                FROM
                    {_tablePrefix}users
            """;

            using (var connection = new MySqlConnection(_connectionString))
            using (var command = new MySqlCommand(sql, connection))
            {
                connection.Open();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        users.Add(new User()
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1)
                        });
                    }
                }
                connection.Close();
            }

            return users;
        }

        public async Task<IEnumerable<ExpenseCategory>> GetExpenseCategories()
        {
            var expenseCategories = new List<ExpenseCategory>();

            var sql = $"""
                SELECT 
                   expense_category_id
                   , expense_category_name
                    , expense_category_color
                    , expense_category_description
                FROM
                    {_tablePrefix}expense_categories
            """;

            using (var connection = new MySqlConnection(_connectionString))
            using (var command = new MySqlCommand(sql, connection))
            {
                connection.Open();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        expenseCategories.Add(new ExpenseCategory()
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Color = reader.GetString(2),
                            Description = reader.IsDBNull(3) ? "" : reader.GetString(3)
                        });
                    }
                }
                connection.Close();
            }

            return expenseCategories;
        }

        public async Task<IEnumerable<Expense>> GetExpenses()
        {
            var users = (await GetUsers()).ToList();    
            var expenseCategories = (await GetExpenseCategories()).ToList();
            var expenses = new List<Expense>();

            var sql = $"""
                SELECT 
                   expense_id
                   , expense_date
                   , expense_description
                   , expense_category_id
                   , expense_amount
                   , image_id
                   , user_id
                FROM
                    {_tablePrefix}expenses
                WHERE
                    expense_delete_at IS NULL
                ORDER BY expense_date
            """;

            using (var connection = new MySqlConnection(_connectionString))
            using (var command = new MySqlCommand(sql, connection))
            {
                connection.Open();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        expenses.Add(new Expense()
                        {
                            Id = reader.GetInt32(0),
                            Date = reader.GetDateOnly(1),
                            Description = reader.GetString(2),
                            Category = expenseCategories.Find((category) => category.Id == reader.GetInt32(3)) ?? new ExpenseCategory(),
                            Amount = reader.GetDecimal(4),
                            User = users.Find((user) => user.Id == reader.GetInt32(6)) ?? new User() { Id = -1, Name = "" },
                        });
                    }
                }
                connection.Close();
            }

            return expenses;
        }
    
        public async Task UpdateExpense(Expense expense)
        {
            var sql = $"""
                UPDATE {_tablePrefix}expenses
                SET
                   expense_date = @date,
                   expense_description = @description,
                   expense_category_id = @category_id,
                   expense_amount = @amount,
                   image_id = @image_id,
                   user_id = @user_id
                WHERE
                    expense_id = @id
            """;

            using (var connection = new MySqlConnection(_connectionString))
            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@date", expense.Date);
                command.Parameters.AddWithValue("@description", expense.Description);
                command.Parameters.AddWithValue("@category_id", expense.Category.Id);
                command.Parameters.AddWithValue("@amount", expense.Amount);
                command.Parameters.AddWithValue("@image_id", expense.Image == null ? DBNull.Value : expense.Image.Id);
                command.Parameters.AddWithValue("@user_id", expense.User.Id);
                command.Parameters.AddWithValue("@id", expense.Id);

                connection.Open();

                await command.ExecuteNonQueryAsync();
                
                connection.Close();
            }
        }

        public async Task<int> InsertExpense(Expense expense)
        {
            var sql = $"""
                INSERT INTO {_tablePrefix}expenses (
                    expense_date,
                    expense_description,
                    expense_category_id,
                    expense_amount,
                    image_id,
                    user_id
                )
                VALUES
                   (@date, @description, @category_id, @amount, @image_id, @user_id);

                SELECT LAST_INSERT_ID();
            """;

            var id = -1;

            using (var connection = new MySqlConnection(_connectionString))
            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@date", expense.Date);
                command.Parameters.AddWithValue("@description", expense.Description);
                command.Parameters.AddWithValue("@category_id", expense.Category.Id);
                command.Parameters.AddWithValue("@amount", expense.Amount);
                command.Parameters.AddWithValue("@image_id", expense.Image == null ? DBNull.Value : expense.Image.Id);
                command.Parameters.AddWithValue("@user_id", expense.User.Id);

                connection.Open();

                try
                {
                    object? result = await command.ExecuteScalarAsync();

                    if (result != null && result != DBNull.Value)
                    {
                        id = Convert.ToInt32(result);
                    }
                } 
                catch(Exception e)
                {
                    Console.WriteLine(e);
                }

                connection.Close();
            }

            return id;
        }

        public async Task DeleteExpense(Expense expense)
        {
            var sql = $"""
                UPDATE {_tablePrefix}expenses
                SET
                    expense_delete_at = @deleted_at
                WHERE
                    expense_id = @id
            """;

            using (var connection = new MySqlConnection(_connectionString))
            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@deleted_at", DateTime.Now);
                command.Parameters.AddWithValue("@id", expense.Id);

                connection.Open();

                try
                {
                    await command.ExecuteNonQueryAsync();
                }
                catch(Exception e)
                {
                    Console.WriteLine(e);
                }

                connection.Close();
            }
        }
    }
}
