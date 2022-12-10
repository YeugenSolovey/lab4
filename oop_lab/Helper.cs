using System;
using System.ComponentModel;

namespace oop_lab
{
    public class Helper
    {
        public string Name { get; set; }
        public string Vendor { get; set; }
        public int GB { get; set; }
        public float Flops { get; set; }
        public float Price { get; set; }
        public int Year { get; set; }
        public Helper(string name, string vendor, int gB, float flops, float price, int year)
        {
            Name = name;
            Vendor = vendor;
            Flops = flops;
            Price = price;
            Year = year;
            GB = gB;
        }
    }
}
