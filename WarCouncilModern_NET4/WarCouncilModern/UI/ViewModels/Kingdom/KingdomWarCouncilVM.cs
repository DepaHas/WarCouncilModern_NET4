using TaleWorlds.Library;
using WarCouncilModern.Initialization;

namespace WarCouncilModern.UI.ViewModels.Kingdom
{
    public class KingdomWarCouncilVM : ViewModel
    {
        public MBBindingCommand OpenWarCouncilCommand { get; }

        public KingdomWarCouncilVM()
        {
            OpenWarCouncilCommand = new MBBindingCommand(ExecuteOpenWarCouncil);
        }

        private void ExecuteOpenWarCouncil()
        {
            // This command will be bound to a button in the XML.
            // When clicked, it will open the main War Council screen.
            SubModule.CouncilUiService?.OpenOverviewScreen();
            SubModule.Logger.Info("OpenWarCouncilCommand executed from Kingdom screen tab.");
        }
    }
}
