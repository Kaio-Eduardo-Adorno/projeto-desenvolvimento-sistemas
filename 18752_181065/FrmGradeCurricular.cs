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
    public partial class FrmGradeCurricular : Form
    {
        MySqlConnection Conexao = new MySqlConnection("server=localhost;uid=root;pwd=etecjau");
        MySqlCommand Comando;
        MySqlDataAdapter Adaptador;

        DataTable datTabela;

        public FrmGradeCurricular()
        {
            InitializeComponent();
        }

        void CarregaGrid()
        {
            Adaptador = new MySqlDataAdapter("SELECT * FROM Grade_Curricular g LEFT JOIN cursos c on (g.id_curso = c.id) LEFT JOIN disciplinas d on (g.id_disciplina = d.id) WHERE c.nome LIKE @Nome ORDER BY c.nome;  ", Conexao);
            Adaptador.SelectCommand.Parameters.AddWithValue("@Nome", txtPesquisa.Text + "%");
            Adaptador.Fill(datTabela = new DataTable());
            dgvGradeCurricular.DataSource = datTabela;
        }

        void CarregarComboDisciplinas()
        {
            Adaptador = new MySqlDataAdapter("SELECT * FROM Disciplinas order by nome", Conexao);
            Adaptador.Fill(datTabela = new DataTable());
            cboDisciplina.DataSource = datTabela;
            cboDisciplina.DisplayMember = "nome";
            cboDisciplina.ValueMember = "id";
        }

        void CarregarComboCurso()
        {
            Adaptador = new MySqlDataAdapter("SELECT * FROM Cursos order by nome", Conexao);
            Adaptador.Fill(datTabela = new DataTable());
            cboCurso.DataSource = datTabela;
            cboCurso.DisplayMember = "nome";
            cboCurso.ValueMember = "id";
        }

        void carregaIdCurso()
        {

            if (cboCurso.SelectedIndex != -1)
            {
                DataRowView reg = (DataRowView)cboCurso.SelectedItem;

                txtID.Text = reg["id"].ToString();
            }
        }

        void LimparCampos()
        {
            txtID.Clear();
            txtCargaHoraria.Clear();
            chkInativo.Checked = false;
            cboCurso.SelectedIndex = -1;
            cboDisciplina.SelectedIndex = -1;
            txtPesquisa.Clear();
        }

        private void btnIncluir_Click(object sender, EventArgs e)
        {
            {
                if (cboCurso.SelectedIndex == -1)
                {
                    MessageBox.Show("Porfavor Selecione o curso", "Inclusão", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                Conexao.Open();
                Comando = new MySqlCommand("SELECT 1 FROM grade_curricular where id_disciplina = @id_disciplina and id_curso = @id_curso", Conexao);
                Comando.Parameters.AddWithValue("@id_curso", cboCurso.SelectedValue);
                Comando.Parameters.AddWithValue("@id_disciplina", cboDisciplina.SelectedValue);
                var resultado = Comando.ExecuteScalar();
                if (resultado != null)
                {
                    MessageBox.Show("Ja foi cadastrado este curso com essa disciplina, verifique a sua tabela porfavor!");
                }
                else
                {

                    Comando = new MySqlCommand("INSERT INTO grade_curricular (id_curso,id_disciplina,carga_horaria,inativo) VALUES (@id_curso,@id_disciplina,@carga_horaria,@inativo)", Conexao);
                    Comando.Parameters.AddWithValue("@id_curso", cboCurso.SelectedValue);
                    Comando.Parameters.AddWithValue("@id_disciplina", cboDisciplina.SelectedValue);
                    Comando.Parameters.AddWithValue("@carga_horaria", Convert.ToDouble(txtCargaHoraria.Text));
                    Comando.Parameters.AddWithValue("@inativo", Convert.ToBoolean(chkInativo.Checked));
                    Comando.ExecuteNonQuery();
                }
                Conexao.Close();
                CarregaGrid();
                LimparCampos();
            }
        }

        private void dgvGradeCurricular_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvGradeCurricular.RowCount > -1)
            {
                cboCurso.SelectedValue = dgvGradeCurricular.CurrentRow.Cells["id_curso"].Value.ToString();
                cboDisciplina.SelectedValue = Convert.ToInt16(dgvGradeCurricular.CurrentRow.Cells["id_disciplina"].Value);
                txtCargaHoraria.Text = dgvGradeCurricular.CurrentRow.Cells["carga_horaria"].Value.ToString();
                txtID.Text = dgvGradeCurricular.CurrentRow.Cells[0].Value.ToString();
                chkInativo.Checked = Convert.ToBoolean(dgvGradeCurricular.CurrentRow.Cells["inativo"].Value);

            }
        }

        private void btnAtualizar_Click(object sender, EventArgs e)
        {
            if (txtID.Text == "")
            {
                MessageBox.Show("Favor Selecionar a Disciplina", "Atualização");
                return;
            }
            Conexao.Open();

            Comando = new MySqlCommand("INSERT INTO Grade_Curricular (curso, id_curso, disciplina, carga_horaria) VALUES (@curso, @id_curso, @disciplina, @carga_horaria)", Conexao);
            Comando.Parameters.AddWithValue("@inativo", Convert.ToBoolean(chkInativo.Checked));
            Comando.Parameters.AddWithValue("@id_curso", cboCurso.SelectedValue);
            Comando.Parameters.AddWithValue("@disciplina", cboDisciplina.SelectedValue);
            Comando.Parameters.AddWithValue("@carga_horaria", Convert.ToDouble(txtCargaHoraria.Text));
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
                MessageBox.Show("Favor Selecionar uma Disciplina", "Exclusão");
                return;
            }

            if (MessageBox.Show("Deseja realmente Excluir a Disciplina? " + cboDisciplina.Text, "Exclusão",
                                 MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Conexao.Open();
                Comando = new MySqlCommand("DELETE FROM Grade_Curricular WHERE id = @id", Conexao);
                Comando.Parameters.AddWithValue("@id", txtID.Text);
                Comando.ExecuteNonQuery();
                Conexao.Close();
                LimparCampos();
                CarregaGrid();
            }
        }
        private void btnFechar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnPesquisar_Click(object sender, EventArgs e)
        {
            CarregaGrid();
            txtPesquisa.Clear();
        }

        private void FrmGradeCurricular_Load(object sender, EventArgs e)
        {
            CarregarComboCurso();
            CarregarComboDisciplinas();
            CarregaGrid();
            carregaIdCurso();
            LimparCampos();
            

            dgvGradeCurricular.Columns[0].Visible = false;
            dgvGradeCurricular.Columns[1].Visible = false;
            dgvGradeCurricular.Columns[2].Visible = false;
            dgvGradeCurricular.Columns[5].Visible = false;
            dgvGradeCurricular.Columns[8].Visible = false;
            dgvGradeCurricular.Columns[9].HeaderText = "Disciplina";
            dgvGradeCurricular.Columns[6].HeaderText = "Curso";
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            printPreviewDialog1.WindowState = FormWindowState.Maximized;
            printPreviewDialog1.ShowDialog();
        }



        private void printDocument1_PrintPage_1(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            double carga = 0;
            Conexao.Open();
            Adaptador = new MySqlDataAdapter("SELECT * FROM Grade_Curricular g left join cursos c on (g.id_curso = c.id)" +
                " left join disciplinas d on (g.id_disciplina = d.id) WHERE g.id_curso = @curso AND g.inativo = 0", Conexao);
            Adaptador.SelectCommand.Parameters.AddWithValue("@curso", cboCurso.SelectedValue);
            Adaptador.Fill(datTabela = new DataTable());
            dgvGradeCurricular.DataSource = datTabela;


            int posicao, itens = 0;

            e.Graphics.DrawString("Relatorio de Carga Horaria", new Font("Arial", 30, FontStyle.Bold), Brushes.Black, 150, 10);
            e.Graphics.DrawString(cboCurso.Text, new Font("Arial", 15, FontStyle.Bold), Brushes.Blue, 150, 60);

            e.Graphics.DrawLine(Pens.Black, 50, 90, 780, 90);
            e.Graphics.DrawString("Curso", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 50, 95);
            e.Graphics.DrawString("Disciplina", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 380, 95);
            e.Graphics.DrawString("Carga Horaria", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 630, 95);
            e.Graphics.DrawLine(Pens.Black, 50, 120, 780, 120);

            posicao = 100;

            foreach (DataGridViewRow linha in dgvGradeCurricular.Rows)
            {
                if (itens > 40)
                {
                    e.HasMorePages = true;
                    return;
                }
                posicao += 30;

                e.Graphics.DrawString(linha.Cells["nome"].Value.ToString(), new Font("Arial", 10), Brushes.Black, 50, posicao);
                e.Graphics.DrawString(linha.Cells["nome1"].Value.ToString(), new Font("Arial", 10), Brushes.Black, 380, posicao);
                e.Graphics.DrawString(linha.Cells["carga_horaria"].Value.ToString(), new Font("Arial", 10), Brushes.Black, 630, posicao);
                
                itens++;
                
                carga += Convert.ToDouble(linha.Cells["carga_horaria"].Value.ToString());
            }

            e.Graphics.DrawString("Total de Carga Horaria: " + carga, new Font("Arial", 15), Brushes.Blue, 380, posicao + 30);


            CarregaGrid();
            LimparCampos();
            Conexao.Close();
        }


    }
}
