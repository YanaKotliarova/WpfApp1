namespace WpfApp1.Model.Interfaces
{
    internal interface IUser
    {
        string City { get; set; }
        string Country { get; set; }
        DateOnly Date { get; set; }
        string FirstName { get; set; }
        int Id { get; set; }
        string LastName { get; set; }
        string Patronymic { get; set; }
    }
}