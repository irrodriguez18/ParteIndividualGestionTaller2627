using System.Xml.Serialization;

namespace New.Models;

[XmlRoot("Reparacion")]
public class ReparacionDTO
{
    [XmlElement("Id")]
    public string Id { get; set; } = string.Empty;

    //Así lo requiere el XmlSerializer, al igual que los get y set públicos
    public ReparacionDTO() {}

    public ReparacionDTO(Reparacion r)
    {
        this.Id = r.Id;
    }

    public Reparacion GetReparacion()
    {
        return new Reparacion(Id);
    }
}