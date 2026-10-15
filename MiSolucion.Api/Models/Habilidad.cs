namespace MiSolucion.Api.Models;
using System.Text.Json.Serialization;
public class Habilidad
{
    public int Id { get; set; }
    public string Nombre { get; set; } = "";
    public string Descripcion { get; set; } = "";

    public int PersonajeId { get; set; }
    [JsonIgnore]
    public Personaje Personaje { get; set; } = null!;
}