using Avalonia.Controls;
using BuySell.ViewModels;

namespace BuySell.Views
{
    public partial class FeatureWindow : Window
    {
        public FeatureWindow()
        {
            InitializeComponent();
            DataContext = new FeatureWindowViewModel();
        }
    }
}
