using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Lab_07
{
    public partial class Form2 : Form
    {
        public string data;

        public Form2()
        {
            InitializeComponent();
        }

        public void setCaption(string caption)
        {
            label1.Text = caption;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            data = textBox1.Text;
            this.Hide();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            data = textBox1.Text;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}