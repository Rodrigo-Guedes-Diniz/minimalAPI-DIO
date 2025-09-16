using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using minimal_api.Dominio.Entidades;

namespace MinimalApi.Infraestrutura.Db;

public class DbContexto : DbContext
{
    private readonly IConfiguration _configuracaoAppSetting;
    public DbContexto(IConfiguration configuracaoAppSetting)
    {
        _configuracaoAppSetting = configuracaoAppSetting;
    }
    // default! serve para indicar que nao pode ser nulo
    public DbSet<Administador> Administradores { get; set; } = default!;
    public DbSet<Veiculo> Veiculos { get; set; } = default!;
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Administador>().HasData(
            new Administador
            {
                Id = 1,
                Email = "administratod@teste.com",
                Senha = "123456",
                Perfil = "Adm"
            }
        );
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            string connectionString = "Server=localhost\\SQLEXPRESS;Initial Catalog=Minimal_api;Integrated Security=True;TrustServerCertificate=True;";
            optionsBuilder.UseSqlServer(connectionString);
        }
        ;



        // var stringConexao = _configuracaoAppSetting.GetConnectionString("sqlserver")?.ToString();
        // if (!string.IsNullOrEmpty(stringConexao))
        // {

        // };
    }
}