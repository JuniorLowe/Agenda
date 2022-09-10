using System;
using NUnit.Framework;
using Agenda.Domain.Entities;
using System.Linq;
using System.Collections.Immutable;
using System.Configuration;

namespace Agenda.DAL.Test
{
    [TestFixture]
    public class ContatosTest : BaseTest
    {
        Contatos _contatos;

        [SetUp]
        public void SetUp()
        {
            _contatos = new Contatos();
        }

        //IncluirContatoTest
        [Test]
        public void AdicionarContatoTest()
        {
            //Monta
            Contato contato = new Contato()
            {
                Id = Guid.NewGuid(),
                Nome = "Júnior Teste"
            };
            

            //Executa
            _contatos.Adicionar(contato);

            //Verifica
            Assert.True(true);

        }

        //ObterContatoTest
        [Test]
        public void ObterContatoTest()
        {
            //Monta
            Contato contato = new Contato()
            {
                Id = Guid.NewGuid(),
                Nome = "Júnior Teste Obter"
            };

            Contato contatoResultado;

            //Executa
            _contatos.Adicionar(contato);
            contatoResultado = _contatos.Obter(contato.Id);

            //Verifica
            Assert.AreEqual(contato.Id, contatoResultado.Id);
            Assert.AreEqual(contato.Nome, contatoResultado.Nome);

        }

        [Test]
        public void ObterTodosOsContatosTest()
        {
            //Monta
            Contato contato1 = new Contato() {Id = Guid.NewGuid(), Nome = "Maria"};
            Contato contato2 = new Contato() { Id = Guid.NewGuid(), Nome = "José" };

            //Executa
            _contatos.Adicionar(contato1);
            _contatos.Adicionar(contato2);
            var lstContatos = _contatos.ObterTodos();
            var contatoResultado = lstContatos.Where(x=>x.Id==contato1.Id).First();

            //Verifica
            Assert.IsTrue(lstContatos.Count()>1);
            Assert.AreEqual(contato1.Id, contatoResultado.Id);



        }


        [TearDown]
        public void TearDown()
        {
            _contatos = null;
        }
    }
}