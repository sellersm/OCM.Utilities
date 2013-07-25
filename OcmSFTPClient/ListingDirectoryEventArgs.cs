using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SFTP_Test
{
	public class ListingDirectoryEventArgs : EventArgs
	{		
		public string FolderName { get; set; }
		public string FileName { get; set; }
	}
}
