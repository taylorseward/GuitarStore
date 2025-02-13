using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuitarStore.Models
{
    public class Guitar
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string? PhotoPath { get; set; }
        public string? Make {  get; set; }
        public string? Model { get; set; }
        public double Price { get; set; }
    }
}
