using TaleWorlds.Library;
using WarCouncilModern.Initialization;
using WarCouncilModern.UI.Commands;

namespace WarCouncilModern.UI.ViewModels.Kingdom
{
    public class KingdomWarCouncilVM : ViewModel
    {
        public DelegateCommand OpenWarCouncilCommand { get; }

        public KingdomWarCouncilVM()
        {
            OpenWarCouncilCommand = new DelegateCommand(ExecuteOpenWarCouncil);
        }

        private void ExecuteOpenWarCouncil(object? obj)
        {
            // This command will be bound to a button in the XML.
            // When clicked, it will open the main War Council screen.
            SubModule.CouncilUiService?.OpenOverviewScreen();
            SubModule.Logger.Info("OpenWarCouncilCommand executed from Kingdom screen tab.");
        }
    }
}
