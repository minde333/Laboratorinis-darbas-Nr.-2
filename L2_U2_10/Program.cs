using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2_U2_10
{
    /// <summary>
    /// Programa skirta dirbti su parduotuvių pateikimo duomenimis
    /// </summary>
    class Program
    {
        private const string ChosenManufacturer = "Siemens"; // Pasirenkamas gamintojas su kuriuo skaičiuojami šaldytuvai
        private const string ChosenInstallationType = "Pastatomas"; // Pasirenkamas montavimo tipas pagal kurį daromi skaičiavimai
        private const int CapacityThreshold = 80; // Talpos kiekis pagal kurį reikia palyginti šaldytuvus
        public const int NumberOfBranches = 2; // Parduotuvių skaičius
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8; //Konsolėje rašo lietuviškas raides

            Program p = new Program();

            Branch[] branches = new Branch[NumberOfBranches];
            branches[0] = new Branch("Elektroluxas", "Forto g. 5", "862469534");
            branches[1] = new Branch("SAMSUNGAS", "Lekonio g. 64", "861478844");
            
            string[] filePaths = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.csv");

            foreach (string path in filePaths)
            {
                bool rado = p.ReadRefrigeratorData(path, branches);
                if (rado == false)
                    Console.WriteLine("Neatpažintas failas {0}", path);
            }

            p.CountSiemens(branches);
            p.PrintRefrigeratorsToConsole(p.SortCheapest(p.TenCheapest(branches)));
            p.ProductsOnlyInOneBranch(branches);
            p.WriteManufacturersToFile(ManufacturerList(branches)); 
            p.CreateAReportTable(branches, "L2ReportTable.txt");

            Console.ReadKey();
        }

        /// <summary>
        /// Skaito šaldytuvų duomenis iš failo
        /// </summary>
        /// <param name="file">Įvesties duomenų failo pavadinimas</param>
        /// <param name="branches">Parduotuvių masyvas</param>
        /// <returns>Parduotuvės ir šaldytuvo duomenis</returns>
        private bool ReadRefrigeratorData(string file, Branch[] branches)
        {
            string name = null;
            string address = null;
            string phone = null;
            int indexas = 0;
           
            using (StreamReader reader = new StreamReader(@file))
            {
                string line = null;
                line = reader.ReadLine();
                while(line != null && indexas <= NumberOfBranches)
                {
                    if(indexas == 0)
                    {
                        name = line;
                        line = reader.ReadLine();
                        indexas++;
                        continue;
                    }
                    if(indexas == 1)
                    {
                        address = line;
                        line = reader.ReadLine();
                        indexas++;
                        continue;
                    }
                    else
                    if(indexas == 2)
                    {
                        phone = line;
                        indexas++;
                        continue;
                    }
                }
                Branch branch = GetBranchByDifference(branches, name, address, phone);
                if (branch == null)
                {
                    return false;
                }
                while (null != (line = reader.ReadLine()))
                {
                    string[] values = line.Split(',');
                    string Manufacturer = values[0];
                    string Model = values[1];
                    int Capacity = int.Parse(values[2]);
                    string EnergyLabel = values[3];
                    string InstallationType = values[4];
                    string Color = values[5];
                    bool IsThereAFreezer = bool.Parse(values[6]);
                    double Price = double.Parse(values[7]);
                    int Height = int.Parse(values[8]);
                    int Width = int.Parse(values[9]);
                    int Depth = int.Parse(values[10]);

                    Refrigerator refrigerator = new Refrigerator(Manufacturer, Model, Capacity, EnergyLabel, InstallationType,
                                                                 Color, IsThereAFreezer, Price, Height, Width, Depth);

                    branch.Refrigerators.AddRefrigerator(refrigerator);                    
                }
                return true;
            }          
        }

        /// <summary>
        /// Į lentelę surašo pradinius duomenis
        /// </summary>
        /// <param name="branches">Parduotuvių masyvas</param>
        /// <param name="file">Įvesties duomenų failo pavadinimas</param>
        void CreateAReportTable(Branch[] branches, string file)
        {
            for (int i = 0; i < branches.Count(); i++)
            {
                using (StreamWriter writer = new StreamWriter(@file, true, Encoding.UTF8))
                {
                    writer.WriteLine("Duomenys apie parduotuvę ir jos parduodamus šaldytuvus");
                    writer.WriteLine(new string('-', 126));
                    writer.WriteLine("| Parduotuvės pavadinimas: {0, -97} |", branches[i].Name);
                    writer.WriteLine(new String('-', 126));
                    writer.WriteLine("| Parduotuvės adresas: {0, -101} |", branches[i].Address);
                    writer.WriteLine(new String('-', 126));
                    writer.WriteLine("| Parduotuvės telefono numeris: {0, -92} |", branches[i].Phone);
                    writer.WriteLine(new String('-', 126));
                    writer.WriteLine("| {0, -10} | {1, -7} | {2, -5} | {3, -10} | {4, -6} | {5, -9} | {6, -9} | {7, -5} | {8, -7}" +
                                     "| {9, -6} | {10, -5} |", "Gamintojas", "Modelis", "Talpa", "Energijos klasė", "Montavimo tipas",
                                     "Spalva", "Šaldiklis", "Kaina", "Aukštis", "Plotis", "Gylis");
                    writer.WriteLine(new string('-', 126));

                    for (int j = 0; j < branches[i].Refrigerators.Count; j++)
                    {
                        writer.WriteLine("| {0, -10} | {1, -7} | {2, -5} | {3, -15} | {4, -15} | {5, -9} | {6, -9} | {7, -5}" +
                            " | {8, -7} | {9, -5} | {10, -5} |", branches[i].Refrigerators.GetRefrigerator(j).Manufacturer,
                            branches[i].Refrigerators.GetRefrigerator(j).Model, branches[i].Refrigerators.GetRefrigerator(j).Capacity,
                            branches[i].Refrigerators.GetRefrigerator(j).EnergyLabel, branches[i].Refrigerators.GetRefrigerator(j).InstallationType,
                            branches[i].Refrigerators.GetRefrigerator(j).Color, branches[i].Refrigerators.GetRefrigerator(j).IsThereAFreezer,
                            branches[i].Refrigerators.GetRefrigerator(j).Price, branches[i].Refrigerators.GetRefrigerator(j).Height,
                            branches[i].Refrigerators.GetRefrigerator(j).Width, branches[i].Refrigerators.GetRefrigerator(j).Depth);
                        writer.WriteLine(new string('-', 126));
                    }
                }
            }
        }

        /// <summary>
        /// Suranda parduotuvę pagal pavadinimą, adresą ir telefono numerį
        /// </summary>
        /// <param name="branches">Parduotuvių masyvas</param>
        /// <param name="name">Pavadinimas</param>
        /// <param name="address">Adresas</param>
        /// <param name="phone">Telefono numeris</param>
        /// <returns></returns>
        private Branch GetBranchByDifference(Branch[] branches, string name, string address, string phone)
        {
            for (int i = 0; i < NumberOfBranches; i++)
            {
                if (branches[i].Name == name && branches[i].Address == address && branches[i].Phone == phone)
                {
                    return branches[i];
                }
            }
            return null;
        }

        /// <summary>
        /// Išspausdina šaldytuvus į konsolę
        /// </summary>
        /// <param name="refrigerators">Šaldytuvų konteineris</param>
        void PrintRefrigeratorsToConsole(RefrigeratorContainer refrigerators)
        {
            Console.WriteLine("10 pigiausių " + "'" + ChosenInstallationType + "'" + " montavimo tipų šaldytuvų, kurių talpa " +
                              CapacityThreshold + " litrų ar didesnė, sąrašas:\n");

            for (int i = 0; i < refrigerators.Count; i++)
            {
                Console.WriteLine("| Nr. {0, 2} | {1}", (i + 1), refrigerators.GetRefrigerator(i).ToString());
            }
        }

        /// <summary>
        /// Suskaičiuoja nurodyto gamintojo šaldytuvus
        /// </summary>
        /// <param name="branches">Parduotuvių masyvas</param>
        public void CountSiemens(Branch[] branches)
        {
            int addFirst = 0;
            RefrigeratorContainer uniqueM1 = new RefrigeratorContainer(10);
            RefrigeratorContainer uniqueM2 = new RefrigeratorContainer(10);

            for (int i = 0; i < branches.Count(); i++)
            {
                addFirst = 0;
                for (int j = 0; j < branches[i].Refrigerators.Count; j++)
                {
                    if (i == 0)
                    {
                        if (branches[i].Refrigerators.GetRefrigerator(j).Manufacturer == ChosenManufacturer)
                        {
                            if (addFirst == 0)
                            {
                                uniqueM1.AddRefrigerator(branches[i].Refrigerators.GetRefrigerator(j));
                                addFirst++;
                                continue;
                            }
                            for (int z = 0; z < uniqueM1.Count; z++)
                            {
                                if (uniqueM1.GetRefrigerator(z).Equals(branches[i].Refrigerators.GetRefrigerator(j)))
                                {
                                    break;
                                }

                            }
                            uniqueM1.AddRefrigerator(branches[i].Refrigerators.GetRefrigerator(j));
                        }
                    }

                    else if (branches[i].Refrigerators.GetRefrigerator(j).Manufacturer == ChosenManufacturer)
                    {
                        if (addFirst == 0)
                        {
                            uniqueM2.AddRefrigerator(branches[i].Refrigerators.GetRefrigerator(j));
                            addFirst++;
                            continue;
                        }
                        for (int z = 0; z < uniqueM2.Count; z++)
                        {
                            if (uniqueM2.GetRefrigerator(z).Equals(branches[i].Refrigerators.GetRefrigerator(j)))
                            {
                                break;
                            }
                        }
                        uniqueM2.AddRefrigerator(branches[i].Refrigerators.GetRefrigerator(j));
                    }
                }
            }
            PrintModels(uniqueM1, uniqueM2);
        }

        /// <summary>
        /// Išspausdina nurodytos parduotuvės skirtingų šaldytuvų modeliu skaičių
        /// </summary>
        /// <param name="uniqueM1">Pirmos parduotuvės skirtingi šaldytuvai</param>
        /// <param name="uniqueM2">Antros parduotuvės skirtingi šaldytuvai</param>
        public void PrintModels(RefrigeratorContainer uniqueM1, RefrigeratorContainer uniqueM2)
        {
            Console.WriteLine("Skirtingų " + ChosenManufacturer + " šaldytuvų modelių kiekviena parduotuvė siūlo:\n ");
            Console.WriteLine("Elektroluxas: " + uniqueM1.Count);
            Console.WriteLine("SAMSUNGAS: " + uniqueM2.Count);
            Console.WriteLine();
        }

        /// <summary>
        /// Suskaičiuoja 10 pigiausių nurodyto montavimo tipo šaldytuvų, kurių talpa nurodyta litrais
        /// </summary>
        /// <param name="branches">Parduotuvių masyvas</param>
        /// <returns></returns>
        public Branch TenCheapest(Branch[] branches)
        {
            bool dontAdd = false;
            int firstAdd = 0;
            Branch cheapest = new Branch(null,null,null);
            
            for (int i = 0; i < branches.Count(); i++)
            {
                for (int j = 0; j < branches[i].Refrigerators.Count; j++)
                {                 
                    if(branches[i].Refrigerators.GetRefrigerator(j).Capacity >= CapacityThreshold &&
                       branches[i].Refrigerators.GetRefrigerator(j).InstallationType == ChosenInstallationType)
                    {
                        dontAdd = false;
                        if(firstAdd == 0)
                        {
                            cheapest.Refrigerators.AddRefrigerator(branches[i].Refrigerators.GetRefrigerator(j));
                            firstAdd++;
                            continue;
                        }
                        if (i == 1)
                        {
                            for (int k = 0; k < cheapest.Refrigerators.Count; k++)
                            {
                                if (cheapest.Refrigerators.GetRefrigerator(k).Equals(branches[i].Refrigerators.GetRefrigerator(j)) &&
                                    cheapest.Refrigerators.GetRefrigerator(k).Equals(branches[i].Refrigerators.GetRefrigerator(j)))
                                {
                                    dontAdd = true;
                                    break;
                                }
                            }
                            if(dontAdd == false)
                            {
                                cheapest.Refrigerators.AddRefrigerator(branches[i].Refrigerators.GetRefrigerator(j));
                            }                          
                        }
                        else
                            cheapest.Refrigerators.AddRefrigerator(branches[i].Refrigerators.GetRefrigerator(j));
                            dontAdd = false;
                    }
                }
            }
            return cheapest;
        }

        /// <summary>
        /// Surušiuoja pigiausius šaldytuvus pagal kainą
        /// </summary>
        /// <param name="cheapest">Pigiausi šaldytuvai</param>
        /// <returns>Surikiuotu šaldytuvus pagal kainą</returns>
        public RefrigeratorContainer SortCheapest(Branch cheapest)
        {
            Refrigerator temp = new Refrigerator();
            for (int i = 0; i < cheapest.Refrigerators.Count; i++)
            {
                for (int j = 0; j < cheapest.Refrigerators.Count; j++)
                {
                    if (cheapest.Refrigerators.GetRefrigerator(i) <= cheapest.Refrigerators.GetRefrigerator(j))
                    {
                        temp = cheapest.Refrigerators.GetRefrigerator(i);
                        cheapest.Refrigerators.AddRefrigerator(cheapest.Refrigerators.GetRefrigerator(j), i);
                        cheapest.Refrigerators.AddRefrigerator(temp, j);
                    }
                }
            }
            return cheapest.Refrigerators;
        }

        /// <summary>
        /// Suskaičiuoja produktus, kurie yra tik vienoje parduotuvėje
        /// </summary>
        /// <param name="branches">Parduotuvių masyvas</param>
        public void ProductsOnlyInOneBranch(Branch[] branches)
        {
            bool dontProcces = false;
            Branch manufacturersElektro = new Branch("Elektroluxas", "Forto g. 5", "862469534");
            Branch manufacturersSams = new Branch("SAMSUNGAS", "Lekonio g. 64", "861478844");
            bool put = false;

            for (int i = 0; i < branches.Count(); i++)
            {
                if (i == 0)
                {
                    for (int j = 0; j < branches[0].Refrigerators.Count; j++)
                    {
                        dontProcces = false;
                        put = true;
                        for (int k = 0; k < branches[1].Refrigerators.Count; k++)
                        {
                            if (branches[0].Refrigerators.GetRefrigerator(j).Manufacturer == (branches[1].Refrigerators.GetRefrigerator(k).Manufacturer))
                            {
                                dontProcces = true;
                                break;
                            }
                        }
                        if (dontProcces == false)
                        {
                            for (int k = 0; k < manufacturersElektro.Refrigerators.Count; k++)
                            {
                                if (manufacturersElektro.Refrigerators.GetRefrigerator(k).Manufacturer == branches[0].Refrigerators.GetRefrigerator(j).Manufacturer)
                                {
                                    put = false;
                                    break;
                                }
                            }
                            if (put == true)
                            {
                                manufacturersElektro.Refrigerators.AddRefrigerator(branches[0].Refrigerators.GetRefrigerator(j));
                            }
                        }
                    }
                }
                else
                    for (int j = 0; j < branches[1].Refrigerators.Count; j++)
                    {
                        dontProcces = false;
                        put = true;
                        for (int k = 0; k < branches[0].Refrigerators.Count; k++)
                        {
                            if (branches[1].Refrigerators.GetRefrigerator(j).Manufacturer == (branches[0].Refrigerators.GetRefrigerator(k).Manufacturer))
                            {
                                dontProcces = true;
                                break;
                            }
                        }
                        if (dontProcces == false)
                        {
                            for (int k = 0; k < manufacturersSams.Refrigerators.Count; k++)
                            {
                                if (manufacturersSams.Refrigerators.GetRefrigerator(k).Manufacturer == branches[1].Refrigerators.GetRefrigerator(j).Manufacturer)
                                {
                                    put = false;
                                    break;
                                }
                            }
                            if (put == true)
                            {
                                manufacturersSams.Refrigerators.AddRefrigerator(branches[1].Refrigerators.GetRefrigerator(j));
                            }
                        }
                    }
            }
            PrintUnique(manufacturersElektro, manufacturersSams);
        }

        /// <summary>
        /// Įrašo į failą gamintojus, kurių produktus galima isigyti tik vienoje parduotuvėje
        /// </summary>
        /// <param name="manufacturersElektro">Pirmos parduotuvės gamintojai</param>
        /// <param name="manufacturersSams">Antros parduotuvės gamintojai</param>
        public void PrintUnique(Branch manufacturersElektro, Branch manufacturersSams)
        {
            using (StreamWriter writer = new StreamWriter(@"TikTen.csv", false, Encoding.UTF8))
            {
                if (manufacturersElektro.Refrigerators.Count >= 1)
                {
                    writer.WriteLine("Parduotuvė:");
                    writer.WriteLine(manufacturersElektro.Name);
                    writer.WriteLine("Gamintojai:");
                    for (int i = 0; i < manufacturersElektro.Refrigerators.Count; i++)
                    {
                        writer.WriteLine(manufacturersElektro.Refrigerators.GetRefrigerator(i).Manufacturer);
                    }
                }
                if (manufacturersSams.Refrigerators.Count >= 1)
                {
                    writer.WriteLine();
                    writer.WriteLine("Parduotuvė:");
                    writer.WriteLine(manufacturersSams.Name);
                    writer.WriteLine("Gamintojai:");
                    for (int i = 0; i < manufacturersSams.Refrigerators.Count; i++)
                    {
                        writer.WriteLine(manufacturersSams.Refrigerators.GetRefrigerator(i).Manufacturer);
                    }
                }
            }
        }

        /// <summary>
        /// Suskaičiuoja visų parduotuvių gamintojus
        /// </summary>
        /// <param name="branches">Parduotuvių masyvas</param>
        /// <returns></returns>
        public static RefrigeratorContainer ManufacturerList(Branch[] branches)
        {
            RefrigeratorContainer manufacturers = new RefrigeratorContainer(10);
            bool put = true;

            for (int i = 0; i < branches.Count(); i++)
            {
                for (int j = 0; j < branches[i].Refrigerators.Count; j++)
                {
                    put = true;
                    for (int k = 0; k < manufacturers.Count; k++)
                    {
                        if (manufacturers.GetRefrigerator(k).Manufacturer == branches[i].Refrigerators.GetRefrigerator(j).Manufacturer)
                        {
                            put = false;
                            break;
                        }
                    }
                    if (put == true)
                    {
                        manufacturers.AddRefrigerator(branches[i].Refrigerators.GetRefrigerator(j));
                    }
                }
            }
            return manufacturers;
        } 

        /// <summary>
        /// Įrašo į failą visų parduotuvių gamintojus
        /// </summary>
        /// <param name="manufacturers">Šaldytuvų gamintojų konteineris</param>
        public void WriteManufacturersToFile(RefrigeratorContainer manufacturers)
        {
            using (StreamWriter writer = new StreamWriter(@"Gamintojai.csv", false, Encoding.UTF8))
            {
                writer.WriteLine("Gamintojai:");
                for (int i = 0; i < manufacturers.Count; i++)
                {
                    writer.WriteLine(manufacturers.GetRefrigerator(i).Manufacturer);
                }
            }
        }
    }
}
