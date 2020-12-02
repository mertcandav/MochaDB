namespace MochaDB.engine {
  using System.Xml.Linq;

  using MochaDB.framework;

  /// <summary>
  /// Structure engine of MochaDB.
  /// </summary>
  internal static class Engine_STRUCTURE {
    /// <summary>
    /// Checks the suitability and robustness of the MochaDB database.
    /// </summary>
    /// <param name="doc">XDocument object of database.</param>
    public static bool CheckMochaDB(XDocument doc) {
      try {
        if(doc.Root.Name.LocalName != "MochaDB")
          return false;
        else if(!Framework_XML.ExistsElement(doc,"Root/Password"))
          return false;
        else if(!Framework_XML.ExistsElement(doc,"Root/Description"))
          return false;
        else if(!Framework_XML.ExistsElement(doc,"Tables"))
          return false;
        else if(!Framework_XML.ExistsElement(doc,"Logs"))
          return false;
        else
          return true;
      } catch { return false; }
    }
  }
}