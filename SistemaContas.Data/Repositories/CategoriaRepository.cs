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
    public class CategoriaRepository
    {
        public void Inserir(Categoria categoria)
        {
            var query = @"
                INSERT INTO CATEGORIA(IDCATEGORIA, NOME, IDUSUARIO)
                VALUES(@IdCategoria, @Nome, @IdUsuario)
                ";
            using (var connection = new SqlConnection(SqlConfiguration.ConnectionString))
            {
                connection.Execute(query, categoria);
            }
        }

        public void Atualizar(Categoria categoria)
        {
            var query = @"
                UPDATE CATEGORIA SET NOME = @Nome
                WHERE IDCATEGORIA = @IdCategoria
                ";
            using (var connection = new SqlConnection(SqlConfiguration.ConnectionString))
            {
                connection.Execute(query, categoria);
            }
        }

        public void Excluir(Categoria categoria)
        {
            var query = @"
                DELETE FROM CATEGORIA
                WHERE IDCATEGORIA = @IdCategoria
                ";
            using (var connection = new SqlConnection(SqlConfiguration.ConnectionString))
            {
                connection.Execute(query, categoria);
            }
        }

        public List<Categoria>? ObterTodos(Guid idUsuario)
        {
            var query = @"
                SELECT * FROM CATEGORIA
                WHERE IDUSUARIO = @idUsuario
                ORDER BY NOME
                ";
            using (var connection = new SqlConnection(SqlConfiguration.ConnectionString))
            {
                return connection.Query<Categoria>(query, new { idUsuario }).ToList();

            }
        }

        public Categoria? ObterPorId(Guid idCategoria)
        {
            var query = @"
                SELECT * FROM CATEGORIA
                WHERE IDCATEGORIA = @idCategoria
                ";
            using (var connection = new SqlConnection(SqlConfiguration.ConnectionString))
            {
                return connection.Query<Categoria>(query, new { idCategoria }).FirstOrDefault();

            }
        }

        public int? ObterQuantidadeContas(Guid idCategoria)
        {
            var query = @"
                SELECT COUNT(IDCONTA) FROM CONTA
                WHERE IDCATEGORIA = @idCategoria
             ";

            using (var connection = new SqlConnection(SqlConfiguration.ConnectionString))
            {
                return connection.Query<int>(query, new { idCategoria }).FirstOrDefault();  
            }
        }

    }

}
