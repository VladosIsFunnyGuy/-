using BuySell.Models;

namespace BuySell.ViewModels
{
    public class ItemDialogWindowViewModel
    {
        public Item item { get; set; }

        public ItemDialogWindowViewModel(Item item)
        {
            this.item = item;
        }
    }
}
