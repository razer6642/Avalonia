namespace MiSolucion.Api.Models;
using System.Text.Json.Serialization;
public class Clase
{
    public int Id { get; set; }
    public string Nombre { get; set; } = "";   
    [JsonIgnore]
    public List<Personaje> Personajes { get; set; } = new();
}