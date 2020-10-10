using System.Xml.Linq;

namespace MochaDB.framework {
  /// <summary>
  /// XML framework of MochaDB.
  /// </summary>
  internal static class Framework_XML {
    /// <summary>
    /// Return element by path.
    /// </summary>
    /// <param name="doc">Base XDocument.</param>
    /// <param name="path">Path of element.</param>
    public static XElement GetXElement(XDocument doc,string path) {
      var elementsName = path.Split('/');
      try {
        var element = doc.Root.Element(elementsName[0]);

        if(element==null)
          return null;

        for(var i = 1; i < elementsName.Length; i++) {
          element = element.Element(elementsName[i]);
          if(element == null)
            return null;
        }
        return element;
      } catch { return null; }
    }

    /// <summary>
    /// Checks for the presence of the element.
    /// </summary>
    /// <param name="doc">Base XDocument.</param>
    /// <param name="path">Path of element.</param>
    public static bool ExistsElement(XDocument doc,string path) =>
        GetXElement(doc,path) != null;
  }
}
