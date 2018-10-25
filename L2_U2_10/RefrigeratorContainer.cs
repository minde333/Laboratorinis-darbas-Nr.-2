namespace L2_U2_10
{
    class RefrigeratorContainer
    {
        private Refrigerator[] Refrigerators; //Masyvas pagal klasės Refrigerator šabloną
        public int Count { get; private set; } //Masyve esančių elementų skaičius

        /// <summary>
        /// Parametrų konstruktorius
        /// </summary>
        /// <param name="size"></param>
        public RefrigeratorContainer(int size)
        {
            Refrigerators = new Refrigerator[size];
            Count = 0;
        }

        /// <summary>
        /// Prideda šaldytuvas į masyvą
        /// </summary>
        /// <param name="refrigerators">Pagal klasės Refrigerator šabloną aprašytas šaldytuvas</param>
        public void AddRefrigerator(Refrigerator refrigerators)
        {
            Refrigerators[Count++] = refrigerators;
        }

        /// <summary>
        /// Prideda šaldytuvą į parduotuvę pagal nurodytą indeksą
        /// </summary>
        /// <param name="refrigerators">Pagal klasės Refrigerator šabloną aprašytas šaldytuvas</param>
        /// <param name="index">Elemento vieta masyve</param>
        public void AddRefrigerator(Refrigerator refrigerators, int index)
        {
            Refrigerators[index] = refrigerators;
        }

        /// <summary>
        /// Paimamas šaldytuvas iš masyvo pagal nurodytą indeksą
        /// </summary>
        /// <param name="index">Elemento vieta masyve</param>
        /// <returns></returns>
        public Refrigerator GetRefrigerator(int index)
        {
            return Refrigerators[index];
        }
    }
}
