using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ControlDemo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Console.WriteLine("Form 1: Application initialized!");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Button Press: You entered the cove!");
            Form2 f2 = new Form2();
            f2.Show();
            this.Hide();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Label Press.");
        }

    }
}