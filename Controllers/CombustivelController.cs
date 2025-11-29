using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PostoConfia.DataContexts; 
using PostoConfia.Models; 
using PostoConfia.Models.Dtos; 
using System.Threading.Tasks;

namespace PostoConfia.Controllers 
{
    [Route("/Combustiveis")]
    [ApiController]
    public class CombustivelController : ControllerBase
    {
        private readonly AppDbContext _context;
        public CombustivelController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /Combustiveis
        [HttpGet]
        public async Task<IActionResult> BuscarCombustiveis([FromQuery] string? tipo)
        {
            var query = _context.Combustiveis.AsQueryable();

            if (tipo is not null)
            {
                query = query.Where(x => x.Tipo.Contains(tipo));
            }

            var combustiveis = await query.ToListAsync();

            return Ok(combustiveis);
        }

        // GET: /Combustiveis/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarCombustivelPorId(int id)
        {
            var combustivel = await _context.Combustiveis.FindAsync(id);

            if (combustivel == null)
            {
                return NotFound();
            }
            return Ok(combustivel);
        }

        // POST: /Criar-Combustivel
        [HttpPost("/Criar-Combustivel")]
        public async Task<IActionResult> Criar([FromBody] CombustivelDTO novoCombustivel)
        {
            var combustivel = new Combustivel()
            {
                Tipo = novoCombustivel.Tipo
            };

            await _context.Combustiveis.AddAsync(combustivel);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(BuscarCombustivelPorId), new { id = combustivel.Id }, combustivel);
        }

        // PUT: /Atualizar-Combustivel/{id}
        [HttpPut("/Atualizar-Combustivel/{id}")]
        public async Task<IActionResult> Atualizar(int id, [FromBody] CombustivelDTO atualizarCombustivel)
        {
            var combustivel = await _context.Combustiveis.FindAsync(id);
            if (combustivel == null)
            {
                return NotFound();
            }

            combustivel.Tipo = atualizarCombustivel.Tipo;

            _context.Combustiveis.Update(combustivel);
            await _context.SaveChangesAsync();
            return Ok(combustivel);
        }

        // DELETE: /Deletar-Combustivel/{id}
        [HttpDelete("/Deletar-Combustivel/{id}")]
        public async Task<IActionResult> Remover(int id)
        {
            var combustivel = await _context.Combustiveis.FindAsync(id);
            if (combustivel == null)
            {
                return NotFound();
            }

            _context.Combustiveis.Remove(combustivel);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}