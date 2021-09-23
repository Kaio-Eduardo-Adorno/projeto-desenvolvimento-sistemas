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
    public partial class FrmAlunos : Form
    {
        MySqlConnection Conexao = new MySqlConnection("server=localhost;uid=root;pwd=etecjau");
        MySqlCommand Comando;
        MySqlDataAdapter Adaptador;

        DataTable datTabela;

        void LimparCampos()
        {
            txtID.Clear();
            txtNome.Clear();
            chkDesistente.Checked = false;
            txtAno.Clear();
            mtbDataNasc.Clear();
            txtUF.Clear();
            cboCidade.SelectedIndex = -1;
            cboCurso.SelectedIndex = -1;
            txtPesquisa.Clear();
            txtRenda.Clear();
            picFoto.ImageLocation = "";
        }

        void CarregaGrid()
        {
            Adaptador = new MySqlDataAdapter("select a.*, ci.nome cidade, ci.UF, c.nome curso FROM alunos a left join  " + 
                "cidades ci on (a.id_cidade = ci.id) left join cursos c on (c.id = a.id_curso)" +
                "where a.nome like @Nome order by a.nome", Conexao);
            Adaptador.SelectCommand.Parameters.AddWithValue("@Nome", txtPesquisa.Text + "%");
            Adaptador.Fill(datTabela = new DataTable());
            dgvAlunos.DataSource = datTabela;
        }

        void CarregarComboCidade()
        {
            Adaptador = new MySqlDataAdapter("SELECT * FROM Cidades order by nome", Conexao);
            Adaptador.Fill(datTabela = new DataTable());
            cboCidade.DataSource = datTabela;
            cboCidade.DisplayMember = "nome";
            cboCidade.ValueMember = "id";
        }

        void CarregarComboCurso()
        {
            Adaptador = new MySqlDataAdapter("SELECT * FROM Cursos order by nome", Conexao);
            Adaptador.Fill(datTabela = new DataTable());
            cboCurso.DataSource = datTabela;
            cboCurso.DisplayMember = "nome";
            cboCurso.ValueMember = "id";
        }

        public FrmAlunos()
        {
            InitializeComponent();
        }

        private void FrmAlunos_Load(object sender, EventArgs e)
        {
            CarregaGrid();
            dgvAlunos.Columns[2].Visible = false;

            dgvAlunos.Columns[7].Visible = false;
            dgvAlunos.Columns[8].Visible = false;
            CarregarComboCidade();
            CarregarComboCurso();
            LimparCampos();
        }

        private void dgvAlunos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtID.Text = dgvAlunos.CurrentRow.Cells["id"].Value.ToString();
            txtNome.Text = dgvAlunos.CurrentRow.Cells["nome"].Value.ToString();
            chkDesistente.Checked = Convert.ToBoolean(dgvAlunos.CurrentRow.Cells["desistente"].Value.ToString());
            cboCurso.SelectedValue = dgvAlunos.CurrentRow.Cells["id_curso"].Value.ToString();
            txtAno.Text = dgvAlunos.CurrentRow.Cells["ano"].Value.ToString();
            mtbDataNasc.Text = dgvAlunos.CurrentRow.Cells["data_Nasc"].Value.ToString();
            cboCidade.SelectedValue = dgvAlunos.CurrentRow.Cells["id_cidade"].Value.ToString();
            txtUF.Text = dgvAlunos.CurrentRow.Cells["uf"].Value.ToString();
            txtRenda.Text = dgvAlunos.CurrentRow.Cells["renda"].Value.ToString();
            picFoto.ImageLocation = dgvAlunos.CurrentRow.Cells["foto"].Value.ToString();
        }

        private void cboCidade_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboCidade.SelectedIndex != -1)
            {
                DataRowView reg = (DataRowView)cboCidade.SelectedItem;
                txtUF.Text = reg["UF"].ToString();
            }
        }

        private void picFoto_Click(object sender, EventArgs e)
        {
            ofdArquivo.InitialDirectory = "D:\\fotos\\";
            ofdArquivo.FileName = "";
            ofdArquivo.ShowDialog();
            picFoto.ImageLocation = ofdArquivo.FileName;

        }

        private void btnIncluir_Click(object sender, EventArgs e)
        {
            if (txtNome.Text == "")
            {
                MessageBox.Show("Favor informar o nome do Aluno", "Inclusão");
                return;
            }
            Conexao.Open();

            Comando = new MySqlCommand("INSERT INTO ALUNOS (nome, id_curso, ano, data_Nasc, desistente, renda, foto, id_cidade) VALUES (@nome, @id_curso, @ano, @Data_Nasc, @desistente, @renda, @foto, @id_cidade)", Conexao);
            Comando.Parameters.AddWithValue("@nome", txtNome.Text);
            Comando.Parameters.AddWithValue("@id_curso", cboCurso.SelectedValue);
            Comando.Parameters.AddWithValue("@ano", Convert.ToInt32 (txtAno.Text));
            Comando.Parameters.AddWithValue("@desistente", Convert.ToBoolean(chkDesistente.Checked));
            Comando.Parameters.AddWithValue("@data_Nasc", Convert.ToDateTime(mtbDataNasc.Text));
            Comando.Parameters.AddWithValue("@renda", Convert.ToDouble(txtRenda.Text));
            Comando.Parameters.AddWithValue("@foto", picFoto.ImageLocation);
            Comando.Parameters.AddWithValue("@id_cidade", cboCidade.SelectedValue);
            Comando.ExecuteNonQuery();
            Conexao.Close();
            CarregaGrid();
            LimparCampos();
        }

        private void btnAtualizar_Click(object sender, EventArgs e)
        {
            if (txtID.Text == "")
            {
                MessageBox.Show("Favor Selecionar um  Aluno", "Atualização");
                return;
            }


            Conexao.Open();
            Comando = new MySqlCommand ("UPDATE alunos set nome = @nome, id_curso = @id_curso," +
                " ano = @ano, desistente = @desistente, data_nasc = @data_nasc, id_cidade = @id_cidade," +
                " renda = @renda, foto = @foto where id = @id", Conexao);
            Comando.Parameters.AddWithValue("@id", Convert.ToInt16(txtID.Text));
            Comando.Parameters.AddWithValue("@nome", txtNome.Text);
            Comando.Parameters.AddWithValue("@id_curso", cboCurso.SelectedValue);
            Comando.Parameters.AddWithValue("@ano", Convert.ToInt32(txtAno.Text));
            Comando.Parameters.AddWithValue("@desistente", Convert.ToBoolean(chkDesistente.Checked));
            Comando.Parameters.AddWithValue("@data_Nasc", Convert.ToDateTime(mtbDataNasc.Text));
            Comando.Parameters.AddWithValue("@renda", Convert.ToDouble(txtRenda.Text));
            Comando.Parameters.AddWithValue("@foto", picFoto.ImageLocation);
            Comando.Parameters.AddWithValue("@id_cidade", cboCidade.SelectedValue);
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
                MessageBox.Show("Favor Selecionar um aluno", "Exclusão");
                return;
            }

            if (MessageBox.Show("Deseja realmente Excluir o Aluno? " + txtNome.Text, "Exclusão",
                                 MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Conexao.Open();
                Comando = new MySqlCommand("DELETE FROM Alunos WHERE id = @id", Conexao);
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
