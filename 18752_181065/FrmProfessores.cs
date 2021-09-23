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
    public partial class FrmProfessores : Form
    {
        MySqlConnection Conexao = new MySqlConnection("server=localhost;uid=root;pwd=etecjau");
        MySqlCommand Comando;
        MySqlDataAdapter Adaptador;

        DataTable datTabela;
        public FrmProfessores()
        {
            InitializeComponent();
        }
        void LimparCampos()
        {
            txtID.Clear();
            txtNome.Clear();
            chkAfastado.Checked = false;
            txtGrau.Clear();
            mtbDataNasc.Clear();
            mtbFone.Clear();
            mtbCelular.Clear();
            txtUF.Clear();
            cboCidade.SelectedIndex = -1;
            cboFormacao.SelectedIndex = -1;
            txtUF.Clear();
            txtEndereco.Clear();
            txtPesquisa.Clear();
            txtBairro.Clear();
          
        }
        void CarregaGrid()
        {
            //Adapter recebendo consulta do banco de dados
            Adaptador = new MySqlDataAdapter("select p.*, ci.nome cidade, ci.UF FROM Professores p left join " +
                                             "cidades ci on (p.id_cidade = ci.id)" +

                                             "where p.nome like @Nome order by p.nome", Conexao);
            // Criacao dos parametros utilizados na instrucao
            Adaptador.SelectCommand.Parameters.AddWithValue("@Nome", txtPesquisa.Text + "%");
            //DataTabek recebendo dados do Adaptador para ligar em um componente DatSource
            Adaptador.Fill(datTabela = new DataTable());
            //Controle do form recebendo so dados da dataTable atraves da propriedade DataSource
            dgvProfessores.DataSource = datTabela;
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
            cboFormacao.DataSource = datTabela;
            cboFormacao.DisplayMember = "nome";
            cboFormacao.ValueMember = "id";
        }



        private void FrmProfessores_Load(object sender, EventArgs e)
        {
            CarregaGrid();
            dgvProfessores.Columns[0].Visible = false;
            dgvProfessores.Columns[4].Visible = false;
            CarregarComboCidade();
            LimparCampos();
        }

        private void dgvProfessores_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvProfessores.RowCount > 0)
            {
                txtID.Text = dgvProfessores.CurrentRow.Cells["id"].Value.ToString();
                mtbFone.Text = dgvProfessores.CurrentRow.Cells["fone"].Value.ToString();
                mtbCelular.Text = dgvProfessores.CurrentRow.Cells["celular"].Value.ToString();
                txtNome.Text = dgvProfessores.CurrentRow.Cells["nome"].Value.ToString();
                txtEndereco.Text = dgvProfessores.CurrentRow.Cells["endereco"].Value.ToString();
                mtbDataNasc.Text = dgvProfessores.CurrentRow.Cells["data_Nasc"].Value.ToString();
                chkAfastado.Checked = Convert.ToBoolean(dgvProfessores.CurrentRow.Cells["afastado"].Value.ToString());
                txtBairro.Text = dgvProfessores.CurrentRow.Cells["bairro"].Value.ToString();
                cboCidade.Text = dgvProfessores.CurrentRow.Cells["cidade"].Value.ToString();
                cboFormacao.SelectedIndex = Convert.ToInt16(dgvProfessores.CurrentRow.Cells["formacao"].Value) - 1;
                txtUF.Text = dgvProfessores.CurrentRow.Cells["UF"].Value.ToString();
                txtGrau.Text = dgvProfessores.CurrentRow.Cells["grau"].Value.ToString();
            }
        }
        private void cboCidade_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboCidade.SelectedIndex != -1)
            {
                DataRowView reg = (DataRowView)cboCidade.SelectedItem;
                txtUF.Text = reg["UF"].ToString();
            }
        }
        private void btnIncluir_Click(object sender, EventArgs e)
        {
            if (txtID.Text == "")
            {
                MessageBox.Show("Porfavor selecione um Professor", "Exclusão", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (MessageBox.Show("Tem certeza que deseja excluir Professor(a) " + txtNome.Text + " ?", "Exclusão", MessageBoxButtons.YesNo, MessageBoxIcon.Error, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {


                Conexao.Open();
                Comando = new MySqlCommand("DELETE FROM Professores where id = @id", Conexao);
                Comando.Parameters.AddWithValue("@id", txtID.Text);
                Comando.ExecuteNonQuery();
                CarregaGrid();
                LimparCampos();
                Conexao.Close();
            }
        }
        private void btnAtualizar_Click(object sender, EventArgs e)
        {
            if (txtID.Text == "")
            {
                MessageBox.Show("Favor Selecionar um  Professor", "Atualização");
                return;
            }


            Conexao.Open();
            Comando = new MySqlCommand("UPDATE professores set nome = nome = @nome,formacao = @formacao, grau = @grau, id_curso = @id_curso,afastado = @afastado,data_nasc = @data_Nasc,fone = @fone, celular = @celular, endereco = @endereco, bairro = @bairro, uf = @uf,formacao = @formacao,id_cidade = @id_cidade, id = @id", Conexao);
            Comando.Parameters.AddWithValue("@nome", txtNome.Text);
            Comando.Parameters.AddWithValue("@grau", Convert.ToDouble(txtGrau.Text));
            Comando.Parameters.AddWithValue("@id_curso", cboFormacao.SelectedValue);
            Comando.Parameters.AddWithValue("@afastado", Convert.ToBoolean(chkAfastado.Checked));
            Comando.Parameters.AddWithValue("@data_Nasc", Convert.ToDateTime(mtbDataNasc.Text));
            Comando.Parameters.AddWithValue("@fone", Convert.ToDateTime(mtbFone.Text));
            Comando.Parameters.AddWithValue("@celular", Convert.ToDateTime(mtbCelular.Text));
            Comando.Parameters.AddWithValue("@endereco", Convert.ToDouble(txtEndereco.Text));
            Comando.Parameters.AddWithValue("@bairro", Convert.ToDouble(txtBairro.Text));
            Comando.Parameters.AddWithValue("@uf", Convert.ToDouble(txtUF.Text));
            Comando.Parameters.AddWithValue("@id_cidade", cboCidade.SelectedValue);
            Comando.Parameters.AddWithValue("@formacao", cboFormacao.SelectedValue);
            Comando.ExecuteNonQuery();
            Conexao.Close();
            CarregaGrid();
            LimparCampos();
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
                MessageBox.Show("Favor Selecionar um Professor", "Exclusão");
                return;
            }

            if (MessageBox.Show("Deseja realmente Excluir o Professor? " + txtNome.Text, "Exclusão",
                                 MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Conexao.Open();
                Comando = new MySqlCommand("DELETE FROM Professores WHERE id = @id", Conexao);
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

        private void btnIncluir_Click_1(object sender, EventArgs e)
        {
            if (txtNome.Text == "")
            {
                MessageBox.Show("Porfavor insira o nome do Professor", "Inclusão", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            Conexao.Open();
            Comando = new MySqlCommand("INSERT INTO Professores (nome, fone, celular, grau, formacao, id_cidade, bairro, afastado, Data_Nasc, endereco) " +
                                       "VALUES (@nome, @fone, @celular,@grau, @formacao, @id_cidade, @bairro, @afastado, @Data_nasc, @endereco)", Conexao);
            Comando.Parameters.AddWithValue("@nome", txtNome.Text);
            Comando.Parameters.AddWithValue("@fone", mtbFone.Text);
            Comando.Parameters.AddWithValue("@celular", mtbCelular.Text);
            Comando.Parameters.AddWithValue("@grau", txtGrau.Text);
            Comando.Parameters.AddWithValue("@formacao", cboFormacao.SelectedIndex + 1);
            Comando.Parameters.AddWithValue("@id_cidade", cboCidade.SelectedValue);
            Comando.Parameters.AddWithValue("@bairro", txtBairro.Text);
            Comando.Parameters.AddWithValue("@afastado", Convert.ToBoolean(chkAfastado.Checked));
            Comando.Parameters.AddWithValue("@Data_Nasc", Convert.ToDateTime(mtbDataNasc.Text));
            Comando.Parameters.AddWithValue("@endereco", txtEndereco.Text);
            Comando.ExecuteNonQuery();
            Conexao.Close();
            LimparCampos();
            CarregaGrid();
        }

        private void btnAtualizar_Click_1(object sender, EventArgs e)
        {
            if (txtID.Text == "")
            {
                MessageBox.Show("Favor Selecionar um Professor", "Atualização");
                return;
            }
            Conexao.Open();
            Comando = new MySqlCommand("UPDATE professores set nome = nome = @nome, formacao = @formacao, grau = @grau, id_curso = @id_curso,afastado = @afastado,data_nasc = @data_Nasc,fone = @fone, celular = @celular, endereco = @endereco, bairro = @bairro, uf = @uf,formacao = @formacao,id_cidade = @id_cidade, id = @id", Conexao);
            Comando.Parameters.AddWithValue("@nome", txtNome.Text);
            Comando.Parameters.AddWithValue("@grau", txtGrau.Text);
            Comando.Parameters.AddWithValue("@id_curso", cboFormacao.SelectedValue);
            Comando.Parameters.AddWithValue("@afastado", Convert.ToBoolean(chkAfastado.Checked));
            Comando.Parameters.AddWithValue("@data_Nasc", Convert.ToDateTime(mtbDataNasc.Text));
            Comando.Parameters.AddWithValue("@fone", Convert.ToDateTime(mtbFone.Text));
            Comando.Parameters.AddWithValue("@celular", Convert.ToDateTime(mtbCelular.Text));
            Comando.Parameters.AddWithValue("@endereco", Convert.ToDouble(txtEndereco.Text));
            Comando.Parameters.AddWithValue("@bairro", Convert.ToDouble(txtBairro.Text));
            Comando.Parameters.AddWithValue("@uf", Convert.ToDouble(txtUF.Text));
            Comando.Parameters.AddWithValue("@id_cidade", cboCidade.SelectedValue);
            Comando.Parameters.AddWithValue("@formacao", cboFormacao.SelectedValue);
            Comando.ExecuteNonQuery();
            Conexao.Close();
            CarregaGrid();
            LimparCampos();
        }

        private void btnCancelar_Click_1(object sender, EventArgs e)
        {
            LimparCampos();
            CarregaGrid();
        }

        private void btnExcluir_Click_1(object sender, EventArgs e)
        {
            if (txtID.Text == "")
            {
                MessageBox.Show("Favor Selecionar um professor", "Exclusão");
                return;
            }

            if (MessageBox.Show("Deseja realmente Excluir o Professor? " + txtNome.Text, "Exclusão",
                                 MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Conexao.Open();
                Comando = new MySqlCommand("DELETE FROM Professores WHERE id = @id", Conexao);
                Comando.Parameters.AddWithValue("@id", txtID.Text);
                Comando.ExecuteNonQuery();
                Conexao.Close();
                LimparCampos();
                CarregaGrid();
            }
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {

        }

        private void btnFechar_Click_1(object sender, EventArgs e)
        {
            Close();
        }

        private void cboCidade_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (cboCidade.SelectedIndex != -1)
            {
                DataRowView reg = (DataRowView)cboCidade.SelectedItem;
                txtUF.Text = reg["UF"].ToString();
            }
        }

        private void btnPesquisar_Click_1(object sender, EventArgs e)
        {
            CarregaGrid();
            txtPesquisa.Clear();
        }
    }
}

