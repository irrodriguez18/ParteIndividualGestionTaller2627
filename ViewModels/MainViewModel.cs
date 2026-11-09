using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using New.Models;
using New.Views;

namespace New.ViewModels;

public enum EntidadAMostrar{
    None,
    Personal,
    Reparacion
}

public partial class MainViewModel : ViewModelBase
{
    private ContenedorDatos _datosTaller;

    [ObservableProperty]
    private ViewModelBase? _tablaActiva;
    private EntidadAMostrar _entidadSeleccionada = EntidadAMostrar.None;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(TablaActiva))]
    private string _textoBusqueda = string.Empty;

    public MainViewModel()
    {
        _datosTaller = new();
        SeleccionarEntidad(EntidadAMostrar.Personal);
    }

    [RelayCommand]
    private void SeleccionarEntidad(EntidadAMostrar entidadNueva)
    {
        if (!_entidadSeleccionada.Equals(entidadNueva))
        {
            _entidadSeleccionada = entidadNueva;
            TextoBusqueda = string.Empty;

            switch (_entidadSeleccionada)
            {
                case EntidadAMostrar.Personal:
                    //TODO Hacer búsqueda por cada campo, no sólo por DNI
                    IEnumerable<Personal> listaPersonalFiltrado = _datosTaller.ListaPersonal;
                    if(!string.IsNullOrEmpty(TextoBusqueda)){
                        listaPersonalFiltrado = [.. listaPersonalFiltrado.Where( p => p.Dni.Contains(TextoBusqueda.Trim()) )];
                    }

                    //Crear DTOs
                    List<PersonalAsignadoDTO> listaPersonalAsignado = [.. listaPersonalFiltrado.Select(
                        p => new PersonalAsignadoDTO
                        {
                            Dni = p.Dni,
                            Nombre = p.Nombre,
                            Email = p.Email,
                            ReparacionesAsignadas = [.. _datosTaller.ListaAsignaciones
                                                        .Where (a => a.Dni.Equals(p.Dni))
                                                        .Select(a => a.IdReparacion)]
                        })];
                    TablaActiva = new TablaPersonalViewModel(listaPersonalAsignado);
                    break;
                default: //Reparacion
                    IEnumerable<Reparacion> listaReparacionFiltrada = _datosTaller.ListaReparaciones;
                    if(!string.IsNullOrEmpty(TextoBusqueda)){
                        listaReparacionFiltrada = [.. listaReparacionFiltrada.Where( r => r.Id.Contains(TextoBusqueda.Trim()) )];
                    }

                    List<ReparacionDTO> listaReparacionDTO = [.. listaReparacionFiltrada.Select( r => new ReparacionDTO
                    {
                        Id = r.Id
                    })];
                    TablaActiva = new TablaReparacionesViewModel(listaReparacionDTO);
                    break;
            }
        }
    }

    [RelayCommand]
    private void GuardarDatos()
    {
        List<PersonalDTO> personal = [.. _datosTaller.ListaPersonal.Select( p => new PersonalDTO
        {
            Dni = p.Dni,
            Nombre = p.Nombre,
            Email = p.Email
        } )];

        List<ReparacionDTO> reparaciones = [.. _datosTaller.ListaReparaciones.Select( r => new ReparacionDTO
        {
            Id = r.Id
        } )];

        List<AsignacionReparacionDTO> asignaciones = [.. _datosTaller.ListaAsignaciones
            .Select( a => new AsignacionReparacionDTO
        {
            Dni = a.Dni,
            IdReparacion = a.IdReparacion
        } )];

        ContenedorDatosDTO datosTaller = new(personal, reparaciones, asignaciones);

        PersistenciaXML.Guardar<ContenedorDatosDTO>("datosTaller.xml", datosTaller);
    }

    [RelayCommand]
    private void CargarDatos(string? rutaArchivo = null)
    {
        ContenedorDatosDTO datosEnMemoria;

        try
        {
            if (string.IsNullOrEmpty(rutaArchivo))
            {
                datosEnMemoria = new();
            }
            else
            {
                datosEnMemoria = PersistenciaXML.Cargar<ContenedorDatosDTO>(rutaArchivo);
            }

            _datosTaller = datosEnMemoria.GetContenedorDatos();
            TextoBusqueda = string.Empty;
            RefrescarTabla();
        }catch(Exception) {}
        
    }

    [RelayCommand]
    private async Task MostrarFormEntidad(PersonalAsignadoDTO? p)
    {
        FormularioPersonalViewModel formModal;

        if(p != null) formModal = new(_datosTaller, p);
        else formModal = new(_datosTaller);

        var modal = new FormularioModalWindow { DataContext = formModal };
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop && desktop.MainWindow != null)
        {
            var resultado = await modal.ShowDialog<bool>(desktop.MainWindow);
            
            PersonalAsignadoDTO nuevoP = formModal.MiPersonal;
            if (resultado && formModal.MiPersonal != null)
            {
                //Si pasaron personal => Update. Sino => Add
                if (p != null){
                    _datosTaller.ActualizarPersonal(nuevoP.Dni, nuevoP.Nombre,
                                    nuevoP.Email, nuevoP.ReparacionesAsignadas);
                }
                else{
                    _datosTaller.AñadirPersonal(nuevoP.Dni, nuevoP.Nombre,
                                    nuevoP.Email, nuevoP.ReparacionesAsignadas);
                }
            }

            RefrescarTabla();
        }
    }

    [RelayCommand]
    public void EliminarPersonalCommand(PersonalAsignadoDTO p)
    {
        _datosTaller.EliminarPersonal(p.Dni);
        RefrescarTabla();
    }

    [RelayCommand]
    public void EliminarReparacionCommand(ReparacionDTO r)
    {
        _datosTaller.EliminarReparacion(r.Id);
        RefrescarTabla();
    }

    private void RefrescarTabla()
    {
        var temp = _entidadSeleccionada;
        _entidadSeleccionada = EntidadAMostrar.None;
        SeleccionarEntidad(temp);
    }

}
