using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SFTP_Test
{
	public class FileUploadCompletedEventArgs : EventArgs
	{
		public ulong BytesSent { get; set; }
		public string UploadedFileName { get; set; }
		public int FileNumber { get; set; }
	}
}
