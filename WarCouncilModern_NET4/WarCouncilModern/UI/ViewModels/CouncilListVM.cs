using System.Collections.ObjectModel;
using WarCouncilModern.UI.DTOs;
using WarCouncilModern.UI.Services;

namespace WarCouncilModern.UI.ViewModels
{
    public class CouncilListVM
    {
        private readonly ICouncilUiService _councilUiService;

        public ReadOnlyObservableCollection<WarCouncilDTO> AllCouncils => _councilUiService.AllCouncils;

        public CouncilListVM(ICouncilUiService councilUiService)
        {
            _councilUiService = councilUiService;
        }
    }
}
