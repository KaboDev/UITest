using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using UITest.Core;

namespace UITest.Model
{
    class Product 
    {
        public string Name { get; }
        public float Price { get; }
        public float Weight { get; }
        public float Size { get; }
        public string Location { get; }

        public SolidColorBrush Category { get; } = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF6969"));

        public string ImagePath { get; }

        public Product(string name, float price, float weight, float size, string imagePath = "", string location = "Unknown", SolidColorBrush category = null)
        {
            Name = name;
            Price = price;
            Weight = weight;
            Size = size;
            Category = category;
            Location = location;
            ImagePath = imagePath;
        }
    }
}
