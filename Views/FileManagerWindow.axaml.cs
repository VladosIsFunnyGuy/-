using System;
using Avalonia;
using Avalonia.Platform;
using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Windows;
using BuySell.ViewModels;

namespace BuySell.Views
{
    public partial class FileManagerWindow : Window
    {
        public FileManagerWindow()
        {
            InitializeComponent();
            DataContext = new FileManagerWindowViewModel();
        }

        private void OpenFileClick(object sender, RoutedEventArgs e)
        {
            if (DataContext is FileManagerWindowViewModel viewModel)
            {
                string path = viewModel.SelectFile();
                if(path != null)
                    this.Close(path);
            }
        }
    }
}
