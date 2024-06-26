using Avalonia.Interactivity;
using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Platform;
using Avalonia.Media.Imaging;
using BuySell.Models;
using BuySell.ViewModels;

namespace BuySell.Views
{
    public partial class AddItemWindow : Window
    {
        public AddItemWindow()
        {
            InitializeComponent();
            DataContext = new AddItemWindowViewModel();
        }

        private async void ChosseFileClick(object sender, RoutedEventArgs e)
        {
            if (DataContext is AddItemWindowViewModel viewModel)
            {
                var fileManagerWindow = new FileManagerWindow();
                viewModel.CurrentFile = await fileManagerWindow.ShowDialog<string>(this);
            }
        }

        private void OnSaveButtonClick(object sender, RoutedEventArgs e)
        {
            if (DataContext is AddItemWindowViewModel viewModel)
            {
                if(!viewModel.ShowNameError && !string.IsNullOrWhiteSpace(viewModel.Name) && 
                    !viewModel.ShowSellerNameError && !string.IsNullOrWhiteSpace(viewModel.SellerName) && 
                    !viewModel.ShowDescriptionError && !string.IsNullOrWhiteSpace(viewModel.Description) && 
                    !viewModel.ShowPriceError && !string.IsNullOrWhiteSpace(viewModel.Price) &&
                    !viewModel.ShowPhoneError && !string.IsNullOrWhiteSpace(viewModel.Phone) &&
                    !viewModel.ShowMaxLengthError && viewModel.maxLength <= 20 &&
                    !viewModel.ShowImageError && !string.IsNullOrWhiteSpace(viewModel.CurrentFile))
                {
                    viewModel.item.Name = viewModel.Name;
                    viewModel.item.Seller.Name = viewModel.SellerName;
                    viewModel.item.Description = viewModel.Description;
                    int.TryParse(viewModel.Price, out int price);
                    viewModel.item.Price = price;
                    viewModel.item.Seller.Phone = viewModel.Phone;
                    viewModel.item.ImagePath = viewModel.CurrentFile;
                    if(File.Exists(viewModel.CurrentFile))
                        viewModel.item.Image = new Bitmap(viewModel.CurrentFile);
                    else
                        viewModel.item.Image = new Bitmap(AssetLoader.Open(new Uri("avares://BuySell/Assets/no-image-icon.png")));
                    viewModel.item.Tegs = new List<Tegs>(viewModel.TextBoxes); 
                    this.Close(viewModel.item);
                }
            }
        }

        private void OnCloseButtonClick(object sender, RoutedEventArgs e) => this.Close();
    }
}

