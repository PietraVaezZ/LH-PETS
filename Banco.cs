using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Projeto_Web_Lh_Pets_versão_1
{
    class Banco
    {
        private List<Clientes> lista = new List<Clientes>();

        public List<Clientes> GetLista()
        {
            return lista;
        }

        public Banco()
        {
            try
            {
                string connectionString = "Data Source=localhost\\SQLEXPRESS;Initial Catalog=vendas;User ID=sa;Password=12345;";

                using (SqlConnection conexao = new SqlConnection(connectionString))
                {
                    conexao.Open();
                    string sql = "SELECT * FROM tblclientes";

                    using (SqlCommand comando = new SqlCommand(sql, conexao))
                    {
                        using (SqlDataReader tabela = comando.ExecuteReader())
                        {
                            while (tabela.Read())
                            {
                                if (!tabela.IsDBNull(0) && !tabela.IsDBNull(1) && !tabela.IsDBNull(2) && !tabela.IsDBNull(3) && !tabela.IsDBNull(4) && !tabela.IsDBNull(5) && !tabela.IsDBNull(6) && !tabela.IsDBNull(7))
                                {
                                    lista.Add(new Clientes()
                                    {
                                        cpf_cnpj = tabela["cpf_cnpj"].ToString(),
                                        nome = tabela["nome"].ToString(),
                                        endereco = tabela["endereco"].ToString(),
                                        rg_ie = tabela["rg_ie"].ToString(),
                                        tipo = tabela["tipo"].ToString(),
                                        valor = tabela.GetFloat(5),
                                        valor_imposto = tabela.GetFloat(6),
                                        total = tabela.GetFloat(7)
                                    });
                                }
                            }
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine("Erro ao acessar o banco de dados: " + e.ToString());
                throw; // Re-throw a exceção para que ela possa ser tratada em um nível superior, se necessário.
            }
        }

        public string GetListaString()
        {
            StringBuilder enviar = new StringBuilder();
            enviar.Append("<!DOCTYPE html>\n<html>\n<head>\n<meta charset='utf-8' />\n");
            enviar.Append("<title>Cadastro de Clientes</title>\n</head>\n<body>");
            enviar.Append("<b>CPF / CNPJ - Nome - Endereço - RG / IE - Tipo - Valor - Valor Imposto - Total</b>");

            int i = 0;
            string corFundo, corTexto;

            foreach (Clientes cli in GetLista())
            {
                corFundo = (i % 2 == 0) ? "#6f47ff" : "#ffffff";
                corTexto = (i % 2 == 0) ? "white" : "#6f47ff";
                i++;

                enviar.AppendFormat("\n<br><div style='background-color:{0};color:{1};'>", corFundo, corTexto);
                enviar.AppendFormat("{0} - {1} - {2} - {3} - {4} - {5:C} - {6:C} - {7:C}<br>", cli.cpf_cnpj, cli.nome, cli.endereco, cli.rg_ie, cli.tipo, cli.valor, cli.valor_imposto, cli.total);
                enviar.Append("</div>");
            }

            enviar.Append("</body>\n</html>");
            return enviar.ToString();
        }

        public void ImprimirListaConsole()
        {
            Console.WriteLine("CPF / CNPJ - Nome - Endereço - RG / IE - Tipo - Valor - Valor Imposto - Total");

            foreach (Clientes cli in GetLista())
            {
                Console.WriteLine($"{cli.cpf_cnpj} - {cli.nome} - {cli.endereco} - {cli.rg_ie} - {cli.tipo} - {cli.valor:C} - {cli.valor_imposto:C} - {cli.total:C}");
            }
        }
    }
}
