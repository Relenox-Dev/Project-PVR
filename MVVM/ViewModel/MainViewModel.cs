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
using System.Windows.Media.Imaging;

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
                _selectedImageIndex = 0;
                SelectedImage = _selectedMod.Thumbnails[_selectedImageIndex];
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

        private ICommand _nextImageButtonClick;
        public ICommand NextItemButtonClick
        {
            get
            {
                if (_nextImageButtonClick == null)
                {
                    _nextImageButtonClick = new RelayCommand(
                        p => true,
                        p => this.NextImage());
                }
                return _nextImageButtonClick;
            }
        }

        private ICommand _prevImageButtonClick;
        public ICommand PrevItemButtonClick
        {
            get
            {
                if (_prevImageButtonClick == null)
                {
                    _prevImageButtonClick = new RelayCommand(
                        p => true,
                        p => this.PrevImage());
                }
                return _prevImageButtonClick;
            }
        }

        private BitmapFrame _selectedImage;
        public BitmapFrame SelectedImage
        {
            get { return _selectedImage; }
            set
            {
                _selectedImage = value;
                OnPropertyChanged();
            }
        }

        private int _selectedImageIndex = 0;

        public void NextImage()
        {
			if (_selectedMod != null)
			{
                if (_selectedImageIndex < (_selectedMod.Thumbnails.Count - 1))
                {
                    _selectedImageIndex++;
                    SelectedImage = _selectedMod.Thumbnails[_selectedImageIndex];
                }
            }
        }

        public void PrevImage()
        {
            if (_selectedMod != null)
            {
                if (_selectedImageIndex > 0)
                {
                    _selectedImageIndex--;
                    SelectedImage = _selectedMod.Thumbnails[_selectedImageIndex];
                }
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
