using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SFTP_Test
{
	public class FileUploadingEventArgs : EventArgs
	{
		public string FileNameInProgress { get; set; }
		public ulong BytesSentSoFar { get; set; }
		public int FileNumberOfTotal { get; set; }
		public long FileBytesTotal { get; set; }
		public int TotalNumberFilesToTransfer { get; set; }
	}
}
