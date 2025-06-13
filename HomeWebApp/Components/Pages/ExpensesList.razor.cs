using HomeWebApp.Services;

namespace HomeWebApp.Components.Pages
{
    public partial class ExpensesList
    {
        private readonly ExpenseService _expenseService;

        public ExpensesList(ExpenseService expenseService)
        {
            _expenseService = expenseService;
        }
    }
}
