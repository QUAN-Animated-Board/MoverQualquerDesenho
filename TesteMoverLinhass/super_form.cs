using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TesteMoverLinhass
{
    public enum action
    {
        none, line, move, bucket, TwoPoint, eraser
    }

    public partial class super_form : Form
    {
        public static super_form sf;
        public static action super_action;

        List<Form1> form_list = new List<Form1>();

        public super_form()
        {
            InitializeComponent();

            Form1 frmChild = new Form1();
            frmChild.Text = "Inicio";
            frmChild.Dock = DockStyle.Fill;
            form_list.Add(frmChild);

            AddNewTab(frmChild);

            super_action = action.none;
            sf = this;
        }

        private void AddNewTab(Form frm)
        {
            TabPage tab = new TabPage(frm.Text);
            frm.TopLevel = false;
            frm.Parent = tab;
            frm.Visible = true;
            frm.Dock = DockStyle.Fill;

            tabControl1.TabPages.Add(tab);

            frm.Location = new Point((tab.Width - frm.Width) / 2, (tab.Height - frm.Height) / 2);

            tabControl1.SelectedTab = tab;

            tabControl1.Select();
        }

        int count = 1;
        private void button1_Click(object sender, EventArgs e)
        {
            Form1 frmChild = new Form1();
            frmChild.Text = "Página " + count;
            frmChild.Dock = DockStyle.Right;
            AddNewTab(frmChild);
            count++;
            form_list.Add(frmChild);
        }

        int selected_tab = 0;
        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            selected_tab = e.TabPageIndex;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            super_action = action.line;
            Form1.q = 0;
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            super_action = action.move;
        }

        public void finalizacao()
        {
            System.Diagnostics.Debug.WriteLine(Form1.super_acao);
            System.Diagnostics.Debug.WriteLine("Ent");

            if (Form1.super_acao.Equals(acao.ending))
            {
                System.Diagnostics.Debug.WriteLine("Entroe");

                Form1 selected_form = form_list.ElementAt<Form1>(selected_tab);

                lines current_line = new lines();
                current_line = selected_form.lines_list.ElementAt<lines>(selected_form.line_counter - 1);

                current_line.draw_arrow();
                super_action = action.none;
                selected_form.Invalidate();
                selected_form.first_point_in_line = true;
                Form1.super_acao = acao.non;
            }

            

        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            super_action = action.bucket;
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            super_action = action.TwoPoint;
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            super_action = action.eraser;
        }
    }
}
