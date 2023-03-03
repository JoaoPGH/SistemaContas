using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SistemaContas.Data.Context;
using SistemaContas.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaContas.Data.Repositories
{
    public class UsuarioRepository
    {
        public void Inserir(Usuario usuario)
        {
            var query = @"
                INSERT INTO USUARIO(
                IDUSUARIO,
                NOME,
                EMAIL,
                SENHA,
                DATAHORACRIACAO)

                VALUES(
                @IdUsuario,
                @Nome,
                @Email,
               CONVERT(VARCHAR(32), HASHBYTES('MD5', @Senha), 2),
                @DataHoraCriacao)
            ";
            using (var connection = new SqlConnection(SqlConfiguration.ConnectionString))
            {
                connection.Execute(query, usuario);
            }
        }

        public Usuario? ObterPorEmail(string email)
        {
            var query = @"
            SELECT * FROM USUARIO
            WHERE EMAIL = @email
            ";
            using (var connection = new SqlConnection(SqlConfiguration.ConnectionString))
            {
                return connection.Query<Usuario>(query, new { email }).FirstOrDefault();
            }
        }

        public Usuario? ObterPorEmailESenha(string email, string senha)
        {
            var query = @"
                SELECT * FROM USUARIO
            WHERE EMAIL = @email
            AND SENHA = CONVERT(VARCHAR(32),
            HASHBYTES('MD5', @senha), 2)
            ";
            using (var connection = new SqlConnection(SqlConfiguration.ConnectionString))

            {
                return connection.Query<Usuario>(query, new { email, senha }).FirstOrDefault();
            }
        }
    }
}
