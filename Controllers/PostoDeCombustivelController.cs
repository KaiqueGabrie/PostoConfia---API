using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PostoConfia.DataContexts; 
using PostoConfia.Models; 
using PostoConfia.Models.Dtos;
using System.Linq;
using System.Threading.Tasks;

namespace PostoConfia.Controllers 
{
    [Route("/Postos")]
    [ApiController]
    public class PostoDeCombustivelController : ControllerBase
    {
        private readonly AppDbContext _context;
        public PostoDeCombustivelController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /Postos
        [HttpGet]
        public async Task<IActionResult> BuscarPostos([FromQuery] string? search)
        {
            var query = _context.PostosDeCombustivel.AsQueryable();

            if (search is not null)
            {
                query = query.Where(x => x.Nome.Contains(search) || x.Endereco.Contains(search) || x.Cnpj.Contains(search));
            }

            var postos = await query
            .Select(x => new
            {
                x.Id,
                x.Nome,
                x.Cnpj,
                x.Endereco,
                x.AvaliacaoMedia
            })
            .ToListAsync();

            return Ok(postos);
        }

        // GET: /Postos/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarPostoPorId(int id)
        {
            var posto = await _context.PostosDeCombustivel
                .Include(p => p.Comentarios).ThenInclude(c => c.Usuario)
                .Include(p => p.Precos).ThenInclude(pr => pr.Combustivel)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (posto is null) return NotFound();

            var precosAtuais = posto.Precos
                .GroupBy(p => p.CombustivelId)
                .Select(g => g.OrderByDescending(p => p.DataRegistro).FirstOrDefault())
                .Where(p => p != null)
                .Select(p => new { CombustivelTipo = p.Combustivel?.Tipo, p.Valor, p.DataRegistro });

            var result = new
            {
                posto.Id,
                posto.Nome,
                posto.Endereco,
                posto.Cnpj,
                posto.Latitude,
                posto.Longitude,
                posto.AvaliacaoMedia,
                PrecosAtuais = precosAtuais,
                Comentarios = posto.Comentarios.OrderByDescending(c => c.DataComentario).Select(c => new { c.Id, c.Texto, Usuario = c.Usuario?.Nome, c.DataComentario }),
            };

            return Ok(result);
        }

        // POST: /Criar-Posto
        [HttpPost("/Criar-Posto")]
        public async Task<IActionResult> Criar([FromBody] PostoCombustivelDTO novoPosto)
        {
            if (await _context.PostosDeCombustivel.AnyAsync(p => p.Cnpj == novoPosto.Cnpj))
            {
                return Conflict("CNPJ já cadastrado.");
            }

            var posto = new PostoDeCombustivel()
            {
                Nome = novoPosto.Nome,
                Cnpj = novoPosto.Cnpj,
                Endereco = novoPosto.Endereco,
                Latitude = novoPosto.Latitude,
                Longitude = novoPosto.Longitude,
                AvaliacaoMedia = 0.0m
            };

            await _context.PostosDeCombustivel.AddAsync(posto);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(BuscarPostoPorId), new { id = posto.Id }, posto);
        }

        // PUT: /Atualizar-Posto/{id}
        [HttpPut("/Atualizar-Posto/{id}")]
        public async Task<IActionResult> Atualizar(int id, [FromBody] PostoCombustivelDTO atualizarPosto)
        {
            var posto = await _context.PostosDeCombustivel.FindAsync(id);
            if (posto == null)
            {
                return NotFound();
            }

            if (posto.Cnpj != atualizarPosto.Cnpj && await _context.PostosDeCombustivel.AnyAsync(p => p.Cnpj == atualizarPosto.Cnpj && p.Id != id))
            {
                return Conflict("Novo CNPJ já cadastrado para outro posto.");
            }

            posto.Nome = atualizarPosto.Nome;
            posto.Cnpj = atualizarPosto.Cnpj;
            posto.Endereco = atualizarPosto.Endereco;
            posto.Latitude = atualizarPosto.Latitude;
            posto.Longitude = atualizarPosto.Longitude;

            _context.PostosDeCombustivel.Update(posto);
            await _context.SaveChangesAsync();
            return Ok(posto);
        }

        // DELETE: /Deletar-Posto/{id}
        [HttpDelete("/Deletar-Posto/{id}")]
        public async Task<IActionResult> Remover(int id)
        {
            var posto = await _context.PostosDeCombustivel.FindAsync(id);
            if (posto == null)
            {
                return NotFound();
            }

            _context.PostosDeCombustivel.Remove(posto);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // AÇÃO ESPECÍFICA: POST: /Postos/{id}/Avaliar
        [HttpPost("{id}/Avaliar")]
        public async Task<IActionResult> AvaliarPosto(int id, [FromBody] AvaliacaoDTO dto)
        {
            var posto = await _context.PostosDeCombustivel.FindAsync(id);
            var usuario = await _context.Usuarios.FindAsync(dto.UsuarioId);

            if (posto is null) return NotFound("Posto não encontrado.");
            if (usuario is null) return NotFound("Usuário não encontrado.");

            var avaliacaoExistente = await _context.Avaliacoes
                .FirstOrDefaultAsync(a => a.PostoId == id && a.UsuarioId == dto.UsuarioId);

            if (avaliacaoExistente is not null)
            {
                avaliacaoExistente.Nota = dto.Nota;
                avaliacaoExistente.DataAvaliacao = DateTime.Now;
                _context.Avaliacoes.Update(avaliacaoExistente);
            }
            else
            {
                var novaAvaliacao = new Avaliacao { PostoId = id, UsuarioId = dto.UsuarioId, Nota = dto.Nota, DataAvaliacao = DateTime.Now };
                await _context.Avaliacoes.AddAsync(novaAvaliacao);
            }

            await _context.SaveChangesAsync();

            var novaMedia = await _context.Avaliacoes
                .Where(a => a.PostoId == id)
                .AverageAsync(a => a.Nota);

            posto.AvaliacaoMedia = novaMedia;
            _context.PostosDeCombustivel.Update(posto);
            await _context.SaveChangesAsync();

            return Ok(new { PostoId = posto.Id, MediaAtualizada = novaMedia });
        }

        // AÇÃO ESPECÍFICA: POST: /Postos/{id}/Comentar
        [HttpPost("{id}/Comentar")]
        public async Task<IActionResult> ComentarPosto(int id, [FromBody] ComentarioDTO dto)
        {
            var posto = await _context.PostosDeCombustivel.FindAsync(id);
            var usuario = await _context.Usuarios.FindAsync(dto.UsuarioId);

            if (posto is null) return NotFound("Posto não encontrado.");
            if (usuario is null) return NotFound("Usuário não encontrado.");

            var comentario = new Comentario
            {
                PostoId = id,
                UsuarioId = dto.UsuarioId,
                Texto = dto.Texto,
                DataComentario = DateTime.Now
            };

            await _context.Comentarios.AddAsync(comentario);
            await _context.SaveChangesAsync();

            return Created("", new { ComentarioId = comentario.Id, ComentarioTexto = comentario.Texto, UsuarioNome = usuario.Nome });
        }
    }
}