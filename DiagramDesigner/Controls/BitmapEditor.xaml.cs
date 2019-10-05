using System;
using System.IO;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using AdventureLandCore.Services.Helpers;
using DiagramDesigner.AdventureWorld.Domain;
using Microsoft.Win32;
using Xceed.Wpf.Toolkit.PropertyGrid;
using Xceed.Wpf.Toolkit.PropertyGrid.Editors;

namespace DiagramDesigner.Controls
{
    public partial class BitmapEditor : ITypeEditor
    {
        private static string _lastDirectory;

        public static readonly DependencyProperty BitmapProperty = DependencyProperty.Register("Bitmap", typeof(BitmapImage), typeof(BitmapEditor),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public BitmapImage Bitmap
        {
            get => (BitmapImage)GetValue(BitmapProperty);
            set => SetValue(BitmapProperty, value);
        }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(string), typeof(BitmapEditor),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        private Guid _currentAdventureObjectGuid;

        public string Value
        {
            get => (string)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }
        
        public BitmapEditor()
        {
            InitializeComponent();
        }

        private void PromptForFileAndOpenIt()
        {
            var openFile = new OpenFileDialog
            {
                Filter = "Image files (*.jpg, *.jpeg, *.png, *.bmp) | *.jpg; ; *.bmp ; *.png",
                RestoreDirectory = false,
                InitialDirectory = _lastDirectory ?? Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
            };
            
            if (openFile.ShowDialog() == true)
            {
               _lastDirectory = Path.GetDirectoryName(openFile.FileName);

                LoadBitmap(openFile.FileName, false);
            }
        }

        private void LoadBitmap(string filePath, bool skipFileSave)
        {
            Value = FileHelper.SaveObjectBitmapAsThumbnail(filePath, Path.Combine(Options.Instance.ImageDirectory, AdventureGameDesignerViewModel.Instance.ControlId.ToString()),
                _currentAdventureObjectGuid.ToString(), skipFileSave);

            if (string.IsNullOrWhiteSpace(Value)) return;
       
            Bitmap = FileHelper.GetBitmapImage(Value);
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            PromptForFileAndOpenIt();
        }

        public FrameworkElement ResolveEditor(PropertyItem propertyItem)
        {
            var binding = new Binding("Value")
            {
                Source = propertyItem,
                Mode = propertyItem.IsReadOnly ? BindingMode.OneWay : BindingMode.TwoWay
            };

            BindingOperations.SetBinding(this, ValueProperty, binding);

            _currentAdventureObjectGuid = ((AdventureObjectBase)propertyItem.Instance).ControlId;

            LoadBitmap(Value, true);
            
            return this;
        }

        private void DeleteButtonClick(object sender, RoutedEventArgs e)
        {
            Value = null;
            Bitmap = null;
        }
    }
}
