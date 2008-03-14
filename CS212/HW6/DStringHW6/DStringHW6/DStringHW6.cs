using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DStringHW6
{
    public partial class DStringHW6 : Form
    {
        public DStringHW6()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            label1.Text = "Christopher Kwan   ckwan@bu.edu   U37-02-3645"
            + "\nCS212 Paradigms Lab 06 DString Homework";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DStringTest2 dst2 = new DStringTest2();
            dst2.run();
        }
    }
}