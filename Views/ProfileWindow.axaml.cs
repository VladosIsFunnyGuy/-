using Avalonia.Controls;
using BuySell.ViewModels;

namespace BuySell.Views
{
    public partial class ProfileWindow : Window
    {
        public ProfileWindow()
        {
            InitializeComponent();
            DataContext = new ProfileWindowViewModel();
        }
    }
}
