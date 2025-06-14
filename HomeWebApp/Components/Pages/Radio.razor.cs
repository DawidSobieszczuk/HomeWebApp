using HomeWebApp.Services;
using Microsoft.JSInterop;

namespace HomeWebApp.Components.Pages
{
    public partial class Radio
    {
        private int _currentId = -1;

        private bool _isPlaying = false;

        private readonly RadioStationService _radioStationService;
        private readonly IJSRuntime _jsRuntime;

        public Radio(RadioStationService radioStationService, IJSRuntime jSRuntime)
        {
            _radioStationService = radioStationService;
            _jsRuntime = jSRuntime;
        }

        private void OnStationClick(int stationId)
        {
            _currentId = stationId;
            var current = _radioStationService.Stations.Find(r => r.Id == stationId);

            if (current == null) return;

            _jsRuntime.InvokeVoidAsync("myApp.newAudioSource", current.Url);

            _jsRuntime.InvokeVoidAsync("myApp.playAudio");
            _isPlaying = true;
        }

        private void OnPlayClick(int stationId)
        {
            if (stationId != _currentId)
            {
                OnStationClick(stationId);
                _isPlaying = !_isPlaying;
            }

            if (!_isPlaying)
            {
                _jsRuntime.InvokeVoidAsync("myApp.playAudio");
            }
            else
            {
                _jsRuntime.InvokeVoidAsync("myApp.pauseAudio");
            }

            _isPlaying = !_isPlaying;
        }
    }
}
