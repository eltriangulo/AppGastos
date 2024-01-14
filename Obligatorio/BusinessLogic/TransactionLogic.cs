using Domain;
using Repository;

namespace BusinessLogic;

public class TransactionLogic
{
    private readonly IRepository<Transaction> _repository;

    public TransactionLogic(IRepository<Transaction> transactionRepository)
    {
        _repository = transactionRepository;
    }

    public Transaction AddTransaction(Transaction transaction)
    {
        return _repository.Add(transaction);
    }

    public List<Transaction> GetAllTransactionsFromAccount(string name, int spaceId)
    {
        return _repository.FindAll().Where(x => x.Space.Id == spaceId && x.Account != null && x.Account.Name == name ).ToList();
    }
    
    public List<Transaction> GetAllTransactionsFromCreditCard(string last4, int spaceId)
    {
        return _repository.FindAll().Where(x => x.Space.Id == spaceId && x.CreditCard != null 
            && x.CreditCard.Last4Digits == last4).ToList();
    }

    public List<Transaction> GetAllTransactionsFromCategory(string name, int id)
    {
        return _repository.FindAll().Where(x => x.Category.Name == name && x.Space.Id == id).ToList();
    }

    public List<Transaction> GetAllTransactionsFromSpace(int id)
    {
        return _repository.FindAll().Where(x => x.Space.Id == id).ToList();
    }

    public int AssignTransactionId()
    {
        return _repository.FindAll().Count + 1;
    }

    public Transaction GetTransactionById(int id)
    {
        return _repository.Find(x => x.Id == id);
    }

    public void UpdateTransaction(Transaction transaction)
    {
        _repository.Update(transaction);
    }

    public decimal GetTotalSpentFromCategoryInMonth(string cCategoryName, int sId, DateTime date)
    {
        List<Transaction> transactions = _repository.FindAll().Where(x => x.Category.Name == cCategoryName
            && x.Space.Id == sId && x.Date.Month == date.Month && x.Date.Year == date.Year).ToList();
        decimal total = 0;
        foreach (Transaction transaction in transactions)
        {
            total += transaction.Amount;
        }

        return total;
    }
    

    public List<Category> GetCategoriesWithTransactionsInThatMonth(int spaceId, DateTime date)
    {
        List<Transaction> transactions = _repository.FindAll()
            .Where(x => x.Space.Id == spaceId && x.Date.Month == date.Month && x.Date.Year == date.Year).ToList();
        List<Category> categories = new List<Category>();
        foreach (Transaction transaction in transactions)
        {
            if (!categories.Contains(transaction.Category))
            {
                categories.Add(transaction.Category);
            }
        }

        return categories;
    }

    public decimal GetTotalSpentInMonth(int spaceId, DateTime date)
    {
        List<Transaction> transactions = _repository.FindAll()
            .Where(x => x.Space.Id == spaceId && x.Date.Month == date.Month && x.Date.Year == date.Year).ToList();
        decimal total = 0;
        foreach (Transaction transaction in transactions)
        {
            total += transaction.Amount;
        }

        return total;
    }
    public List<Transaction> GetExpensesFromCreditCard(int spaceId, string last4)
    {
        List<Transaction> transactions = _repository.FindAll()
            .Where(x => x.Space.Id == spaceId && x.Category.Type == "Expenses" && x.CreditCard != null
                        && x.CreditCard.Last4Digits == last4).ToList();
        return transactions;
    }
    public List<Transaction> GetExpensesIntoDeadlineDate(int spaceId, DateTime deadline, string last4)
    {
        List<Transaction> transactions = GetExpensesFromCreditCard(spaceId, last4);
        List<Transaction> transactionsIntoDeadlineDate = new List<Transaction>();
        foreach (Transaction transaction in transactions)
        {
            if (transaction.Date <= deadline && transaction.Date >= deadline.AddDays(-30))
            {
                transactionsIntoDeadlineDate.Add(transaction);
            }
        }
        return transactionsIntoDeadlineDate;
    }
    public Transaction DuplicateTransaction(Transaction transaction)
    {
        Transaction newTransaction = new Transaction{Title = transaction.Title,Date = transaction.Date,
            Amount = transaction.Amount,Currency = transaction.Currency,Category = transaction.Category,Account = transaction.Account,
            Space = transaction.Space,CreditCard = transaction.CreditCard
        };
        AddTransaction(newTransaction);
        return newTransaction;
    }
    
    public List<Transaction> GetAllExpensesFromSpace(int spaceId)
    {
        List<Transaction> transactions = _repository.FindAll()
            .Where(x => x.Space.Id == spaceId && x.Category.Type == "Expenses").ToList();
        return transactions;
    }
    
    public List<Transaction> GetAllExpensesByCategoryFromSpace(int spaceId, string categoryName)
    {
        List<Transaction> transactions = _repository.FindAll()
            .Where(x => x.Space.Id == spaceId && x.Category.Type == "Expenses" && x.Category.Name == categoryName).ToList();
        return transactions;
    }
    
    public List<Transaction> GetAllExpensesByAccountFromSpace(int spaceId, string accountName)
    {
        List<Transaction> transactions = _repository.FindAll()
            .Where(x => x.Space.Id == spaceId && x.Category.Type == "Expenses" 
                                              && x.Account != null && x.Account.Name == accountName).ToList();
        return transactions;
    }

    public List<Transaction> GetAllExpensesIntoDateRange(int spaceId, DateTime firstDate, DateTime secondDate)
    {
        List<Transaction> transactions = _repository.FindAll()
            .Where(x => x.Space.Id == spaceId && x.Category.Type == "Expenses" 
                                              && x.Date >= firstDate && x.Date <= secondDate).ToList();
        return transactions;
    }
  
    public List<Transaction> GetAllExpensesInMonth(int spaceId, DateTime date)
    {
        List<Transaction> transactions = _repository.FindAll()
            .Where(x => x.Space.Id == spaceId && x.Category.Type == "Expenses" 
                                              && x.Date.Month == date.Month && x.Date.Year == date.Year).ToList();
        return transactions;
    }
    public List<int> GetExpensesPerDayInMonth(int spaceId, DateTime date)
    {
        List<Transaction> transactions = GetAllExpensesInMonth(spaceId, date);
        List<int> expensesPerDay = new List<int>();
        for (int i = 1; i <= DateTime.DaysInMonth(date.Year, date.Month); i++)
        {
            int total = 0;
            foreach (Transaction transaction in transactions)
            {
                if (transaction.Date.Day == i)
                {
                    total += (int) transaction.Amount;
                }
            }
            expensesPerDay.Add(total);
        }
        return expensesPerDay;
    }

    public List<int> GetIncomesPerDayInMonth(int spaceId, DateTime date)
    {
        List<Transaction> transactions = GetAllIncomesInMonth(spaceId, date);
        List<int> incomesPerDay = new List<int>();
        for (int i = 1; i <= DateTime.DaysInMonth(date.Year, date.Month); i++)
        {
            int total = 0;
            foreach (Transaction transaction in transactions)
            {
                if (transaction.Date.Day == i)
                {
                    total += (int) transaction.Amount;
                }
            }
            incomesPerDay.Add(total);
        }
        return incomesPerDay;
    }

    public List<Transaction> GetAllIncomesInMonth(int spaceId, DateTime date)
    {
        List<Transaction> transactions = _repository.FindAll()
            .Where(x => x.Space.Id == spaceId && x.Category.Type == "Income" 
                                              && x.Date.Month == date.Month && x.Date.Year == date.Year).ToList();
        return transactions;
    }
    
    public decimal GetTotalSpentFromCategoriesGoal(SpendingGoals goals)
    {
        decimal total = 0;
        foreach (Category category in goals.Categories)
        {
            total += GetTotalSpentFromCategoryInMonth(category.Name, goals.Space.Id, DateTime.Now);
        }
        return total;
    }
}