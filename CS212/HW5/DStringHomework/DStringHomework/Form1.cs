using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DStringApp
{
    public partial class DStringHWButtons : Form
    {
        DStringHomework dsh = new DStringHomework();

        public DStringHWButtons()
        {
            InitializeComponent();
            //this.BackColor = Color.Azure;
            //label1.BackColor = Color.AliceBlue;
            
            label2.Text = "DString Homework - Chris Kwan";
            
            label1.Text = "Please click the buttons to run"
                + "\nthe problems in a Console Window."
                + "\n\nPlease note that some problems"
                + "\nrequire Console input from the user.";
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            dsh.Problem1();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dsh.Problem2();
        }

        private void DStringHWButtons_Load(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            dsh.Problem3();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            dsh.Problem4();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            dsh.Problem5();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            dsh.Problem6();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            dsh.Problem7();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            dsh.Problem8();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            dsh.Problem9();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            dsh.Problem10();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            Console.WriteLine("\nChristopher Kwan  U37-02-3645  ckwan@bu.edu\n");
            Console.WriteLine("CS212 Paradigms Lab 05 2/28/08\n");
            Console.WriteLine("DString Homework\n");
        }

        private void button12_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void label1_Click_2(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        
    }
}