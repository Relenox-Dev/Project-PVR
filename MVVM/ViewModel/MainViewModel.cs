using PVR.Core;
using PVR.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ookii.Dialogs.Wpf;
using System.Windows.Input;

namespace PVR.MVVM.ViewModel
{
    internal class MainViewModel : ObservableObject
    {
        public ObservableCollection<ModModel> Mods { get; set; }

        public string modFolderPath;

		private ModModel _selectedMod;
		public ModModel SelectedMod
        {
			get { return _selectedMod; }
			set 
            { 
                _selectedMod = value;
                OnPropertyChanged();
            }
		}

        private ICommand _browseButtonClick;
        public ICommand BrowseButtonClick
        {
            get
            {
                if (_browseButtonClick == null)
                {
                    _browseButtonClick = new RelayCommand(
                        p => true,
                        p => this.PopulateModNameList());
				}
                return _browseButtonClick;
            }
		}


		public MainViewModel()
		{
            Mods = new ObservableCollection<ModModel>();
            /*
            

            Mods.Add(new ModModel
            {
                Name = "123",
                Type = "Boots"
            });

            for (int i = 0; i < 20; i++)
			{
                Mods.Add(new ModModel
                {
                    Name = $"{i}",
                    Type = "Furniture"
                });
            }
            
             */

        }

        public void PopulateModNameList()
		{
            VistaFolderBrowserDialog _modFolder = new VistaFolderBrowserDialog();
            if ((bool)_modFolder.ShowDialog())
            {
                if (_modFolder.SelectedPath.Length != 0)
                {
                    modFolderPath = _modFolder.SelectedPath;

                    GetModData modData  = new GetModData();
                    List<string> modNames = modData.ModDataList(modFolderPath);

                    foreach (string modName in modNames)
                    {
                        Mods.Add(new ModModel
                        {
                            Name = modName,
                            Type = "TEST"
                        });
                    }
                }
            }
        }

        public void FillList()
        {
            
        }

    }
}
