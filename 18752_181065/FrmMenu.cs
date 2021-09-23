using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace _18752_181065
{
    public partial class FrmMenu : Form
    {
        MySqlConnection Conexao = new MySqlConnection("server=localhost;uid=root;pwd=etecjau");
        MySqlCommand Comando;

        public FrmMenu()
        {
            InitializeComponent();
        }

        private void FrmMenu_Load(object sender, EventArgs e)
        {
            Conectar();
        }

        void Conectar()
        {
            Conexao.Open();
            Comando = new MySqlCommand("CREATE DATABASE IF NOT EXISTS bd_escola;USE bd_escola", Conexao);
            Comando.ExecuteNonQuery();
            Comando = new MySqlCommand("CREATE TABLE IF NOT EXISTS Cidades " +
                                      "(id integer auto_increment primary key, " +
                                      "nome char (40), " +
                                      "uf char(02))", Conexao);
            Comando.ExecuteNonQuery();
            Comando = new MySqlCommand("CREATE TABLE IF NOT EXISTS Alunos" + 
                                       "(id integer auto_increment primary key, " + 
                                       "nome char(40), " + 
                                       "id_curso integer(30), " + 
                                       "ano integer, " + 
                                       "data_nasc date, " + 
                                       "desistente boolean, " +
                                       "renda decimal(10,2), " +
                                       "foto char(50), " + 
                                       "id_cidade integer)", Conexao);
            Comando.ExecuteNonQuery();
            Comando = new MySqlCommand("CREATE TABLE IF NOT EXISTS Cursos" +
                                       "(id integer auto_increment primary key, " +
                                       "nome char(30), " +
                                       "duracao integer)", Conexao);
            Comando.ExecuteNonQuery();
            Comando = new MySqlCommand("CREATE TABLE IF NOT EXISTS Disciplinas" +
                                       "(id integer auto_increment primary key," +
                                       "nome char(30)," +
                                       "area integer)", Conexao);
            Comando.ExecuteNonQuery();
            Comando = new MySqlCommand("CREATE TABLE IF NOT EXISTS Professores" +
                                      "(id integer auto_increment primary key," +
                                      "nome char(40), " +
                                      "endereco char(40), " +
                                      "bairro char(30)," +
                                      "id_cidade integer, " +
                                      "fone char(14)," +
                                      "celular char(15), " +
                                      "formacao integer, " +
                                      "grau char(1), " +
                                      "data_nasc date," +
                                      "afastado boolean)", Conexao);
            Comando.ExecuteNonQuery();
            Comando = new MySqlCommand("CREATE TABLE IF NOT EXISTS Grade_Curricular" +
                                        "(id integer auto_increment primary key," +
                                        "id_curso integer," +
                                        "id_disciplina integer," +
                                        "carga_horaria double(3,1)," +
                                        "inativo boolean)", Conexao);
            Comando.ExecuteNonQuery();
            Comando = new MySqlCommand("CREATE TABLE IF NOT EXISTS Boletins" +
                                      "(id integer auto_increment primary key," +                        
                                      "id_aluno integer," +
                                      "id_grade integer," +
                                      "id_professor integer," +
                                      "ano integer," +
                                      "bimestre tinyint,"+
                                      "media double(4,2))", Conexao);
            Comando.ExecuteNonQuery();
            Conexao.Close();
        }

        //ToolStripItems
        private void FinalizarOSistemaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void CidadesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmCidades Form = new FrmCidades();
            Form.Show();
        }

        private void CursosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmCursos Form = new FrmCursos();
            Form.Show();
        }

        private void DisciplinasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmDisciplinas Form = new FrmDisciplinas();
            Form.Show();
        }

        private void AlunosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmAlunos Form = new FrmAlunos();
            Form.Show();
        }

        private void PorCidadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmConsultaCidade Form = new FrmConsultaCidade();
            Form.Show();
        }

        private void PorCursoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmConsultaCurso Form = new FrmConsultaCurso();
            Form.Show();
        }

        private void professoresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmProfessores Form = new FrmProfessores();
            Form.Show();
        }

        private void gradeCurricularToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmGradeCurricular Form = new FrmGradeCurricular();
            Form.Show();
        }

        private void boletimToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmBoletim Form = new FrmBoletim();
            Form.Show();
        }
    }
}
        