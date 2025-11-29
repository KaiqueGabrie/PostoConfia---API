using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PostoConfia.DataContexts; 
using PostoConfia.Models; 
using PostoConfia.Models.Dtos; 
using System.Threading.Tasks;

namespace PostoConfia.Controllers 
{ 
    [Route("/Usuarios")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly AppDbContext _context;
        public UsuarioController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /Usuarios
        [HttpGet]
        public async Task<IActionResult> BuscarUsuarios([FromQuery] string? nome)
        {
            var query = _context.Usuarios.AsQueryable();

            if (nome is not null)
            {
                query = query.Where(x => x.Nome.Contains(nome) || x.Email.Contains(nome));
            }

            var usuarios = await query
            .Select(x => new
            {
                x.Id,
                x.Nome,
                x.Email,
                x.DataCadastro
            })
            .ToListAsync();

            return Ok(usuarios);
        }

        // GET: /Usuarios/{id} (Buscar por ID)
        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarUsuarioPorId(int id)
        {
            var usuario = await _context.Usuarios
                .Select(x => new
                {
                    x.Id,
                    x.Nome,
                    x.Email,
                    x.DataCadastro
                })
                .FirstOrDefaultAsync(x => x.Id == id);

            if (usuario == null)
            {
                return NotFound();
            }
            return Ok(usuario);
        }

        // POST: /Criar-Usuario
        [HttpPost("/Criar-Usuario")]
        public async Task<IActionResult> Criar([FromBody] UsuarioDTO novoUsuario)
        {
            
            if (await _context.Usuarios.AnyAsync(u => u.Email == novoUsuario.Email))
            {
                return Conflict("O email fornecido já está em uso.");
            }

            var usuario = new Usuario()
            {
                Nome = novoUsuario.Nome,
                Email = novoUsuario.Email,
                Senha = novoUsuario.Senha,
                DataCadastro = DateTime.Now
            };

            await _context.Usuarios.AddAsync(usuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(BuscarUsuarioPorId), new { id = usuario.Id }, new { usuario.Id, usuario.Nome, usuario.Email });
        }

        // PUT: /Atualizar-Usuario/{id}
        [HttpPut("/Atualizar-Usuario/{id}")]
        public async Task<IActionResult> Atualizar(int id, [FromBody] UsuarioDTO atualizarUsuario)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            if (usuario.Email != atualizarUsuario.Email && await _context.Usuarios.AnyAsync(u => u.Email == atualizarUsuario.Email && u.Id != id))
            {
                return Conflict("O novo email fornecido já está em uso.");
            }

            usuario.Nome = atualizarUsuario.Nome;
            usuario.Email = atualizarUsuario.Email;
            usuario.Senha = atualizarUsuario.Senha;

            _context.Usuarios.Update(usuario);
            await _context.SaveChangesAsync();
            return Ok(new { usuario.Id, usuario.Nome, usuario.Email });
        }

        // DELETE: /Deletar-Usuario/{id}
        [HttpDelete("/Deletar-Usuario/{id}")]
        public async Task<IActionResult> Remover(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}