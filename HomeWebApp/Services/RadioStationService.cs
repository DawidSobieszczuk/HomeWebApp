using HomeWebApp.Models;

namespace HomeWebApp.Services
{
    public class RadioStationService
    {
        private static bool _isLoaded = false;
        public bool IsLoaded { get => _isLoaded; }

        private readonly DBService _dbService;

        private static List<RadioStation> _stations = [];
        public List<RadioStation> Stations { get => _stations; }

        public RadioStationService(DBService dBService)
        {
            _dbService = dBService;
        }

        public async Task LoadData()
        {
            if (_isLoaded) return;

            _stations = [.. await _dbService.GetRadioStations()];

            _isLoaded = true;
        }
    }
}
