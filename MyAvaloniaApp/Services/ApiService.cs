using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using MyAvaloniaApp.Models;

namespace MyAvaloniaApp.Services;

public class ApiService
{
    private readonly HttpClient _http = new()
    {
        BaseAddress = new Uri("http://localhost:5028/")
    };

    public async Task<List<PersonajeApi>> GetPersonajesAsync()
        => await _http.GetFromJsonAsync<List<PersonajeApi>>("api/Personajes") ?? new();

    public async Task<List<ClaseApi>> GetClasesAsync()
        => await _http.GetFromJsonAsync<List<ClaseApi>>("api/Clases") ?? new();

    public async Task<List<RazaApi>> GetRazasAsync()
        => await _http.GetFromJsonAsync<List<RazaApi>>("api/Razas") ?? new();

    public async Task<bool> CrearPersonajeAsync(PersonajeGuardar personaje)
    {
        var respuesta = await _http.PostAsJsonAsync("api/Personajes", personaje);
        return respuesta.IsSuccessStatusCode;
    }

    public async Task<bool> EditarPersonajeAsync(int id, PersonajeGuardar personaje)
    {
        var respuesta = await _http.PutAsJsonAsync($"api/Personajes/{id}", personaje);
        return respuesta.IsSuccessStatusCode;
    }

    public async Task<bool> BorrarPersonajeAsync(int id)
    {
        var respuesta = await _http.DeleteAsync($"api/Personajes/{id}");
        return respuesta.IsSuccessStatusCode;
    }
}