using System.Xml.Serialization;

[XmlRoot("Personal")]
public class PersonalDTO
{
    [XmlElement("Dni")]
    public string Dni { get; set; } = string.Empty;

    [XmlElement("Nombre")]
    public string Nombre { get; set; } = string.Empty;

    [XmlElement("Email")]
    public string Email { get; set; } = string.Empty;

    //Así lo requiere el XmlSerializer, al igual que los get y set públicos
    public PersonalDTO() {}

    public PersonalDTO(Personal p)
    {
        this.Dni = p.Dni;
        this.Nombre = p.Nombre;
        this.Email = p.Email;
    }

    public Personal GetPersonal()
    {
        return new Personal(Dni, Nombre, Email);
    }
}