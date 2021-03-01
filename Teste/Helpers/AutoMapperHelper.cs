using AutoMapper;
using Cliente_CRUD.SeedWork.Mapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cliente_Test.Helpers
{
    public static class AutoMapperHelper
    {
        public static IMapper RetornarMapper()
        {
            var profile = new AutoMapperProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(profile));
            return configuration.CreateMapper();
        }
    }
}
