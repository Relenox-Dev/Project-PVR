using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace PVR.MVVM.Model
{
	internal class ModModel
	{
		public string FileName { get; set; }
		public string Type { get; set; }
		public string CASRandom { get; set; }
		public string AgeGender { get; set; }
		public string Tuning { get; set; }
		public List<BitmapFrame> Thumbnails	{ get; set; }
	}
}
