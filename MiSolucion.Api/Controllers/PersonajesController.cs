using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiSolucion.Api.Data;
using MiSolucion.Api.Models;
using MiSolucion.Api.Dtos;

namespace MiSolucion.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PersonajesController : ControllerBase
{
    private readonly AppDbContext _context;

    public PersonajesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<Personaje>>> GetAll()
    {
        return await _context.Personajes
            .Include(p => p.Clase)
            .Include(p => p.Raza)
            .Include(p => p.Habilidades)
            .Include(p => p.Hechizos)
            .ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Personaje>> GetById(int id)
    {
        var personaje = await _context.Personajes
            .Include(p => p.Clase)
            .Include(p => p.Raza)
            .Include(p => p.Habilidades)
            .Include(p => p.Hechizos)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (personaje == null)
            return NotFound();

        return personaje;
    }

    [HttpPost]
    public async Task<ActionResult<Personaje>> Create(PersonajeDto dto)
    {
        if (!await _context.Clases.AnyAsync(c => c.Id == dto.ClaseId) ||
            !await _context.Razas.AnyAsync(r => r.Id == dto.RazaId))
            return BadRequest("La clase o la raza no existen.");

        var personaje = new Personaje
        {
            Nombre = dto.Nombre,
            Nivel = dto.Nivel,
            ClaseId = dto.ClaseId,
            RazaId = dto.RazaId
        };

        _context.Personajes.Add(personaje);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = personaje.Id }, personaje);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, PersonajeDto dto)
    {
        var personaje = await _context.Personajes.FindAsync(id);
        if (personaje == null)
            return NotFound();

        if (!await _context.Clases.AnyAsync(c => c.Id == dto.ClaseId) ||
            !await _context.Razas.AnyAsync(r => r.Id == dto.RazaId))
            return BadRequest("La clase o la raza no existen.");

        personaje.Nombre = dto.Nombre;
        personaje.Nivel = dto.Nivel;
        personaje.ClaseId = dto.ClaseId;
        personaje.RazaId = dto.RazaId;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var personaje = await _context.Personajes.FindAsync(id);
        if (personaje == null)
            return NotFound();

        _context.Personajes.Remove(personaje);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}