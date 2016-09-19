using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SoundSourceCoverApplyTool.Models
{
	public class SoundSourceFilePathModel
	{
		public BitmapImage ImageSource { get; set; }
		public string FileName { get; set; }
		public string Artist { get; set; }
		public string Title { get; set; }
		public int FileSize { get; set; }
		public string FileType { get; set; }
	}
}
