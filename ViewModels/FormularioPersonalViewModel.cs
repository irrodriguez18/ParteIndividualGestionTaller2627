using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using New.Models;
using New.Utilities;

namespace New.ViewModels;

public partial class FormularioPersonalViewModel : ViewModelBase, IModalFormularioViewModel
{
    [ObservableProperty]
    private string _dni = string.Empty;
    [ObservableProperty]
    private string _nombre = string.Empty;
    [ObservableProperty]
    private string _email = string.Empty;
    [ObservableProperty]
    private List<SeleccionReparacion> _reparacionesAsignadas;

    public PersonalAsignadoDTO MiPersonal { get; private set; } = new();

    public FormularioPersonalViewModel(ContenedorDatos datosTaller, PersonalAsignadoDTO? p = null)
    {
        ReparacionesAsignadas = [.. datosTaller.ListaReparaciones.Select(
                a => new SeleccionReparacion{
                     IdReparacion = a.Id,
                     Seleccionada = false
                } )];

        if (p != null)
        {
            for (int i=0; i<ReparacionesAsignadas.Count; i++)
            {
                if (p.ReparacionesAsignadas.Equals(ReparacionesAsignadas[i].IdReparacion))
                {
                    ReparacionesAsignadas[i] =
                        new SeleccionReparacion
                        {
                            IdReparacion = ReparacionesAsignadas[i].IdReparacion,
                            Seleccionada = true
                        };
                }
            }

            Dni = p.Dni;
            Nombre = p.Nombre;
            Email = p.Email;
        }
    }

    [RelayCommand]
    private void GuardarCambios(Window? window)
    {
        MiPersonal = new PersonalAsignadoDTO
        {
            Dni = this.Dni,
            Nombre = this.Nombre,
            Email = this.Email,
            ReparacionesAsignadas = [.. ReparacionesAsignadas
                                        .Where( r => r.Seleccionada )
                                        .Select( r => r.IdReparacion )]
        };
        window?.Close(true);
    }

    [RelayCommand]
    private void Cancelar(Window? window)
    {
        window?.Close(false);
    }
}