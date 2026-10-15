using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiSolucion.Api.Data;
using MiSolucion.Api.Models;

namespace MiSolucion.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RazasController : ControllerBase
{
    private readonly AppDbContext _context;

    public RazasController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<Raza>>> GetAll()
    {
        return await _context.Razas.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Raza>> GetById(int id)
    {
        var raza = await _context.Razas.FindAsync(id);

        if (raza == null)
            return NotFound();

        return raza;
    }
}