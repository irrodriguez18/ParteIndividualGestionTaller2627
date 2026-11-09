using System;
using System.Text.RegularExpressions;

public enum CodigoValidacion{
    
    Ok,
    
    //DNI
    DniInvalidLength,
    DniInvalidFormat,
    DniInvalidLetter,
    DniDuplicated,

    //Nombre Personal
    NombreEmpty,

    //Email
    EmailEmpty,
    EmailInvalidFormat,
    EmailDuplicated,

    //Personal
    PersonalNotFound,
    PersonalDuplicated,
    PersonalModificationSameValues,

    //Reparaciones
    ReparacionNotFound,

    //AsignacionReparaciones
    AsignPersRepDuplicada,
    AsignPersRepPersonalNoExiste,
    AsignPersRepReparacionNoExiste,
    AsignPersRepNotFound,

    //Unexpected
    PersonalDeleteUnexpected,
    ReparacionDeleteUnexpected

}

public class ValidadorComun
{
    public static CodigoValidacion ComprobarDni(string dni){
        //9 dígitos
        if (string.IsNullOrEmpty(dni) || dni.Length != 9)
        {
            return CodigoValidacion.DniInvalidLength;
        }

        //Comprobar primero 8 números, luego 1 letra entre las permitidas
        string patronRegex = "^[0-9]{8}[a-zA-Z]$";
        if(!Regex.IsMatch(dni, patronRegex)){
            return CodigoValidacion.DniInvalidFormat;
        }

        //Extraer y guardar parte número de parte letra
        int parteNumero = Int32.Parse(dni.Substring(0, 8));
        string parteLetra = dni.Substring(8).ToUpper();

        //Comprobar letra DNI (módulo 23)
        string letrasDni = "TRWAGMYFPDXBNJZSQVHLCKE";
        int temp = parteNumero%23;
        
        if(!letrasDni.Substring(temp, 1).Equals(parteLetra)){
            return CodigoValidacion.DniInvalidLetter;
        }

        return CodigoValidacion.Ok;
    }

    public static CodigoValidacion ComprobarNombre(string nombre){
        //Comprobar vacío
        if (string.IsNullOrEmpty(nombre))
        {
            return CodigoValidacion.NombreEmpty;
        }
        
        return CodigoValidacion.Ok;
    }

    public static CodigoValidacion ComprobarEmail(string email){
        //Comprobar vacío
        if (string.IsNullOrEmpty(email))
        {
            return CodigoValidacion.EmailEmpty;
        }

        //Comprobar formato (cabeza @ cola . extensión)
        string patronRegex = @"^[^\s@]+@[^\s@]+\.[^\s@]+$";
        if(!Regex.IsMatch(email, patronRegex)){
            return CodigoValidacion.EmailInvalidFormat;
        }
        
        return CodigoValidacion.Ok;
    }
}