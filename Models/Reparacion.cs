public class Reparacion
{
    public string Id { get; init; }
    
    public Reparacion(string id)
    {
        Id = id;
    }

    public override string ToString()
    {
        return $"Id Reparacion: {this.Id}";
    }

}