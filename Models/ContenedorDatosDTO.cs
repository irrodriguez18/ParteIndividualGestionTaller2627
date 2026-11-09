using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using New.Models;

[XmlRoot("DatosTaller")]
public class ContenedorDatosDTO{

    [XmlArray("ListaPersonal")]
    [XmlArrayItem("Personal")]
    public List<PersonalDTO> ListaPersonal { get; set; }

    [XmlArray("ListaReparaciones")]
    [XmlArrayItem("Reparacion")]
    public List<ReparacionDTO> ListaReparaciones { get; set; }

    [XmlArray("ListaAsignaciones")]
    [XmlArrayItem("Asignacion")]
    public List<AsignacionReparacionDTO> ListaAsignaciones { get; set; }

    public ContenedorDatosDTO()
    {
        ListaPersonal = [];
        ListaReparaciones = [];
        ListaAsignaciones = [];
    }

    public ContenedorDatosDTO(List<PersonalDTO> personal,
        List<ReparacionDTO> reparaciones, List<AsignacionReparacionDTO> asignaciones)
    {
        ListaPersonal = personal;
        ListaReparaciones = reparaciones;
        ListaAsignaciones = asignaciones;
    }

    public ContenedorDatos GetContenedorDatos()
    {
        List<Personal> personal = [.. ListaPersonal.Select( p => p.GetPersonal())];
        List<Reparacion> reparaciones = [.. ListaReparaciones.Select( r => r.GetReparacion())];
        List<AsignacionReparacion> asignaciones = [.. ListaAsignaciones.Select( a => a.GetAsignacion())];

        return new ContenedorDatos(personal, reparaciones, asignaciones);
    }
}