namespace WpfApp1.Model
{
    internal struct EntranceInfoStruct
    {
        public string DateOfEntrance { get; set; }
        public string City { get; set; }
        public string Country { get; set; }

        public EntranceInfoStruct(string dateOfEntrance, string city, string country)
        {
            DateOfEntrance = dateOfEntrance;
            City = city;
            Country = country;
        }
    }
}
