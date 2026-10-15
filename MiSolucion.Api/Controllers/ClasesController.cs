using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiSolucion.Api.Data;
using MiSolucion.Api.Models;

namespace MiSolucion.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClasesController : ControllerBase
{
    private readonly AppDbContext _context;

    public ClasesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<Clase>>> GetAll()
    {
        return await _context.Clases.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Clase>> GetById(int id)
    {
        var clase = await _context.Clases.FindAsync(id);

        if (clase == null)
            return NotFound();

        return clase;
    }
}