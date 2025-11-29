using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PostoConfia.DataContexts;
using PostoConfia.Models; 
using PostoConfia.Models.Dtos;
using System.Threading.Tasks;

namespace PostoConfia.Controllers 
{
    // O recurso Preco está junto dentro do Posto para fazer sentido na URL
    [Route("/Postos/{postoId}/Precos")]
    [ApiController]
    public class PrecoController : ControllerBase
    {
        private readonly AppDbContext _context;
        public PrecoController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /Postos/{postoId}/Precos (Histórico de Preços)
        [HttpGet]
        public async Task<IActionResult> GetHistoricoPrecos(int postoId)
        {
            var posto = await _context.PostosDeCombustivel.FindAsync(postoId);
            if (posto is null) return NotFound("Posto não encontrado.");

            var precos = await _context.Precos
                .Where(p => p.PostoId == postoId)
                .Include(p => p.Combustivel)
                .OrderByDescending(p => p.DataRegistro)
                .Select(p => new
                {
                    p.Valor,
                    p.DataRegistro,
                    CombustivelTipo = p.Combustivel!.Tipo
                })
                .ToListAsync();

            return Ok(precos);
        }

        // POST: /Postos/{postoId}/Precos/Registrar
        [HttpPost("/Postos/{postoId}/Precos/Registrar")]
        public async Task<IActionResult> Registrar(int postoId, [FromBody] PrecoDTO dto)
        {
            
            var posto = await _context.PostosDeCombustivel.FindAsync(postoId);
            var combustivel = await _context.Combustiveis.FindAsync(dto.CombustivelId);

            if (posto is null) return NotFound("Posto não encontrado.");
            if (combustivel is null) return NotFound("Combustível não encontrado.");

            var preco = new Preco
            {
                PostoId = postoId,
                CombustivelId = dto.CombustivelId,
                Valor = dto.Valor,
                DataRegistro = DateTime.Now
            };

            await _context.Precos.AddAsync(preco);
            await _context.SaveChangesAsync();

            return Created("", new { preco.Valor, preco.DataRegistro, CombustivelTipo = combustivel.Tipo });
        }
    }
}   