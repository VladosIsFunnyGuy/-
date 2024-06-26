using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using BuySell.Models;
using BuySell.ViewModels;

namespace BuySell.Views;

public partial class ItemDialogWindow : Window
{
    public ItemDialogWindow(Item item)
    {
        InitializeComponent();
        DataContext = new ItemDialogWindowViewModel(item);
    }

    private void OnCloseButtonClick(object sender, RoutedEventArgs e) => this.Close();
}
