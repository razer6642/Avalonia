using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace MyAvaloniaApp.ViewModels;

public partial class LoginViewModel : ViewModelBase
{
    private readonly MainViewModel _main;

    [ObservableProperty]
    public partial string Usuario { get; set; } = "";

    [ObservableProperty]
    public partial string Contrasena { get; set; } = "";

    [ObservableProperty]
    public partial string MensajeError { get; set; } = "";

    public LoginViewModel(MainViewModel main)
    {
        _main = main;
    }

    [RelayCommand]
    private void IniciarSesion()
    {
        if (Usuario == "admin" && Contrasena == "1234")
        {
            MensajeError = "";
            _main.MostrarInicio();
        }
        else
        {
            MensajeError = "Usuario o contraseña incorrectos.";
        }
    }
}