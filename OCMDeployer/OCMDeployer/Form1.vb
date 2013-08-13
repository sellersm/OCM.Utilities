Imports System.Drawing.Drawing2D
'Imports System.Drawing.Bitmap
'Imports System.Windows.Media.Imaging
Imports System.Configuration
Imports System.Collections.Specialized
Imports System.IO
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Reflection
Imports SFTP_Test
Imports System.ComponentModel

Public Class Form1

	Private _sftpClient As New OcmSFTPClient()

	' background worker for GUI:
	Private _Worker As New BackgroundWorker()

	' holds the SFTP specific items:
	' ************************************************************************************************************
	' These will need to be moved to the App.Config file before rollout!!!
	Private _host As String = String.Empty '"files.blackbaudhosting.com"
	Private _username As String = String.Empty '"MarkSe21195D@BBEC"
	Private _password As String = "P@ssword2"
	Private _workingdirectory As String = String.Empty ' "/staging/21195D/ChildPhotos"
	Private _uploadfile As String = String.Empty ' "C:\OCM\PhotoImports\2013 Photos_Processing_6_25_2013\_Import\C210608.jpg"
	Private _ftpHtmlFileLocation As String = String.Empty
	Private _numberOfFiles As Integer = 0
	Private _ftpAssemblyFileLocation As String = String.Empty
	'Private _ftpUIModelAssemblyFileLocation As String = String.Empty

	Private _pictureType As String = String.Empty
	Private _defaultSourceFolder As String = String.Empty

	Private _ftpFolderAlreadyFormatted As Boolean


	' holds the files to be transferred:
	Private _assemblyFiles As String() = {}
	Private _htmlFiles As String() = {}
	Private _numberAssemblyFiles As Integer = 0
	Private _numberHtmlFiles As Integer = 0

	Private Const _messageBoxTitle As String = "Deployment Utility"


	Private Sub btnTransfer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTransfer.Click
		btnTransfer.Enabled = False
		lblFilesInProgress.Visible = True

		'first transfer the DLL Assembly files:
		'_assemblyFiles = GetAssemblyFiles()   'moved to leave event of folder text box
		'_htmlFiles = GetHtmlFiles()			'moved to leave event of folder text box

		_numberOfFiles = _numberAssemblyFiles + _numberHtmlFiles '_assemblyFiles.Count + _htmlFiles.Count

		If _numberOfFiles <= 0 Then
			MessageBox.Show("No files have been found to deploy!", _messageBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
		Else
			'begin the transfer process with assembly files first:
			If _numberAssemblyFiles > 0 Then
				TransferFiles(_assemblyFiles, _ftpAssemblyFileLocation)
			End If
			If _numberHtmlFiles > 0 Then
				TransferFiles(_htmlFiles, _ftpHtmlFileLocation)
			End If
			Me.overallProgressBar.Value = 100
			MessageBox.Show("Finished deploying the files to DEV.  Any Packages can be loaded after about 15 minutes...", _messageBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Information)
		End If

			'BackgroundWorker1.RunWorkerAsync(_sftpClient)
	End Sub

	Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		_ftpFolderAlreadyFormatted = False

		progressBar1.Maximum = 100
		progressBar1.Style = ProgressBarStyle.Blocks
		progressBar1.[Step] = 1
		progressBar1.Value = 0
		' user can't transfer until they select everything else & process the folder:
		'btnTransfer.Enabled = False

		GetConfigEntries()

		AddHandler _sftpClient.FileUploadCompleted, AddressOf fileUploadCompleted
		AddHandler _sftpClient.FileUploading, AddressOf fileUploading
	End Sub


	Private Sub GetConfigEntries()
		'sets the Photo FTP foldername using the App.config settings entries plus the type of picture title the user selected:
		' the DEVworkingdirectory = /staging/21195D/ChildPhotos
		' the PRODworkingdirectory = /Production/21195P/ChildPhotos
		' the PRODPhotoImportFileLocation = "\\bbhcifs01\clientdata\Production\21195P\ChildPhotos"
		' the DEVhotoImportFileLocation = "\\bbhcifs01\clientdata\staging\21195D\ChildPhotos"
		'Const _host As String = "files.blackbaudhosting.com"
		'Const _username As String = "MarkSe21195D@BBEC"
		'Const _password As String = "P@ssword2"
		'Const _workingdirectory As String = "/staging/21195D/ChildPhotos"
		'Const _uploadfile As String = "C:\OCM\PhotoImports\2013 Photos_Processing_6_25_2013\_Import\C210608.jpg"
		'Source Folder default location: C:\OCM\PhotoImports

		' This will use an account specially purposed for these SFTP transfers:  OcMFTPUser21195D@BBEC and OcMFTPUser21195P@BBEC

		'_ftpUIModelAssemblyFileLocation = My.Settings.FTPUIModelAssembliesLocation
		_ftpAssemblyFileLocation = My.Settings.FTPAssembliesLocation
		_ftpHtmlFileLocation = My.Settings.FTPHTMLFilesLocation
		_username = My.Settings.FTPUserName
		_host = My.Settings.FTPHostName
		_workingdirectory = My.Settings.FTPRootFolderLocation
		_password = "P@ssword1"

	End Sub

#Region "EventHandlers"
	Private Sub fileUploadCompleted(ByVal sender As Object, ByVal e As FileUploadCompletedEventArgs)
		OutputThis("Finished uploading the file!")
		OutputThis(String.Format("Uploaded {0} {1} {2:#########}", e.UploadedFileName, e.FileNumber, e.BytesSent))
	End Sub

	Private Sub fileUploading(ByVal sender As Object, ByVal e As FileUploadingEventArgs)
		'var backgrounder = sender as BackgroundWorker;
		Dim progress As Integer = 100
		'progress = (Convert.ToInt32(e.BytesSentSoFar) / Convert.ToInt32(e.FileBytesTotal)) * 100;
		progress = CInt(Math.Truncate((CSng(e.BytesSentSoFar) / CSng(e.FileBytesTotal)) * 100))

		LogThis(String.Format("Bytes sent so far: {0}", e.BytesSentSoFar))
		LogThis(String.Format("File Bytes Total: {0}", e.FileBytesTotal))
		LogThis(String.Format("Progress %{0}", progress))
		LogThis(String.Format("File {0} of {1}", e.FileNumberOfTotal, e.TotalNumberFilesToTransfer))

		SetControlPropertyValue(lblFilesInProgress, "text", String.Format("Transferring {0} of {1} files...", e.FileNumberOfTotal, e.TotalNumberFilesToTransfer))

		BackgroundWorker1.ReportProgress(progress)
	End Sub

#End Region


	Private Sub OutputThis(ByVal outputMessage As String)
		'richTextBox1.AppendText(outputMessage)
		rtbMessage.AppendText(vbNewLine + outputMessage)
		LogThis(outputMessage)
	End Sub

	Private Sub LogThis(ByVal logMessage As String)
		System.Diagnostics.Debug.WriteLine(logMessage)
	End Sub

#Region "BackgroundWorker Methods"
	''' <summary>
	''' Creates the BackgroundWorker with WorkerReportsProgress and 
	''' WorkerSupportsCancellation set to true. 
	''' Also creates the three Eventhandlers
	''' </summary>
	Public Sub CreateBackgroundWorker()
		'Set WorkerReportsProgress and WorkerSupportsCancellation to true
		_Worker.WorkerSupportsCancellation = True
		_Worker.WorkerReportsProgress = True

		'Generate the EventHandler
		AddHandler _Worker.RunWorkerCompleted, New RunWorkerCompletedEventHandler(AddressOf Worker_RunWorkerCompleted)
		AddHandler _Worker.ProgressChanged, New ProgressChangedEventHandler(AddressOf Worker_ProgressChanged)
		AddHandler _Worker.DoWork, New DoWorkEventHandler(AddressOf Worker_DoWork)

		_Worker.RunWorkerAsync()
	End Sub

	Private Sub Worker_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs) Handles BackgroundWorker1.DoWork
		' wire up the event listeners to catch the events raised by the client:
		AddHandler _sftpClient.FileUploadCompleted, AddressOf fileUploadCompleted
		AddHandler _sftpClient.FileUploading, AddressOf fileUploading
		'AddHandler _sftpClient.ListingDirectory, AddressOf listingDirectory

		' setup the transfer properties needed by the client:
		_sftpClient.HostName = _host
		_sftpClient.UserName = _username
		_sftpClient.Password = _password
		_sftpClient.WorkingDirectory = _workingdirectory
		_sftpClient.UploadFileName = _uploadfile
		_sftpClient.PhotoFTPFolderName = _ftpHtmlFileLocation  'FormatFTPFolderName()

		' perform the transfer of a single file:
		'_sftpClient.transferFile();

		' multiple files get this call:
		_sftpClient.transferFiles(_assemblyFiles)
	End Sub

	Private Sub Worker_ProgressChanged(ByVal sender As Object, ByVal e As ProgressChangedEventArgs) Handles BackgroundWorker1.ProgressChanged
		'Create an instace of the single file progressBar
		Dim ProgressBarSingle As ProgressBar = DirectCast(Me.Controls("progressBar1"), ProgressBar)

		'Increase the value of the progressBar by e.ProgressPercentage
		ProgressBarSingle.Value = e.ProgressPercentage

		Dim ProgressBarOverall As ProgressBar = DirectCast(Me.Controls("overallProgressBar"), ProgressBar)

		'Calculate by whic the Overall ProgressBar needs to be increased,
		'so that we get a smooth rise.
		Dim PercentRise As Integer = 100 \ _numberOfFiles

		' only increment the overall bar if this single file is done
		If ProgressBarSingle.Value = 100 Then
			'Duno why it has to be here, but if it isn't the progressBarSingle
			'doesn't stop working. So leave it here with no code...
			If ProgressBarOverall.Value >= 100 Then
			Else
				'Set the SingleFile progressBar to 0, so it cann be filled again
				'ProgressBarSingle.Value = 0;

				'This is the "catch" part, so the programm doesn't throw up an error
				If ProgressBarOverall.Value + PercentRise >= 100 Then
					'Set both progress bars to 100 so it looks better.
					ProgressBarOverall.Value = 100
					ProgressBarSingle.Value = 100
				Else
					'increase the Global progressBar by the calculated amount for each file
					ProgressBarOverall.Value = ProgressBarOverall.Value + PercentRise
				End If
			End If
		End If
	End Sub

	Private Sub Worker_RunWorkerCompleted(ByVal sender As Object, ByVal e As RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted
		Dim ProgressBarSingle As ProgressBar = DirectCast(Me.Controls("progressBar1"), ProgressBar)
		ProgressBarSingle.Value = 100

		SetControlPropertyValue(overallProgressBar, "value", 100)

		SetControlPropertyValue(btnTransfer, "enabled", True)

		SetControlPropertyValue(lblFilesInProgress, "text", "All files transferred!")

		MessageBox.Show("Image files have been transferred. You may now close this application...", _messageBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Information)
	End Sub
#End Region



	''' <summary>
	''' These are used to manage the UI controls from the worker thread
	''' </summary>
	''' <param name="oControl"></param>
	''' <param name="propName"></param>
	''' <param name="propValue"></param>
	Private Delegate Sub SetControlValueCallback(ByVal oControl As Control, ByVal propName As String, ByVal propValue As Object)
	Private Sub SetControlPropertyValue(ByVal oControl As Control, ByVal propName As String, ByVal propValue As Object)
		If oControl.InvokeRequired Then
			Dim d As New SetControlValueCallback(AddressOf SetControlPropertyValue)
			oControl.Invoke(d, New Object() {oControl, propName, propValue})
		Else
			Dim t As Type = oControl.[GetType]()
			Dim props As PropertyInfo() = t.GetProperties()
			For Each p As PropertyInfo In props
				If p.Name.ToUpper() = propName.ToUpper() Then
					p.SetValue(oControl, propValue, Nothing)
				End If
			Next
		End If
	End Sub

	Private Function GetAssemblyFiles() As String()
		Dim filesList As String()
		If Not Me.txtAssemblyFilesFolder.Text.EndsWith("\") Then
			txtAssemblyFilesFolder.Text = txtAssemblyFilesFolder.Text + "\"
		End If
		filesList = Directory.GetFiles(Me.txtAssemblyFilesFolder.Text, "*.dll")
		_numberAssemblyFiles = filesList.Count()
		deploymentFilesSelectedLabel.Text = String.Format("{0} Assembly files ready to transfer.", _numberAssemblyFiles)

		Return filesList
	End Function

	Private Function GetHtmlFiles() As String()
		Dim filesList As String()
		If Not Me.txtHtmlFilesFolder.Text.EndsWith("\") Then
			txtHtmlFilesFolder.Text = txtHtmlFilesFolder.Text + "\"
		End If
		filesList = Directory.GetFiles(Me.txtHtmlFilesFolder.Text, "*.html")
		_numberHtmlFiles = filesList.Count()
		lblHtmlFilesTotal.Text = String.Format("{0} Html files ready to transfer.", _numberHtmlFiles)

		Return filesList
	End Function

	Private Sub TransferFiles(ByVal filesToTransfer As String(), ByVal transferLocation As String)
		If _sftpClient Is Nothing Then
			_sftpClient = New OcmSFTPClient()
		End If

		If Not _sftpClient Is Nothing Then
			' setup the transfer properties needed by the client:
			_sftpClient.HostName = _host
			_sftpClient.UserName = _username
			_sftpClient.Password = _password
			_sftpClient.WorkingDirectory = _workingdirectory
			_sftpClient.UploadFileName = _uploadfile
			_sftpClient.PhotoFTPFolderName = transferLocation

			' multiple files get this call:
			_sftpClient.transferFiles(filesToTransfer)
		End If

	End Sub

	Private Sub btnSelectAssembliesFolder_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelectAssembliesFolder.Click
		Dim dlg As FolderBrowserDialog

		dlg = New FolderBrowserDialog()
		If dlg.ShowDialog() = Windows.Forms.DialogResult.OK Then
			txtAssemblyFilesFolder.Text = dlg.SelectedPath
		End If

		If Not String.IsNullOrEmpty(txtAssemblyFilesFolder.Text) Then
			_assemblyFiles = GetAssemblyFiles()
		End If

		txtHtmlFilesFolder.Focus()
	End Sub

	Private Sub btnSelectHtmlFilesFolder_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelectHtmlFilesFolder.Click
		Dim dlg As FolderBrowserDialog

		dlg = New FolderBrowserDialog()
		If dlg.ShowDialog() = Windows.Forms.DialogResult.OK Then
			txtHtmlFilesFolder.Text = dlg.SelectedPath
		End If

		If Not String.IsNullOrEmpty(txtHtmlFilesFolder.Text) Then
			_htmlFiles = GetHtmlFiles()
		End If

		btnTransfer.Focus()

	End Sub

	Private Sub txtAssemblyFilesFolder_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtAssemblyFilesFolder.Leave
		If Not String.IsNullOrEmpty(txtAssemblyFilesFolder.Text) Then
			_assemblyFiles = GetAssemblyFiles()
		End If
	End Sub

	Private Sub txtHtmlFilesFolder_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtHtmlFilesFolder.Leave
		If Not String.IsNullOrEmpty(txtHtmlFilesFolder.Text) Then
			_htmlFiles = GetHtmlFiles()
		End If
	End Sub
End Class
