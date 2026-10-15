using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyAvaloniaApp.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace MyAvaloniaApp.ViewModels;

public partial class AgregarPersonajeViewModel : ViewModelBase
{
    private readonly MainViewModel _main;
    private readonly Personaje? _editando;

    public string Titulo { get; }

    public ObservableCollection<ClaseApi> Clases { get; } = new();
    public ObservableCollection<RazaApi> Razas { get; } = new();

    [ObservableProperty]
    public partial string Nombre { get; set; }

    [ObservableProperty]
    public partial int Nivel { get; set; }

    [ObservableProperty]
    public partial ClaseApi? ClaseSeleccionada { get; set; }

    [ObservableProperty]
    public partial RazaApi? RazaSeleccionada { get; set; }

    [ObservableProperty]
    public partial string Mensaje { get; set; }

    public AgregarPersonajeViewModel(MainViewModel main, Personaje? personaje = null)
    {
        _main = main;
        _editando = personaje;
        Titulo = personaje == null ? "Nuevo Personaje" : "Editar Personaje";
        Nombre = personaje?.Nombre ?? "";
        Nivel = personaje?.Nivel ?? 1;
        Mensaje = "";
        _ = CargarListasAsync();
    }

    private async Task CargarListasAsync()
    {
        try
        {
            foreach (var c in await _main.Api.GetClasesAsync())
                Clases.Add(c);

            foreach (var r in await _main.Api.GetRazasAsync())
                Razas.Add(r);

            if (_editando != null)
            {
                ClaseSeleccionada = Clases.FirstOrDefault(c => c.Nombre == _editando.Clase);
                RazaSeleccionada = Razas.FirstOrDefault(r => r.Nombre == _editando.Raza);
            }
        }
        catch (Exception)
        {
            Mensaje = "No se pudo conectar con la API.";
        }
    }

    [RelayCommand]
    private async Task Guardar()
    {
        if (string.IsNullOrWhiteSpace(Nombre) || ClaseSeleccionada == null || RazaSeleccionada == null)
        {
            Mensaje = "Completá el nombre y elegí una clase y una raza.";
            return;
        }

        if (Nivel < 1 || Nivel > 20)
        {
            Mensaje = "El nivel debe estar entre 1 y 20.";
            return;
        }

        try
        {
            var datos = new PersonajeGuardar
            {
                Nombre = Nombre,
                Nivel = Nivel,
                ClaseId = ClaseSeleccionada.Id,
                RazaId = RazaSeleccionada.Id
            };

            var ok = _editando == null
                ? await _main.Api.CrearPersonajeAsync(datos)
                : await _main.Api.EditarPersonajeAsync(_editando.Id, datos);

            if (!ok)
            {
                Mensaje = "La API rechazó los datos.";
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

    [RelayCommand]
    private void Volver() => _main.MostrarPersonajes();
}   