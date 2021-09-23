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
    public partial class FrmBoletim : Form
    {

        MySqlConnection Conexao = new MySqlConnection("server=localhost;database=bd_escola;uid=root;pwd=etecjau");
        MySqlConnection ConexaoL = new MySqlConnection("server=localhost;database=bd_escola;uid=root;pwd=etecjau");
        MySqlConnection ConexaoD = new MySqlConnection("server=localhost;database=bd_escola;uid=root;pwd=etecjau");
        MySqlCommand Comando;
        MySqlDataAdapter Adaptador;
        MySqlDataReader Leitor;

        DataTable datTabela;

        //VOIDS 

        void LimparCampos()
        {
            txtID_Professor.Clear();
            txtNome_Professor.Clear();
            txtID_Aluno.Clear();
            txtNome_Aluno.Clear();
            cboCidade.SelectedIndex = -1;
            txtUF.Clear();
            cboCurso.SelectedIndex = -1;
            mtbDataNasc.Clear();
            cboDisciplina.SelectedIndex = -1;
            txtID_AlunoN.Clear();
            txtBimestre.Clear();
            txtMedia.Clear();
        }
        
        void CarregaGrid()
        {
            Adaptador = new MySqlDataAdapter("SELECT b.*, a.nome, ci.nome, ci.uf, cu.nome, d.nome, p.nome FROM " +
                "boletins b INNER JOIN alunos a ON (b.id_aluno = a.id) INNER JOIN professores p ON (b.id_professor = p.id) " +
                "INNER JOIN grade_curricular g ON (b.id_grade = g.id) INNER JOIN cidades ci ON (a.id_cidade = ci.id) " + 
                "INNER JOIN cursos cu ON (g.id_curso = cu.id) INNER JOIN disciplinas d ON (g.id_disciplina = d.id) " +
                "WHERE a.nome LIKE @Nome ORDER BY a.nome", Conexao);
            Adaptador.SelectCommand.Parameters.AddWithValue("@Nome", txtPesquisa.Text + "%");
            Adaptador.Fill(datTabela = new DataTable());
            dgvBoletim.DataSource = datTabela;
        }

        void CarregarComboCurso()
        {
            Adaptador = new MySqlDataAdapter("SELECT * FROM Cursos order by nome", Conexao);
            Adaptador.Fill(datTabela = new DataTable());
            cboCurso.DataSource = datTabela;
            cboCurso.DisplayMember = "nome";
            cboCurso.ValueMember = "id";
        }

        void CarregarComboCidade()
        {
            Adaptador = new MySqlDataAdapter("SELECT * FROM Cidades order by nome", Conexao);
            Adaptador.Fill(datTabela = new DataTable());
            cboCidade.DataSource = datTabela;
            cboCidade.DisplayMember = "nome";
            cboCidade.ValueMember = "id";
        }

        //VOIDS

        public FrmBoletim()
        {
            InitializeComponent();
        }

        private void FrmBoletim_Load(object sender, EventArgs e)
        {
            txtAno.Text = DateTime.Now.Year.ToString();
            CarregarComboCidade();
            CarregarComboCurso();
            CarregaGrid();
            LimparCampos();
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
            if (txtID_Aluno.Text == "")
            {
                MessageBox.Show("Favor informar o ID do Aluno", "Inclusão");
                return;
            }
            Conexao.Open();

            Comando = new MySqlCommand("INSERT INTO BOLETINS (id_aluno, id_grade, id_professor, ano, bimestre, media) VALUES (@id_aluno, @id_grade, @id_professor, @ano, @bimestre, @media)", Conexao);
            Comando.Parameters.AddWithValue("@id_aluno", txtID_Aluno.Text);
            Comando.Parameters.AddWithValue("@id_grade", cboGrade.SelectedValue);
            Comando.Parameters.AddWithValue("@id_professor", txtID_Professor.Text);
            Comando.Parameters.AddWithValue("@ano", Convert.ToInt32(txtAno.Text));
            Comando.Parameters.AddWithValue("@bimestre", Convert.ToInt32(txtBimestre.Text));
            Comando.Parameters.AddWithValue("@media", Convert.ToInt32(txtMedia.Text));
            Comando.ExecuteNonQuery();
            Conexao.Close();
            CarregaGrid();
            LimparCampos();
        }

        private void cboCurso_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cboCurso.SelectedIndex > -1)
            {
                Adaptador = new MySqlDataAdapter("SELECT d.nome, d.id FROM Grade_Curricular g INNER JOIN disciplinas d ON g.id_disciplina = d.id WHERE g.id_curso='" + (cboCurso.SelectedValue) + "'", Conexao);
                Adaptador.Fill(datTabela = new DataTable());
                cboDisciplina.DataSource = datTabela;
                cboDisciplina.DisplayMember = "nome";
                cboDisciplina.ValueMember = "id";
                cboDisciplina.SelectedIndex = -1;

                Adaptador = new MySqlDataAdapter("SELECT g.id FROM Grade_Curricular g WHERE g.id_curso = '" + (cboCurso.SelectedValue) + "'", Conexao);
                Adaptador.Fill(datTabela = new DataTable());
                cboGrade.DataSource = datTabela;
                cboGrade.DisplayMember = "id";
                cboGrade.ValueMember = "id";
                cboGrade.SelectedIndex = -1;
            }
        }

        private void txtID_Aluno_Leave(object sender, EventArgs e)
        {
            if (txtID_Aluno.Text == "")
            {
                
            }
            else
            {
                ConexaoL.Open();
                Comando = new MySqlCommand("SELECT a.id,a.nome,a.id_cidade,a.id_curso,a.data_nasc FROM Alunos a WHERE a.id LIKE '" + (txtID_Aluno.Text) + "'", ConexaoL);
                Leitor = Comando.ExecuteReader();
                Leitor.Read();
                mtbDataNasc.Text = Leitor.GetString("data_nasc");
                txtID_AlunoN.Text = txtID_Aluno.Text;
                txtNome_Aluno.Text = Leitor.GetString("nome");
                cboCidade.SelectedValue = Leitor.GetInt32("id_cidade");
                cboCurso.SelectedValue = Leitor.GetInt32("id_curso");
                
                ConexaoL.Close();
            }
            
        }

        private void txtID_Professor_Leave(object sender, EventArgs e)
        {
            if (txtID_Professor.Text == "")
            {

            }
            else
            {
                ConexaoL.Open();
                Comando = new MySqlCommand("SELECT p.id,p.nome FROM Professores p WHERE p.id LIKE '" + (txtID_Professor.Text) + "'", ConexaoL);
                Leitor = Comando.ExecuteReader();
                Leitor.Read();
                txtNome_Professor.Text = Leitor.GetString("nome");
                ConexaoL.Close();
            }
        }

        private void cboDisciplina_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboDisciplina.SelectedIndex > -1)
            {
                ConexaoD.Open();
                Comando = new MySqlCommand("SELECT g.id FROM grade_curricular g WHERE g.id_disciplina = '" + (cboDisciplina.SelectedValue) + "' AND g.id_curso = '" + (cboCurso.SelectedValue) + "'", ConexaoD);
                Leitor = Comando.ExecuteReader();
                Leitor.Read();
                cboGrade.SelectedValue = Leitor.GetInt32("id");
                ConexaoD.Close();
            }
        }
    }
}
