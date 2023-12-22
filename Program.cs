using DesafioFinal;
using DesafioFinal.BancoDeDados;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

CultureInfo culture = new CultureInfo("pt-BR");
CultureInfo.DefaultThreadCurrentCulture = culture;
CultureInfo.DefaultThreadCurrentUICulture = culture;



var builder = WebApplication.CreateBuilder(args);

#region Cria banco de dados em memória

builder.Services.AddDbContext<InMemoryContext>(options => options.UseInMemoryDatabase("ecommerce"));

#endregion

#region Adiciona o serviço CarregarDados na aplicação

builder.Services.AddTransient<CarregarDados>();

#endregion

# region build da aplicação

var app = builder.Build();

# endregion

# region Mapeamento de endpoints
app.MapClientesEndpoint();
app.MapClientesResumoEndpoint();
app.MapPedidosResumoEndpoint();
app.MapPedidosMaisComprados();
app.MapPedidosMaisCompradosPorCategoriaEndpoint();
app.MapPedidosMaisCompradosPorFornecedorEndpoint();
# endregion

# region Adiciona os dados no banco
var scopedFactory = app.Services.GetService<IServiceScopeFactory>();
using (var scope = scopedFactory.CreateScope())
{
    var service = scope.ServiceProvider.GetService<CarregarDados>();
    service.Carregar();
}
# endregion

await app.RunAsync();

