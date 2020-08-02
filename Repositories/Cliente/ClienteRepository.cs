using Cliente_CRUD.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cliente_CRUD.Repositories.Cliente
{
    public class ClienteRepository : IClienteRepository
    {
        public async Task<List<Models.Cliente>> Obter()
        {
            using (var context = new ClienteContext())
            {
                return await context.Clientes.Select(s => s).ToListAsync();
            }
        }

        public async Task<Models.Cliente> Obter(Guid id)
        {
            using (var context = new ClienteContext())
            {
                return await context.Clientes.FirstAsync(f => f.Id == id);
            }
        }

        public async Task<Models.Cliente> Criar(Models.Cliente cliente)
        {
            using (var context = new ClienteContext())
            {
                cliente.Id = Guid.NewGuid();
                await context.Clientes.AddAsync(cliente);

                if((await context.SaveChangesAsync()) > 0)
                    return cliente;

                return null;
            }
        }

        public async Task<Models.Cliente> Atualizar(Models.Cliente cliente)
        {
            using (var context = new ClienteContext())
            {
                context.Entry(await context.Clientes.FirstOrDefaultAsync(f => f.Id == cliente.Id)).CurrentValues.SetValues(cliente);

                if ((await context.SaveChangesAsync()) > 0)
                    return cliente;

                return null;
            }
        }

        public async Task<bool> Excluir(Guid id)
        {
            using (var context = new ClienteContext())
            {
                var clienteDb = await context.Clientes.FirstAsync(f => f.Id == id);
                context.Clientes.Remove(clienteDb);

                return (await context.SaveChangesAsync()) > 0;
            }
        }
    }
}
