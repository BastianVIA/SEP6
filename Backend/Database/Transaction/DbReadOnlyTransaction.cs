namespace Backend.Database.Transaction;

public class DbReadOnlyTransaction
{
    public DataContext DataContext { get;}

    public DbReadOnlyTransaction(DataContext dataContext)
    {
        DataContext = dataContext;
    }
}