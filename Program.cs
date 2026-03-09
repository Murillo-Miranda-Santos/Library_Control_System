namespace LibrarySystem;

using DotNetEnv;
using MySql.Data.MySqlClient;
using System.IO;

public class Program
{
    public static void Main()
    {
        Env.Load();

        string server = Environment.GetEnvironmentVariable("DB_SERVER");
        string database = Environment.GetEnvironmentVariable("DB_DATABASE");
        string user = Environment.GetEnvironmentVariable("DB_USER");
        string password = Environment.GetEnvironmentVariable("DB_PASSWORD");

        string path = $"server={server};database={database};user={user};password={password};";

        MySqlConnection Connection;
        MySqlCommand Command;
        MySqlDataReader Reader;

        string space = "================================";

        bool running = true;
        while (running)
        {
            Console.WriteLine(space);
            Console.WriteLine("   Library Control System");
            Console.WriteLine(space);

            Console.WriteLine("Choose an option to continue: ");

            Console.WriteLine("[1] Register book \n[2] Register user \n[3] Loan book \n[4] Return book " +
                "\n[5] List books \n[6] Generate report \n[7] Finish");

            int choice;
            int cont = 0;
            while (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > 7)
            {
                cont++;
                Console.WriteLine("Choose a valid option.");
                if (cont == 4)
                {
                    Console.WriteLine("[1] Register book \n[2] Register user \n[3] Loan book \n[4] Return book " +
                    "\n[5] List books \n[6] Generate report \n[7] Finish");

                    cont = 0;
                }
            }

            Console.Clear();

            switch (choice)
            {
                case 1:
                    // Register book
                    while (true)
                    {
                        Console.WriteLine("Type the title of the book: ");
                        string title = Console.ReadLine()?.Trim() ?? "";
                        while (title == "")
                        {
                            Console.WriteLine("Type a valid title: ");
                            title = Console.ReadLine()?.Trim() ?? "";
                        }

                        using (Connection = new MySqlConnection(path))
                        {
                            Connection.Open();
                            string sql = "SELECT id FROM books WHERE title = @title";

                            using (Command = new MySqlCommand(sql, Connection))
                            {
                                Command.Parameters.AddWithValue("@title", title);
                                using (Reader = Command.ExecuteReader())
                                {
                                    if (Reader.Read())
                                    {
                                        Console.WriteLine("This book is already registered. Do you want:");
                                        Console.WriteLine("[1] Try another name?");
                                        Console.WriteLine("[2] Finish?");

                                        int option;
                                        while (!int.TryParse(Console.ReadLine(), out option) || option < 1 || option > 2)
                                            Console.WriteLine("Choose a valid option.");

                                        if (option == 2)
                                        {
                                            break;
                                        }
                                        else
                                        {
                                            continue;
                                        }
                                    }
                                }
                            }

                            Console.WriteLine("Type the quantity of the book: ");
                            int quantity;
                            while (!int.TryParse(Console.ReadLine(), out quantity))
                                Console.WriteLine("Type a valid quantity.");

                            sql = "INSERT INTO books VALUES (DEFAULT, @title, @quantity)";
                            using (Command = new MySqlCommand(sql, Connection))
                            {
                                Command.Parameters.AddWithValue("@title", title);
                                Command.Parameters.AddWithValue("@quantity", quantity);
                                Command.ExecuteNonQuery();
                            }

                            Console.WriteLine("Book successfully registered!");
                        }
                        break;
                    }
                    break;

                case 2:
                    // Register user
                    using (Connection = new MySqlConnection(path))
                    {
                        Connection.Open();
                        string username;
                        bool usernameExists = true;

                        while (usernameExists)
                        {
                            Console.WriteLine("Type the username: ");
                            username = Console.ReadLine()?.Trim() ?? "";
                            while (username == "")
                            {
                                Console.WriteLine("Type a valid username: ");
                                username = Console.ReadLine()?.Trim() ?? "";
                            }

                            string sql = "SELECT username FROM userLibrary WHERE username = @username";
                            using (Command = new MySqlCommand(sql, Connection))
                            {
                                Command.Parameters.AddWithValue("@username", username);
                                using (Reader = Command.ExecuteReader())
                                {
                                    if (Reader.Read())
                                    {
                                        Console.WriteLine("This username is already in use. Try another.");
                                        continue;
                                    }
                                    else
                                    {
                                        usernameExists = false;
                                    }
                                }
                            }

                            if (!usernameExists)
                            {
                                sql = "INSERT INTO userLibrary VALUES (DEFAULT, @username)";
                                using (Command = new MySqlCommand(sql, Connection))
                                {
                                    Command.Parameters.AddWithValue("@username", username);
                                    Command.ExecuteNonQuery();
                                }
                                Console.WriteLine("User successfully registered!");
                            }
                        }
                    }
                    break;

                case 3:
                    // Loan book
                    using (Connection = new MySqlConnection(path))
                    {
                        Connection.Open();

                        int userLibrary_id = 0;
                        bool userFound = false;
                        string username = "";

                        while (!userFound)
                        {
                            Console.WriteLine("Type the username: ");
                            username = Console.ReadLine()?.Trim() ?? "";
                            while (username == "")
                            {
                                Console.WriteLine("Type a valid username: ");
                                username = Console.ReadLine()?.Trim() ?? "";
                            }

                            string sql = "SELECT id FROM userLibrary WHERE username = @username";
                            using (Command = new MySqlCommand(sql, Connection))
                            {
                                Command.Parameters.AddWithValue("@username", username);
                                using (Reader = Command.ExecuteReader())
                                {
                                    if (Reader.Read())
                                    {
                                        userLibrary_id = Reader.GetInt32("id");
                                        userFound = true;
                                    }
                                }
                            }

                            if (!userFound)
                            {
                                Console.WriteLine("User not found. Do you want:");
                                Console.WriteLine("[1] Try another username?");
                                Console.WriteLine("[2] Finish?");
                                int option;
                                while (!int.TryParse(Console.ReadLine(), out option) || option < 1 || option > 2)
                                    Console.WriteLine("Choose a valid option.");
                                if (option == 2)
                                    break;
                            }
                        }

                        if (!userFound)
                            break;

                        int book_id = 0;
                        int quantity = 0;
                        bool bookFound = false;

                        while (!bookFound)
                        {
                            Console.WriteLine("Type the title of the book to be borrowed: ");
                            string title = Console.ReadLine()?.Trim() ?? "";
                            while (title == "")
                            {
                                Console.WriteLine("Type a valid title: ");
                                title = Console.ReadLine()?.Trim() ?? "";
                            }

                            string sql = "SELECT id, quantity FROM books WHERE title = @title LIMIT 1";
                            using (Command = new MySqlCommand(sql, Connection))
                            {
                                Command.Parameters.AddWithValue("@title", title);
                                using (Reader = Command.ExecuteReader())
                                {
                                    if (Reader.Read())
                                    {
                                        book_id = Reader.GetInt32("id");
                                        quantity = Reader.GetInt32("quantity");
                                        bookFound = true;
                                    }
                                }
                            }

                            if (!bookFound)
                            {
                                Console.WriteLine("Book not found. Do you want:");
                                Console.WriteLine("[1] Try another title?");
                                Console.WriteLine("[2] Finish?");
                                int option;
                                while (!int.TryParse(Console.ReadLine(), out option) || option < 1 || option > 2)
                                    Console.WriteLine("Choose a valid option.");
                                if (option == 2)
                                    break;
                            }
                        }

                        if (!bookFound)
                            break;

                        if (quantity == 0)
                        {
                            Console.WriteLine("Book out of stock.");
                            break;
                        }

                        string updateSql = "UPDATE books SET quantity = quantity - 1 WHERE id = @id";
                        using (Command = new MySqlCommand(updateSql, Connection))
                        {
                            Command.Parameters.AddWithValue("@id", book_id);
                            Command.ExecuteNonQuery();
                        }

                        string insertLoanSql = "INSERT INTO loans VALUES (DEFAULT, @book_id, @userLibrary_id, @loan_date)";
                        using (Command = new MySqlCommand(insertLoanSql, Connection))
                        {
                            Command.Parameters.AddWithValue("@book_id", book_id);
                            Command.Parameters.AddWithValue("@userLibrary_id", userLibrary_id);
                            Command.Parameters.AddWithValue("@loan_date", DateTime.UtcNow);
                            Command.ExecuteNonQuery();
                        }

                        Console.WriteLine("Book successfully borrowed.");
                    }
                    break;

                case 4:
                    // Return book
                    using (Connection = new MySqlConnection(path))
                    {
                        Connection.Open();

                        int bookId = 0;
                        bool bookFound = false;
                        while (!bookFound)
                        {
                            Console.WriteLine("Type the title of the book to be returned: ");
                            string title = Console.ReadLine()?.Trim() ?? "";
                            while (title == "")
                            {
                                Console.WriteLine("Type a valid title: ");
                                title = Console.ReadLine()?.Trim() ?? "";
                            }

                            string sql = "SELECT id FROM books WHERE title = @title";
                            using (Command = new MySqlCommand(sql, Connection))
                            {
                                Command.Parameters.AddWithValue("@title", title);
                                using (Reader = Command.ExecuteReader())
                                {
                                    if (Reader.Read())
                                    {
                                        bookId = Reader.GetInt32("id");
                                        bookFound = true;
                                    }
                                }
                            }

                            if (!bookFound)
                            {
                                Console.WriteLine("Book not found. Do you want:");
                                Console.WriteLine("[1] Try another title?");
                                Console.WriteLine("[2] Finish?");
                                int option;
                                while (!int.TryParse(Console.ReadLine(), out option) || option < 1 || option > 2)
                                    Console.WriteLine("Choose a valid option.");
                                if (option == 2)
                                    break;
                            }
                        }

                        if (!bookFound)
                            break;

                        int userLibraryId = 0;
                        bool userFound = false;
                        string username = "";

                        while (!userFound)
                        {
                            Console.WriteLine("Type the username:");
                            username = Console.ReadLine()?.Trim() ?? "";
                            while (username == "")
                            {
                                Console.WriteLine("Type a valid name:");
                                username = Console.ReadLine()?.Trim() ?? "";
                            }

                            string sql = "SELECT id FROM userLibrary WHERE username = @username";
                            using (Command = new MySqlCommand(sql, Connection))
                            {
                                Command.Parameters.AddWithValue("@username", username);
                                using (Reader = Command.ExecuteReader())
                                {
                                    if (Reader.Read())
                                    {
                                        userLibraryId = Reader.GetInt32("id");
                                        userFound = true;
                                    }
                                }
                            }

                            if (!userFound)
                            {
                                Console.WriteLine("User not found. Do you want:");
                                Console.WriteLine("[1] Try another name? \n[2] Finish?");
                                int option;
                                while (!int.TryParse(Console.ReadLine(), out option) || option < 1 || option > 2)
                                    Console.WriteLine("Type a valid option.");
                                if (option == 2)
                                    break;
                            }
                        }

                        if (!userFound)
                            break;

                        int loan_id = 0;
                        DateTime loanDate = DateTime.MinValue;
                        bool loanFound = false;

                        string selectLoanSql = "SELECT id, loan_date FROM loans " +
                                               "WHERE book_id = @BookId AND userLibrary_id = @userLibraryId " +
                                               "ORDER BY loan_date ASC LIMIT 1";
                        using (Command = new MySqlCommand(selectLoanSql, Connection))
                        {
                            Command.Parameters.AddWithValue("@BookId", bookId);
                            Command.Parameters.AddWithValue("@userLibraryId", userLibraryId);
                            using (Reader = Command.ExecuteReader())
                            {
                                if (Reader.Read())
                                {
                                    loan_id = Reader.GetInt32("id");
                                    loanDate = Reader.GetDateTime("loan_date");
                                    loanFound = true;
                                }
                            }
                        }

                        if (!loanFound)
                        {
                            Console.WriteLine("Loan not found.");
                            break;
                        }

                        string updateBookSql = "UPDATE books SET quantity = quantity + 1 WHERE id = @id";
                        using (Command = new MySqlCommand(updateBookSql, Connection))
                        {
                            Command.Parameters.AddWithValue("@id", bookId);
                            Command.ExecuteNonQuery();
                        }

                        string deleteLoanSql = "DELETE FROM loans WHERE id = @loan_id";
                        using (Command = new MySqlCommand(deleteLoanSql, Connection))
                        {
                            Command.Parameters.AddWithValue("@loan_id", loan_id);
                            Command.ExecuteNonQuery();
                        }

                        Console.WriteLine($"Book borrowed on {loanDate} by {username}, successfully returned.");
                    }
                    break;

                case 5:
                    // List books
                    Console.WriteLine("List of books: ");
                    using (Connection = new MySqlConnection(path))
                    {
                        Connection.Open();
                        string sql = "SELECT * FROM books";
                        using (Command = new MySqlCommand(sql, Connection))
                        using (Reader = Command.ExecuteReader())
                        {
                            cont = 0;
                            while (Reader.Read())
                            {
                                cont++;
                                Console.WriteLine("-------------------------------");
                                Console.WriteLine($"{cont}º Book");
                                Console.WriteLine($"ID: {Reader["id"]}");
                                Console.WriteLine($"Title: {Reader["title"]}");
                                Console.WriteLine($"Quantity: {Reader["quantity"]}");
                            }
                        }
                    }
                    Console.WriteLine("");
                    break;

                case 6:
                    // Generate report
                    string projectPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
                    string docName = "Report.txt";
                    string pathName = Path.Combine(projectPath, docName);

                    using (StreamWriter Writer = new StreamWriter(pathName))
                    {
                        Writer.WriteLine("============ LIBRARY REPORT ============");
                        Writer.WriteLine("\n");

                        using (Connection = new MySqlConnection(path))
                        {
                            Connection.Open();

                            string sql = "SELECT * FROM books";
                            using (Command = new MySqlCommand(sql, Connection))
                            {
                                Writer.WriteLine("BOOKS LIST: ");
                                Writer.WriteLine("\n");
                                using (Reader = Command.ExecuteReader())
                                {
                                    while (Reader.Read())
                                    {
                                        Writer.WriteLine($"ID: {Reader["id"]}");
                                        Writer.WriteLine($"Title: {Reader["title"]}");
                                        Writer.WriteLine($"Quantity: {Reader["quantity"]}");
                                        Writer.WriteLine("-----------------------------------");
                                    }
                                }
                                Writer.WriteLine("\n");
                            }

                            sql = "SELECT * FROM userLibrary";
                            using (Command = new MySqlCommand(sql, Connection))
                            {
                                Writer.WriteLine("USERS LIST: ");
                                Writer.WriteLine("\n");
                                using (Reader = Command.ExecuteReader())
                                {
                                    while (Reader.Read())
                                    {
                                        Writer.WriteLine($"ID: {Reader["id"]}");
                                        Writer.WriteLine($"Username: {Reader["username"]}");
                                        Writer.WriteLine("-----------------------------------");
                                    }
                                }
                                Writer.WriteLine("\n");
                            }

                            sql = "SELECT * FROM loans";
                            using (Command = new MySqlCommand(sql, Connection))
                            {
                                Writer.WriteLine("LOANS LIST: ");
                                Writer.WriteLine("\n");
                                using (Reader = Command.ExecuteReader())
                                {
                                    while (Reader.Read())
                                    {
                                        Writer.WriteLine($"ID: {Reader["id"]}");
                                        Writer.WriteLine($"Book ID: {Reader["book_id"]}");
                                        Writer.WriteLine($"User ID: {Reader["userLibrary_id"]}");
                                        Writer.WriteLine($"Loan Date: {Reader["loan_date"]}");
                                        Writer.WriteLine("-----------------------------------");
                                    }
                                }
                                Writer.WriteLine("\n");
                                Writer.WriteLine(space);
                            }
                        }
                    }

                    Console.WriteLine($"Report generated successfully at {docName}.");
                    break;

                case 7:
                    Console.WriteLine(space);
                    Console.WriteLine("Program finished.");
                    Console.WriteLine(space);
                    running = false;
                    break;
            }
        }
    }
}

