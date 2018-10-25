namespace L2_U2_10
{
    /// <summary>
    /// Klasė, kuri yra skirta parduotuvės informacijai aprašyti
    /// </summary>
    class Branch
    {
        public const int MaxNumberOfRefrigerator = 100; //Didžiausias šaldytuvų skaičius parduotuvėje

        public string Name { get; set; } //Pavadinimas
        public string Address { get; set; } //Adresas
        public string Phone { get; set; } //Telefono numeris
        public RefrigeratorContainer Refrigerators { get; private set; } //Šaldytuvų sąrašas

        /// <summary>
        /// Parametrų konstruktorius
        /// </summary>
        /// <param name="name"></param>
        /// <param name="address"></param>
        /// <param name="phone"></param>
        public Branch(string name, string address, string phone)
        {
            Name = name;
            Address = address;
            Phone = phone;
            Refrigerators = new RefrigeratorContainer(MaxNumberOfRefrigerator);
        }
    }
}
