using System.Collections.Generic;

namespace New.Models;
public class PersonalAsignadoDTO
{
    public string Dni { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public List<string> ReparacionesAsignadas = [];
    public string TextoAsignaciones => string.Join(", ", ReparacionesAsignadas);
}