using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Lab_07
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();
            f2.Text = "Modal Dialog";
            f2.setCaption("Modal Dialog");
            f2.ShowDialog();
            textBox1.Text = f2.data;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();
            f2.Text = "Modeless Dialog";
            f2.setCaption("Modeless Dialog");
            f2.Show();
            textBox1.Text = f2.data;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}