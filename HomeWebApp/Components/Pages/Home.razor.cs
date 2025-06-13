using HomeWebApp.Services;
using Microsoft.AspNetCore.Components;

namespace HomeWebApp.Components.Pages
{
    public partial class Home
    {
        public Home(LoadingService loadingService, NavigationManager navigationManager)
        {
            if(!loadingService.IsLoaded)
                navigationManager.NavigateTo("/loading");
        }
    }
}
