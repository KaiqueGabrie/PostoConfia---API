using PostoConfia.Models;
using Microsoft.EntityFrameworkCore;
using PostoConfia.Models;

namespace PostoConfia.DataContexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // DbSets para todas as entidades
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<PostoDeCombustivel> PostosDeCombustivel { get; set; }
        public DbSet<Combustivel> Combustiveis { get; set; }
        public DbSet<Avaliacao> Avaliacoes { get; set; }
        public DbSet<Comentario> Comentarios { get; set; }
        public DbSet<Preco> Precos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1. Configuração da Tabela PRECO (Chave Primária Composta)
            modelBuilder.Entity<Preco>()
                .HasKey(p => new { p.PostoId, p.CombustivelId, p.DataRegistro });

            // 2. Configuração da Tabela AVALIACAO (Índice Único)
            // Garante que um usuário só possa fazer uma avaliação por posto.
            modelBuilder.Entity<Avaliacao>()
                .HasIndex(a => new { a.UsuarioId, a.PostoId })
                .IsUnique();

            // 3. Configuração de Chaves Estrangeiras (Opcional, mas explícito)

            // Avaliacao
            modelBuilder.Entity<Avaliacao>()
                .HasOne(a => a.Usuario)
                .WithMany(u => u.Avaliacoes)
                .HasForeignKey(a => a.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade); // Exclui avaliações se o usuário for excluído

            modelBuilder.Entity<Avaliacao>()
                .HasOne(a => a.Posto)
                .WithMany(p => p.Avaliacoes)
                .HasForeignKey(a => a.PostoId)
                .OnDelete(DeleteBehavior.Cascade); // Exclui avaliações se o posto for excluído

            // Comentario (Mesma lógica da Avaliacao)
            modelBuilder.Entity<Comentario>()
                .HasOne(c => c.Usuario)
                .WithMany(u => u.Comentarios)
                .HasForeignKey(c => c.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Comentario>()
                .HasOne(c => c.Posto)
                .WithMany(p => p.Comentarios)
                .HasForeignKey(c => c.PostoId)
                .OnDelete(DeleteBehavior.Cascade);

            // Preco
            modelBuilder.Entity<Preco>()
                .HasOne(p => p.Posto)
                .WithMany(p => p.Precos)
                .HasForeignKey(p => p.PostoId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Preco>()
                .HasOne(p => p.Combustivel)
                .WithMany(c => c.Precos)
                .HasForeignKey(p => p.CombustivelId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configuração para garantir que o email do usuário seja único
            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }
    }
}