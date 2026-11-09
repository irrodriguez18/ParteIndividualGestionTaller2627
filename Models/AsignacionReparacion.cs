public class AsignacionReparacion
{
    public string Dni { get; init;}
    public string IdReparacion { get; init;}
    
    public AsignacionReparacion(string dni, string idReparacion)
    {
        Dni = dni;
        IdReparacion = idReparacion;
    }

    public override string ToString()
    {
        return $"{Dni} -> {IdReparacion}";
    }

}