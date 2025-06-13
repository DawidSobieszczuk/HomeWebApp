using HomeWebApp.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using MudBlazor.Utilities;

namespace HomeWebApp.Components.Layout
{
    public partial class MainLayout
    {
        private readonly LoadingService _loadingService;
        private readonly NavigationManager _navigationManager;

        MudTheme MyCustomTheme = new MudTheme()
        {
            PaletteDark = new PaletteDark()
            {
                Background = new MudColor(30, 30, 30, 1.0),
                Surface = new MudColor(51, 51, 51, 1.0),

                Primary = new MudColor(91, 153, 194, 1.0),
                Warning = new MudColor(249, 217, 35, 1.0),
                Success = new MudColor(54, 174, 124, 1.0),
                Error = new MudColor(235, 83, 83, 1.0),

                TableStriped = new MudColor(0, 0, 0, 0.15),
                TableHover = new MudColor(0, 0, 0, 0.1),

                AppbarBackground = new MudColor(51, 51, 51, 1.0)
            },
            PaletteLight = new PaletteLight()
            {
                Background = new MudColor(240, 240, 240, 1.0),
                Surface = new MudColor(225, 225, 225, 1.0),

                Primary = new MudColor(91, 153, 194, 1.0),
                Warning = new MudColor(249, 217, 35, 1.0),
                Success = new MudColor(54, 174, 124, 1.0),
                Error = new MudColor(235, 83, 83, 1.0),

                TableStriped = new MudColor(0, 0, 0, 0.1),
                TableHover = new MudColor(0, 0, 0, 0.05),

                AppbarBackground = new MudColor(225, 225, 225, 1.0),
                AppbarText = new MudColor(66, 66, 66, 1.0)
            }
        };

        public MainLayout(LoadingService loadingService, NavigationManager navigationManager)
        {
            _loadingService = loadingService;
            _navigationManager = navigationManager;

            if (!_loadingService.IsLoaded && !_navigationManager.Uri.Contains("loading"))
                _navigationManager.NavigateTo("/loading");
        }
    }
}
