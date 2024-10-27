using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AAY
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            // Create an instance of the ExhibitionSpaceManagement form
            ExhibitionSpaceManagement exhibitionForm = new ExhibitionSpaceManagement();

            // Show the ExhibitionSpaceManagement form
            exhibitionForm.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            frmTicketBooking newForm = new frmTicketBooking();
            newForm.Show();
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            Form6 form6 = new Form6();
            form6.Show();
        }
    }
}
