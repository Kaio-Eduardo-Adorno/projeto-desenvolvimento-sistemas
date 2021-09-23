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
    public partial class FrmCursos : Form
    {
        MySqlConnection Conexao = new MySqlConnection("server=localhost;uid=root;pwd=etecjau");
        MySqlCommand Comando;
        MySqlDataAdapter Adaptador;

        DataTable datTabela;

        void LimparCampos()
        {
            txtID.Clear();
            txtNome.Clear();
            txtDuracao.Clear();
            txtPesquisa.Clear();
        }

        void CarregaGrid()
        {
            Adaptador = new MySqlDataAdapter("select * from cursos where nome like @nome order by nome", Conexao);
            Adaptador.SelectCommand.Parameters.AddWithValue("@Nome", txtPesquisa.Text + "%");
            Adaptador.Fill(datTabela = new DataTable());
            dgvCursos.DataSource = datTabela;
        }

        public FrmCursos()
        {
            InitializeComponent();
        }

        private void FrmCursos_Load(object sender, EventArgs e)
        {
            CarregaGrid();
            LimparCampos();
        }

        private void dgvCursos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvCursos.RowCount > 0)
            {
                txtID.Text = dgvCursos.CurrentRow.Cells["id"].Value.ToString();
                txtNome.Text = dgvCursos.CurrentRow.Cells["nome"].Value.ToString();
                txtDuracao.Text = dgvCursos.CurrentRow.Cells["duracao"].Value.ToString();
            }
        }

        private void btnIncluir_Click(object sender, EventArgs e)
        {
            if (txtNome.Text == "")
            {
                MessageBox.Show("Favor informar o nome do Curso", "Inclusão");
                return;
            }
            Conexao.Open();

            Comando = new MySqlCommand("INSERT INTO CURSOS (nome, duracao) VALUES (@nome, @duracao)", Conexao);
            Comando.Parameters.AddWithValue("@nome", txtNome.Text);
            Comando.Parameters.AddWithValue("@duracao", txtDuracao.Text);
            Comando.ExecuteNonQuery();
            Conexao.Close();
            LimparCampos();
            CarregaGrid();
        }

        private void btnAtualizar_Click(object sender, EventArgs e)
        {
            if (txtID.Text == "")
            {
                MessageBox.Show("Favor Selecionar um curso", "Atualização");
                return;
            }


            Conexao.Open();
            Comando = new MySqlCommand("UPDATE cursos set nome = @nome, duracao = @duracao where id = @id", Conexao);
            Comando.Parameters.AddWithValue("@id", Convert.ToInt16(txtID.Text));
            Comando.Parameters.AddWithValue("@nome", txtNome.Text);
            Comando.Parameters.AddWithValue("@duracao", txtDuracao.Text);
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
                MessageBox.Show("Favor Selecionar um Curso", "Exclusão");
                return;
            }

            if (MessageBox.Show("Deseja realmente Excluir o Curso? " + txtNome.Text, "Exclusão",
                                 MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Conexao.Open();
                Comando = new MySqlCommand("DELETE FROM Cursos WHERE id = @id", Conexao);
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
