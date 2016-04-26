using System;
using System.Collections.Generic;

using AutoMapper;
using AutoMapper.Mappers;

namespace IntraWeb
{
    public static class TestHelper
    {

        /// <summary>
        /// Creates a mapper for tests.
        /// </summary>
        /// <param name="configuration">Configuration of mapper.</param>
        /// <returns>
        /// <para>Unit tests are executed <b>in parallel</b>. beacuse of this, it is not possible to create a mapper
        /// using standard way: <c>new new MapperConfiguration(Action&lt;IMapperConfiguration&gt;)</c>. Mapping objects with
        /// such mappers throws an <c>InvalidOperationException</c>, because one thread is iterating inner <b>static</b>
        /// list of mappers, while another thread tries to add to the same list. This method creates separate lists for
        /// each returned mapper.</para>
        /// </returns>
        public static IMapper CreateMapper(Action<IMapperConfiguration> configuration)
        {
            var mapperConfig = new MapperConfiguration(
                configuration,
                new List<IObjectMapper>(MapperRegistry.Mappers),
                new List<ITypeMapObjectMapper>(TypeMapObjectMapperRegistry.Mappers)
            );

            return mapperConfig.CreateMapper();
        }

    }
}
