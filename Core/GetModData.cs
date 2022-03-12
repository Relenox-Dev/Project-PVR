using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using PVR.MVVM.Model;
using s4pi.Interfaces;
using s4pi.Package;
using s4pi.WrapperDealer;

namespace PVR.Core
{
	internal class GetModData
	{

		private static uint[] thumbnails = new uint[]
		{
			0x0D338A3A,
			0x16CCF748,
			0x35D45407,
			0x3C1AF1F2,
			0xC32A8647,
			0x56278554,
			0x58282D45,
			0x9C925813,
			0xA1FF2FC4,
			0xCD9DE247,
			0xE1BCAEE2,
			0xE254AE6E,
			0x3C2A8647
		};

		private static uint[] flags = new uint[]
		{
			0x034AEECB
		};

		private static uint[] types = new uint[]
		{
			0xC0DB5AE7
		};

		private List<string> _modFilePathList = new List<string>();

		private void ModPathList(string modFolderPath)
		{

			// list of folders in selected directory
			string[] folderList = Directory.GetDirectories(modFolderPath);
			foreach (string folder in folderList)
			{
				_modFilePathList.AddRange(Directory.GetFiles(folder).ToList());
			}

			// list of mods in the selected directory
			string[] modList = Directory.GetFiles(modFolderPath);
			foreach (string modPath in modList)
			{
				if (modPath.Contains(".package"))
				{
					_modFilePathList.Add(modPath);
				}
			}
		}


		public List<ModModel> ReturnModModelObjectCollection(string modFolderPath)
		{
			ModPathList(modFolderPath);
			string _name = "";
			string _bodyType = "";
			List<ModModel> result = new List<ModModel>();
			CASPartResource.CASPartResource resource;

			foreach (string modPath in _modFilePathList)
			{
				if (!modPath.Contains(".package"))
				{
					continue;
				}

				IPackage nowOpenPackage = Package.OpenPackage(0, modPath);
				foreach (IResourceIndexEntry resourceIndexEntry in nowOpenPackage.GetResourceList)
				{
					
					
					if (flags.Contains(resourceIndexEntry.ResourceType))
					{
						try
						{
							resource = (CASPartResource.CASPartResource)WrapperDealer.GetResource(1, nowOpenPackage, resourceIndexEntry);
						}
						catch
						{
							continue;
						}
						
						
						_bodyType = resource.BodyType.ToString();
						_name = resource.Name.ToString();
					}
					else if (types.Contains(resourceIndexEntry.ResourceType))
					{
						_bodyType = "Objects";
						_name = "empty";

						continue;
					}
				}

				
				result.Add(new ModModel
				{
					FileName = Regex.Match(modPath, @"[^\\]+(?=\.package$)").ToString(),
					Name = _name,
					Type = _bodyType
				}); 

			}

			return result;
		}
	}
}