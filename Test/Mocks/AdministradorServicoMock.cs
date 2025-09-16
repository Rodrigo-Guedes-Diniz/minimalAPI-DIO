using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Azure.Core;
using minimal_api.Dominio.DTO;
using minimal_api.Dominio.Entidades;

namespace Test.Mocks
{
    public class AdministradorServicoMock
    {

        private static List<Administador> administradores = new List<Administador>()
        {
            new Administador{
                Id = 1,
                Email = "adm@teste.com",
                Senha = "123456",
                Perfil = "Editor"
            },
            new Administador{
                Id = 2,
                Email = "editor@teste.com",
                Senha = "123456",
                Perfil = "Editor"
            }
        };

        public Administador? BuscaPorId(int id)
        {
            return administradores.Find(a => a.Id == id);
        }

        public Administador Incluir(AdministradorServicoMock administrador)
        {
            administrador.Id = administradores.Count() + 1;
            administradores.Add(administrador);

            return administrador;
        }

        public Administador Login(LoginDTO loginDTO)
        {
            return administradores.Find(a => a.Email == loginDTO.Email && a.Email == loginDTO.Senha);
        }

        public List<AdministradorDTO> Todos(int? pagina)
        {
            return administradores;
        }

    }
}