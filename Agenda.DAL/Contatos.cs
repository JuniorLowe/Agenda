using Agenda.Domain.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using Dapper;
using System.Linq;
using System.Drawing;

namespace Agenda.DAL
{
    public class Contatos
    {
        string _strCon;

        public Contatos()
        {

            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            _strCon = configuration.GetConnectionString("dev");

           
        }

        public void Adicionar(Contato contato)
        {
            using (var conn = new SqlConnection(_strCon))
            {
                conn.Execute("INSERT INTO Contato (Id, Nome) VALUES (@Id, @Nome);", contato);
            }       
        }

        public Contato Obter(Guid id)
        {
            Contato contato;
            using (var conn = new SqlConnection(_strCon))
            {
                contato = conn.QueryFirst<Contato>("select id, nome from contato where Id=@Id", new {Id=id}); //Parametro new é objeto anônimo
            }

            return contato;
        }

        public List<Contato> ObterTodos()
        {


            
            List<Contato> contatos = new List<Contato>();

            using (var conn = new SqlConnection(_strCon))
            {
                contatos = conn.Query<Contato>("select id, nome from contato").ToList();
               
                #region CÓDIGO ANTIGO SEM DAPPER
                //conn.Open();
                //string sql = String.Format("select id, nome from contato");
                //SqlCommand cmd = new SqlCommand(sql, conn);

                //var sqlDataReader = cmd.ExecuteReader();
                //while (sqlDataReader.Read())
                //{
                //    var contato = new Contato()
                //    {
                //        Id = Guid.Parse(sqlDataReader["id"].ToString()),
                //        Nome = sqlDataReader["nome"].ToString()
                //    };
                //    contatos.Add(contato);
                //}
                #endregion

            }
            return contatos;
        }
    }
}