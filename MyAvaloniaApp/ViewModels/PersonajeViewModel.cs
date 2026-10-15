using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyAvaloniaApp.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace MyAvaloniaApp.ViewModels;

public partial class PersonajesViewModel : ViewModelBase
{
    private readonly MainViewModel _main;

    public MainViewModel Main => _main;

    public ObservableCollection<Personaje> Filtrados { get; } = new();

    [ObservableProperty]
    public partial string Busqueda { get; set; } = "";

    [ObservableProperty]
    public partial Personaje? SelectedPersonaje { get; set; }

    public PersonajesViewModel(MainViewModel main)
    {
        _main = main;
        _main.Personajes.CollectionChanged += (_, _) => Filtrar();
        Filtrar();
    }

    partial void OnBusquedaChanged(string value) => Filtrar();

    private void Filtrar()
    {
        var texto = Busqueda.Trim();

        var resultado = _main.Personajes
            .Where(p => p.Nombre.Contains(texto, StringComparison.OrdinalIgnoreCase)
                     || p.Clase.Contains(texto, StringComparison.OrdinalIgnoreCase))
            .OrderBy(p => p.Nombre);

        Filtrados.Clear();
        foreach (var p in resultado)
            Filtrados.Add(p);
    }

    partial void OnSelectedPersonajeChanged(Personaje? value)
    {
        if (value != null)
            _main.MostrarFicha(value);
    }

    [RelayCommand]
    private void NuevoPersonaje() => _main.MostrarAgregarPersonaje();

    [RelayCommand]
    private void Volver() => _main.MostrarInicio();
}