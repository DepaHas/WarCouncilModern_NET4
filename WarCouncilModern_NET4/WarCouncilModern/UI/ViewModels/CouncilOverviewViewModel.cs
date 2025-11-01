using System.Collections.ObjectModel;
using System.Linq;
using WarCouncilModern.UI.Enums;
using WarCouncilModern.UI.Services;

namespace WarCouncilModern.UI.ViewModels
{
    public class CouncilOverviewViewModel : ViewModelBase
    {
        private readonly ICouncilUiService _uiService;

        public ObservableCollection<CouncilItemViewModel> Councils { get; } = new ObservableCollection<CouncilItemViewModel>();

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

            // This is a simplified way to populate. A real implementation might use commands.
            PopulateCouncils();
        }

        private void PopulateCouncils()
        {
            Councils.Clear();
            foreach (var dto in _uiService.AllCouncils)
            {
                Councils.Add(new CouncilItemViewModel(dto));
            }

            _uiService.AllCouncils.CollectionChanged += (sender, args) =>
            {
                // Handle additions and removals to keep the ViewModel in sync
                if (args.NewItems != null)
                {
                    foreach (var newItem in args.NewItems.Cast<Dto.WarCouncilDto>())
                    {
                        Councils.Add(new CouncilItemViewModel(newItem));
                    }
                }
                if (args.OldItems != null)
                {
                    foreach (var oldItem in args.OldItems.Cast<Dto.WarCouncilDto>())
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
