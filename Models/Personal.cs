public class Personal
{
    public string Dni { get; init; }
    public string Nombre { get; private set; }
    public string Email { get; private set; }
    
    public Personal(string dni, string nombre, string email)
    {
        Dni = dni;
        Nombre = nombre;
        Email = email;
    }

    public void ActualizarDatos(string nuevoNombre, string nuevoEmail)
    {
        Nombre = nuevoNombre.Trim();
        Email = nuevoEmail.Trim();
    }

    public override string ToString()
    {
        return $"Dni: {this.Dni}, Nombre: {this.Nombre}, Email: {this.Email}";
    }
}