namespace MochaDB {
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;
  using System.Text;
  using System.Xml;
  using System.Xml.Linq;

  using MochaDB.Mhql;

  /// <summary>
  /// Converter for MochaDB.
  /// </summary>
  public static class MochaConvert {
    /// <summary>
    /// Returns bytes of string with UTF8 encoding.
    /// </summary>
    /// <param name="value">String value.</param>
    public static byte[] GetStringBytes(string value) =>
        Encoding.UTF8.GetBytes(value);

    /// <summary>
    /// Returns bytes of string with encoding.
    /// </summary>
    /// <param name="value">String value.</param>
    /// <param name="encoding">Encoding.</param>
    public static byte[] GetStringBytes(string value,Encoding encoding) =>
        encoding.GetBytes(value);

    /// <summary>
    /// Returns bytes of string with encoding.
    /// </summary>
    /// <param name="value">String value.</param>
    /// <param name="encoding">Name of encoding.</param>
    public static byte[] GetStringBytes(string value,string encoding) =>
        Encoding.GetEncoding(encoding).GetBytes(value);

    /// <summary>
    /// Returns Base64 string from bytes.
    /// </summary>
    /// <param name="bytes">Bytes.</param>
    public static string ToBase64String(params byte[] bytes) =>
        Convert.ToBase64String(bytes);

    /// <summary>
    /// Returns Base64 string from bytes.
    /// </summary>
    /// <param name="bytes">Bytes.</param>
    public static string ToBase64String(IEnumerable<byte> bytes) =>
        Convert.ToBase64String(bytes.ToArray());

    /// <summary>
    /// Returns Base64 bytes from Base64 string.
    /// </summary>
    /// <param name="value">Bytes.</param>
    public static byte[] FromBase64String(string value) =>
        Convert.FromBase64String(value);

    /// <summary>
    /// Returns value as MochaData.
    /// </summary>
    /// <param name="value">Value to convert.</param>
    public static MochaData ToMochaData(this string value) =>
        (MochaData)value;

    /// <summary>
    /// Returns value as MochaData.
    /// </summary>
    /// <param name="value">Value to convert.</param>
    public static MochaData ToMochaData(this char value) =>
        (MochaData)value;

    /// <summary>
    /// Returns value as MochaData.
    /// </summary>
    /// <param name="value">Value to convert.</param>
    public static MochaData ToMochaData(this int value) =>
        (MochaData)value;

    /// <summary>
    /// Returns value as MochaData.
    /// </summary>
    /// <param name="value">Value to convert.</param>
    public static MochaData ToMochaData(this long value) =>
        (MochaData)value;

    /// <summary>
    /// Returns value as MochaData.
    /// </summary>
    /// <param name="value">Value to convert.</param>
    public static MochaData ToMochaData(this short value) =>
        (MochaData)value;

    /// <summary>
    /// Returns value as MochaData.
    /// </summary>
    /// <param name="value">Value to convert.</param>
    public static MochaData ToMochaData(this uint value) =>
        (MochaData)value;

    /// <summary>
    /// Returns value as MochaData.
    /// </summary>
    /// <param name="value">Value to convert.</param>
    public static MochaData ToMochaData(this ulong value) =>
        (MochaData)value;

    /// <summary>
    /// Returns value as MochaData.
    /// </summary>
    /// <param name="value">Value to convert.</param>
    public static MochaData ToMochaData(this ushort value) =>
        (MochaData)value;

    /// <summary>
    /// Returns value as MochaData.
    /// </summary>
    /// <param name="value">Value to convert.</param>
    public static MochaData ToMochaData(this byte value) =>
        (MochaData)value;

    /// <summary>
    /// Returns value as MochaData.
    /// </summary>
    /// <param name="value">Value to convert.</param>
    public static MochaData ToMochaData(this sbyte value) =>
        (MochaData)value;

    /// <summary>
    /// Returns value as MochaData.
    /// </summary>
    /// <param name="value">Value to convert.</param>
    public static MochaData ToMochaData(this float value) =>
        (MochaData)value;

    /// <summary>
    /// Returns value as MochaData.
    /// </summary>
    /// <param name="value">Value to convert.</param>
    public static MochaData ToMochaData(this decimal value) =>
        (MochaData)value;

    /// <summary>
    /// Returns value as MochaData.
    /// </summary>
    /// <param name="value">Value to convert.</param>
    public static MochaData ToMochaData(this double value) =>
        (MochaData)value;

    /// <summary>
    /// Returns value as MochaData.
    /// </summary>
    /// <param name="value">Value to convert.</param>
    public static MochaData ToMochaData(this bool value) =>
        (MochaData)value;

    /// <summary>
    /// Returns value as MochaData.
    /// </summary>
    /// <param name="value">Value to convert.</param>
    public static MochaData ToMochaData(this DateTime value) =>
        (MochaData)value;

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

      return doc.CreateReader();
    }
  }
}
