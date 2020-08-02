using AutoMapper;
using Cliente_CRUD.Dtos.Cliente;
using Cliente_CRUD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cliente_CRUD.SeedWork.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Cliente, CriarClienteDto>().ReverseMap();
            CreateMap<Cliente, ObterClienteDto>().ReverseMap();
            CreateMap<ObterClienteDto, CriarClienteDto>().ReverseMap();
        }
    }
}
