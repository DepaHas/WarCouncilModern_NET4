using System.Collections.ObjectModel;
using WarCouncilModern.UI.DTOs;

namespace WarCouncilModern.UI.Services
{
    public interface ICouncilUiService
    {
        ReadOnlyObservableCollection<WarCouncilDTO> AllCouncils { get; }
    }
}
