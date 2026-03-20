using MySql.Data.MySqlClient;

namespace LibraryControlSystem_2._0;

internal class Repository
{
    Database database = new Database();


    //SQL BOOK
    public Book SelectBook(Book book)
    {
        using (MySqlConnection Connection = database.GetConnection())
        {
            Connection.Open();

            string sql = "SELECT * FROM books WHERE title = @title LIMIT 1";

            using (MySqlCommand Command = new MySqlCommand(sql, Connection))
            {
                Command.Parameters.AddWithValue("@title", book.Title);

                using (MySqlDataReader Reader = Command.ExecuteReader())
                {
                    if (Reader.Read())
                    {
                        return new Book
                        {
                            Id = Reader.GetInt32("id"),
                            Title = Reader.GetString("title"),
                            Quantity = Reader.GetInt32("quantity")
                        };
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }
    }

    public List<Book> SelectAllBooks()
    {
        List<Book> books = new List<Book>();

        using (MySqlConnection Connection = database.GetConnection())
        {
            Connection.Open();

            string sql = "SELECT * FROM books";

            using (MySqlCommand Command = new MySqlCommand(sql, Connection))
            using (MySqlDataReader Reader = Command.ExecuteReader())
            {
                while (Reader.Read())
                {
                    Book book = new Book()
                    {
                        Id = Reader.GetInt32("id"),
                        Title = Reader.GetString("title"),
                        Quantity = Reader.GetInt32("quantity")
                    };

                    books.Add(book);
                }
            }
        }

        return books;
    }

    public void InsertBook(Book book)
    {
        using (MySqlConnection Connection = database.GetConnection())
        {
            Connection.Open();

            string sql = "INSERT INTO books VALUES (DEFAULT, @title, @quantity)";

            using (MySqlCommand Command = new MySqlCommand(sql, Connection))
            {
                Command.Parameters.AddWithValue("@title", book.Title);
                Command.Parameters.AddWithValue("@quantity", book.Quantity);
                Command.ExecuteNonQuery();
            }
        }
    }

    public void UpdateBook(Book book)
    {
        using (MySqlConnection Connection = database.GetConnection())
        {
            Connection.Open();

            string sql = "UPDATE books SET quantity = @quantity WHERE id = @id LIMIT 1";

            using (MySqlCommand Command = new MySqlCommand(sql, Connection))
            {
                Command.Parameters.AddWithValue("@quantity", book.Quantity);
                Command.Parameters.AddWithValue("@id", book.Id);
                Command.ExecuteNonQuery();
            }
        }
    }



    //SQL USER
    public User SelectUser(User user)
    {
        using (MySqlConnection Connection = database.GetConnection())
        {
            Connection.Open();

            string sql = "SELECT * FROM userlibrary WHERE username = @username LIMIT 1";

            using (MySqlCommand Command = new MySqlCommand(sql, Connection))
            {
                Command.Parameters.AddWithValue("@username", user.Username);

                using (MySqlDataReader Reader = Command.ExecuteReader())
                {
                    if (Reader.Read())
                    {
                        return new User
                        {
                            Id = Reader.GetInt32("id"),
                            Username = Reader.GetString("username")
                        };
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }
    }

    public List<User> SelectAllUsers()
    {
        List<User> users = new List<User>();

        using (MySqlConnection Connection = database.GetConnection())
        {
            Connection.Open();

            string sql = "SELECT * FROM userlibrary";

            using (MySqlCommand Command = new MySqlCommand(sql, Connection))
            using (MySqlDataReader Reader = Command.ExecuteReader())
            {
                while (Reader.Read())
                {
                    User user = new User()
                    {
                        Id = Reader.GetInt32("id"),
                        Username = Reader.GetString("username"),
                    };

                    users.Add(user);
                }
            }
        }

        return users;
    }

    public void InsertUser(User user)
    {
        using (MySqlConnection Connection = database.GetConnection())
        {
            Connection.Open();

            string sql = "INSERT INTO userlibrary VALUES (DEFAULT, @username)";

            using (MySqlCommand Command = new MySqlCommand(sql, Connection))
            {
                Command.Parameters.AddWithValue("@username", user.Username);
                Command.ExecuteNonQuery();
            }
        }
    }




    //SQL LOAN
    public Loan SelectLoan(Loan loan)
    {
        using (MySqlConnection Connection = database.GetConnection())
        {
            Connection.Open();

            string sql = "SELECT * FROM loans WHERE book_id = @book_id && userlibrary_id = @userlibrary_id LIMIT 1";

            using (MySqlCommand Command = new MySqlCommand(sql, Connection))
            {
                Command.Parameters.AddWithValue("@book_id", loan.Book_id);
                Command.Parameters.AddWithValue("@userlibrary_id", loan.UserLibrary_id);

                using (MySqlDataReader Reader = Command.ExecuteReader())
                {
                    if (Reader.Read())
                    {
                        return new Loan
                        {
                            Id = Reader.GetInt32("id")
                        };
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }
    }

    public List<Loan> SelectAllLoans()
    {
        List<Loan> loans = new List<Loan>();

        using (MySqlConnection Connection = database.GetConnection())
        {
            Connection.Open();

            string sql = "SELECT * FROM loans";

            using (MySqlCommand Command = new MySqlCommand(sql, Connection))
            using (MySqlDataReader Reader = Command.ExecuteReader())
            {
                while (Reader.Read())
                {
                    Loan loan = new Loan()
                    {
                        Id = Reader.GetInt32("id"),
                        Book_id = Reader.GetInt32("book_id"),
                        UserLibrary_id = Reader.GetInt32("userlibrary_id"),
                        LoanDate = Reader.GetDateTime("loan_date")
                    };

                    loans.Add(loan);
                }
            }
        }

        return loans;
    }

    public void InsertLoan(Loan loan)
    {
        using (MySqlConnection connection = database.GetConnection())
        {
            connection.Open();

            string sql = "INSERT INTO loans VALUES (DEFAULT, @book_id, @userlibrary_id, @loan_date)";

            using (MySqlCommand Command = new MySqlCommand(sql, connection))
            {
                Command.Parameters.AddWithValue("@book_id", loan.Book_id);
                Command.Parameters.AddWithValue("@userlibrary_id", loan.UserLibrary_id);
                Command.Parameters.AddWithValue("@loan_date", loan.LoanDate);
                Command.ExecuteNonQuery();
            }
        }
    }

    public void DeleteLoan(Loan loan)
    {
        using (MySqlConnection connection = database.GetConnection())
        {
            connection.Open();

            string sql = "DELETE FROM loans WHERE id = @id";

            using (MySqlCommand Command = new MySqlCommand(sql, connection))
            {
                Command.Parameters.AddWithValue("@id", loan.Id);
                Command.ExecuteNonQuery();
            }
        }
    }
}

