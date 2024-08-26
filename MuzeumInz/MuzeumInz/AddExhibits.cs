using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace MuzeumInz
{
    public class AddExhibits
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Year { get; set; }
        public string Category { get; set; }
        public string Author { get; set; }
        public string Origin { get; set; }
        public BitmapImage Image { get; set; }
        public string Location { get; set; }

        public AddExhibits(int id, string name, string description, int year, string category, string author, string origin, BitmapImage image, string location) 
        { 
            Id = id;
            Name = name;
            Description = description;
            Year = year;
            Category = category;
            Author = author;
            Origin = origin;
            Image = image;
            Location = location;
        }
        //w comboboxe wyswietla nazwe eksponatu bo napisujemy tostring
        public override string ToString()
        {
            return Name;
        }

    }
}
