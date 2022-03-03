using PVR.Core;
using PVR.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PVR.MVVM.ViewModel
{
    internal class MainViewModel : ObservableObject
    {
        public ObservableCollection<ModModel> Mods { get; set; }

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


		public MainViewModel()
		{
            Mods = new ObservableCollection<ModModel>();

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
		}

    }
}
