namespace DOCSeal.Domain.Entities.Organisations;

public class Organisation : Entity
{
    public List<Guid> Employees  { get; set; }
    public string Name { get; set; }
    public string? ImageUrl { get; set; }

    public Organisation(List<Guid> employees, string name, string imageUrl)
    {
        Employees = employees;
        Name = name;
        ImageUrl = imageUrl;
    }
}