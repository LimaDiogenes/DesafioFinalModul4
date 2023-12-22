using DesafioFinal.BancoDeDados;
using DesafioFinal.BancoDeDados.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.RegularExpressions;

namespace DesafioFinal
{
    public static class EndpointPedidos
    {
        #region MapClientesPedidos
        public static void MapPedidosEndpoint(this WebApplication app)
        {
            app.MapPost("/pedidos", async (InMemoryContext context) =>
            {
                return new
                {
                    placeholder = "placeholder"
                };
            });
        }
        #endregion

        #region MapPedidosResumoEndpoint
        public static void MapPedidosResumoEndpoint(this WebApplication app)
        {
            app.MapPost("/pedidos/resumo", async (InMemoryContext context) =>
            {
                var clientList = await context.Clientes.ToListAsync();
                var ordersList = await context.Pedidos.ToListAsync();
                Dictionary<string, string> totalOrders = new();
                Dictionary<string, string> topClients = new();
                Dictionary<string, string> totalOrdersPerFortnight = new();

                #region total pedidos

                var groupedByDate = ordersList.GroupBy(order => order.order_date.Month);
                foreach (var group in groupedByDate)
                {
                    group.GroupBy(order => order.order_date.Year);
                }

                foreach (var groupOfGroups in groupedByDate)
                {
                    double groupTotal = Math.Round(groupOfGroups.Sum(order => order.total_amount), 2);

                    var firstInGroup = groupOfGroups.FirstOrDefault();
                    string groupName = $"{firstInGroup.order_date.Year}-{firstInGroup.order_date.Month.ToString().PadLeft(2, '0')}";

                    totalOrders.Add(groupName, $"{groupTotal:f2}");
                }
                #endregion

                #region top clientes

                var groupedByClient = ordersList.GroupBy(order => order.customer_id)
                                                .OrderByDescending(total => total.Sum(orders => orders.total_amount))
                                                .Take(10);

                foreach (var group in groupedByClient)
                {
                    var groupTotal = group.Sum(order => order.total_amount);
                    var client = clientList.FirstOrDefault(c => c.customer_id == group.Key);
                    string clientName = $"{client.first_name} {client.last_name}";

                    topClients.Add(clientName, $"{groupTotal:f2}");
                }


                #endregion

                #region total quinzena
                Dictionary<string, Dictionary<string, string>> byFortnight = new();

                foreach (var dateGroup in groupedByDate)
                {
                    var dateYearMonth = $"{dateGroup.First().order_date.Year}-{dateGroup.Key.ToString().PadLeft(2, '0')}";
                    var First15 = dateGroup.Where(o => o.order_date.Day < 16)
                                                   .Sum(x => x.total_amount);

                    var Last15 = dateGroup.Where(o => o.order_date.Day >= 16)
                                                  .Sum(x => x.total_amount);


                    Dictionary<string, string> final = new()
                    {
                        { $"primeira", $"{First15:f2}" },
                        { $"segunda", $"{Last15:f2}" }
                    };

                    byFortnight.Add(dateYearMonth, final);
                }

                byFortnight = byFortnight.OrderBy(entry => entry.Key)
                                         .ToDictionary(entry => entry.Key, entry => entry.Value);

                #endregion

                return new
                {
                    totalPedidos = totalOrders,
                    topClientes = topClients,
                    totalPedidosPorQuinzena = byFortnight

                };
            });
        }
        #endregion

        #region MapPedidosMaisComprados
        public static void MapPedidosMaisComprados(this WebApplication app)
        {
            app.MapPost("/pedidos/mais_comprados", async (InMemoryContext context) =>
            {
                IQueryable<Pedidos> orders = context.Pedidos;
                IQueryable<ItensDePedidos> itemsInOrders = context.ItensDePedidos;
                IQueryable<Produtos> items = context.Produtos;
                IQueryable<Categorias> categories = context.Categorias;

                #region topItemsByValue
                var topItemsByValue = itemsInOrders.ToList()
                                                   .GroupBy(item => item.product_id)
                                                   .OrderByDescending(price => price.Sum(item => item.price * item.quantity))
                                                   .Take(30);

                List<Dictionary<string, string>> categoriasPorValor = new();

                foreach (var group in topItemsByValue)
                {
                    //string p = $"{sumPrice:f2}";
                    var thisItem = items.FirstOrDefault(item => item.product_id == group.Key);
                    var itemName = thisItem.description;
                    var itemCat = categories.FirstOrDefault(cat => cat.category_id == thisItem.category_id).category_name;
                    var itemQtt = $"{group.Sum(x => x.quantity)}";
                    var itemValue = $"{group.Sum(x => x.price * x.quantity):f2}";
                    Dictionary<string, string> itemDict = new()
                    {
                        {"nome", itemName},
                        {"categoria", itemCat},
                        {"quantidade", itemQtt},
                        {"valor", itemValue}
                    };
                    categoriasPorValor.Add(itemDict);
                }

                #endregion
                #region topItemsByQuantity

                var topItemsByQuantity = itemsInOrders.ToList()
                                   .GroupBy(item => item.product_id)
                                   .OrderByDescending(qtt => qtt.Sum(item => item.quantity))
                                   .ThenByDescending(value => value.Sum(x => x.price * x.quantity))
                                   .Take(30);

                List<Dictionary<string, string>> categoriasPorQuantidade = new();

                foreach (var group in topItemsByQuantity)
                {
                    //string p = $"{sumPrice:f2}";
                    var thisItem = items.FirstOrDefault(item => item.product_id == group.Key);
                    var itemName = thisItem.description;
                    var itemCat = categories.FirstOrDefault(cat => cat.category_id == thisItem.category_id).category_name;
                    var itemQtt = $"{group.Sum(x => x.quantity)}";
                    var itemValue = $"{group.Sum(x => x.price * x.quantity):f2}";
                    Dictionary<string, string> itemDict = new()
                    {
                        {"nome", itemName},
                        {"categoria", itemCat},
                        {"quantidade", itemQtt},
                        {"valor", itemValue}
                    };
                    categoriasPorQuantidade.Add(itemDict);
                }
                #endregion
                return new
                {
                    produtosMaisCompradosPorValor = categoriasPorValor,
                    produtosMaisCompradosPorQuantidade = categoriasPorQuantidade
                };
            });
        }
        #endregion

        #region MapPedidosMaisCompradosPorCategoriaEndpoint
        public static void MapPedidosMaisCompradosPorCategoriaEndpoint(this WebApplication app)
        {
            app.MapPost("/pedidos/mais_comprados_por_categoria", async (InMemoryContext context) =>
            {
                IQueryable<Pedidos> orders = context.Pedidos;
                IQueryable<ItensDePedidos> itemsInOrders = context.ItensDePedidos;
                IQueryable<Produtos> items = context.Produtos;
                IQueryable<Categorias> categories = context.Categorias.OrderBy(name => name.category_name);

                var itemsById = itemsInOrders.ToList()
                                             .GroupBy(item => item.product_id)
                                             .ToDictionary(group => group.Key, group => group.Sum(item => item.quantity));

                Dictionary<string, List<Dictionary<string, string>>> categoriasPorValor = new();
                Dictionary<string, List<Dictionary<string, string>>> categoriasPorQuantidade = new();

                foreach (var cat in categories)
                {
                    var itemsInCat = await items.Where(item => item.category_id == cat.category_id)
                                                .ToListAsync();

                    List<Dictionary<string, string>> catListByValue = new();
                    List<Dictionary<string, string>> catListByQtt = new();

                    foreach (var item in itemsInCat)
                    {
                        itemsById.TryGetValue(item.product_id, out int qtt);
                        var valueI = qtt * item.price;
                        #region parte que nao funcionou
                        // NAO FUNCIONOU \/ ( POSTMAN FICOU CARREGANDO ETERNAMENTE )
                        //var filteredItems = itemsById.Where(i => i.Key == item.product_id);

                        //int qtt = 5;

                        //foreach (var i in filteredItems)
                        //{
                        //    qtt += i.Sum(x => x.quantity);
                        //}

                        //var qtt = itemsInOrders.Where(i => i.product_id == item.product_id)
                        //                       .Sum(iQtt => iQtt.quantity);
                        //var valueI = itemsInOrders.Where(v => v.product_id == item.product_id)
                        //                         .Sum(iValue => iValue.price * iValue.quantity);
                        #endregion
                        Dictionary<string, string> d = new()
                        {
                            { "nome", item.product_name },
                            { "quantidade", $"{qtt}" },
                            { "valor", $"{valueI:f2}" }
                        };
                        catListByValue.Add(d);
                        catListByQtt.Add(d);
                    }
                    catListByValue = catListByValue.OrderByDescending(d => decimal.Parse(d["valor"]))
                                     .Take(30)
                                     .ToList();
                    catListByQtt = catListByQtt.OrderByDescending(d => decimal.Parse(d["quantidade"]))
                                                   .Take(30)
                                                   .ToList();

                    var catNameFormatted = Regex.Replace(cat.category_name.Replace(" ", ""), @"[^\w\s]", "");
                    catNameFormatted = Regex.Replace(catNameFormatted.Normalize(NormalizationForm.FormD), @"[\p{M}]", "");

                    categoriasPorValor.Add(catNameFormatted, catListByValue);
                    categoriasPorQuantidade.Add(catNameFormatted, catListByQtt);
                }

                return new
                {
                    categoriasPorValor,
                    categoriasPorQuantidade
                };
            });
        }
        #endregion
        #region MapPedidosMaisCompradosPorFornecedorEndpoint
        public static void MapPedidosMaisCompradosPorFornecedorEndpoint(this WebApplication app)
        {
            app.MapPost("/pedidos/mais_comprados_por_fornecedor", async (InMemoryContext context) =>
            {
                IQueryable<Pedidos> orders = context.Pedidos;
                IQueryable<ItensDePedidos> itemsInOrders = context.ItensDePedidos;
                IQueryable<Produtos> items = context.Produtos;
                IQueryable<Categorias> categories = context.Categorias;
                IQueryable<Fornecedores> suppliers = context.Fornecedores.OrderBy(name => name.supplier_name);

                var newItems = from item in items
                               join io in itemsInOrders
                               on item.product_id equals io.product_id
                               select new
                               {
                                   nome = item.product_name,
                                   quantidade = io.quantity,
                                   valor = io.price * io.quantity,
                                   categoriaId = item.category_id,
                                   supplierId = item.supplier_id
                               };
                var catItems = from item in newItems
                               join cat in categories
                               on item.categoriaId equals cat.category_id
                               select new
                               {
                                   item.nome,
                                   categoria = cat.category_name,
                                   item.quantidade,
                                   item.valor,
                                   item.supplierId
                               };
                var supItems = from item in catItems
                               join sup in suppliers
                               on item.supplierId equals sup.supplier_id
                               select new
                               {
                                   item.nome,
                                   item.categoria,
                                   item.quantidade,
                                   item.valor,
                                   sup.supplier_id,
                                   sup.supplier_name
                               };
                var groupedBySup = supItems.ToList()
                                           .GroupBy(s => s.supplier_name)
                                           .Select(s => s.OrderByDescending(s => s.quantidade)
                                                         .ThenByDescending(s => s.valor)
                                                         .Take(30));
                List<Dictionary<string, object>> TopProdutos = new();
                
                foreach(var g in groupedBySup)
                {
                    foreach (var group in g)
                    {
                        Dictionary<string, object> groupsBySup = new()
                        {
                           { "nome", group.nome },
                           { "categoria", group.categoria },
                           { "quantidade", group.quantidade },
                           { "valor", group.valor }
                        };
                        TopProdutos.Add(groupsBySup);
                    }
                }

                #region algumas tentativas falhas
                //List<Dictionary<string, string>> itemsQtt = new();

                //foreach( var i in itemsBySup)
                //{
                //    var supId = i.Key;

                //    var supName = suppliers.FirstOrDefault(s => s.supplier_id == supId).supplier_name;

                //    var itemName = i.Select(i => i.product_name);
                //    var category = categories.FirstOrDefault(cat => cat.category_id == i.First().category_id).category_id;
                //    var quantity = i.Sum(item => itemsInOrders.Count(io => io.product_id == item.product_id));
                //    var value = quantity * i.First().price;
                //    Dictionary<string, string> supplierInfo = new Dictionary<string, string>
                //    {                        
                //        { "nome", itemName.ToString() },
                //        { "categoria", category.ToString() },
                //        { "Quantity", quantity.ToString() },
                //        { "TotalValue", value.ToString() }
                //    };

                //    itemsQtt.Add(supplierInfo);
                //}
                //{
                //    "nomeDoFornecedor": [
                //        {
                //            "nome": "nome do produto",
                //            "categoria": "nome da categoria",
                //            "quantidade": 50,
                //            "valor": 1000
                //        }
                //    ],
                #endregion
                return new
                {
                    TopProdutos
                };

                /// p.s.: Professor, desculpe, nao consegui finalizar este ultimo por falta de tempo, sei que nao ficou como deveria.
                /// Tentei usar a sintaxe do linq para aprender melhor, e acabei me complicando um pouco, mas
                /// mesmo nao valendo mais nota, vou tentar resolver na minha folga, segunda.
                /// Muito obrigado pelas aulas!!
            });
        }
        #endregion
    }
}
