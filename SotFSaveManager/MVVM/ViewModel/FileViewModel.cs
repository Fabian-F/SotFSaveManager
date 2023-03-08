using SotFSaveManager.MVVM.ViewModel;
using SotFSaveManager.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SotFSaveManager.MVVM.ViewModel
{
    public class FileViewModel : ObservableObject
    {
        private MainViewModel _mainVm;

        private string[] NeededFiles = {
            "GameStateSaveData.json",
            "SaveData.json",
            "WorldObjectLocatorManagerSaveData.json"
        };

        private string _uiError;
        public string UiError
        {
            get => _uiError;
            set
            {
                _uiError = value;
                OnPropertyChanged();
            }
        }

        public FileViewModel(MainViewModel mainVm)
        {
            _mainVm = mainVm;
        }

        public void Load(string savePath)
        {
            if (IsValidSavePath(savePath))
                _mainVm.ManagerViewCommand.Execute(savePath);
        }

        public bool IsValidSavePath(string savePath)
        {
            FileAttributes attr = File.GetAttributes(savePath);
            if (!attr.HasFlag(FileAttributes.Directory))
            {
                UiError = "File is not a directory!";
                return false;
            }

            var files = Directory
                .EnumerateFiles(savePath, "*", SearchOption.TopDirectoryOnly)
                .Select(Path.GetFileName).ToList();

            if (!NeededFiles.All(file => files.Contains(file)))
            {
                UiError = "Directory is not a SotF Save Path!";
                return false;
            }

            return true;
        }
    }
}
