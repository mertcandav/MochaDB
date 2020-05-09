// MIT License
// 
// Copyright (c) 2020 Mertcan Davulcu
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

//Libraries
open System
open System.Drawing
open System.Windows.Forms
open MochaDB
open MochaDB.Mhql

//Fields
let form = new Form()
let codebox = new RichTextBox()
let gridview = new DataGridView()

//Functions

//codebox keydown event.
//e: Key arguments
let codebox_keydown(e: KeyEventArgs) =
    if e.KeyData = Keys.F5 then
        let path = new MochaPath(__SOURCE_DIRECTORY__)
        path.ParentDirectory()
        path.ParentDirectory()
        let database = new MochaDatabase("path=" + path.ToString() + "/testdocs/testdb; autoconnect=true")
        try
        gridview.Columns.Clear()
        let command = new MochaDbCommand(database)
        let mhqlcommand = new MhqlCommand(codebox.Text)
        if mhqlcommand.IsReaderCompatible() then
        let result = command.ExecuteScalar(mhqlcommand.ToString()) :?> MochaTableResult
        let table = System.Data.DataTable()
        for column in result.Columns do
            table.Columns.Add(column.Name)
        for row in result.Rows do
            table.Rows.Add([| for data in row.Datas -> data.Data |])
            gridview.DataSource <- table
        else if mhqlcommand.IsExecuteCompatible() then
            command.ExecuteCommand(mhqlcommand.ToString())
        else
            Console.WriteLine("ERROR: Command is not defined!");
        with
            | :? MochaException as excep ->
                Console.WriteLine("ERROR: " + excep.Message)
            | :? Exception as excep ->
                Console.WriteLine("ERROR: " + excep.Message)
        database.Dispose()


[<EntryPoint>]
let main argv =
    Console.Title <- "F# MHQL Test Console"
    form.Text <- "F# based mhql test application"
    form.ShowIcon <- false
    form.Size <- new Size(1000,500)
    form.StartPosition <- FormStartPosition.CenterScreen
    codebox.BorderStyle <- BorderStyle.None
    codebox.Anchor <- AnchorStyles.Top ||| AnchorStyles.Bottom ||| AnchorStyles.Left ||| AnchorStyles.Right
    codebox.Location <- Point.Empty
    codebox.Size <- new Size(form.Width,form.Height/2)
    codebox.KeyDown.Add codebox_keydown
    form.Controls.Add codebox
    gridview.Anchor <- AnchorStyles.Bottom ||| AnchorStyles.Left ||| AnchorStyles.Right
    gridview.Location <- new Point(0,codebox.Height)
    gridview.Size <- new Size(form.Width,form.Height/2)
    form.Controls.Add gridview
    Application.Run form
    0
