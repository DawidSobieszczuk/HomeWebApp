using HomeWebApp.Services;
using Microsoft.AspNetCore.Components;

namespace HomeWebApp.Components.Pages
{
    public partial class Expense
    {
        public bool IsNotFound { get; private set; }
        public bool IsEditing { get; private set; }
        public bool IsSaving { get; private set; }
        public bool IsNew { get => Id == -1; }

        [Parameter]
        public int Id { get; set; }

        private Models.Expense _current = Models.Expense.Empty;
        public Models.Expense Current
        {
            get => _current;
        }

        private readonly ExpenseService _expenseService;
        private readonly NavigationManager _navigationManager;

        public Expense(ExpenseService expenseService, NavigationManager navigationManager)
        {
            _expenseService = expenseService;
            _navigationManager = navigationManager;
        }

        protected override void OnAfterRender(bool firstRender)
        {
            if(firstRender)
            {
                _current = _expenseService.Expenses.Find(expense => expense.Id == Id) ?? Models.Expense.Empty;
                IsNotFound = _current == null;
                IsEditing = IsNew;
                StateHasChanged();
            }
            base.OnAfterRender(firstRender);
        }

        private void OnEditClick()
        {
            IsEditing = true;
            StateHasChanged();
        }

        private void OnSaveClick()
        {
            IsSaving = true;

            if (IsNew)
            {
                InvokeAsync(async () =>
                {
                    var id = await _expenseService.Insert(Current);
                    IsEditing = false;
                    IsSaving = false;
                    _navigationManager.NavigateTo("/expense/" + id);
                });
            }
            else
            {
                InvokeAsync(async () =>
                {
                    await _expenseService.Update(Current);
                    IsEditing = false;
                    IsSaving = false;
                    StateHasChanged();
                });
            }
        }

        private void OnDeleteClick()
        {
            InvokeAsync(async () => 
            {
                await _expenseService.Delete(Current);
                _navigationManager.NavigateTo("/expenses");
            });
        }
    }
}
