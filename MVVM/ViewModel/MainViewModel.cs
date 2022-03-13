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

        }

        public void PopulateModNameList()
        {
            VistaFolderBrowserDialog _modFolder = new VistaFolderBrowserDialog();
            if ((bool)_modFolder.ShowDialog())
            {
                if (_modFolder.SelectedPath.Length != 0)
                {
                    modFolderPath = _modFolder.SelectedPath;

                    GetModData modData = new GetModData();
                    List<ModModel> modList = modData.ReturnModModelObjectCollection(modFolderPath);
                    foreach (ModModel mod in modList){
                        Mods.Add(mod);
                    }
                }
            }
        }

    }
}
