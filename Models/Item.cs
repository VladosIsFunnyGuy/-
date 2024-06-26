using System;
using System.IO;
using System.Collections.Generic;
using Avalonia.Platform;
using Avalonia.Media.Imaging;

namespace BuySell.Models;

public class Item
{
    public string Name { get; set; }
    public List<Tegs> Tegs { get; set; }
    public string Description { get; set; }
    public int Price { get; set; }
    public string ImagePath { get; set; }
    public Bitmap Image { get; set; }
    public User Seller { get; set; }
    public bool Feature { get; set; }

    public Item()
    {
        Name = "";
        Tegs = new List<Tegs>(0);
        Description = "";
        Price = 0;
        ImagePath = "";
        Image = null;
        Seller = new User
        {
            Name = "",
            Phone = "",
            Address = new string[3]{ "", "", "" }
        };
    }
}
