/**
 * Christopher Kwan
 * ckwan@bu.edu     U37-02-3645
 * CS212 Project Set
 * GUI
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SetProject
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == "dog")
            {
                Console.WriteLine("dogg");
                richTextBox1.Text = "Dogggg";
            }
            else
            {
                Console.WriteLine("dino");
                richTextBox1.Text = "Dingo";
                for (int i = 0; i < 1000; i++)
                {
                    richTextBox1.Text += "\n" + i;
                }
                //richTextBox1.SelectionLength = 0;
                richTextBox1.SelectionStart = richTextBox1.TextLength;
                richTextBox1.ScrollToCaret();
                //richTextBox1.Select();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
        }
    }
}