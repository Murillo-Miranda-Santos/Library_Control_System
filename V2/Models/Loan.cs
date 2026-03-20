namespace LibraryControlSystem_2._0;

public class Loan
{
    public int Id { get; set; }
    public int Book_id { get; set; }
    public int UserLibrary_id { get; set; }
    public DateTime LoanDate { get; set; }
}
