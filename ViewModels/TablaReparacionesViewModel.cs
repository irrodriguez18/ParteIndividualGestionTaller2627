using System.Collections.Generic;
using System.Collections.ObjectModel;
using New.Models;

namespace New.ViewModels;

public partial class TablaReparacionesViewModel : ViewModelBase
{
    public ObservableCollection<ReparacionDTO> ListaDatosFiltrada { get; }

    public TablaReparacionesViewModel(List<ReparacionDTO> lista)
    {
        ListaDatosFiltrada = new(lista);
    }
}