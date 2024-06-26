using Avalonia;
using Avalonia.Controls;
using System;
using BuySell.Models;
using BuySell.ViewModels;

namespace BuySell.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainWindowViewModel();
    }
}
