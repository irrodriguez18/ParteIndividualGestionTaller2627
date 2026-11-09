using System.Collections.Generic;
using System.Linq;
public class ContenedorDatos{

    private List<Personal> _listaPersonal;
    private List<Reparacion> _listaReparaciones;
    private List<AsignacionReparacion> _listaAsignaciones;

    public IReadOnlyList<Personal> ListaPersonal => _listaPersonal;
    public IReadOnlyList<Reparacion> ListaReparaciones => _listaReparaciones;
    public IReadOnlyList<AsignacionReparacion> ListaAsignaciones => _listaAsignaciones;

    public ContenedorDatos()
    {
        this._listaPersonal = [];
        this._listaReparaciones = [];
        this._listaAsignaciones = []; 
    }
    
    public ContenedorDatos(List<Personal> personal,
                           List<Reparacion> reparaciones,
                           List<AsignacionReparacion> asignaciones)
    {
        this._listaPersonal = personal;
        this._listaReparaciones = reparaciones;
        this._listaAsignaciones = asignaciones; 
    }

    public CodigoValidacion AñadirPersonal(string dni, string nombre, string email, List<string>? reparacionesAsignadas)
    {
        CodigoValidacion temp = ValidadorComun.ComprobarDni(dni);
        if(!temp.Equals(CodigoValidacion.Ok)){
            return temp;
        }

        IQueryable<Personal> query = _listaPersonal.AsQueryable();
        if(query.Where( p => p.Dni.Equals(dni)).ToList().Count > 0){
            return CodigoValidacion.DniDuplicated;
        }

        temp = ValidadorComun.ComprobarNombre(nombre);
        if(!temp.Equals(CodigoValidacion.Ok)){
            return temp;
        }

        temp = ValidadorComun.ComprobarEmail(email);
        if(!temp.Equals(CodigoValidacion.Ok)){
            return temp;
        }

        if(query.Where( p => p.Email.Equals(email)).ToList().Count > 0){
            return CodigoValidacion.EmailDuplicated;
        }

        _listaPersonal.Add(new Personal(dni, nombre.Trim(), email));

        if (reparacionesAsignadas != null)
        {
            foreach (string idReparacion in reparacionesAsignadas)
            {
                AñadirAsignacion(dni, idReparacion);
            }
        }

        return CodigoValidacion.Ok;
    }

    public CodigoValidacion EliminarPersonal(string dni)
    {
        List<Personal> queryList = [.. _listaPersonal.AsQueryable().Where( p => p.Dni.Equals(dni) )];

        if (queryList.Count == 0) {return CodigoValidacion.PersonalNotFound;}
        
        _listaAsignaciones.RemoveAll( a => a.Dni.Equals(dni) );

        if (_listaPersonal.RemoveAll( p => p.Dni.Equals(dni) ) > 0) {return CodigoValidacion.Ok;}
        else {return CodigoValidacion.PersonalDeleteUnexpected;}
    }

    public CodigoValidacion ActualizarPersonal(string dni, string nombre, string email, List<string> reparacionesAsignadas)
    {
        IQueryable<Personal> query = _listaPersonal.AsQueryable();

        //Comprobar que existe Personal y es único
        List<Personal> queryList = [.. query.Where( p => p.Dni.Equals(dni))];
        if (queryList.Count == 0) {return CodigoValidacion.PersonalNotFound;}
        if (queryList.Count > 1) {return CodigoValidacion.PersonalDuplicated;}

        //Comprobar que al menos 1 atributo es distinto a los originales
        Personal datosActuales = queryList[0];
        List<string> reparacionesActuales = [.. _listaAsignaciones.Where( a => a.Dni.Equals(dni) )
                                                .Select( a => a.IdReparacion)
                                                .OrderBy( a => a ) ];
        if (datosActuales.Nombre.Equals(nombre)
            && datosActuales.Email.Equals(email)
            && reparacionesActuales.SequenceEqual(reparacionesAsignadas.OrderBy( a => a )))
        {
            return CodigoValidacion.PersonalModificationSameValues;
        }

        CodigoValidacion temp = ValidadorComun.ComprobarNombre(nombre);
        if (!temp.Equals(CodigoValidacion.Ok)) return temp;
        
        //Si Email nuevo == Email actual => OK
        // Si Email nuevo == Email de OTRO usuario => MAL, EmailDuplicated
        if (!datosActuales.Email.Equals(email))
        {
            if(queryList.Where( p => p.Email.Equals(email)).ToList().Count != 0)
            {
                return CodigoValidacion.EmailDuplicated;
            }

            temp = ValidadorComun.ComprobarEmail(email);
            if(!temp.Equals(CodigoValidacion.Ok)) return temp;
        }

        //Eliminar instancia de Personal actual y añadir otra con los datos nuevos
        _listaPersonal.Remove(datosActuales);
        _listaPersonal.Add(new Personal(dni, nombre, email));

        //TODO Cambiar asignaciones

        return CodigoValidacion.Ok;
    }

    public CodigoValidacion AñadirReparacion(string idReparacion)
    {
        _listaReparaciones.Add(new Reparacion(idReparacion));
        return CodigoValidacion.Ok;
    }

    public CodigoValidacion EliminarReparacion(string idReparacion)
    {
        List<Reparacion> queryList = [.. _listaReparaciones.AsQueryable()
            .Where( r => r.Id.Equals(idReparacion))];

        if (queryList.Count == 0)
        {
            return CodigoValidacion.ReparacionNotFound;
        }

        _listaAsignaciones.RemoveAll( a => a.IdReparacion.Equals(idReparacion) );

        _listaReparaciones.Remove(queryList[0]);

        if (_listaReparaciones.RemoveAll( p => p.Id.Equals(idReparacion) ) > 0) {return CodigoValidacion.Ok;}
        else {return CodigoValidacion.ReparacionDeleteUnexpected;}
    }

    public CodigoValidacion ActualizarReparacion(string idReparacion)
    {
        return CodigoValidacion.Ok;
    }

    private CodigoValidacion AñadirAsignacion(string dni, string idReparacion)
    {
        AsignacionReparacion asignTemp = new(dni, idReparacion);

        if (_listaAsignaciones.Contains(asignTemp))
        {
            return CodigoValidacion.AsignPersRepDuplicada;
        }

        _listaAsignaciones.Add(asignTemp);
        return CodigoValidacion.Ok;
    }
}