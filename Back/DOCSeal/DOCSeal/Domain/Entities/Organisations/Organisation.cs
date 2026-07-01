namespace DOCSeal.Domain.Entities;

public class Organisation : Entity
{
    public string Name { get; set; }
    public List<string> PossiblePositions { get; set; }

    public Organisation(Guid id, string name,List<string> possiblePositions) :  base(id)
    {
        Name = name;
        PossiblePositions = possiblePositions;
    }
    
    protected Organisation()
    {}
}