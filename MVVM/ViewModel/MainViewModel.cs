using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace course_project1.MVVM.ViewModel
{
    public class MainViewModel: ObservableObject
    {
        public ProfileViewModel ProfileVM { get; set; }
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
            CurrentView = ProfileVM;
        }
    }
}
