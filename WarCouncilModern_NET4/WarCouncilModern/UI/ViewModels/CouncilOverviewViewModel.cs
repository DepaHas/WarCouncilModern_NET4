using System.Collections.ObjectModel;
using System.Linq;
using WarCouncilModern.UI.Enums;
using WarCouncilModern.UI.Services;
using WarCouncilModern.UI.Dto;

namespace WarCouncilModern.UI.ViewModels
{
    public class CouncilOverviewViewModel : ViewModelBase
    {
        private readonly ICouncilUiService _uiService;

        public ObservableCollection<CouncilItemViewModel> Councils { get; } = new ObservableCollection<CouncilItemViewModel>();

        private CouncilItemViewModel? _selectedCouncil;
        public CouncilItemViewModel? SelectedCouncil
        {
            get => _selectedCouncil;
            private set
            {
                _selectedCouncil = value;
                OnPropertyChanged();
                // In a real implementation, this would trigger navigation.
            }
        }

        private OperationState _currentOperation;
        public OperationState CurrentOperation
        {
            get => _currentOperation;
            private set
            {
                _currentOperation = value;
                OnPropertyChanged();
            }
        }

        public CouncilOverviewViewModel(ICouncilUiService uiService)
        {
            _uiService = uiService;
            _uiService.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(ICouncilUiService.CurrentOperation))
                {
                    CurrentOperation = _uiService.CurrentOperation;
                }
            };

            PopulateCouncils();
        }

        public void SelectCouncil(CouncilItemViewModel item)
        {
            SelectedCouncil = item;
            Logger?.Info($"Council selected: {item?.Name}");
        }

        private void PopulateCouncils()
        {
            Councils.Clear();
            foreach (var dto in _uiService.AllCouncils)
            {
                Councils.Add(new CouncilItemViewModel(dto, this));
            }

            _uiService.AllCouncils.CollectionChanged += (sender, args) =>
            {
                if (args.NewItems != null)
                {
                    foreach (var newItem in args.NewItems.Cast<WarCouncilDto>())
                    {
                        Councils.Add(new CouncilItemViewModel(newItem, this));
                    }
                }
                if (args.OldItems != null)
                {
                    foreach (var oldItem in args.OldItems.Cast<WarCouncilDto>())
                    {
                        var vmToRemove = Councils.FirstOrDefault(vm => vm.Id == oldItem.SaveId);
                        if (vmToRemove != null)
                        {
                            Councils.Remove(vmToRemove);
                        }
                    }
                }
            };
        }
    }
}
