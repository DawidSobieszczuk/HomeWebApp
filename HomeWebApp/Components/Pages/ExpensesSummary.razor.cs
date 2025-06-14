using HomeWebApp.Services;

namespace HomeWebApp.Components.Pages
{
    public partial class ExpensesSummary
    {
        private readonly ExpenseService _expenseService;

        private int Year { get => _expenseService.CurrnetYear; set { _expenseService.CurrnetYear = value; _expenseService.CalculateTotals(); } }
        private int Month { get => (int)_expenseService.CurrnetMonth; set { _expenseService.CurrnetMonth = (ExpenseService.Month)value; } }



        public ExpensesSummary(ExpenseService expenseService)
        {
            _expenseService = expenseService;

            _expenseService.CalculateTotals();
        }


    }
}
