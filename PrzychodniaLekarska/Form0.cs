using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrzychodniaLekarska
{
    public partial class Form0 : Form
    {
        Form nextForm;
        public Form0()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
           nextForm= new Form1(2);
           nextForm.Show();
           this.Hide();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            nextForm = new Form1(3);
            nextForm.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            nextForm = new Form1(4);
            nextForm.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            nextForm = new Form1(5);
            nextForm.Show();
            this.Hide();
        }
    }
}
