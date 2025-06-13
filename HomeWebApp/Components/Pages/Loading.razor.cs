
using HomeWebApp.Services;
using Microsoft.AspNetCore.Components;

namespace HomeWebApp.Components.Pages
{
    public partial class Loading
    {
        private readonly NavigationManager _navigationManager;
        private readonly LoadingService _loadingService;

        public Loading(LoadingService loadingService, NavigationManager navigationManager)
        {
            _loadingService = loadingService;
            _navigationManager = navigationManager;

            _loadingService.OnLoaded += OnLoaded;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await _loadingService.LoadData();
        }

        private void OnLoaded()
        {
            if (_loadingService.IsLoaded)
            {
                StateHasChanged();
                _navigationManager.NavigateTo("/");
            }
        }
    }
}
