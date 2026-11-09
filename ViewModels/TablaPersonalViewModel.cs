using System.Collections.Generic;
using System.Collections.ObjectModel;
using New.Models;

namespace New.ViewModels;

public partial class TablaPersonalViewModel : ViewModelBase
{
    public ObservableCollection<PersonalAsignadoDTO> ListaDatosFiltrada { get; }

    public TablaPersonalViewModel(List<PersonalAsignadoDTO> lista)
    {
        ListaDatosFiltrada = new(lista);
    }
}