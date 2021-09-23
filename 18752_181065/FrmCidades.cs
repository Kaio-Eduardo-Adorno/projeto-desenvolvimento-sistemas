using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace _18752_181065
{
    public partial class FrmCidades : Form
    {
        MySqlConnection Conexao = new MySqlConnection("server=localhost;uid=root;pwd=etecjau");
        MySqlCommand Comando;
        MySqlDataAdapter Adaptador;

        DataTable datTabela;

        void LimparCampos()
        {
            txtID.Clear();
            txtNome.Clear();
            txtUF.Clear();
            txtPesquisa.Clear();
        }

        void CarregaGrid()
        {
            Adaptador = new MySqlDataAdapter("select * from cidades where nome like @nome order by nome", Conexao);
            Adaptador.SelectCommand.Parameters.AddWithValue("@Nome", txtPesquisa.Text + "%");
            Adaptador.Fill(datTabela = new DataTable());
            dgvCidades.DataSource = datTabela;
        }
        
        public FrmCidades()
        {
            InitializeComponent();
        }

        private void FrmCidades_Load(object sender, EventArgs e)
        {
            CarregaGrid();
            LimparCampos();
        }

        private void dgvCidades_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvCidades.RowCount > 0)
            {
                txtID.Text = dgvCidades.CurrentRow.Cells["id"].Value.ToString();
                txtNome.Text = dgvCidades.CurrentRow.Cells["nome"].Value.ToString();
                txtUF.Text = dgvCidades.CurrentRow.Cells["uf"].Value.ToString();
            }
        }

        private void btnIncluir_Click(object sender, EventArgs e)
        {
            if (txtNome.Text == "")
            {
                MessageBox.Show("Favor informar o nome da Cidade", "Inclusão");
                return;
            }
            Conexao.Open();

            Comando = new MySqlCommand("INSERT INTO CIDADES (nome, uf) VALUES (@nome, @uf)", Conexao);
            Comando.Parameters.AddWithValue("@nome", txtNome.Text);
            Comando.Parameters.AddWithValue("@uf", txtUF.Text);
            Comando.ExecuteNonQuery();
            Conexao.Close();
            CarregaGrid();
            LimparCampos();
        }

        private void btnAtualizar_Click(object sender, EventArgs e)
        {
            if (txtID.Text == "")
            {
                MessageBox.Show("Favor Selecionar uma cidade", "Atualização");
                return;
            }


            Conexao.Open();
            Comando = new MySqlCommand("UPDATE cidades set nome = @nome, uf = @uf where id = @id", Conexao);
            Comando.Parameters.AddWithValue("@id", Convert.ToInt16(txtID.Text));
            Comando.Parameters.AddWithValue("@nome", txtNome.Text);
            Comando.Parameters.AddWithValue("@uf", txtUF.Text);
            Comando.ExecuteNonQuery();
            Conexao.Close();
            LimparCampos();
            CarregaGrid();
        }
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            LimparCampos();
            CarregaGrid();
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            if (txtID.Text == "")
            {
                MessageBox.Show("Favor Selecionar uma cidade", "Exclusão");
                return;
            }

            if (MessageBox.Show("Deseja realmente Excluir a Cidade? " + txtNome.Text, "Exclusão",
                                 MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Conexao.Open();
                Comando = new MySqlCommand("DELETE FROM Cidades WHERE id = @id", Conexao);
                Comando.Parameters.AddWithValue("@id", txtID.Text);
                Comando.ExecuteNonQuery();
                Conexao.Close();
                LimparCampos();
                CarregaGrid();
            }
        }

        private void btnPesquisar_Click(object sender, EventArgs e)
        {
            CarregaGrid();
            txtPesquisa.Clear();
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}
