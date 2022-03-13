using System.Collections.Generic;
using System.Diagnostics;
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
			string _type = "";
			string _casRandom = "";
			string _ageGender = "";
			string _tuning = "";
			List<ModModel> result = new List<ModModel>();

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
							CASPartResource.CASPartResource resource = (CASPartResource.CASPartResource)WrapperDealer.GetResource(1, nowOpenPackage, resourceIndexEntry);
							if (resource.ParameterFlags.HasFlag(CASPartResource.ParmFlag.AllowForCASRandom))
							{
								_casRandom = "True";
							}
							else
							{
								_casRandom = "False";
							}

							_type = resource.BodyType.ToString();
							_ageGender = resource.AgeGender.ToString();
						}
						catch
						{
							continue;
						}
						
						
					}
					else if (types.Contains(resourceIndexEntry.ResourceType))
					{
						IResource resource = WrapperDealer.GetResource(1, nowOpenPackage, resourceIndexEntry);
						List<string> v = resource.ContentFields;

						_type = "Object";
						_tuning = resource["Tuning"];
						continue;
					}
				}

				nowOpenPackage.Dispose();
				result.Add(new ModModel
				{
					FileName = Regex.Match(modPath, @"[^\\]+(?=\.package$)").ToString(),
					Type = _type,
					CASRandom = _casRandom,
					AgeGender = _ageGender,
					Tuning = _tuning
				});

				_type = "";
				_casRandom = "";
				_ageGender = "";
				_tuning = "";
			}

			return result;
		}
	}
}