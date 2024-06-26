using ReactiveUI;

namespace BuySell.Models
{
    public class Tegs : ReactiveObject
    {
        private string tegText;
        public string TegText
        {
            get => tegText;
            set => this.RaiseAndSetIfChanged(ref tegText, value);
        }
    }
}

