using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace MyAvaloniaApp.ViewModels;

public partial class InicioViewModel : ViewModelBase
{
    private readonly MainViewModel _main;

    public InicioViewModel(MainViewModel main)
    {
        _main = main;
    }
    [RelayCommand]
    private void IrAPersonajes() => _main.MostrarPersonajes();

    [RelayCommand]
    private void CerrarSesion() => _main.MostrarLogin();
}