namespace LibraryControlSystem_2._0;

public class Program
{
    public static void Main()
    {
        Service service = new Service();

        string space = "================================";

        //MAIN

        bool running = true;
        while (running)
        {

            int choice = MenuOptions(space);

            switch (choice)
            {
                case 1:

                    BookFlow(service);
                    break;

                case 2:

                    UserFlow(service);
                    break;

                case 3:

                    LoanFlow(service);
                    break;

                case 4:

                    ReturnFlow(service);
                    break;


                case 5:

                    ListFlow(service);
                    break;


                case 6:

                    ReportFlow(service) ;
                    break;


                case 7:

                    FinishProgram(service);
                    running = false;
                    break;
            }
        }
    }

    //METHODS

    //Menu options
    static int MenuOptions(string space)
    {
        Console.WriteLine($"{space}\n     Library Control System     \n{space}");

        Console.WriteLine("Choose an option to continue: " +
            "\n[1] Register or Update book \n[2] Register user \n[3] Loan book " +
            "\n[4] Return book \n[5] List books \n[6] Generate report \n[7] Finish");

        int choice;
        while (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > 7)
        {
            Console.WriteLine("Choose a valid option."); 
        }

        return choice;
    }

    //Case 1
    static void BookFlow(Service service)
    {
        Console.Clear();

        Book book = new Book();

        string message = "Type the title of the book";
        string input = ReadNonEmptyString(message);
        book.Title = input;

        message = "Type the quantity of the book: ";
        int value = ReadInt(message);
        book.Quantity = value;

        Book result = service.CreateOrUpdateBook(book);

        if (result == null)
            Console.WriteLine("Book successfully registered!");
        else
            Console.WriteLine("Book successfully updated!");
    }

    //Case 2
    static void UserFlow(Service service)
    {
        Console.Clear();

        while (true)
        {
            User user = new User();

            string message = "Type the Username: ";
            string imput = ReadNonEmptyString(message);
            user.Username = imput;

            User result = service.RegisterUser(user);

            if (result != null)
                Console.WriteLine("User successfully registered!");
            else
                Console.WriteLine("User already registered!");

            break;
        }  
    }

    //Case 3
    static void LoanFlow(Service service)
    {
        Console.Clear();

        while (true)
        {
            Book book = new Book();
            User user = new User();

            string message = "Type the title of the book";
            string input = ReadNonEmptyString(message);
            book.Title = input;

            message = "Type the Username: ";
            string imput = ReadNonEmptyString(message);
            user.Username = imput;

            int result = service.RegisterLoan(book, user);

            if (result == 1)
                Console.WriteLine("Book and User not registered.");
            else if (result == 2)
                Console.WriteLine("Book not registered.");
            else if (result == 3)
                Console.WriteLine("User not registered.");
            else if (result == 4)
                Console.WriteLine("tris book is already borrowed by this user.");
            else if (result ==5)
                Console.WriteLine("Book out of stock.");
            else
                Console.WriteLine("Book successfully borrowed.");

            break;
        }
    }

    //Case 4
    static void ReturnFlow(Service service)
    {
        Console.Clear();

        Book book = new Book();
        User user = new User();

        string message = "Type the title of the book: ";
        string input = ReadNonEmptyString(message);
        book.Title = input;

        message = "Type the Username: ";
        string imput = ReadNonEmptyString(message);
        user.Username = imput;

        int result = service.ReturnLoan(book, user);

        if (result == 1)
            Console.WriteLine("Book and User not registered.");
        else if (result == 2)
            Console.WriteLine("Book not registered.");
        else if (result == 3)
            Console.WriteLine("User not registered.");
        else if (result == 4)
            Console.WriteLine("Loan not found.");
        else
            Console.WriteLine("Book returned successfully.");
    }

    //Case 5
    static void ListFlow(Service service)
    {
        Console.Clear();

        List<Book> books = service.Listbooks();

        if (books ==  null || books.Count == 0)
        {
            Console.WriteLine("No registered book");
        }
        else
        {
            Console.WriteLine("List of books: \n----------------------------");

            foreach (Book book in books)
            {
                Console.WriteLine($"ID: {book.Id}");
                Console.WriteLine($"Title: {book.Title}");
                Console.WriteLine($"Quantity {book.Quantity}");
                Console.WriteLine("----------------------------");
            }
        }   
    }

    //Case 6
    static void ReportFlow(Service service)
    {
        var (books, users, loans) = service.GetFullReport();

        string path = Path.Combine(Directory.GetCurrentDirectory(), "relatorio.txt");

        using (StreamWriter writer = new StreamWriter(path))
        {
            writer.WriteLine("===== BOOKS =====");
            foreach (var book in books)
            {
                writer.WriteLine($"ID: {book.Id} \nTitle: {book.Title} \nQuantity: {book.Quantity}");
            }

            writer.WriteLine("\n===== USERS =====");
            foreach (var user in users)
            {
                writer.WriteLine($"ID: {user.Id} \nUsername: {user.Username}");
            }

            writer.WriteLine("\n===== LOANS =====");
            foreach (var loan in loans)
            {
                writer.WriteLine($"ID: {loan.Id} \nBookID: {loan.Book_id} \nUserID: {loan.UserLibrary_id} \nDate: {loan.LoanDate}");
            }
        }

        Console.WriteLine("Report generated successfully!");
    }

    //case 7
    static void FinishProgram(Service service)
    {
        Console.WriteLine("  Finishing program...");
        Console.WriteLine("==========================");
    }
  

    //IMPUT METHODS

    //Imput string
    static string ReadNonEmptyString(string message)
    {
        string input;

        do
        {
            Console.WriteLine(message);
            input = Console.ReadLine().Trim();
        }
        while (string.IsNullOrEmpty(input));

        return input;
    }
    //Imput int
    static int ReadInt(string message)
    {
        int value;

        while (true)
        {
            Console.WriteLine(message);

            if (int.TryParse(Console.ReadLine(), out value) && value >= 0)
                return value;

            Console.WriteLine("Invalid number, try again.");
        }
    }
}

