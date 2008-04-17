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
        SetTest tester = new SetTest();

        public Form1()
        {
            InitializeComponent();
            comboBox1.Text = "";
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                //Console.WriteLine("Set<T>");
                richTextBox1.Text += "\nSet<T>: Click \"Run Test\" to run the test\n";
            }
            else if (comboBox1.SelectedIndex == 1)
            {
                //Console.WriteLine("SSet");
                richTextBox1.Text += "\nSSet: Click \"Run Test\" to run the test\n";
            }
            else if (comboBox1.SelectedIndex == 2)
            {
                //Console.WriteLine("SetEnumerator<T>");
                richTextBox1.Text += "\nSetEnumerator<T>: Click \"Run Test\" to run the test\n";
            }

                //richTextBox1.SelectionLength = 0;
                richTextBox1.SelectionStart = richTextBox1.TextLength;
                richTextBox1.ScrollToCaret();
                //richTextBox1.Select();
            
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

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                richTextBox1.Text += tester.runSetTests();
            }
            else if (comboBox1.SelectedIndex == 1)
            {
                richTextBox1.Text += tester.runSSetTests();
            }
            else if (comboBox1.SelectedIndex == 2)
            {
                richTextBox1.Text += tester.runSetEnumTests();
            }
            richTextBox1.SelectionStart = richTextBox1.TextLength;
            richTextBox1.ScrollToCaret();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            richTextBox1.Text += "\n\nChristopher Kwan\nckwan@bu.edu\nCS212 Devlin\nProject Set\n\n";
            richTextBox1.SelectionStart = richTextBox1.TextLength;
            richTextBox1.ScrollToCaret();
        }
    }
}