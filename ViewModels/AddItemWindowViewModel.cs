using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using System;
using System.IO;
using System.Windows;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ReactiveUI;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Avalonia.Platform.Storage;
using BuySell.Views;
using BuySell.Models;

namespace BuySell.ViewModels
{
    public class AddItemWindowViewModel : ViewModelBase, INotifyPropertyChanged
    {
        //Tovar name 
        private bool showNameError;
        public bool ShowNameError
        {
            get => showNameError;
            set => this.RaiseAndSetIfChanged(ref showNameError, value);
        }
        public string? name;
        public string? Name
        {
            get => name;
            set
            {
                if(value.Replace(" ", string.Empty).Length < 9)
                    ShowNameError = true;
                else
                    ShowNameError = false;
                this.RaiseAndSetIfChanged(ref name, DeleteExtraSpaces(value));
            }
        }
        //Seller name
        private bool showSellerNameError;
        public bool ShowSellerNameError
        {
            get => showSellerNameError;
            set => this.RaiseAndSetIfChanged(ref showSellerNameError, value);
        }
        public string? sellerName;
        public string? SellerName
        {
            get => sellerName;
            set
            {
                if(!(value.Replace(" ", string.Empty).Length < 9))
                    ShowSellerNameError = false;
                else
                    ShowSellerNameError = true;
                this.RaiseAndSetIfChanged(ref sellerName, string.IsNullOrWhiteSpace(value) ? null : DeleteExtraSpaces(value));
            }
        }
        //Description
        private bool showDescriptionError;
        public bool ShowDescriptionError
        {
            get => showDescriptionError;
            set => this.RaiseAndSetIfChanged(ref showDescriptionError, value);
        }
        public string? description;
        public string? Description
        {
            get => description;
            set
            {
                if(string.IsNullOrWhiteSpace(value) || value.Replace(" ", string.Empty).Length < 49)
                    ShowDescriptionError = true;
                else
                    ShowDescriptionError = false;

                this.RaiseAndSetIfChanged(ref description, value);
            }
        }
        //Price
        private bool showPriceError;
        public bool ShowPriceError
        {
            get => showPriceError;
            set => this.RaiseAndSetIfChanged(ref showPriceError, value);
        }
        public string? price;
        public string? Price
        {
            get => price;
            set
            {
                bool doesInt = int.TryParse(value, out int result);
                if(string.IsNullOrWhiteSpace(value))
                {
                    ShowPriceError = true;
                    this.RaiseAndSetIfChanged(ref price, value);
                }
                else if(doesInt)
                {
                    ShowPriceError = false;
                    this.RaiseAndSetIfChanged(ref price, value);
                }
            }
        }
        //Phone
        private bool showPhoneError;
        public bool ShowPhoneError
        {
            get => showPhoneError;
            set => this.RaiseAndSetIfChanged(ref showPhoneError, value);
        }
        public string? phone;
        public string? Phone
        {
            get => phone;
            set
            {
                string digitsOnly = new string(value.Where(char.IsDigit).ToArray());
                char[] result = { '(','_','_','_',')','-','_','_','_','-','_','_','-','_','_' };
                for(int i = 0; i < digitsOnly.Length; i++)
                {
                    for(int j = i; j < result.Length; j++)
                    {
                        if(result[j] == '_')
                        {
                            result[j] = digitsOnly[i];
                            break;
                        }
                    }
                }
                if(!string.IsNullOrWhiteSpace(digitsOnly) && new string(new string(result).Where(char.IsDigit).ToArray()).Length != 10)
                    ShowPhoneError = true;
                else
                    ShowPhoneError = false;
                phone = new string(result);
            }
        }
        //teg
        public int maxLength = 20;
        private bool showMaxLengthError;
        public bool ShowMaxLengthError
        {
            get => showMaxLengthError;
            set => this.RaiseAndSetIfChanged(ref showMaxLengthError, value);
        }
        private ObservableCollection<Tegs> textBoxes;
        public ObservableCollection<Tegs> TextBoxes
        {
            get => textBoxes;
            set => this.RaiseAndSetIfChanged(ref textBoxes, value);
        }
        private int selectedIndex;
        public int SelectedIndex
        {
            get => selectedIndex;
            set => this.RaiseAndSetIfChanged(ref selectedIndex, value);
        }
        //Image 
        private bool showImageError;
        public bool ShowImageError
        {
            get => showImageError;
            set => this.RaiseAndSetIfChanged(ref showImageError, value);
        }
        private string currentFile;
        public string CurrentFile
        {
            get => currentFile;
            set
            {
                this.RaiseAndSetIfChanged(ref currentFile, value);
                if(!string.IsNullOrEmpty(value) && File.Exists(currentFile))
                {
                    string fileExtension = value.Substring(value.LastIndexOf('.') + 1);
                    
                    if(fileExtension != "jpg" && fileExtension != "jpeg" && fileExtension != "png")
                        ShowImageError = true;
                    else
                        ShowImageError = false;
                }
            }
        }

        public Bitmap AddImage { get; set; }
        public Bitmap DeleteImage { get; set; }
        public Bitmap ImageImage { get; set; } 

        //Item
        public Item item { get; set; }

        //ViewModel create
        public AddItemWindowViewModel()
        {
            item = new Item();
            TextBoxes = new ObservableCollection<Tegs>();
            ShowNameError = false;
            ShowSellerNameError = false;
            ShowDescriptionError = false;
            ShowPriceError = false;
            ShowPhoneError = false;
            ShowMaxLengthError = false;
            ShowImageError = false;
            selectedIndex = -1;
            Phone = "(___)-___-__-__";
            Uri uriAddImage = new Uri("avares://BuySell/Assets/add-icon.png");
            if(AssetLoader.Exists(uriAddImage))
                AddImage  = new Bitmap(AssetLoader.Open(uriAddImage));
            Uri uriDeleteImage = new Uri("avares://BuySell/Assets/cancel-icon.png");
            if(AssetLoader.Exists(uriDeleteImage))
                DeleteImage  = new Bitmap(AssetLoader.Open(uriDeleteImage));
            Uri uriImageImage = new Uri("avares://BuySell/Assets/no-image-icon.png");
            if(AssetLoader.Exists(uriImageImage))
                ImageImage  = new Bitmap(AssetLoader.Open(uriImageImage));

        }

        private string DeleteExtraSpaces(string str)
        {
            if(string.IsNullOrWhiteSpace(str))
                return string.Join(" ", str.Split(' ', StringSplitOptions.RemoveEmptyEntries));
            else
                return string.Join(" ", str.Split(' ', StringSplitOptions.RemoveEmptyEntries)) + (str[^1] == ' ' ? " " : string.Empty);
        }

        //Buttons for tegs
        //Button aad
        public void AddItem()
        {
            if(TextBoxes.Count != maxLength)
            {
                ShowMaxLengthError = false;
                TextBoxes.Add(new Tegs{ TegText = "Good" });
            }
            else
                ShowMaxLengthError = true;
        }
        //Button delete
        public void DeleteItem()
        {
            if (SelectedIndex >= 0 && SelectedIndex < TextBoxes.Count)
            {
                TextBoxes.RemoveAt(SelectedIndex);
                SelectedIndex = -1;
                ShowMaxLengthError = false; 
            }
        }
    }
}
