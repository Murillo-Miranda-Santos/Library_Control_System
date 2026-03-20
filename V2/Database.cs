namespace LibraryControlSystem_2._0;

using DotNetEnv;
using MySql.Data.MySqlClient;

public class Database
{
    private string connectionString;

    public Database()
    {
        Env.Load(Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", ".env"));

        string server = Environment.GetEnvironmentVariable("DB_SERVER");
        string database = Environment.GetEnvironmentVariable("DB_DATABASE");
        string user = Environment.GetEnvironmentVariable("DB_USER");
        string password = Environment.GetEnvironmentVariable("DB_PASSWORD");

        connectionString = $"server={server};database={database};user={user};password={password};";
    }

    public MySqlConnection GetConnection()
    {
        return new MySqlConnection(connectionString);
    }
}

