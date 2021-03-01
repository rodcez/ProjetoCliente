using AutoMapper;
using Cliente_CRUD.SeedWork.Mapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cliente_Test.Helpers
{
    public class TestAssembliesResolver
    {
    }

    [TestClass]
    public class InitializerTest
    {
        public TestContext TestContext { get; set; }

        [AssemblyInitialize]
        public static void Inicializar(TestContext context)
        {

        }

        [TestMethod]
        public void VerificarMapper()
        {
            try
            {
                var configuration = new MapperConfiguration(config =>
                {
                    config.AddProfile(new AutoMapperProfile());
                });

                configuration.AssertConfigurationIsValid();
            }
            catch (AutoMapperConfigurationException ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Existem diferenças em algumas classes sendo mapeadas no AutoMapper.");
                sb.AppendLine("Isso não é necessariamente um erro, mas pode mascarar erros na sua implementação.");
                sb.AppendLine("Para evitar este aviso, realize uma das seguintes ações:");
                sb.AppendLine("1: Crie uma interface para o mapeamento entre as classes");
                sb.AppendLine("2: Construa as classes apenas com as propriedades iguais e crie uma classe filho com as propriedades adicionais.");
                sb.AppendLine("3: Mapeie manualmente as propriedades de cada classe.");
                sb.AppendLine();

                foreach (var mapeamento in ex.Errors)
                {
                    sb.AppendLine(string.Format("Mapeamento: {0} -> {1}",
                        mapeamento.TypeMap.SourceType.Name,
                        mapeamento.TypeMap.DestinationType.Name));

                    foreach (var prop in mapeamento.UnmappedPropertyNames)
                    {
                        sb.AppendLine(prop);
                    }

                    sb.AppendLine();
                }

                string msg = sb.ToString();
                Console.WriteLine(msg);
            }
        }
    }
}
