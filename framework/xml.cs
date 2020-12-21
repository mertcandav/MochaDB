namespace MochaDB.framework {
  using System.Xml.Linq;

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
      string[] elementsName = path.Split('/');
      XElement element = doc.Root.Element(elementsName[0]);

      if(element==null)
        return null;

      for(int i = 1; i < elementsName.Length; ++i) {
        element = element.Element(elementsName[i]);
        if(element == null)
          return null;
      }
      return element;
    }
  }
}
