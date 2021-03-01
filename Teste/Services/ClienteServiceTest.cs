using AutoMapper;
using Cliente_CRUD.Dtos.Cliente;
using Cliente_CRUD.Models;
using Cliente_CRUD.Repositories.Cliente;
using Cliente_CRUD.Services.ClienteService;
using Cliente_Test.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Cliente_Test.Services
{
    [TestClass]
    public class ClienteServiceTest
    {
        public ClienteService _clienteService;

        public Mock<IClienteRepository> _clienteRepository;
        public IMapper _mapper;

        [TestInitialize]
        public void Init()
        {
            _mapper = AutoMapperHelper.RetornarMapper();
            _clienteRepository = new Mock<IClienteRepository>();

            _clienteService = new ClienteService(_mapper, _clienteRepository.Object);

            Setup();
        }

        #region Setup dos métodos do repositório
        private void Setup()
        {
            var clientes = new List<Cliente>();

            _clienteRepository.Setup(s => s.Obter()).ReturnsAsync(clientes);

            _clienteRepository.Setup(s => s.Obter(It.IsAny<Guid>()))
                                .ReturnsAsync((Guid id) =>
                                {
                                    return clientes.First(f => f.Id == id);
                                });

            _clienteRepository.Setup(s => s.Criar(It.IsAny<Cliente>()))
                                .ReturnsAsync((Cliente cliente) =>
                                {
                                    cliente.Id = Guid.NewGuid();
                                    clientes.Add(cliente);
                                    return cliente;
                                });

            _clienteRepository.Setup(s => s.Atualizar(It.IsAny<Cliente>()))
                                .ReturnsAsync((Cliente cliente) =>
                                {
                                    var index = clientes.FindIndex(f => f.Id == cliente.Id);
                                    clientes[index] = cliente;

                                    return cliente;
                                });

            _clienteRepository.Setup(s => s.Excluir(It.IsAny<Guid>()))
                                .ReturnsAsync((Guid id) =>
                                {
                                    var clienteDb = clientes.First(f => f.Id == id);
                                    clientes.Remove(clienteDb);
                                    return !clientes.Any(a => a.Id == id);
                                });
        }
        #endregion

        #region Teste de todo CRUD caminho feliz
        [TestMethod]
        public async Task Cliente_CRUD_Valido()
        {
            //Criar primeiro cliente
            var novoCliente = new CriarClienteDto()
            {
                Nome = "Nome Exemplo",
                Idade = 20
            };

            var responseCriarCliente = await _clienteService.Criar(novoCliente);

            Assert.IsTrue(responseCriarCliente.Sucess);
            Assert.IsNotNull(responseCriarCliente.Value);
            Assert.IsNotNull(responseCriarCliente.Value.Id);

            //Consultar lista de clientes
            var responseObterTodosClientes = await _clienteService.Obter();

            Assert.IsTrue(responseObterTodosClientes.Sucess);
            Assert.IsNotNull(responseObterTodosClientes.Value);
            Assert.AreEqual(responseObterTodosClientes.Value.Count, 1);

            //Consultar cliente na lista de clientes
            var responseObterCliente = await _clienteService.Obter(responseCriarCliente.Value.Id);

            Assert.IsTrue(responseObterCliente.Sucess);
            Assert.IsNotNull(responseObterCliente.Value);
            Assert.AreEqual(responseObterCliente.Value.Nome, "Nome Exemplo");
            Assert.IsTrue(responseObterCliente.Value.Idade == 20);

            //Atualizar cliente
            var atualizarCliente = new AtualizarClienteDto()
            {
                Id = responseObterCliente.Value.Id,
                Nome = responseObterCliente.Value.Nome,
                Idade = responseObterCliente.Value.Idade,
            };

            atualizarCliente.Nome = "Novo Nome Exemplo";
            atualizarCliente.Idade = 30;

            var responseAtualizarCliente = await _clienteService.Atualizar(atualizarCliente);

            Assert.IsTrue(responseAtualizarCliente.Sucess);
            Assert.IsNotNull(responseAtualizarCliente.Value);

            //Validar se foi atualizado
            var responseObterClienteAtualizado = await _clienteService.Obter(responseAtualizarCliente.Value.Id);

            Assert.IsTrue(responseObterClienteAtualizado.Sucess);
            Assert.IsNotNull(responseObterClienteAtualizado.Value);
            Assert.AreEqual(responseObterClienteAtualizado.Value.Nome, "Novo Nome Exemplo");
            Assert.AreEqual(responseObterClienteAtualizado.Value.Idade, 30);

            //Validar se não duplicou dados
            var responseObterTodosClientes2 = await _clienteService.Obter();

            Assert.IsTrue(responseObterTodosClientes2.Sucess);
            Assert.IsNotNull(responseObterTodosClientes2.Value);
            Assert.AreEqual(responseObterTodosClientes2.Value.Count, 1);

            //Excluir cliente
            var responseExcluirCliente = await _clienteService.Excluir(responseCriarCliente.Value.Id);

            Assert.IsTrue(responseExcluirCliente.Sucess);
            Assert.IsTrue(responseExcluirCliente.Value);

            //Validar se exclui mesmo o cliente
            var responseObterTodosClientes3 = await _clienteService.Obter();

            Assert.IsTrue(responseObterTodosClientes3.Sucess);
            Assert.IsNotNull(responseObterTodosClientes3.Value);
            Assert.AreEqual(responseObterTodosClientes3.Value.Count, 0);
        }
        #endregion

        #region Testes inválidos
        [TestMethod]
        public void Cliente_PropriedadesCliente_Invalido()
        {
            List<ValidationResult> results;
            System.ComponentModel.DataAnnotations.ValidationContext context;
            bool isModelStateValid;

            //Nome: 100 caracteres / Idade 100
            var testeLimiteMaximo = new CriarClienteDto()
            {
                Nome = "1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890",
                Idade = 100
            };

            results = new List<ValidationResult>();
            context = new System.ComponentModel.DataAnnotations.ValidationContext(testeLimiteMaximo, null, null);
            isModelStateValid = Validator.TryValidateObject(testeLimiteMaximo, context, results, true);

            Assert.IsTrue(isModelStateValid);

            //Nome: 5 caracteres / Idade 10
            var testeLimiteMinimo = new CriarClienteDto()
            {
                Nome = "12345",
                Idade = 10

            };

            results = new List<ValidationResult>();
            context = new System.ComponentModel.DataAnnotations.ValidationContext(testeLimiteMinimo, null, null);
            isModelStateValid = Validator.TryValidateObject(testeLimiteMinimo, context, results, true);

            Assert.IsTrue(isModelStateValid);

            //Nome: 101 caracteres / Idade 101
            var testeAcimaLimiteMaximo = new CriarClienteDto()
            {
                Nome = "12345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901",
                Idade = 101

            };

            results = new List<ValidationResult>();
            context = new System.ComponentModel.DataAnnotations.ValidationContext(testeAcimaLimiteMaximo, null, null);
            isModelStateValid = Validator.TryValidateObject(testeAcimaLimiteMaximo, context, results, true);

            Assert.IsFalse(isModelStateValid);

            //Nome: 4 caracteres / Idade 9
            var testeAbaixoLimiteMinimo = new CriarClienteDto()
            {
                Nome = "1234",
                Idade = 9

            };

            results = new List<ValidationResult>();
            context = new System.ComponentModel.DataAnnotations.ValidationContext(testeAbaixoLimiteMinimo, null, null);
            isModelStateValid = Validator.TryValidateObject(testeAbaixoLimiteMinimo, context, results, true);

            Assert.IsFalse(isModelStateValid);
        }

        [TestMethod]
        public async Task Cliente_ObterCliente_Invalido()
        {
            //Consultar cliente não existente na lista de clientes
            var responseObterCliente = await _clienteService.Obter(Guid.NewGuid());

            Assert.IsFalse(responseObterCliente.Sucess);
            Assert.IsNull(responseObterCliente.Value);
            Assert.IsTrue(responseObterCliente.Messages.Count > 0);
        }

        [TestMethod]
        public async Task Cliente_ExcluirCliente_Invalido()
        {
            //Excluir cliente não existente na lista de clientes
            var responseExcluirCliente = await _clienteService.Excluir(Guid.NewGuid());

            Assert.IsFalse(responseExcluirCliente.Sucess);
            Assert.IsFalse(responseExcluirCliente.Value);
            Assert.IsTrue(responseExcluirCliente.Messages.Count > 0);
        }
        #endregion
    }
}
