namespace MochaDB {
  using System.IO;
  using System.Xml;
  using System.Xml.Linq;

  using MochaDB.Mhql;

  /// <summary>
  /// Converter for MochaDB.
  /// </summary>
  public static class MochaConvert {
    /// <summary>
    /// Returns table as XmlReader.
    /// </summary>
    /// <param name="table">Table to convert.</param>
    public static XmlReader ToXmlTable(MochaTable table) {
      if(table.Columns.Count == 0) {
        string val =
$@"<Root>
    <{table.Name}></{table.Name}>
</Root>";
        return XmlReader.Create(new StringReader(val));
      }

      XDocument doc = XDocument.Parse(
@"<Root>
</Root>");
      if(table.Rows.Count == 0) {
        XElement element = new XElement(table.Name);
        for(int index = 0; index < table.Columns.Count; ++index)
          element.Add(new XElement(table.Columns[index].Name));
        doc.Root.Add(element);
        goto ret;
      }
      for(int dex = 0; dex < table.Rows.Count; ++dex) {
        XElement row = new XElement(table.Name);
        for(int columnIndex = 0; columnIndex < table.Columns.Count; ++columnIndex) {
          MochaColumn column = table.Columns[columnIndex];
          XElement value = new XElement(column.Name);
          value.Value = column.Datas[dex].Data.ToString();
          row.Add(value);
        }
        doc.Root.Add(row);
      }
    ret:
      return doc.CreateReader();
    }

    /// <summary>
    /// Returns table as XmlReader.
    /// </summary>
    /// <param name="table">Table to convert.</param>
    public static XmlReader ToXmlTable(MochaTableResult table) {
      if(table.Columns.Length == 0) {
        string val =
$@"<Root>
    <Table></Table>
</Root>";
        return XmlReader.Create(new StringReader(val));
      }

      XDocument doc = XDocument.Parse(
@"<Root>
</Root>");
      if(table.Rows.Length == 0) {
        XElement element = new XElement("Table");
        for(int index = 0; index < table.Columns.Length; ++index)
          element.Add(new XElement(table.Columns[index].Name));
        doc.Root.Add(element);
        goto ret;
      }
      for(int dex = 0; dex < table.Rows.Length; ++dex) {
        XElement row = new XElement("Table");
        for(int columnIndex = 0; columnIndex < table.Columns.Length; ++columnIndex) {
          MochaColumn column = table.Columns[columnIndex];
          XElement value = new XElement(column.Name);
          value.Value = column.Datas[dex].Data.ToString();
          row.Add(value);
        }
        doc.Root.Add(row);
      }
    ret:
      return doc.CreateReader();
    }
  }
}
