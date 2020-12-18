namespace utils

open System
open System.Xml.Linq

open MochaDB
open MochaDB.Mhql

/// <summary>
/// Parser.
/// </summary>
type parser() =
  /// <summary>
  /// Parse MochaTable to XmlString.
  /// </summary>
  /// <param name="table">Table to parse.</param>
  /// <returns>Xml code.</returns>
  static member parseTableToXmlString(table:MochaTable) : string =
    let doc = XDocument.Parse("<" + table.Name + "></" + table.Name + ">")
    doc.Root.Add(new XAttribute(XName.Get("Description"), table.Description))
    for column in table.Columns do
      let xcol = new XElement(XName.Get(column.Name), String.Empty)
      xcol.Add(new XAttribute(XName.Get("Description"), column.Description))
      xcol.Add(new XAttribute(XName.Get("DataType"), column.DataType))
      for data in column.Datas do
        xcol.Add(new XElement(XName.Get("Data"), data.Data))
      doc.Root.Add(xcol)
    doc.ToString()

  /// <summary>
  /// Parse MochaTableResult to XmlString.
  /// </summary>
  /// <param name="table">Table to parse.</param>
  /// <returns>Xml code.</returns>
  static member parseTableToXmlString(table:MochaTableResult) : string =
    let doc = XDocument.Parse("<Table></Table>")
    for column in table.Columns do
      let xcol = new XElement(XName.Get(column.Name), String.Empty)
      xcol.Add(new XAttribute(XName.Get("Description"), column.Description))
      xcol.Add(new XAttribute(XName.Get("DataType"), column.DataType))
      for data in column.Datas do
        xcol.Add(new XElement(XName.Get("Data"), data.Data))
      doc.Root.Add(xcol)
    doc.ToString()

