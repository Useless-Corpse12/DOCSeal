namespace DOCSeal.Domain.Entities.Organisations;

public class Organisation : Entity
{
    public List<Guid> Employees  { get; set; }
    public string Name { get; set; }
    public string? ImageUrl { get; set; }

    public Organisation(Guid id,List<Guid> employees, string name, string imageUrl) :  base(id)
    {
        Employees = employees;
        Name = name;
        ImageUrl = imageUrl;
    }
}