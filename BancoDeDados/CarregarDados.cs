using System.Globalization;
using CsvHelper;
using DesafioFinal.BancoDeDados.DTOs;

namespace DesafioFinal.BancoDeDados
{
    public class CarregarDados
    {
        private readonly InMemoryContext _inMemoryContext;

        public CarregarDados(InMemoryContext inMemoryContext)
        {
            _inMemoryContext = inMemoryContext;
        }

        public void Carregar()
        {
            if (!_inMemoryContext.Clientes.Any())
            {
                List<Clientes> clientes;
                using (var reader = new StreamReader("arquivos/clientes.csv"))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    var records = csv.GetRecords<Clientes>();
                    _inMemoryContext.Clientes.AddRange(records);
                    _inMemoryContext.SaveChanges();
                }
            }

            if (!_inMemoryContext.Fornecedores.Any())
            {
                List<Fornecedores> items;
                using (var reader = new StreamReader("arquivos/fornecedores.csv"))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    var records = csv.GetRecords<Fornecedores>();
                    _inMemoryContext.Fornecedores.AddRange(records);
                    _inMemoryContext.SaveChanges();
                }
            }

            if (!_inMemoryContext.Categorias.Any())
            {
                List<Categorias> items;
                using (var reader = new StreamReader("arquivos/categorias.csv"))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    var records = csv.GetRecords<Categorias>();
                    _inMemoryContext.Categorias.AddRange(records);
                    _inMemoryContext.SaveChanges();
                }
            }

            if (!_inMemoryContext.ItensDePedidos.Any())
            {
                List<ItensDePedidos> items;
                using (var reader = new StreamReader("arquivos/itens_de_pedidos.csv"))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    var records = csv.GetRecords<ItensDePedidos>();
                    _inMemoryContext.ItensDePedidos.AddRange(records);
                    _inMemoryContext.SaveChanges();
                }
            }

            if (!_inMemoryContext.Pedidos.Any())
            {
                List<Pedidos> items;
                using (var reader = new StreamReader("arquivos/pedidos.csv"))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    var records = csv.GetRecords<Pedidos>();
                    _inMemoryContext.Pedidos.AddRange(records);
                    _inMemoryContext.SaveChanges();
                }
            }

            if (!_inMemoryContext.Produtos.Any())
            {
                List<Produtos> items;
                using (var reader = new StreamReader("arquivos/produtos.csv"))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    var records = csv.GetRecords<Produtos>();
                    _inMemoryContext.Produtos.AddRange(records);
                    _inMemoryContext.SaveChanges();
                }
            }

        }
    }
}
