using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Media.Imaging;
using Avalonia;
using Avalonia.Platform;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Avalonia;
using System;
using System.ComponentModel;
using System.IO;
using System.Text.Json;
using System.Linq;
using BuySell.Models;
using BuySell.Views;
using ReactiveUI;

namespace BuySell.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private const string PATH_TO_DATA = "Storage/storage.txt";
    private Library library;
    public ObservableCollection<Item> ListItems { get; set; }
    private Item selectedItem;
    private bool isUpdate;
    public Item SelectedItem
    {
        get => selectedItem;
        set
        {
            if(!isUpdate)
            {
                selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
                OpenItem(selectedItem);
            }
        }
    }
    private bool showListError;
    public bool ShowListError
    { 
        get => showListError;
        set => this.RaiseAndSetIfChanged(ref showListError, value);
    }
    public string SearchText { get; set; }
    public string AddressRegion { get; set; }
    public string AddressCity { get; set; }
    public string AddressStreet { get; set; }

    private int? _priceFrom;
    public int? PriceFrom
    {
        get => _priceFrom;
        set
        {
            _priceFrom = value == null ? 0 : value;
            OnPropertyChanged(nameof(PriceFrom));
        }
    }

    private int? _priceTo;
    public int? PriceTo
    {
        get => _priceTo;
        set
        {
            _priceTo = value == null ? 0 : value;
            OnPropertyChanged(nameof(PriceTo));
        }
    }
    
    public Bitmap AddItemImage { get; set; }
    public Bitmap SearchButtonImage { get; set; }
    public Bitmap DeleteImage { get; set; }

    public MainWindowViewModel()
    {
        library = Library.LoadData(PATH_TO_DATA);
        //library = new Library(10);
        library.SaveData(PATH_TO_DATA);
        ListItems = new ObservableCollection<Item>(library.Items);
        SearchText = "";
        AddressRegion = "";
        AddressCity = "";
        AddressStreet = "";
        PriceFrom = 0;
        PriceTo = 0;
        Uri uriAddImage = new Uri("avares://BuySell/Assets/add-icon.png");
        if(AssetLoader.Exists(uriAddImage))
            AddItemImage = new Bitmap(AssetLoader.Open(uriAddImage));
        Uri uriSearchButtonImage = new Uri("avares://BuySell/Assets/search-icon.png");
        if(AssetLoader.Exists(uriSearchButtonImage))
            SearchButtonImage  = new Bitmap(AssetLoader.Open(uriSearchButtonImage));
        ShowListError = false;
    }

    public void SearchButton()
    {
        isUpdate = true;

        var result = library.Search(SearchText, AddressRegion, AddressCity, AddressStreet, PriceFrom.Value, PriceTo.Value);
        
        ShowListError = result?.Any() != true ? true : false;
        
        ListItems.Clear();
        foreach (var item in result)
            ListItems.Add(item);
        
        isUpdate = false;
    }

    public void OpenItem(Item item)
    {
        var itemViewerWindow = new ItemDialogWindow(item);
        itemViewerWindow.ShowDialog((Application.Current.ApplicationLifetime 
            as IClassicDesktopStyleApplicationLifetime)?.MainWindow);
    }

    public async void AddItem()
    {
        var addItemWindow = new AddItemWindow();
        var result = await addItemWindow.ShowDialog<Item>(
            (Application.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.MainWindow);
        if(result != null)
        {
            library.Items.Insert(0, result);
            library.SaveData(PATH_TO_DATA);
            ListItems.Clear();
            foreach(Item item in library.Items)
                ListItems.Add(item);
        }
        library.SaveData(PATH_TO_DATA);
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
