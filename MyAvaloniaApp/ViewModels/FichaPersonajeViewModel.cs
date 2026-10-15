using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyAvaloniaApp.Models;
using MyAvaloniaApp.Services;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MyAvaloniaApp.ViewModels;

public partial class FichaPersonajeViewModel : ViewModelBase
{
    private readonly MainViewModel _main;

    public FichaPersonajeViewModel(MainViewModel main, Personaje personaje)
    {
        _main = main;
        Personaje = personaje;
    }

    public Personaje Personaje { get; }

    [ObservableProperty]
    public partial string Mensaje { get; set; } = "";

    [ObservableProperty]
    public partial string MensajeExito { get; set; } = "";

    [RelayCommand]
    private void Volver() => _main.MostrarPersonajes();

    [RelayCommand]
    private void Editar() => _main.MostrarEditarPersonaje(Personaje);

    [RelayCommand]
    private void ExportarPdf()
    {
        Mensaje = "";
        MensajeExito = "";

        string ruta;
        try
        {
            ruta = new PdfService().ExportarFicha(Personaje);
        }
        catch (Exception)
        {
            Mensaje = "No se pudo generar el PDF.";
            return;
        }

        MensajeExito = $"PDF guardado en: {ruta}";

        try
        {
            Process.Start(new ProcessStartInfo(ruta) { UseShellExecute = true });
        }
        catch (Exception)
        {
        }
    }

    [RelayCommand]
    private async Task Borrar()
    {
        try
        {
            var ok = await _main.Api.BorrarPersonajeAsync(Personaje.Id);

            if (!ok)
            {
                Mensaje = "No se pudo borrar el personaje (puede que ya no exista).";
                return;
            }

            await _main.CargarPersonajesAsync();
            _main.MostrarPersonajes();
        }
        catch (Exception)
        {
            Mensaje = "No se pudo conectar con la API.";
        }
    }
}