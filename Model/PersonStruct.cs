namespace WpfApp1.Model
{
    internal struct PersonStruct
    {
        internal string FirstName { get; set; }
        internal string LastName { get; set; }
        internal string Patronymic { get; set; }

        internal PersonStruct(string firstName, string lastName, string patronymic)
        {
            FirstName = firstName;
            LastName = lastName;
            Patronymic = patronymic;
        }
    }
}
