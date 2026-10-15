using System.Collections.Generic;

namespace MyAvaloniaApp.Models;

public class ClaseApi
{
    public int Id { get; set; }
    public string Nombre { get; set; } = "";

    public override string ToString() => Nombre;
}

public class RazaApi
{
    public int Id { get; set; }
    public string Nombre { get; set; } = "";

    public override string ToString() => Nombre;
}

public class HabilidadApi
{
    public int Id { get; set; }
    public string Nombre { get; set; } = "";
    public string Descripcion { get; set; } = "";
}

public class HechizoApi
{
    public int Id { get; set; }
    public string Nombre { get; set; } = "";
    public string Descripcion { get; set; } = "";
    public int Nivel { get; set; }
}

public class PersonajeApi
{
    public int Id { get; set; }
    public string Nombre { get; set; } = "";
    public int Nivel { get; set; }
    public int ClaseId { get; set; }
    public ClaseApi? Clase { get; set; }
    public int RazaId { get; set; }
    public RazaApi? Raza { get; set; }
    public List<HabilidadApi> Habilidades { get; set; } = new();
    public List<HechizoApi> Hechizos { get; set; } = new();
}

public class PersonajeGuardar
{
    public string Nombre { get; set; } = "";
    public int Nivel { get; set; }
    public int ClaseId { get; set; }
    public int RazaId { get; set; }
}