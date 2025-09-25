using System;
using System.Data;
using System.Windows.Forms;
using Npgsql;

namespace ControleDeEstoqueDeProdutos
{
    public partial class Form1 : Form
    {

        private string connString = "Host=localhost;Port=5432;Username=postgres;Password=suasenha;Database=seubanco";

        public Form1()
        {
            InitializeComponent();
            CarregarProdutos();
        }

        private void CarregarProdutos()
        {
            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                string sql = "SELECT id_produto, nome, preco, quantidade FROM produto ORDER BY id_produto";
                using (var da = new NpgsqlDataAdapter(sql, conn))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridViewProdutos.DataSource = dt;
                }
            }
        }

        private void btnCadastrar_Click(object sender, EventArgs e)
        {
            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                string sql = "INSERT INTO produto (nome, preco, quantidade) VALUES (@nome, @preco, @qtd)";
                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("nome", txtNome.Text);
                    cmd.Parameters.AddWithValue("preco", decimal.Parse(txtPreco.Text));
                    cmd.Parameters.AddWithValue("qtd", int.Parse(txtQuantidade.Text));
                    cmd.ExecuteNonQuery();
                }
            }
            MessageBox.Show("Produto cadastrado!");
            CarregarProdutos();
        }


        private void btnIncrementar_Click(object sender, EventArgs e)
        {
            if (dataGridViewProdutos.SelectedRows.Count > 0)
            {
                int id = Convert.ToInt32(dataGridViewProdutos.SelectedRows[0].Cells["id_produto"].Value);
                int qtd = int.Parse(txtQuantidade.Text);

                using (var conn = new NpgsqlConnection(connString))
                {
                    conn.Open();
                    string sql = "UPDATE produto SET quantidade = quantidade + @qtd WHERE id_produto = @id";
                    using (var cmd = new NpgsqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("qtd", qtd);
                        cmd.Parameters.AddWithValue("id", id);
                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Quantidade incrementada!");
                CarregarProdutos();
            }
        }

        private void btnDecrementar_Click(object sender, EventArgs e)
        {
            if (dataGridViewProdutos.SelectedRows.Count > 0)
            {
                int id = Convert.ToInt32(dataGridViewProdutos.SelectedRows[0].Cells["id_produto"].Value);
                int qtd = int.Parse(txtQuantidade.Text);

                using (var conn = new NpgsqlConnection(connString))
                {
                    conn.Open();
                    string sql = "UPDATE produto SET quantidade = quantidade - @qtd WHERE id_produto = @id";
                    using (var cmd = new NpgsqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("qtd", qtd);
                        cmd.Parameters.AddWithValue("id", id);
                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Quantidade decrementada!");
                CarregarProdutos();
            }
        }
    }
}
