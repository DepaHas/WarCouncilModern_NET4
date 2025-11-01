using System.IO;
using System.Xml.Linq;

namespace WarCouncilModern.Initialization
{
    public static class InitHelpers
    {
        public static XDocument? LoadConfigXml(string path)
        {
            if (!File.Exists(path)) return null;
            try { return XDocument.Load(path); }
            catch { return null; }
        }

        public static string? ReadElementValue(XDocument doc, string xpath)
        {
            if (doc == null) return null;
            var el = doc.Root.Element(xpath);
            return el?.Value;
        }
    }
}