﻿// Copyright 2020 The MochaDB Authors.
// Use of this source code is governed by a MIT
// license that can be found in the LICENSE file.

// Libraries
open System
open System.IO
open System.Drawing
open System.Windows.Forms
open MochaDB
open MochaDB.Mhql

// Fields

/// <summary>
/// Main form of application.
/// </summary>
let form:Form = new Form()

/// <summary>
/// Editor area for mhql command.
/// </summary>
let codebox:RichTextBox = new RichTextBox()

/// <summary>
/// DataGrid for show tables.
/// </summary>
let gridview:DataGridView = new DataGridView()


// Functions

/// <summary>
/// Go previous directory.
/// </summary>
/// <param name="path">Path.</param>
/// <returns>Previous path.</returns>
let parentPath(path:string) : string =
  let dex:int = path.LastIndexOf(Path.DirectorySeparatorChar)
  if dex <> -1 then path.Substring(0, dex) else path

/// <summary>
/// Keywdown event of codebox.
/// </summary>
/// <param name="e">Key arguments.</param>
let codebox_keydown(e: KeyEventArgs) : unit =
  if e.KeyData = Keys.F5 then
    let mutable path:string = __SOURCE_DIRECTORY__
    path <- parentPath(path)
    path <- parentPath(path)
    let database : MochaDatabase = new MochaDatabase(path = path + "/tests/testdb", autoConnect = true)
    try
      gridview.Columns.Clear()
      let command:MochaDbCommand = new MochaDbCommand(database)
      let result:MochaTableResult = command.ExecuteScalar(codebox.Text) :?> MochaTableResult
      let table:System.Data.DataTable = System.Data.DataTable()
      for column in result.Columns do
        table.Columns.Add(column.Name)
      for row in result.Rows do
        table.Rows.Add([| for data in row.Datas -> data.Data |])
        gridview.DataSource <- table
    with
    | :? Exception as excep ->
      printfn "ERROR: %s" excep.Message
      database.Dispose()

/// <summary>
/// Entry point.
/// </summary>
/// <param name="argv">Arguments.</param>
/// <returns>Exit code.</returns>
[<EntryPoint>]
let main(argv:string[]) : int =
  Console.Title <- "F# MHQL Test Console"
  form.Text <- "F# based mhql test application"
  form.ShowIcon <- false
  form.Size <- new Size(1000,500)
  form.StartPosition <- FormStartPosition.CenterScreen
  codebox.BorderStyle <- BorderStyle.None
  codebox.Anchor <- AnchorStyles.Top ||| AnchorStyles.Bottom ||| AnchorStyles.Left ||| AnchorStyles.Right
  codebox.Location <- Point.Empty
  codebox.Size <- new Size(form.Width,form.Height/2)
  codebox.KeyDown.Add(codebox_keydown)
  form.Controls.Add(codebox)
  gridview.Anchor <- AnchorStyles.Bottom ||| AnchorStyles.Left ||| AnchorStyles.Right
  gridview.Location <- new Point(0,codebox.Height)
  gridview.Size <- new Size(form.Width,form.Height/2)
  form.Controls.Add(gridview)
  Application.Run(form)
  0
