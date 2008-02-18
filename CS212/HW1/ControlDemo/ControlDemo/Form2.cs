using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ControlDemo
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            label1.Text = "Hello, welcome to the secret cove!";
            pictureBox1.Hide();
            listBox1.Items.Add("Parrots");
            listBox1.Items.Add("Gold");
            listBox1.Items.Add("Cookies");
            Console.WriteLine("Form 2: You are in the cove!");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Button Press: Back to entrance!");
            Form1 f1 = new Form1();
            f1.Show();
            this.Hide();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            label1.Text = "Please click on the other controls too!";
            Console.WriteLine("Label press.");
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            pictureBox1.Show();
            if (checkBox1.Checked == false)
            {
                pictureBox1.Hide();
                Console.WriteLine("Check box: no more parrot :(");
            }
            else if(checkBox1.Checked == true)
            {
                Console.WriteLine("Check box check: You like parrots.");
            }

        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Picture: Squawk!");
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Console.WriteLine("List box.");
        }

     

        private void groupBox1_Enter(object sender, EventArgs e)
        {
            Console.WriteLine("Group box.");
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            listBox1.SelectedIndex = listBox1.FindString("Parrots");
            comboBox1.Items.Add("Parrots");
            Console.WriteLine("Radio button.");
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            listBox1.SelectedIndex = listBox1.FindString("Gold");
            comboBox1.Items.Add("Gold");
            Console.WriteLine("Radio button.");
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            listBox1.SelectedIndex = listBox1.FindString("Cookies");
            comboBox1.Items.Add("Cookies");
            Console.WriteLine("Radio button.");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label1.Text = DateTime.Now.ToShortTimeString();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            label1.Text = DateTime.Now.ToShortTimeString();
            Console.WriteLine("Time check.");
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            progressBar1.Increment(5);
            Console.WriteLine("Numeric Up/Down.");
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {
        }
    }
}