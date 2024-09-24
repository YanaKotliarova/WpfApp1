namespace WpfApp1.Model
{
    internal struct EntranceInfoStruct
    {
        internal string DateOfEntrance {  get; set; }
        internal string City {  get; set; }
        internal string Country { get; set; }

        internal EntranceInfoStruct(string dateOfEntrance, string city, string country)
        {
            DateOfEntrance = dateOfEntrance;
            City = city;
            Country = country;
        }
    }
}
