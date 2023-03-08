using SotFSaveManager.Core;
using SotFSaveManager.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Collections.ObjectModel;
using SotFSaveManager.MVVM.Model;

namespace SotFSaveManager.MVVM.ViewModel
{
    /*
     * Features:
     * x Backups
     * O Revive Verginia
     * O Revive Kelvin
     * O Teleport Verginia to Player
     * O Teleport Kelvin to Player
     * O Respawn all trees
     * O Respawn stumps
    */

    public class ManagerViewModel : ObservableObject
    {
        private MainViewModel _mainVm;

        private bool _loading;

        public bool Loading
        {
            get { return _loading; }
            set { _loading = value; }
        }


        private string _savePath;
        public string SavePath
        {
            get => _savePath;
            set
            {
                _savePath = value;
                OnPropertyChanged();
            }
        }

        public string BackupPath
        {
            get
            {
                string targetPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                targetPath += Path.DirectorySeparatorChar + "SotF Manager" + Path.DirectorySeparatorChar + "Backups";
                return targetPath;
            }
        }

        public RelayCommand BackupCommand { get; set; }

        public RelayCommand GenerateCommand { get; set; }

        public List<RelayCommand> actions = new List<RelayCommand>();

        public ObservableCollection<ObservableBoolean> isChecked = new ObservableCollection<ObservableBoolean>() 
        { 
            new ObservableBoolean(),
            new ObservableBoolean(),
            new ObservableBoolean(),
            new ObservableBoolean(),
            new ObservableBoolean(),
            new ObservableBoolean()
        };

        public ObservableCollection<ObservableBoolean> IsChecked
        {
            get
            {
                return isChecked;
            }
            set
            {
                isChecked = value;
                OnPropertyChanged();
            }
        }

        private bool backupCreated = false;

        public ManagerViewModel(MainViewModel mainVm)
        {
            _mainVm = mainVm;
            InitializeActions();
            BackupCommand = new RelayCommand(o =>
            {
                CreateBackup();
            });

            GenerateCommand = new RelayCommand(o =>
            {
                GenerateFiles();
            });
        }

        public ManagerViewModel() { }

        public void InitializeActions()
        {
            actions.Add(new RelayCommand(_ => Manager.ReviveKelvin(SavePath)));
            actions.Add(new RelayCommand(_ => Manager.ReviveVirginia(SavePath)));
            actions.Add(new RelayCommand(_ => Manager.TeleportKelvin(SavePath)));
            actions.Add(new RelayCommand(_ => Manager.TeleportVirginia(SavePath)));
            actions.Add(new RelayCommand(_ => Manager.RegrowStumps(SavePath)));
            actions.Add(new RelayCommand(_ => Manager.RegrowAllTrees(SavePath)));
        }

        public void CreateBackup()
        {
            Loading = true;
            string sourcePath = SavePath;
            string targetPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            targetPath += Path.DirectorySeparatorChar + "SotF Manager" + Path.DirectorySeparatorChar + "Backups";
            targetPath += Path.DirectorySeparatorChar + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            targetPath += Path.DirectorySeparatorChar + Path.GetFileName(sourcePath);
            Directory.CreateDirectory(targetPath);

            //Create all of the directories
            foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
            {
                Directory.CreateDirectory(dirPath.Replace(sourcePath, targetPath));
            }

            //Copy all the files & Replaces any files with the same name
            foreach (string newPath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
            {
                File.Copy(newPath, newPath.Replace(sourcePath, targetPath), true);
            }
            Loading = false;
            backupCreated = true;
            InfoDialog dialog = new InfoDialog("Backup created!", "Done!");
            dialog.ShowDialog();
        }

        public void GenerateFiles()
        {
            if (!isChecked.Any(boo => boo.Property))
            {
                InfoDialog errorDialog = new InfoDialog("Choose one or more actions first", "Error");
                errorDialog.ShowDialog();
                return;
            }

            if (!backupCreated)
            {
                YesNoDialog backupDialog = new YesNoDialog("This can damage your savefiles! Do you want to create a Backup?", "Backup");
                backupDialog.ShowDialog();
                if (backupDialog.Result == YesNoDialog.ResultType.Yes)
                {
                    CreateBackup();
                } else if (backupDialog.Result == YesNoDialog.ResultType.Cancel)
                {
                    return;
                }
            }

            for(int i = 0; i < isChecked.Count; i++) {
                if (isChecked[i].Property)
                {
                    actions[i].Execute(null);
                }
            }

            InfoDialog dialog = new InfoDialog("Files were generated!", "Info");
            dialog.ShowDialog();
        }
    }
}
