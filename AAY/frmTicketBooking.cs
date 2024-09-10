using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AAY
{
    public partial class frmTicketBooking : Form
    {

        public frmTicketBooking()
        {
            InitializeComponent();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            // Ενέργειες όταν πατηθεί το button2
        }

        private void frmTicketBooking_Load(object sender, EventArgs e)
        {
           Draw100chairs();
        }
        
        

        private void Draw100chairs()
        {
            int chair = 1;
            for (int i = 0; i < PnChair.RowCount; i++)
            {
                for (int j = 0; j < PnChair.ColumnCount; j++)
                {
                    Label lblchair = new Label();

                    lblchair.Text = chair + "";
                    lblchair.AutoSize = false;
                    lblchair.Dock = DockStyle.Fill;
                    lblchair.TextAlign = ContentAlignment.MiddleCenter;
                    lblchair.BackColor = Color.White;

                    PnChair.Controls.Add(lblchair, i, j);

                    chair++;


                }

            }

        }

    } 
}
