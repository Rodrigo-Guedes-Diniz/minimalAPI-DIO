using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using minimal_api.Dominio.Entidades;
using minimal_api.Infraestrutura.Servicos;
using MinimalApi.Infraestrutura.Db;

namespace Test.Domain.Servicos
{
    [TestClass]
    public class AdministradorServicoTest
    {
        private DbContexto CriarContextoDeTeste()
{
            var assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var path = Path.GetFullPath(Path.Combine(assemblyPath ?? "", "..", "..", ".."));

    // tenta ler appsettings.json na pasta atual
            var basePath = path;

    // Caso seus testes rodem em outra pasta (ex: pasta de testes), você pode
    // ajustar manualmente o caminho aqui (ex: Path.Combine(basePath, "..", "API"))
    var configuration = new ConfigurationBuilder()
        .SetBasePath(basePath)
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .AddEnvironmentVariables() // permite sobrescrever via vars de ambiente
        .Build();

    // Procura connection string "ConexaoPadrao" (padrão em ConnectionStrings: { "ConexaoPadrao": "..." })
    var connectionString = configuration.GetConnectionString("ConexaoPadrao");

    // Se não encontrar no appsettings.json, tenta variáveis de ambiente:
    // nome esperado: ConnectionStrings__ConexaoPadrao
    if (string.IsNullOrWhiteSpace(connectionString))
    {
        connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__ConexaoPadrao");
    }

    if (string.IsNullOrWhiteSpace(connectionString))
    {
        throw new InvalidOperationException(
            "Connection string 'ConexaoPadrao' não encontrada. " +
            "Defina no appsettings.json ou na variável de ambiente ConnectionStrings__ConexaoPadrao.");
    }

    var optionsBuilder = new DbContextOptionsBuilder<DbContexto>();
    optionsBuilder.UseSqlServer(connectionString, sqlOptions =>
    {
        // opcional: especifica assembly das migrations
        sqlOptions.MigrationsAssembly(typeof(DbContexto).Assembly.FullName);
    });

    var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: true)
    .AddEnvironmentVariables()
    .Build();

    var context = new DbContexto(config);

    // Opcional: aplicar migrations automaticamente (descomente se quiser que o banco seja atualizado ao criar o contexto)
    // context.Database.Migrate();

    return context;
}

        [TestMethod]
        public void TestanSalvarAdministrador()
        {
            // Arrange
            var context = CriarContextoDeTeste();
            context.Database.ExecuteSqlRaw("TRUNCATE TABLE Administradores");

            var adm = new Administador();
            adm.Id = 1;
            adm.Email = "teste@teste.com";
            adm.Senha = "teste";
            adm.Perfil = "Adm";

            var administradorServico = new AdministradorServico(context);

            // Act
            administradorServico.Incluir(adm);
            var admDoBanco = administradorServico.BuscaPorId(adm.Id);

            // Assert
            Assert.AreEqual(1, admDoBanco?.Id);
        }
    }
}