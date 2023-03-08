using SotFSaveManager.Core;
using SotFSaveManager.MVVM.ViewModel;
using System.Windows;

namespace SotFSaveManager.MVVM.ViewModel
{

    public class MainViewModel : ObservableObject
    {
        public RelayCommand FileViewCommand { get; set; }

        public RelayCommand ManagerViewCommand { get; set; }

        public FileViewModel FileVm { get; set; }

        public ManagerViewModel ManagerVm { get; set; }

        private object _currentView;

        public object CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }
        
        public MainViewModel()
        {
            FileVm = new FileViewModel(this);
            ManagerVm = new ManagerViewModel(this);

            CurrentView = FileVm;

            FileViewCommand = new RelayCommand(o =>
            {
                CurrentView = FileVm;
            });
            ManagerViewCommand = new RelayCommand(o =>
            {
                ManagerVm.SavePath = o.ToString();
                CurrentView = ManagerVm;
            });
        }

        public bool IsFileView => CurrentView == FileVm;
        public bool IsManagerView => CurrentView == ManagerVm;
    }
}