namespace HomeWebApp.Services
{
    public class LoadingService
    {
        public event Action? OnLoaded;

        private bool _isLoading = false;

        public bool IsLoaded { get { return _expenseService.IsLoaded && _radioStationService.IsLoaded; } }

        private readonly ExpenseService _expenseService;
        private readonly RadioStationService _radioStationService;

        public LoadingService(ExpenseService expenseService, RadioStationService radioStationService)
        {
            _expenseService = expenseService;
            _radioStationService = radioStationService;
        }

        public async Task LoadData()
        {
            if (_isLoading) return;
            _isLoading = true;

            if (!_expenseService.IsLoaded)
            {
                await _expenseService.LoadData();
                await _radioStationService.LoadData();
            }

            _isLoading = false;
            OnLoaded?.Invoke();
        }
    }
}
