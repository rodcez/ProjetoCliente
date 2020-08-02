using Cliente_CRUD.Dtos.Cliente;
using Cliente_CRUD.Models;
using Cliente_CRUD.Services.ClienteService;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cliente_CRUD.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteService _clienteService;

        public ClienteController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        [HttpGet]
        public async Task<IActionResult> Obter()
        {
            var response = await _clienteService.Obter();
            if (!response.Sucess)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpGet]
        [Route("/{id}")]
        public async Task<IActionResult> Obter([FromRoute] Guid id)
        {
            var response = await _clienteService.Obter(id);
            if (!response.Sucess)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] CriarClienteDto cliente)
        {
            var response = await _clienteService.Criar(cliente);
            if (!response.Sucess)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> Atualizar([FromBody] AtualizarClienteDto cliente)
        {
            var response = await _clienteService.Atualizar(cliente);
            if (!response.Sucess)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Excluir([FromRoute] Guid id)
        {
            var response = await _clienteService.Excluir(id);
            if (!response.Sucess)
                return BadRequest(response);

            return Ok(response);
        }
    }
}
