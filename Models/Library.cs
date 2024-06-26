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

namespace BuySell.Models;

public class Library
{
    public ObservableCollection<Item> Items { get; set; }

    public Library(int count)
    {
        Items = new ObservableCollection<Item>();
        FillListWithTestData(count);
    }

    public Library()
    {
        Items = new ObservableCollection<Item>();
    }

    public void FillListWithTestData(int count)
    {
        for(int i = 0; i < count; i++)
        {
            Items.Add(new Item
            {
                Name = $"Name {i}",
                Tegs = new List<Tegs>(2)
                {
                    new Tegs { TegText = $"Good {i}" },
                    new Tegs { TegText = $"Tovar {i}" }
                },
                Description = $"Very nice tovar plise buy me {i}",
                Price = i,
                ImagePath = "/home/vlados/programming/BuySell/Assets/images/image;s=1000x700(1).jpg",
                Image = new Bitmap(AssetLoader.Open(new Uri("avares://BuySell/Assets/images/image;s=1000x700(1).jpg"))),
                Seller = new User
                {
                    Name = $"Seller {i}",
                    Phone = "0" + $"{960000000 + i}",
                    Address = new string[3]{ $"Sumy {i}", $"Konotop {i}", $"Stepana Banderi {i}" }
                }
            });
        }
    }

    public ObservableCollection<Item> Search(string text = "", string aR = "", string aC = "", string aS = "", int priceStart = 0, int priceEnd = int.MaxValue)
    {
        var result = new ObservableCollection<Item>();

        foreach (Item item in Items)
        {
            bool matchesText = item.Name.Contains(text) || item.Tegs.Any(tag => tag.TegText != null ? tag.TegText.Contains(text) : false);
            bool matchesAddress = (string.IsNullOrWhiteSpace(aR) || item.Seller.Address.Any(address => address.Contains(aR))) && (string.IsNullOrWhiteSpace(aC) || item.Seller.Address.Any(address => address.Contains(aC))) && (string.IsNullOrWhiteSpace(aS) || item.Seller.Address.Any(address => address.Contains(aS)));
            bool matchesPrice = item.Price >= priceStart && item.Price <= priceEnd;

            if (matchesText && matchesAddress && matchesPrice)
                result.Add(item);
        }

        return result;
    }

    public void SaveData(string path)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            IgnoreNullValues = true
        };

        var itemsWithoutImages = new List<Item>();
        foreach (var item in Items)
        {
            var itemWithoutImage = new Item
            {
                Name = item.Name,
                Tegs = item.Tegs,
                Description = item.Description,
                Price = item.Price,
                ImagePath = item.ImagePath,
                Seller = item.Seller
            };
            itemsWithoutImages.Add(itemWithoutImage);
        }

        var jsonString = JsonSerializer.Serialize(itemsWithoutImages, options);
        File.WriteAllText(path, jsonString);
    }

    public static Library LoadData(string path)
    {
        if(File.Exists(path))
        {
            var jsonString = File.ReadAllText(path);
            var items = JsonSerializer.Deserialize<ObservableCollection<Item>>(jsonString);
            foreach(Item item in items)
                item.Image = File.Exists(item.ImagePath) ? new Bitmap(item.ImagePath) : File.Exists("Assets/no-image-icon.png") ? new Bitmap(AssetLoader.Open(new Uri("avares://BuySell/Assets/no-image-icon.png"))) : null;
            return new Library { Items = items };
        }
        else
            return new Library();
    }
}
