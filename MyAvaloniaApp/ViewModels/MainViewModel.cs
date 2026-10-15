using CommunityToolkit.Mvvm.ComponentModel;
using MyAvaloniaApp.Models;
using MyAvaloniaApp.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace MyAvaloniaApp.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty]
    public partial ViewModelBase CurrentPage { get; set; }

    [ObservableProperty]
    public partial string MensajeApi { get; set; } = "";

    public ObservableCollection<Personaje> Personajes { get; } = new();

    public ApiService Api { get; } = new();

    public MainViewModel()
    {
        CurrentPage = new LoginViewModel(this);
        _ = CargarPersonajesAsync();
    }

    public void MostrarLogin() => CurrentPage = new LoginViewModel(this);

    public void MostrarInicio() => CurrentPage = new InicioViewModel(this);

    public void MostrarPersonajes() => CurrentPage = new PersonajesViewModel(this);

    public void MostrarFicha(Personaje personaje) => CurrentPage = new FichaPersonajeViewModel(this, personaje);

    public void MostrarAgregarPersonaje() => CurrentPage = new AgregarPersonajeViewModel(this);

    public void MostrarEditarPersonaje(Personaje personaje) => CurrentPage = new AgregarPersonajeViewModel(this, personaje);

    public async Task CargarPersonajesAsync()
    {
        try
        {
            var lista = await Api.GetPersonajesAsync();
            MensajeApi = "";

            Personajes.Clear();
            foreach (var p in lista)
            {
                Personajes.Add(new Personaje
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    Clase = p.Clase?.Nombre ?? "",
                    Raza = p.Raza?.Nombre ?? "",
                    Nivel = p.Nivel,
                    Habilidades = p.Habilidades.Select(h => h.Nombre).ToList(),
                    Hechizos = p.Hechizos.Select(h => h.Nombre).ToList()
                });
            }
        }
        catch (Exception)
        {
            MensajeApi = "No se pudo conectar con la API. Verificá que esté corriendo.";
        }
    }
}