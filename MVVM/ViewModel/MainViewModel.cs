using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace course_project1.MVVM.ViewModel
{
    public class MainViewModel: ObservableObject
    {
        public RelayCommand ProfileViewCommand { get; set; }
        public RelayCommand FoldersViewCommand { get; set; }
        public RelayCommand SettingsViewCommand { get; set; }

        public ProfileViewModel ProfileVM { get; set; }
        public FoldersViewModel FoldersVM { get; set; }
        public SettingsViewModel SettingsVM { get; set; }
        private object _currentView;

        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }


        public MainViewModel()
        {
            ProfileVM = new ProfileViewModel();
            FoldersVM = new FoldersViewModel();
            SettingsVM = new SettingsViewModel();
            CurrentView = ProfileVM;

            ProfileViewCommand = new RelayCommand(() => CurrentView = ProfileVM);
            FoldersViewCommand = new RelayCommand(() => CurrentView = FoldersVM);
            SettingsViewCommand = new RelayCommand(() => CurrentView = SettingsVM);
        }
    }
}
