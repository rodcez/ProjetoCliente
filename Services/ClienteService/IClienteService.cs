using Cliente_CRUD.Dtos.Cliente;
using Cliente_CRUD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cliente_CRUD.Services.ClienteService
{
    public interface IClienteService
    {
        Task<ServiceResponse<List<ObterClienteDto>>> Obter();
        Task<ServiceResponse<ObterClienteDto>> Obter(Guid id);
        Task<ServiceResponse<ObterClienteDto>> Criar(CriarClienteDto cliente);
        Task<ServiceResponse<ObterClienteDto>> Atualizar(AtualizarClienteDto cliente);
        Task<ServiceResponse<bool>> Excluir(Guid id);
    }
}
