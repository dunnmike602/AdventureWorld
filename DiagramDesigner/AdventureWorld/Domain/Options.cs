using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization;
using System.Windows;
using AdventureLandCore.Domain;
using AdventureLandCore.Extensions;
using AdventureLandCore.Services.Helpers;
using DiagramDesigner.Controls;
using DiagramDesigner.Properties;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace DiagramDesigner.AdventureWorld.Domain
{
    [DataContract]
    [CategoryOrder("Dialog Options", 1)]
    [CategoryOrder("File Management", 2)]
    [CategoryOrder("Game Designer", 2)]
    public class Options : ReactiveObjectBase
    {
        private static readonly Options OptionsInstance = new Options();

        private bool _autoSave;
        private bool _showModelValidationDialog;
        private string _lastProjectDirectory;
        private int _backups;
        private bool _showGrid;
        private bool _snapToGrid;
        private int _gridSize;
        private bool _warnOnReset;
        private string _imageDirectory;
      
        private Options()
        {
            _autoSave = Settings.Default.AutoSave;
            _showModelValidationDialog = Settings.Default.ShowModelValidationDialog;
            _lastProjectDirectory = Settings.Default.LastProjectDirectory;
            _backups = Settings.Default.Backups;
            _snapToGrid = Settings.Default.SnapToGrid;
            _showGrid = Settings.Default.ShowGrid;
            _gridSize = Settings.Default.GridSize;
            _warnOnReset = Settings.Default.WarnOnReset;
            _imageDirectory = Settings.Default.ImageDirectory;

            AdventureDesigner.Instance.SetGrid(ShowGrid);
            AdventureDesigner.Instance.SetGridSize(GridSize);
        }

        [Category("Dialog Options")]
        [PropertyOrder(1)]
        [DisplayName("Save on Exit")]
        [Description(
            "Automatically save project when application is closed. When false a confirmation dialog appears.")]
        public bool AutoSave
        {
            get => _autoSave;
            set
            {
                _autoSave = value;
                OnPropertyChanged();
                SaveSettings();
            }
        }


        [DataMember, Category("File Management"), DisplayName("Image Directory"), Editor(typeof(DirectoryEditor), typeof(DirectoryEditor)),
         PropertyOrder(1), Description("Global image directory for all image assets for all projects. For example thumbnails for game objects are all stored here. NOTE" +
                                       " you must restart the application for a change in this setting to take full effect.")]
        public string ImageDirectory
        {
            get
            {
                var directory = string.IsNullOrWhiteSpace(_imageDirectory) ? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "Images")
                    : _imageDirectory;

                Directory.CreateDirectory(directory);

                return directory;
            }
            set
            {
                var oldValue = _imageDirectory;
                _imageDirectory = value;
                OnPropertyChanged();
                SaveSettings();

                if (!oldValue.IsEqualTo(value))
                {
                    PerformImageDirectoryChanged(oldValue, value);
                }
            }
        }

        private void PerformImageDirectoryChanged(string oldValue, string newValue)
        {
            if (newValue.IsSubPathOf(oldValue))
            {
                MessageBox.Show("The new directory cannot be a sub-directory of the old one.", "Adventure World",
                    MessageBoxButton.OK);

                _imageDirectory = oldValue;

                return;
            }

            FileHelper.CopyFolder(oldValue, newValue);
        }

        [Category("Game Designer")]
        [PropertyOrder(1)]
        [DisplayName("Show Grid")]
        [Description("Show the built-in grid on the Game Designer windows.")]
        public bool ShowGrid
        {
            get => _showGrid;
            set
            {
                _showGrid = value;
                OnPropertyChanged();
                SaveSettings();
                AdventureDesigner.Instance.SetGrid(ShowGrid);
            }
        }

        [Category("Game Designer")]
        [PropertyOrder(2)]
        [DisplayName("Grid Size")]
        [Description("Size in pixels of the built in grid.")]
        public int GridSize
        {
            get => _gridSize;
            set
            {
                if (value >= 16 && value <= 128)
                {
                    _gridSize = value;
                    OnPropertyChanged();
                    SaveSettings();
                    AdventureDesigner.Instance.SetGridSize(GridSize);
                }
            }
        }

        [Category("Game Designer")]
        [PropertyOrder(3)]
        [DisplayName("Snap to Grid")]
        [Description("Align shapes to the background grid.")]
        public bool SnapToGrid
        {
            get => _snapToGrid;
            set
            {
                _snapToGrid = value;
                OnPropertyChanged();
                SaveSettings();
            }
        }

        [DataMember, Category("File Management"), DisplayName("Maintain Backups"),
         PropertyOrder(1),
         Description(
             "If this is set to a none zero value, the project file (*.xml), which includes all game definitions and scripts, will be backed up every time you save. The number of backups specified will be kept, the others will be deleted.")]
        public int Backups
        {
            get => _backups;
            set
            {
                if (value >= 0 && value <= 20)
                {
                    _backups = value;
                    OnPropertyChanged();
                    SaveSettings();
                }
            }
        }

        [Category("Dialog Options")]
        [PropertyOrder(2)]
        [DisplayName("Perform Model Valid Check")]
        [Description(
            "Enables/disables the dialog on the Script screen that prompts when running a script if the game model is invalid.")]
        public bool ShowModelValidationDialog
        {
            get => _showModelValidationDialog;
            set
            {
                _showModelValidationDialog = value;
                OnPropertyChanged();
                SaveSettings();
            }
        }


        [Category("Dialog Options")]
        [PropertyOrder(3)]
        [DisplayName("Warn On Command Reset")]
        [Description(
            "Enables/disables the dialog on the Commands Tab that prompts when resetting a built-in command to its original settings.")]
        public bool WarnOnReset
        {
            get => _warnOnReset;
            set
            {
                _warnOnReset = value;
                OnPropertyChanged();
                SaveSettings();
            }
        }

        [Browsable(false)]
        [ReadOnly(true)] public string ConsoleClientExe =>
            Path.Combine(Environment.CurrentDirectory, "GameClients", "AdventureLand.exe");

        [Browsable(false)]
        [ReadOnly(true)]
        public string GameExplorerClientExe =>
            Path.Combine(Environment.CurrentDirectory, "GameClients", "AdventureLandExplorer.exe");

        [Browsable(false)]
        [ReadOnly(true)] public string TempGameDirectory =>
            Path.Combine(Environment.CurrentDirectory, "GameClients", "GameData");

        [Browsable(false)]
        [ReadOnly(true)] public string LastProjectDirectory
        {
            get => string.IsNullOrWhiteSpace(_lastProjectDirectory)
                ? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    "AdventureWorld")
                : _lastProjectDirectory;

            set
            {
                _lastProjectDirectory = value;
                SaveSettings();
            }
        }

        [Browsable(false)] [ReadOnly(true)] public string ClientDirectory => Path.GetDirectoryName(ConsoleClientExe);

        public static bool HasInstance => OptionsInstance != null;

        public static Options Instance => OptionsInstance;

        private void SaveSettings()
        {
            Settings.Default.AutoSave = AutoSave;
            Settings.Default.ShowModelValidationDialog = ShowModelValidationDialog;
            Settings.Default.LastProjectDirectory = LastProjectDirectory;
            Settings.Default.Backups = Backups;
            Settings.Default.GridSize = GridSize;
            Settings.Default.ShowGrid = ShowGrid;
            Settings.Default.SnapToGrid = SnapToGrid;
            Settings.Default.WarnOnReset = WarnOnReset;
            Settings.Default.ImageDirectory = ImageDirectory;
            Settings.Default.Save();
        }
    }
}