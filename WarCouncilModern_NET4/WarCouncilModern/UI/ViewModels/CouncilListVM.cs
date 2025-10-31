using System.Collections.ObjectModel;
using System.ComponentModel;
using System;
using WarCouncilModern.UI.Dto;
using WarCouncilModern.UI.Services;
using WarCouncilModern.UI.Platform;

namespace WarCouncilModern.UI.ViewModels
{
    public class CouncilListVM : INotifyPropertyChanged, IDisposable
    {
        private readonly ICouncilUiService _uiService;

        public ObservableCollection<WarCouncilDto> Councils => _uiService.AllCouncils;
        public bool IsLoading => _uiService.IsLoading;

        public CouncilListVM(ICouncilUiService uiService)
        {
            _uiService = uiService;
            _uiService.PropertyChanged += UiServiceOnPropertyChanged;
        }

        private void UiServiceOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_uiService.IsLoading))
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsLoading)));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void Dispose()
        {
            _uiService.PropertyChanged -= UiServiceOnPropertyChanged;
        }
    }
}
