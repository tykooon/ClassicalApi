namespace ClassicalApi.Core.Models;

public class Composer
{
    public int Id { get; set; }
    public string LastName { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public int? YearOfBirth { get; set; }
    public int? YearOfDeath { get; set; }
    public string CountryOfBirth { get; set; } = string.Empty;
    public string CityOfBirth { get; set; } = string.Empty;
    public string ShortBio { get; set; } = string.Empty;
}
