namespace DOCSeal.Domain.Entities;

public class Organisation : Entity
{
    public string Name { get; set; }
    public List<string> PossiblePositions { get; set; }

    public Organisation(Guid id, string name) :  base(id)
    {
        Name = name;
        PossiblePositions = new List<string> { "Big Boss","Employee"};
    }
    
    protected Organisation()
    {}
}