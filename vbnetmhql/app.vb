' MIT License
' 
' Copyright(c) 2020 Mertcan Davulcu
' 
' Permission Is hereby granted, free Of charge, to any person obtaining a copy
' of this software And associated documentation files (the "Software"), to deal
' in the Software without restriction, including without limitation the rights
' to use, copy, modify, merge, publish, distribute, sublicense, And/Or sell
' copies of the Software, And to permit persons to whom the Software Is
' furnished to do so, subject to the following conditions
' 
' The above copyright notice And this permission notice shall be included In all
' copies Or substantial portions of the Software.
' 
' THE SOFTWARE Is PROVIDED "AS IS", WITHOUT WARRANTY Of ANY KIND, EXPRESS Or
' IMPLIED, INCLUDING BUT Not LIMITED To THE WARRANTIES Of MERCHANTABILITY,
' FITNESS FOR A PARTICULAR PURPOSE And NONINFRINGEMENT. IN NO EVENT SHALL THE
' AUTHORS Or COPYRIGHT HOLDERS BE LIABLE For ANY CLAIM, DAMAGES Or OTHER
' LIABILITY, WHETHER In AN ACTION Of CONTRACT, TORT Or OTHERWISE, ARISING FROM,
' OUT OF Or IN CONNECTION WITH THE SOFTWARE Or THE USE Or OTHER DEALINGS IN THE
' SOFTWARE.


Imports System.IO
Imports MochaDB
Imports MochaDB.Mhql

Public Class app
  ''' <summary>
  ''' Go previous directory.
  ''' </summary>
  ''' <param name="path">Path.</param>
  ''' <returns>Previous path.</returns>
  Private Function parentPath(path As String) As String
    Dim index As Integer = path.LastIndexOf(IO.Path.DirectorySeparatorChar)
    If index <> -1 Then
      Return path.Substring(0, index)
    Else
      Return path
    End If
  End Function

  ''' <summary>
  ''' Key down event of codebox.
  ''' </summary>
  ''' <param name="sender">Object of event.</param>
  ''' <param name="e">Event arguments.</param>
  Private Sub codebox_KeyDown(sender As Object, e As KeyEventArgs) Handles codebox.KeyDown
    If e.KeyCode = Keys.F5 Then
      Dim db As MochaDatabase = Nothing
      Try
        Dim path As String = Directory.GetCurrentDirectory
        path = parentPath(path)
        path = parentPath(path)
        db = New MochaDatabase($"{path}/tests/testdb", autoConnect:=True)
        Dim command As MochaDbCommand = New MochaDbCommand(codebox.Text, db)
        Dim result As MochaTableResult = command.ExecuteScalar
        datasource.Columns.Clear()
        datasource.Rows.Clear()
        For index = 0 To result.Columns.Count - 1
          datasource.Columns.Add(String.Empty, result.Columns.ElementAt(index).MHQLAsText)
        Next
        For index = 0 To result.Rows.Count - 1
          datasource.Rows.Add(result.Rows.ElementAt(index).Datas.ToArray)
        Next
        db.Dispose()
      Catch excep As Exception
        If db IsNot Nothing Then
          db.Dispose()
        End If
        MessageBox.Show(excep.Message, "VB.NET MHQL Test App", MessageBoxButtons.OK, MessageBoxIcon.Error)
      End Try
    End If
  End Sub
End Class
