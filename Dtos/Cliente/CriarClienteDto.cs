using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cliente_CRUD.Dtos.Cliente
{
    public class CriarClienteDto
    {
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Nome do Cliente: Deve ter entre 1 e 100 caracteres.")]
        [Required(ErrorMessage = "Nome do Cliente: Campo Obrigatório.")]
        public string Nome { get; set; }

        [Range(10, 100, ErrorMessage = "Idade do Cliente: Deve ser entre 10 e 100 anos.")]
        [Required(ErrorMessage = "Idade do Cliente: Campo Obrigatório.")]
        public int Idade { get; set; }
    }
}
