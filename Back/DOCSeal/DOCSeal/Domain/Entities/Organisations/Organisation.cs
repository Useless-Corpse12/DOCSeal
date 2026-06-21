namespace DOCSeal.Domain.Entities.Organisations;

public class Organisation : Entity
{
    public List<Guid> Employees  { get; set; }
    public string Name { get; set; }

    public Organisation(Guid id,List<Guid> employees, string name) :  base(id)
    {
        Employees = employees;
        Name = name;
    }
}