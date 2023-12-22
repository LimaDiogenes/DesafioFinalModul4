using DesafioFinal.BancoDeDados;
using DesafioFinal.BancoDeDados.DTOs;
using Microsoft.EntityFrameworkCore;


namespace DesafioFinal
{
    public static class EndpointClientes
    {
        #region MapClientesEndpoint
        public static void MapClientesEndpoint(this WebApplication app)
        {
            app.MapPost("/clientes", async (InMemoryContext context) =>
            {
                var clientList = await context.Clientes.ToListAsync();

                var topClients = clientList.OrderBy(client => client.first_name).Take(100);
                List<Dictionary<string, object>> finalList = new();

                foreach (var c in topClients)
                {
                    if (c.country == "-")
                    {
                        c.country = "desconhecido";
                    }

                    Dictionary<string, object> client = new()
                    {   //customer_id,first_name,last_name,email,address,city,state,country
                        { "nome_completo", (c.first_name + c.last_name) },
                        { "email", c.email },
                        { "id_do_cliente", c.customer_id },
                        { "endereco", c.address },
                        { "cidade", c.city },
                        { "estado", c.state },
                        { "pais", c.country }
                    };
                    finalList.Add(client);
                }

                return new 
                {
                    clientes = finalList
                };
            });
        }
        #endregion

        #region MapClientesResumoEndpoint
        public static void MapClientesResumoEndpoint(this WebApplication app)
        {
            app.MapPost("/clientes/resumo", async (InMemoryContext context) =>
            {
                var consumerList = await context.Clientes.ToListAsync();
                #region topCountries
                var groupedCountries = consumerList.GroupBy(consumer => consumer.country)
                                                   .OrderByDescending(group => group.Count())
                                                   .Take(5)
                                                   .ToList();

                Dictionary<string, int> topCountries = new();
                foreach (var group in groupedCountries)
                {
                    var countryKey = group.Key;
                    if (countryKey == "-")
                    {
                        countryKey = "desconhecido";
                    }
                    var number = group.Count();
                    topCountries.Add(countryKey, number);
                }
                #endregion

                #region topDomains
                List<string> domains = new();
                foreach (var c in consumerList)
                {
                    var d = c.email.Split('@');
                    var e = d.Last();
                    domains.Add(e);
                }

                var groupedDomains = domains.GroupBy(domain => domain)
                                            .OrderByDescending(domain => domain.Count())
                                            .Take(5)
                                            .ToList();

                Dictionary<string, int> topDomains = new();
                foreach (var group in groupedDomains)
                {
                    var domainKey = group.Key;
                    var number = group.Count();
                    topDomains.Add(domainKey, number);
                }

                #endregion

                return new
                {
                    paisesComMaisClientes = topCountries
 ,
                    dominios = topDomains
                };
            });
        }
        #endregion
    }
}
