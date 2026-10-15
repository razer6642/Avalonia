using System.ComponentModel.DataAnnotations;

namespace MiSolucion.Api.Dtos;

public class PersonajeDto
{
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "El nombre debe tener 2 a 50 letras.")]
    public string Nombre { get; set; } = "";

    [Range(1, 20, ErrorMessage = "El nivel debe estar entre 1 y 20.")]
    public int Nivel { get; set; }

    public int ClaseId { get; set; }
    public int RazaId { get; set; }
}