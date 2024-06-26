using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using ReactiveUI;
using BuySell.Models;

namespace BuySell.ViewModels;

public class FileManagerWindowViewModel : ReactiveObject
{
    private string currentPath;
    public string CurrentPath
    {
        get => currentPath;
        set
        {
            this.RaiseAndSetIfChanged(ref currentPath, value);
            LoadFiles(value);
        }
    }

    private FileItem selectedItem;
    public FileItem SelectedItem
    {
        get => selectedItem;
        set
        {
            this.RaiseAndSetIfChanged(ref selectedItem, value);
            if(value != null && value.IsDirectory)
                CurrentPath = value.Path;
        }
    }
    public string ShowMessage { get; set; }

    public FileManagerWindowViewModel()
    {
        Files = new ObservableCollection<FileItem>();
        CurrentPath = Directory.GetCurrentDirectory();
        ShowMessage = "False";
    }

    public ObservableCollection<FileItem> Files { get; }

    private void LoadFiles(string path)
    {
        Files.Clear();

        if (Directory.Exists(path))
        {
            var directories = Directory.GetDirectories(path).Select(d => new FileItem
            {
                Name = Path.GetFileName(d),
                Path = d,
                IsDirectory = true
            });

            var files = Directory.GetFiles(path).Select(f => new FileItem
            {
                Name = Path.GetFileName(f),
                Path = f,
                IsDirectory = false
            });

            foreach (var dir in directories)
                Files.Add(dir);

            foreach (var file in files)
                Files.Add(file);
        }
    }

    public string SelectFile()
    {
        if (SelectedItem != null && SelectedItem.IsDirectory)
            ShowMessage = "True";
        else if (SelectedItem != null && !SelectedItem.IsDirectory)
            return SelectedItem.Path;
        return null;
    }
}
