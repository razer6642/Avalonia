using System.Collections.Generic;

namespace MyAvaloniaApp.Models;

public record Personaje
{
    public int Id { get; init; }
    public string Nombre { get; init; } = "";
    public string Clase { get; init; } = "";
    public string Raza { get; init; } = "";
    public int Nivel { get; init; }
    public List<string> Habilidades { get; init; } = new();
    public List<string> Hechizos { get; init; } = new();
}