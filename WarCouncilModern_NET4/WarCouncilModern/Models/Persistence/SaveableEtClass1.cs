using TaleWorlds.SaveSystem;

namespace WarCouncilModern.Models
{
    /// <summary>
    /// صف تجريبي قابل للتوسعة.
    /// </summary>
    public class SaveableEtClass1
    {
        [SaveableField(1)] private int _someValue;
        [SaveableField(2)] private string _someText;

        public SaveableEtClass1()
        {
            _someValue = 0;
            _someText = string.Empty;
        }

        public int SomeValue { get { return _someValue; } set { _someValue = value; } }
        public string SomeText { get { return _someText; } set { _someText = value ?? string.Empty; } }
    }
}