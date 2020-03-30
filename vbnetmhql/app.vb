Imports System.IO
Imports MochaDB
Imports MochaDB.Mhql

Public Class app
    Private Sub codebox_KeyDown(sender As Object, e As KeyEventArgs) Handles codebox.KeyDown
        Dim path = New MochaPath(Directory.GetCurrentDirectory)
        path.ParentDirectory()
        path.ParentDirectory()
        path.ParentDirectory()
        Dim db As MochaDatabase = New MochaDatabase($"Path={path.Path}/testdocs/testdb; Password=; AutoConnect=True")

        Try
            If e.KeyCode = Keys.F5 Then
                Dim command As MochaDbCommand = New MochaDbCommand(codebox.Text, db)
                Dim mhqlcommand As MhqlCommand = codebox.Text
                If mhqlcommand.IsReaderCompatible Then
                    Dim result As MochaTableResult = command.ExecuteScalar

                    datasource.Columns.Clear()
                    datasource.Rows.Clear()

                    For index = 0 To result.Columns.Count - 1
                        datasource.Columns.Add("", result.Columns.ElementAt(index).Name)
                    Next
                    For index = 0 To result.Rows.Count - 1
                        datasource.Rows.Add(result.Rows.ElementAt(index).Datas.ToArray)
                    Next
                Else
                    command.ExecuteCommand()
                End If
            End If
        Catch excep As MochaException
            MessageBox.Show(excep.Message, "VB.NET MHQL Test App", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Catch excep As Exception
            MessageBox.Show(excep.Message, "VB.NET MHQL Test App", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        db.Dispose()

    End Sub
End Class
