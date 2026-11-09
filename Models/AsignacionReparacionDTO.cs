using System.Xml.Serialization;

[XmlRoot("AsignacionReparacion")]
public class AsignacionReparacionDTO
{
    [XmlElement("Dni")]
    public string Dni { get; set; } = string.Empty;

    [XmlElement("IdReparacion")]
    public string IdReparacion { get; set; } = string.Empty;

    //Así lo requiere el XmlSerializer, al igual que los get y set públicos
    public AsignacionReparacionDTO() {}

    public AsignacionReparacionDTO(AsignacionReparacion a)
    {
        this.Dni = a.Dni;
        this.IdReparacion = a.IdReparacion;
    }

    public AsignacionReparacion GetAsignacion()
    {
        return new AsignacionReparacion(Dni, IdReparacion);
    }
}