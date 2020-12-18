namespace utils

open System
open System.Xml.Linq

open MochaDB
open MochaDB.Mhql

// Parser.
type parser() =
  // Parser MochaTable to XmlString
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

  // Parser MochaTableResult to XmlString
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

