using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GuitarStore.Models
{
    public class Product
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string? PhotoPath { get; set; }
        public string? Make {  get; set; }
        public string? Model { get; set; }
        public double Price { get; set; }

        public string? ProductType { get; set; }
    }

    public class Guitar : Product
    {
        public string? GuitarType { get; set; }
        public string? NumberOfStrings { get; set; }

        public Guitar()
        {
            ProductType = "Guitar";
        }
    }
    public class Amp : Product
    {
        public string? AmpType { get; set; }
        public Amp()
        {
            ProductType = "Amp";
        }
    }

    public class Pedal : Product
    {
        public string? PedalType { get; set; }
        public Pedal()
        {
            ProductType = "Pedal";
        }
    }
    public class Accessory : Product
    {
        public string? AccessoryType { get; set; }
        public Accessory()
        {
            ProductType = "Accessory";
        }
    }
}
