using HomeWebApp.Models;
using MudBlazor;

namespace HomeWebApp.Services
{
    public class ExpenseService
    {
        public enum Month
        {
            NotSet = 0,
            January = 1,
            February = 2,
            March = 3,
            April = 4,
            May = 5,
            June = 6,
            July = 7,
            August = 8,
            September = 9,
            October = 10,
            November = 11,
            December = 12
        }

        public Action? OnChenge;

        private static bool _isLoaded = false;
        public bool IsLoaded { get => _isLoaded; }

        private readonly DBService _dbService;

        private static List<ExpenseCategory> _categories = [];
        public List<ExpenseCategory> Categories
        {
            get => _categories;
        }
        private static List<Expense> _expenses = [];
        public List<Expense> Expenses
        {
            get => _expenses;
        }

        private static double[] _donutData = [];
        private static string[] _donutLabels = [];
        public double[] DonutData { get => _donutData; }
        public string[] DonutLabels { get => _donutLabels; }


        public int CurrnetYear { get; set; }
        public Month CurrnetMonth { get; set; }
        public ChartOptions ChartOptions = new();
        public List<List<double>> Totals = [];


        public ExpenseService(DBService dBService)
        {
            _dbService = dBService;
            CurrnetYear = DateTime.Now.Year;
            CurrnetMonth = (Month)DateTime.Now.Month;
        }

        public void CalculateTotals()
        {
            Totals.Clear();
            
            var labelList = new List<string>();
            var colors = new List<string>();

            var totals = new List<double>();
            foreach (var category in Categories)
            {
                labelList.Add(category.Name);
                colors.Add(category.Color);

                totals.Add(0);
            }
            _donutLabels = [.. labelList];
            ChartOptions.ChartPalette = [.. colors];

            Totals.Add(totals); // For Month.NotSet

            for (int i = 1; i <= 12; i++)
            {

                totals = new List<double>();
                foreach (var category in Categories)
                {
                    totals.Add((double)Expenses.Where(e => e.Category.Id == category.Id && e.Date.Year == CurrnetYear && e.Date.Month == i).Sum(e => e.Amount));
                }

                Totals.Add(totals);
            }
            OnChenge?.Invoke();
        }

        public async Task LoadData()
        {
            if (IsLoaded) return;

            _categories = [.. await _dbService.GetExpenseCategories()];
            _expenses = [.. await _dbService.GetExpenses()];

            CalculateTotals();

            _isLoaded = true;
        }

        public async Task Update(Expense expense)
        {
            var index = _expenses.FindIndex(e => e.Id == expense.Id);

            if (index == -1) return; // TODO: Log warning

            _expenses[index] = expense;
            await _dbService.UpdateExpense(expense);
        }

        public async Task<int> Insert(Expense expense)
        {
            if (expense.Id != -1) return -1; // TODO: Log warning

            int id = await _dbService.InsertExpense(expense);
            expense.Id = id;
            _expenses.Add(expense);

            return id;
        }

        public async Task Delete(Expense expense)
        {
            var index = _expenses.FindIndex(e => e.Id == expense.Id);
            if (index == -1) return; // TODO: Log warning

            await _dbService.DeleteExpense(expense);
            _expenses.RemoveAt(index);
        }
    }
}
