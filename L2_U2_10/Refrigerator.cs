using System;

namespace L2_U2_10
{
    /// <summary>
    /// Klasė, skirta šaldytuvų duomenų saugojimui
    /// </summary>
    class Refrigerator
    {        
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public int Capacity { get; set; }
        public string EnergyLabel { get; set; }
        public string InstallationType { get; set; }
        public string Color { get; set; }
        public bool IsThereAFreezer { get; set; }
        public double Price { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public int Depth { get; set; }

        /// <summary>
        /// Numatytasis kontruktorius
        /// </summary>
        public Refrigerator()
        {
        }

        /// <summary>
        /// Parametrų kontruktorius
        /// </summary>
        /// <param name="manufacturer"></param>
        /// <param name="model"></param>
        /// <param name="capacity"></param>
        /// <param name="energyLabel"></param>
        /// <param name="installationType"></param>
        /// <param name="color"></param>
        /// <param name="isThereAFreezer"></param>
        /// <param name="price"></param>
        /// <param name="height"></param>
        /// <param name="width"></param>
        /// <param name="depth"></param>
        public Refrigerator(string manufacturer, string model, int capacity, string energyLabel, string installationType,
                            string color, bool isThereAFreezer, double price, int height, int width, int depth)
        {
            Manufacturer = manufacturer;
            Model = model;
            Capacity = capacity;
            EnergyLabel = energyLabel;
            InstallationType = installationType;
            Color = color;
            IsThereAFreezer = isThereAFreezer;
            Price = price;
            Height = height;
            Width = width;
            Depth = depth;
        }

        /// <summary>
        /// Perrašytas išspausdinimo šablonas
        /// </summary>
        /// <returns></returns>
        public override String ToString()
        {
            return String.Format("Gamintojas: {0,10} | Modelis: {1,6} | Talpa: {2,3} | Kaina: {3,3} |",
                                 Manufacturer, Model, Capacity, Price);
        }

        /// <summary>
        /// Perrašomas Equals metodas, kuris leidžia patikrinti ar vienodi du pateikti šaldytuvai
        /// </summary>
        /// <param name="obj">Lyginamasis objektas</param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            Refrigerator e = obj as Refrigerator;
            if (e.Manufacturer == Manufacturer && e.Model == Model)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Dėl pakeisto Equals metodo, pakeičiamas GetHashCode metodas
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Manufacturer.GetHashCode() ^ Model.GetHashCode();
        }

        /// <summary>
        /// Perrašomas mažiau arba lygu operatorius
        /// </summary>
        /// <param name="lhs">Pirmas objektas</param>
        /// <param name="rhs">Antras objektas</param>
        /// <returns></returns>
        public static bool operator <=(Refrigerator lhs, Refrigerator rhs)
        {
            return lhs.Price < rhs.Price || lhs.Price == rhs.Price;
        }

        /// <summary>
        /// Perrašomas daugiau arba lygu operatorius
        /// </summary>
        /// <param name="lhs">Pirmas objektas</param>
        /// <param name="rhs">Antras objektas</param>
        /// <returns></returns>
        public static bool operator >=(Refrigerator lhs, Refrigerator rhs)
        {
            return lhs.Price > rhs.Price || lhs.Price == rhs.Price;
        }        
    }
}