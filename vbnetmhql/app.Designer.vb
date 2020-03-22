<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class app
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.codebox = New System.Windows.Forms.RichTextBox()
        Me.datasource = New System.Windows.Forms.DataGridView()
        CType(Me.datasource, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'codebox
        '
        Me.codebox.AcceptsTab = True
        Me.codebox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.codebox.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.codebox.Font = New System.Drawing.Font("Consolas", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(162, Byte))
        Me.codebox.Location = New System.Drawing.Point(0, 0)
        Me.codebox.Name = "codebox"
        Me.codebox.Size = New System.Drawing.Size(800, 211)
        Me.codebox.TabIndex = 0
        Me.codebox.Text = ""
        '
        'datasource
        '
        Me.datasource.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.datasource.Location = New System.Drawing.Point(0, 211)
        Me.datasource.Name = "datasource"
        Me.datasource.Size = New System.Drawing.Size(800, 239)
        Me.datasource.TabIndex = 1
        '
        'app
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.ControlDarkDark
        Me.ClientSize = New System.Drawing.Size(800, 450)
        Me.Controls.Add(Me.datasource)
        Me.Controls.Add(Me.codebox)
        Me.Name = "app"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "MHQL Test App"
        CType(Me.datasource, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents codebox As RichTextBox
    Friend WithEvents datasource As DataGridView
End Class
