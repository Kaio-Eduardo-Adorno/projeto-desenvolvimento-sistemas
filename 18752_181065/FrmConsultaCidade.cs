﻿using System;
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
    public partial class FrmConsultaCidade : Form
    {
        MySqlConnection Conexao = new MySqlConnection("server=localhost;uid=root;pwd=etecjau");
        MySqlDataAdapter Adaptador;
        DataTable datTabela;

        void CarregaGrid()
        {
            Adaptador = new MySqlDataAdapter("select a.*, ci.nome cidade, ci.UF, c.nome curso FROM alunos a left join  " +
                "cidades ci on (a.id_cidade = ci.id) left join cursos c on (c.id = a.id_curso)" +
                "where a.id_cidade like @id_cidade order by a.id_cidade", Conexao);
            Adaptador.SelectCommand.Parameters.AddWithValue("@id_cidade", cboPesquisa.SelectedValue);
            Adaptador.Fill(datTabela = new DataTable());
            dgvConsulta_cidades.DataSource = datTabela;
        }

        void CarregarComboCidade()
        {
            Adaptador = new MySqlDataAdapter("SELECT * FROM Cidades order by nome", Conexao);
            Adaptador.Fill(datTabela = new DataTable());
            cboPesquisa.DataSource = datTabela;
            cboPesquisa.DisplayMember = "nome";
            cboPesquisa.ValueMember = "id";
        }

        public FrmConsultaCidade()
        {
            InitializeComponent();
        }

        private void FrmConsultaCidade_Load(object sender, EventArgs e)
        {
            CarregaGrid();
            CarregarComboCidade();
            cboPesquisa.SelectedIndex = -1;
        }

        private void btnPesquisar_Click(object sender, EventArgs e)
        {
            CarregaGrid();
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            printPreviewDialog1.WindowState = FormWindowState.Maximized;
            printPreviewDialog1.ShowDialog();
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            int posicao, itens = 0;
            e.Graphics.DrawString("Relatório de Alunos", new Font("Arial", 30, FontStyle.Bold), Brushes.Black, 230, 10);
            e.Graphics.DrawLine(Pens.Black, 50, 90, 780, 90);
            e.Graphics.DrawString("Código", new Font("Arial", 10), Brushes.Black, 50, 95);
            e.Graphics.DrawString("Nome", new Font("Arial", 10), Brushes.Black, 130, 95);
            e.Graphics.DrawString("Cidade", new Font("Arial", 10), Brushes.Black, 450, 95);
            e.Graphics.DrawLine(Pens.Black, 50, 120, 780, 120);

            posicao = 100;

            foreach (DataGridViewRow linha in dgvConsulta_cidades.Rows)
            {
                if (itens > 40)
                {
                    e.HasMorePages = true;
                    return;
                }
                posicao += 30;
                e.Graphics.DrawString(linha.Cells[0].Value.ToString(), new Font("Arial", 10), Brushes.Black, 50, posicao);
                e.Graphics.DrawString(linha.Cells[1].Value.ToString(), new Font("Arial", 10), Brushes.Black, 130, posicao);
                e.Graphics.DrawString(cboPesquisa.Text.ToString(), new Font("Arial", 10), Brushes.Black, 450, posicao);
                itens++;
            }
        }
    }
}
