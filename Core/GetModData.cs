using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PVR.Core
{
	internal class GetModData
	{
		public List<string> ModDataList(string modFolderPath)
		{
			
			List<string> nameList = new List<string>();
			List<string> modNameList = new List<string>();
			string regexPattern = @"[^\\]+(?=\.package$)";

			// list of folders in selected directory
			string[] folderList = Directory.GetDirectories(modFolderPath);
			foreach (string folder in folderList)
			{
				nameList.AddRange(Directory.GetFiles(folder).ToList());
			}

			// list of mods in the selected directory
			string[] modList = Directory.GetFiles(modFolderPath);
			foreach (string modPath in modList)
			{
				if (modPath.Contains(".package"))
				{
					nameList.Add(modPath);
				}
			}

			foreach (string name in nameList)
			{
				Match my_Match = Regex.Match(name, regexPattern);
				if (my_Match.Success)
				{
					modNameList.Add(my_Match.Value);
				}
			}

			return modNameList;
		}
	}
}
