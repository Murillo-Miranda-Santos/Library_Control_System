namespace LibraryControlSystem_2._0;

public class Service
{
    Repository repository = new Repository();

    //BOOK SERVICES
    public Book GetBookByTitle(Book book)
    {
        Book newBook = repository.SelectBook(book);
        return newBook;
    }

    public Book CreateOrUpdateBook(Book book)
    {
        Book newBook = GetBookByTitle(book);

        if (newBook != null)
        {
            newBook.Quantity = book.Quantity;
            repository.UpdateBook(newBook);
        }
        else              
        {
            repository.InsertBook(book);
        }
        return newBook;
    }

    public List<Book> Listbooks()
    {
        List<Book> books = repository.SelectAllBooks();
        return books;
    }


    //USER SERVICES
    public User GetUserByName(User user)
    {
        User newUser = repository.SelectUser(user);
        return newUser;
    }

    public User RegisterUser(User user)
    {
        User newUser = GetUserByName(user);

        if (newUser == null)
        {
            repository.InsertUser(user);
            return user;
        }
        else
        {
            return null;
        }
    }


    //LOAN SERVICES
    public int RegisterLoan(Book book, User user)
    {
        Book newBook = GetBookByTitle(book);
        User newUser = GetUserByName(user);

        if (newBook == null && newUser == null)
        {
            return 1;
        }
        else if (newBook == null)
        {
            return 2;
        }
        else if (newUser == null)
        {
            return 3;
        }
        else 
        {
            Loan loan = new Loan()
            {
                Book_id = newBook.Id,
                UserLibrary_id = newUser.Id,
                LoanDate = DateTime.Now
            };

            Loan newLoan = repository.SelectLoan(loan);

            if (newLoan != null)
            {
                return 4;
            }
            else
            {
                if (newBook.Quantity < 0)
                {
                    return 5;
                }
                else
                {
                    repository.InsertLoan(loan);

                    newBook.Quantity = newBook.Quantity - 1;
                    repository.UpdateBook(newBook);

                    return 6;
                }
            }
        }
    }

    public int ReturnLoan(Book book, User user)
    {
        Book newBook = GetBookByTitle(book);
        User newUser = GetUserByName(user);

        if (newBook == null && newUser == null)
        {
            return 1;
        }
        else if (newBook == null)
        {
            return 2;
        }
        else if (newUser == null)
        {
            return 3;
        }

        Loan loan = new Loan()
        {
            Book_id = newBook.Id,
            UserLibrary_id = newUser.Id
        };

        Loan newLoan = repository.SelectLoan(loan);

        if (newLoan == null)
        {
            return 4;
        }
        else
        {
            newBook.Quantity = newBook.Quantity++;
            repository.UpdateBook(newBook);
            repository.DeleteLoan(newLoan);
            return 5;
        }  
    }

    public (List<Book>, List<User>, List<Loan>) GetFullReport()
    {
        var books = repository.SelectAllBooks();
        var users = repository.SelectAllUsers();
        var loans = repository.SelectAllLoans();

        return (books, users, loans);
    }
}

