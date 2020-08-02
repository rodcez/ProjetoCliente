using AutoMapper;
using Cliente_CRUD.Dtos.Cliente;
using Cliente_CRUD.Models;
using Cliente_CRUD.Repositories.Cliente;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cliente_CRUD.Services.ClienteService
{
    public class ClienteService : IClienteService
    {
        private readonly IMapper _mapper;
        private readonly IClienteRepository _clienteRepository;

        public ClienteService(IMapper mapper, IClienteRepository clienteRepository)
        {
            _mapper = mapper;
            _clienteRepository = clienteRepository;
        }

        public async Task<ServiceResponse<List<ObterClienteDto>>> Obter()
        {
            var sr = new ServiceResponse<List<ObterClienteDto>>();
            try
            {
                var listaCliente = await _clienteRepository.Obter();

                sr.Value = _mapper.Map<List<ObterClienteDto>>(listaCliente);
            }
            catch (Exception ex)
            {
                sr.Sucess = false;
                sr.Messages.Add(ex.Message);
            }
            return sr;
        }

        public async Task<ServiceResponse<ObterClienteDto>> Obter(Guid id)
        {
            var sr = new ServiceResponse<ObterClienteDto>();
            try
            {
                var cliente = await _clienteRepository.Obter(id);

                sr.Value = _mapper.Map<ObterClienteDto>(cliente);
            }
            catch (Exception ex)
            {
                sr.Sucess = false;
                sr.Messages.Add(ex.Message);
            }
            return sr;
        }

        public async Task<ServiceResponse<ObterClienteDto>> Criar(CriarClienteDto criarCliente)
        {
            var sr = new ServiceResponse<ObterClienteDto>();
            try
            {
                var cliente = _mapper.Map<Cliente>(criarCliente);
                var response = await _clienteRepository.Criar(cliente);
                sr.Value = _mapper.Map<ObterClienteDto>(response);
            }
            catch (Exception ex)
            {
                sr.Sucess = false;
                sr.Messages.Add(ex.Message);
            }
            return sr;
        }

        public async Task<ServiceResponse<ObterClienteDto>> Atualizar(AtualizarClienteDto atualizarCliente)
        {
            var sr = new ServiceResponse<ObterClienteDto>();
            try
            {
                var obterCliente = await Obter(atualizarCliente.Id);
                if (!obterCliente.Sucess)
                    throw new Exception();

                var cliente = _mapper.Map<Cliente>(obterCliente.Value);
                cliente.Nome = atualizarCliente.Nome;
                cliente.Idade = atualizarCliente.Idade;

                var response = await _clienteRepository.Atualizar(cliente);
                sr.Value = _mapper.Map<ObterClienteDto>(response);
            }
            catch (Exception ex)
            {
                sr.Sucess = false;
                sr.Messages.Add(ex.Message);
            }
            return sr;
        }

        public async Task<ServiceResponse<bool>> Excluir(Guid id)
        {
            var sr = new ServiceResponse<bool>();
            try
            {
                sr.Value = await _clienteRepository.Excluir(id);
            }
            catch (Exception ex)
            {
                sr.Sucess = false;
                sr.Messages.Add(ex.Message);
            }
            return sr;
        }
    }
}
