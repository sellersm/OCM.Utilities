using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Renci.SshNet;
using System.IO;
using System.Threading;

namespace SFTP_Test
{
	public class OcmSFTPClient
	{
		const int _port = 22;
		const string _host = "files.blackbaudhosting.com";
		const string _username = "MarkSe21195D@BBEC";
		const string _password = "P@ssword2";
		const string _workingdirectory = "/staging/21195D/ChildPhotos";
		const string _uploadfile = @"C:\OCM\PhotoImports\2013 Photos_Processing_6_25_2013\_Import\C210590.jpg";

		// folder name to be created on the FTP Site and used for Import photo .jpg file location:
		private string _ftpPhotoFolderName = "2013_ChildPhotos_";

		// properties to hold the FTP items:
		public string PhotoFTPFolderName { get; set; }
		public string HostName { get; set; }
		public string UserName { get; set; }
		public string WorkingDirectory { get; set; }
		public string UploadFileName { get; set; }
		public string[] UploadFileNameList { get; set; }
		public string Password { get; set; }

		// event stuff:
		public event EventHandler<FileUploadCompletedEventArgs> FileUploadCompleted;
		public event EventHandler<FileUploadingEventArgs> FileUploading;
		public event EventHandler<ListingDirectoryEventArgs> ListingDirectory;

		// new() constructor:
		public OcmSFTPClient()
		{
			// set the ftp photo directory
			//FormatFTPFolderName();
			// 7/24/13: caller must set it via the public property
		}

		public void transferFile()
		{
			//richTextBox1.AppendText("Creating client and connecting");
			LogThis("\rCreating client and connecting...");

			bool directoryAlreadyExists = false;

			using (var client = new SftpClient(HostName, _port, UserName, Password))
			{
				try
				{
					// connect to SFTP host:
					client.Connect();
					OutputThis(string.Format("\rConnected to {0}", HostName));

					// cd to the remote folder we want as the root:
					client.ChangeDirectory(WorkingDirectory);
					LogThis(string.Format("\rChanged directory to {0}", WorkingDirectory));
					LogThis(string.Format("\rChanged directory to {0}", WorkingDirectory));

					// this just lists the current [root] folder contents to make sure we're where we think we are:  for testing only
					//var listDirectory = client.ListDirectory(workingdirectory);
					//LogThis("\rListing directory:");

					//// event args for listing directory event:
					//ListingDirectoryEventArgs listArgs = new ListingDirectoryEventArgs();
					//foreach (var fi in listDirectory)
					//{						
					//    listArgs.FolderName = workingdirectory;
					//    listArgs.FileName = fi.Name;
					//    OnListingDirectory(listArgs);
					//    LogThis(string.Format("\r{0}", fi.Name));
					//}

					// make the new directory on the server, test if it already exists though:
					// a bug in client.Exists prevents me from using it reliably, so just try to 
					// cd to the dir and trap for the error if it doesn't exist:
					//if (client.Exists(workingdirectory + "/" + PhotoFTPFolderName) == false)
					//{
					//    client.CreateDirectory(PhotoFTPFolderName);
					//    LogThis("directory created.");
					//}

					try
					{
						client.ChangeDirectory(WorkingDirectory + "/" + PhotoFTPFolderName);
						directoryAlreadyExists = true;
					}
					catch (Exception exDirError)
					{
						if (exDirError != null)
						{
							if (exDirError.Message.Contains("not found"))
							{
								directoryAlreadyExists = false;
							}
						}
					}

					if (directoryAlreadyExists == false)
					{
						client.CreateDirectory(PhotoFTPFolderName);
						LogThis("directory created.");
					}

					client.ChangeDirectory(WorkingDirectory + "/" + PhotoFTPFolderName);
					LogThis("set working directory to photo Directory");
					LogThis(string.Format("\rset working directory to {0}", WorkingDirectory + "/" + PhotoFTPFolderName));

					//listDirectory = client.ListDirectory(workingdirectory);
					//LogThis("\rListing directory:");

					//foreach (var fi in listDirectory)
					//{
					//    LogThis(string.Format("\r{0}", fi.Name));
					//}


					using (var fileStream = new FileStream(UploadFileName, FileMode.Open))
					{
						LogThis(string.Format("\rUploading {0} ({1:N0} bytes)", UploadFileName, fileStream.Length));
						client.BufferSize = 4 * 1024; // bypass Payload error large files
						//client.UploadFile(fileStream, Path.GetFileName(uploadfile));

						// try async:
						var asyncResult = client.BeginUploadFile(fileStream, Path.GetFileName(UploadFileName), null, null) as Renci.SshNet.Sftp.SftpUploadAsyncResult;

						//var asyncResult = async1 as CommandAsyncResult;
						while (!asyncResult.IsCompleted)
						{
							FileUploadingEventArgs args = new FileUploadingEventArgs();
							args.BytesSentSoFar = asyncResult.UploadedBytes;
							args.FileNameInProgress = UploadFileName;
							args.FileBytesTotal = fileStream.Length;
							LogThis(string.Format("\rUploaded {0:#########}", (asyncResult.UploadedBytes * 1024)));
							OnFileUploading(args);
							Thread.Sleep(200);
						}
						LogThis(string.Format("\rUploaded {0:#########}", (asyncResult.UploadedBytes * 1024)));

						if (asyncResult.IsCompleted)
						{
							FileUploadCompletedEventArgs fileCompleteArgs = new FileUploadCompletedEventArgs();
							fileCompleteArgs.UploadedFileName = UploadFileName;
							fileCompleteArgs.BytesSent = asyncResult.UploadedBytes;
							client.EndUploadFile(asyncResult);
							OnFileUploadCompleted(fileCompleteArgs);
							// raise an event to tell the client we're done!

							//MessageBox.Show("File upload completed!", "File Upload", MessageBoxButtons.OK, MessageBoxIcon.Information);
						}

						//MessageBox.Show("File upload completed!", "File Upload", MessageBoxButtons.OK, MessageBoxIcon.Information);
					}
				}
				catch (Exception ex)
				{
					if (ex == null)
					{
						throw;
					}
					else
					{
						LogThis(ex.Message);
						// throw this exception so the consumer knows
						throw ex;
					}
				}
				finally
				{
					client.Disconnect();
				}

			}

		}


		public void transferFiles(string[] filesToUpload)
		{
			//richTextBox1.AppendText("Creating client and connecting");
			LogThis("\rCreating client and connecting...");

			bool directoryAlreadyExists = false;
			int fileCount = filesToUpload.Count();
			int fileCounter = 0;

			using (var client = new SftpClient(HostName, _port, UserName, Password))
			{
				try
				{
					// connect to SFTP host:
					client.Connect();
					OutputThis(string.Format("\rConnected to {0}", HostName));

					// cd to the remote folder we want as the root:
					client.ChangeDirectory(WorkingDirectory);
					LogThis(string.Format("\rChanged directory to {0}", WorkingDirectory));
					LogThis(string.Format("\rChanged directory to {0}", WorkingDirectory));

					try
					{
						client.ChangeDirectory(WorkingDirectory + "/" + PhotoFTPFolderName);
						directoryAlreadyExists = true;
					}
					catch (Exception exDirError)
					{
						if (exDirError != null)
						{
							if (exDirError.Message.Contains("not found"))
							{
								directoryAlreadyExists = false;
							}
						}
					}

					if (directoryAlreadyExists == false)
					{
						client.CreateDirectory(PhotoFTPFolderName);
						LogThis("directory created.");
					}

					client.ChangeDirectory(WorkingDirectory + "/" + PhotoFTPFolderName);
					LogThis("set working directory to photo Directory");
					LogThis(string.Format("\rset working directory to {0}", WorkingDirectory + "/" + PhotoFTPFolderName));

					// manage the uploading of multiple files
					foreach (var file in filesToUpload)
					{
						fileCounter++;
						using (var fileStream = new FileStream(file, FileMode.Open))
						{
							LogThis(string.Format("\rUploading {0} ({1:N0} bytes)", file, fileStream.Length));
							client.BufferSize = 4 * 1024; // bypass Payload error large files
							//client.UploadFile(fileStream, Path.GetFileName(uploadfile));

							// try async:
							var asyncResult = client.BeginUploadFile(fileStream, Path.GetFileName(file), null, null) as Renci.SshNet.Sftp.SftpUploadAsyncResult;

							//var asyncResult = async1 as CommandAsyncResult;
							while (!asyncResult.IsCompleted)
							{
								FileUploadingEventArgs args = new FileUploadingEventArgs();
								args.BytesSentSoFar = asyncResult.UploadedBytes;
								args.FileNameInProgress = file;
								args.FileBytesTotal = fileStream.Length;
								args.FileNumberOfTotal = fileCounter;
								args.TotalNumberFilesToTransfer = fileCount;
								LogThis(string.Format("\rUploaded {0:#########}", (asyncResult.UploadedBytes * 1024)));
								OnFileUploading(args);
								Thread.Sleep(200);
							}
							LogThis(string.Format("\rUploaded {0:#########}", (asyncResult.UploadedBytes * 1024)));

							if (asyncResult.IsCompleted)
							{
								FileUploadCompletedEventArgs fileCompleteArgs = new FileUploadCompletedEventArgs();
								fileCompleteArgs.UploadedFileName = file;
								fileCompleteArgs.BytesSent = asyncResult.UploadedBytes;
								client.EndUploadFile(asyncResult);
								OnFileUploadCompleted(fileCompleteArgs);
								// raise an event to tell the client we're done!

								//MessageBox.Show("File upload completed!", "File Upload", MessageBoxButtons.OK, MessageBoxIcon.Information);
							}

							//MessageBox.Show("File upload completed!", "File Upload", MessageBoxButtons.OK, MessageBoxIcon.Information);
						}
					}
					
				}
				catch (Exception ex)
				{
					if (ex == null)
					{
						throw;
					}
					else
					{
						LogThis(ex.Message);
						// throw this exception so the consumer knows
						throw ex;
					}
				}
				finally
				{
					client.Disconnect();
				}

			}

		}


		public void removeDirectory(string directoryToRemove)
		{
			using (var client = new SftpClient(HostName, _port, _username, _password))
			{
				try
				{
					client.Connect();
					LogThis(string.Format("\rConnected to {0}", HostName));

					client.DeleteDirectory(directoryToRemove);
					LogThis("\rdirectory removed!");
				}
				catch (Exception ex)
				{
					LogThis(ex.Message);
					// throw for the caller's benefit:
					throw ex;
				}
				finally
				{
					client.Disconnect();
				}
			}

		}

		protected void OnFileUploadCompleted(FileUploadCompletedEventArgs e)
		{
			EventHandler<FileUploadCompletedEventArgs> handler = FileUploadCompleted;
			if (handler != null)
			{
				handler(this, e);
			}
		}

		protected void OnFileUploading(FileUploadingEventArgs e)
		{
			EventHandler<FileUploadingEventArgs> handler = FileUploading;
			if (handler != null)
			{
				handler(this, e);
			}
		}

		protected void OnListingDirectory(ListingDirectoryEventArgs e)
		{
			EventHandler<ListingDirectoryEventArgs> handler = ListingDirectory;
			if (handler != null)
			{
				handler(this, e);
			}
		}

		private void OutputThis(string outputMessage)
		{
			//richTextBox1.AppendText(outputMessage);
		}

		private void LogThis(string logMessage)
		{
			System.Diagnostics.Debug.WriteLine(logMessage);
		}

		private void FormatFTPFolderName()
		{
			string todayDate = DateTime.Now.ToShortDateString();
			todayDate = todayDate.Replace('/', '_');
			PhotoFTPFolderName = string.Format(_ftpPhotoFolderName + "{0}", todayDate).Trim();
			LogThis(string.Format("\rFTP folder for Photos will be: {0}", PhotoFTPFolderName));
			//PhotoFTPFolderName = _ftpPhotoFolderName;
		}

		
	}
}
