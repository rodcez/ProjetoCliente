using System;
using System.Collections.Generic;
using Cliente_CRUD.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Cliente_CRUD.Repositories.Cliente
{
    public interface IClienteRepository
    {
        Task<List<Models.Cliente>> Obter();
        Task<Models.Cliente> Obter(Guid id);
        Task<Models.Cliente> Criar(Models.Cliente cliente);
        Task<Models.Cliente> Atualizar(Models.Cliente cliente);
        Task<bool> Excluir(Guid id);
    }
}
