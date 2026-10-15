namespace MiSolucion.Api.Models;

public class Personaje
{
    public int Id { get; set; }
    public string Nombre { get; set; } = "";
    public int Nivel { get; set; }

    public int ClaseId { get; set; }
    public Clase Clase { get; set; } = null!;

    public int RazaId { get; set; }
    public Raza Raza { get; set; } = null!;

    public List<Habilidad> Habilidades { get; set; } = new();

    public List<Hechizo> Hechizos { get; set; } = new();
}