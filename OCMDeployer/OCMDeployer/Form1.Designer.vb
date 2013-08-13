<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
		Me.lblFilesInProgress = New System.Windows.Forms.Label()
		Me.deploymentFilesSelectedLabel = New System.Windows.Forms.Label()
		Me.Label9 = New System.Windows.Forms.Label()
		Me.Label8 = New System.Windows.Forms.Label()
		Me.btnTransfer = New System.Windows.Forms.Button()
		Me.progressBar1 = New System.Windows.Forms.ProgressBar()
		Me.overallProgressBar = New System.Windows.Forms.ProgressBar()
		Me.btnSelectHtmlFilesFolder = New System.Windows.Forms.Button()
		Me.Label1 = New System.Windows.Forms.Label()
		Me.rtbMessage = New System.Windows.Forms.RichTextBox()
		Me.txtHtmlFilesFolder = New System.Windows.Forms.TextBox()
		Me.btnSelectAssembliesFolder = New System.Windows.Forms.Button()
		Me.Label2 = New System.Windows.Forms.Label()
		Me.txtAssemblyFilesFolder = New System.Windows.Forms.TextBox()
		Me.Label4 = New System.Windows.Forms.Label()
		Me.BackgroundWorker1 = New System.ComponentModel.BackgroundWorker()
		Me.lblHtmlFilesTotal = New System.Windows.Forms.Label()
		Me.SuspendLayout()
		'
		'lblFilesInProgress
		'
		Me.lblFilesInProgress.AutoSize = True
		Me.lblFilesInProgress.Location = New System.Drawing.Point(350, 201)
		Me.lblFilesInProgress.Name = "lblFilesInProgress"
		Me.lblFilesInProgress.Size = New System.Drawing.Size(115, 13)
		Me.lblFilesInProgress.TabIndex = 56
		Me.lblFilesInProgress.Text = "About to transfer files..."
		Me.lblFilesInProgress.Visible = False
		'
		'deploymentFilesSelectedLabel
		'
		Me.deploymentFilesSelectedLabel.AutoSize = True
		Me.deploymentFilesSelectedLabel.Location = New System.Drawing.Point(142, 189)
		Me.deploymentFilesSelectedLabel.Name = "deploymentFilesSelectedLabel"
		Me.deploymentFilesSelectedLabel.Size = New System.Drawing.Size(141, 13)
		Me.deploymentFilesSelectedLabel.TabIndex = 55
		Me.deploymentFilesSelectedLabel.Text = "number of DLL files selected"
		'
		'Label9
		'
		Me.Label9.AutoSize = True
		Me.Label9.Location = New System.Drawing.Point(240, 258)
		Me.Label9.Name = "Label9"
		Me.Label9.Size = New System.Drawing.Size(126, 13)
		Me.Label9.TabIndex = 53
		Me.Label9.Text = "Overall Transfer Progress"
		'
		'Label8
		'
		Me.Label8.AutoSize = True
		Me.Label8.Location = New System.Drawing.Point(3, 258)
		Me.Label8.Name = "Label8"
		Me.Label8.Size = New System.Drawing.Size(107, 13)
		Me.Label8.TabIndex = 52
		Me.Label8.Text = "Deployment Progress"
		'
		'btnTransfer
		'
		Me.btnTransfer.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.btnTransfer.Location = New System.Drawing.Point(6, 189)
		Me.btnTransfer.Name = "btnTransfer"
		Me.btnTransfer.Size = New System.Drawing.Size(130, 36)
		Me.btnTransfer.TabIndex = 51
		Me.btnTransfer.Text = "Deploy"
		Me.btnTransfer.UseVisualStyleBackColor = True
		'
		'progressBar1
		'
		Me.progressBar1.Location = New System.Drawing.Point(6, 274)
		Me.progressBar1.Name = "progressBar1"
		Me.progressBar1.Size = New System.Drawing.Size(217, 23)
		Me.progressBar1.TabIndex = 50
		'
		'overallProgressBar
		'
		Me.overallProgressBar.Location = New System.Drawing.Point(243, 274)
		Me.overallProgressBar.Name = "overallProgressBar"
		Me.overallProgressBar.Size = New System.Drawing.Size(298, 23)
		Me.overallProgressBar.TabIndex = 49
		'
		'btnSelectHtmlFilesFolder
		'
		Me.btnSelectHtmlFilesFolder.Location = New System.Drawing.Point(547, 142)
		Me.btnSelectHtmlFilesFolder.Name = "btnSelectHtmlFilesFolder"
		Me.btnSelectHtmlFilesFolder.Size = New System.Drawing.Size(24, 22)
		Me.btnSelectHtmlFilesFolder.TabIndex = 39
		Me.btnSelectHtmlFilesFolder.Text = "..."
		Me.btnSelectHtmlFilesFolder.UseVisualStyleBackColor = True
		'
		'Label1
		'
		Me.Label1.AutoSize = True
		Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Label1.Location = New System.Drawing.Point(6, 128)
		Me.Label1.Name = "Label1"
		Me.Label1.Size = New System.Drawing.Size(114, 13)
		Me.Label1.TabIndex = 38
		Me.Label1.Text = "HTML Files Folder:"
		'
		'rtbMessage
		'
		Me.rtbMessage.BackColor = System.Drawing.SystemColors.Control
		Me.rtbMessage.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.rtbMessage.ForeColor = System.Drawing.Color.Navy
		Me.rtbMessage.Location = New System.Drawing.Point(6, 309)
		Me.rtbMessage.Name = "rtbMessage"
		Me.rtbMessage.ReadOnly = True
		Me.rtbMessage.Size = New System.Drawing.Size(534, 146)
		Me.rtbMessage.TabIndex = 37
		Me.rtbMessage.Text = ""
		'
		'txtHtmlFilesFolder
		'
		Me.txtHtmlFilesFolder.Location = New System.Drawing.Point(7, 144)
		Me.txtHtmlFilesFolder.Name = "txtHtmlFilesFolder"
		Me.txtHtmlFilesFolder.Size = New System.Drawing.Size(534, 20)
		Me.txtHtmlFilesFolder.TabIndex = 35
		'
		'btnSelectAssembliesFolder
		'
		Me.btnSelectAssembliesFolder.Location = New System.Drawing.Point(546, 96)
		Me.btnSelectAssembliesFolder.Name = "btnSelectAssembliesFolder"
		Me.btnSelectAssembliesFolder.Size = New System.Drawing.Size(24, 22)
		Me.btnSelectAssembliesFolder.TabIndex = 59
		Me.btnSelectAssembliesFolder.Text = "..."
		Me.btnSelectAssembliesFolder.UseVisualStyleBackColor = True
		'
		'Label2
		'
		Me.Label2.AutoSize = True
		Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Label2.Location = New System.Drawing.Point(5, 82)
		Me.Label2.Name = "Label2"
		Me.Label2.Size = New System.Drawing.Size(135, 13)
		Me.Label2.TabIndex = 58
		Me.Label2.Text = "Assembly DLLs Folder:"
		'
		'txtAssemblyFilesFolder
		'
		Me.txtAssemblyFilesFolder.Location = New System.Drawing.Point(6, 98)
		Me.txtAssemblyFilesFolder.Name = "txtAssemblyFilesFolder"
		Me.txtAssemblyFilesFolder.Size = New System.Drawing.Size(534, 20)
		Me.txtAssemblyFilesFolder.TabIndex = 57
		'
		'Label4
		'
		Me.Label4.AutoSize = True
		Me.Label4.Location = New System.Drawing.Point(13, 13)
		Me.Label4.Name = "Label4"
		Me.Label4.Size = New System.Drawing.Size(537, 52)
		Me.Label4.TabIndex = 63
		Me.Label4.Text = resources.GetString("Label4.Text")
		'
		'BackgroundWorker1
		'
		Me.BackgroundWorker1.WorkerReportsProgress = True
		Me.BackgroundWorker1.WorkerSupportsCancellation = True
		'
		'lblHtmlFilesTotal
		'
		Me.lblHtmlFilesTotal.AutoSize = True
		Me.lblHtmlFilesTotal.Location = New System.Drawing.Point(142, 212)
		Me.lblHtmlFilesTotal.Name = "lblHtmlFilesTotal"
		Me.lblHtmlFilesTotal.Size = New System.Drawing.Size(151, 13)
		Me.lblHtmlFilesTotal.TabIndex = 64
		Me.lblHtmlFilesTotal.Text = "number of HTML files selected"
		'
		'Form1
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(576, 462)
		Me.Controls.Add(Me.lblHtmlFilesTotal)
		Me.Controls.Add(Me.Label4)
		Me.Controls.Add(Me.btnSelectAssembliesFolder)
		Me.Controls.Add(Me.Label2)
		Me.Controls.Add(Me.txtAssemblyFilesFolder)
		Me.Controls.Add(Me.lblFilesInProgress)
		Me.Controls.Add(Me.deploymentFilesSelectedLabel)
		Me.Controls.Add(Me.Label9)
		Me.Controls.Add(Me.Label8)
		Me.Controls.Add(Me.btnTransfer)
		Me.Controls.Add(Me.progressBar1)
		Me.Controls.Add(Me.overallProgressBar)
		Me.Controls.Add(Me.btnSelectHtmlFilesFolder)
		Me.Controls.Add(Me.Label1)
		Me.Controls.Add(Me.rtbMessage)
		Me.Controls.Add(Me.txtHtmlFilesFolder)
		Me.Name = "Form1"
		Me.Text = "Deploy to DEV"
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub
	Friend WithEvents lblFilesInProgress As System.Windows.Forms.Label
	Friend WithEvents deploymentFilesSelectedLabel As System.Windows.Forms.Label
	Friend WithEvents Label9 As System.Windows.Forms.Label
	Friend WithEvents Label8 As System.Windows.Forms.Label
	Friend WithEvents btnTransfer As System.Windows.Forms.Button
	Friend WithEvents progressBar1 As System.Windows.Forms.ProgressBar
	Friend WithEvents overallProgressBar As System.Windows.Forms.ProgressBar
	Friend WithEvents btnSelectHtmlFilesFolder As System.Windows.Forms.Button
	Friend WithEvents Label1 As System.Windows.Forms.Label
	Friend WithEvents rtbMessage As System.Windows.Forms.RichTextBox
	Friend WithEvents txtHtmlFilesFolder As System.Windows.Forms.TextBox
	Friend WithEvents btnSelectAssembliesFolder As System.Windows.Forms.Button
	Friend WithEvents Label2 As System.Windows.Forms.Label
	Friend WithEvents txtAssemblyFilesFolder As System.Windows.Forms.TextBox
	Friend WithEvents Label4 As System.Windows.Forms.Label
	Friend WithEvents BackgroundWorker1 As System.ComponentModel.BackgroundWorker
	Friend WithEvents lblHtmlFilesTotal As System.Windows.Forms.Label

End Class
