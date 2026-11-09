using CommunityToolkit.Mvvm.ComponentModel;

namespace New.Utilities;

public partial class SeleccionReparacion : ObservableObject
{
    public string IdReparacion { get; set; } = string.Empty;

    [ObservableProperty]
    private bool _seleccionada;
}