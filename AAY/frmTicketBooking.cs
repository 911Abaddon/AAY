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
        private const int SeatCost = 10;

        public frmTicketBooking()
        {
            InitializeComponent();
        }
        private void b(object sender, EventArgs e)
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
                    lblchair.Click += Lblchair_Click;


                }

            }

        }

        private void Lblchair_Click(object sender, EventArgs e)
        {
            Label Lblchair = sender as Label;
            if (Lblchair.BackColor == Color.White)
            {
                Lblchair.BackColor = Color.SkyBlue;
            }
            else if (Lblchair.BackColor == Color.SkyBlue)
            {
                Lblchair.BackColor = Color.White;
            }

            // Ενημέρωση συνολικού κόστους
            lbltotalcost.Text = $"Συνολικό Κόστος: {CalculateTotalCost()} ευρώ";
        }
        private int CalculateTotalCost()
        {
            int selectedSeats = 0;

            // Διατρέχουμε τις θέσεις στο `PnChair` και μετράμε πόσες είναι επιλεγμένες
            foreach (Control control in PnChair.Controls)
            {
                if (control is Label && control.BackColor == Color.SkyBlue)
                {
                    selectedSeats++;
                }
            }

            // Υπολογισμός συνολικού κόστους
            return selectedSeats * SeatCost;
        }

        private void PnChair_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int totalCost = CalculateTotalCost();

            if (totalCost == 0)
            {
                MessageBox.Show("Παρακαλώ επιλέξτε τουλάχιστον μία θέση για να κάνετε κράτηση.", "Επιλογή θέσεων", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (frmCustomers customerForm = new frmCustomers())
            {
                if (customerForm.ShowDialog() == DialogResult.OK)
                {
                    // Λήψη των στοιχείων πελάτη
                    string customerName = $"{customerForm.FirstName} {customerForm.LastName}";
                    string paymentMethod = customerForm.PaymentMethod;

                    // Εμφάνιση της κράτησης στο ListBox
                    Customers.Items.Add($"{customerName} - Πληρωμή με: {paymentMethod}");

                    // Ολοκλήρωση της κράτησης: Αλλαγή χρώματος στις επιλεγμένες θέσεις
                    foreach (Control control in PnChair.Controls)
                    {
                        if (control is Label && control.BackColor == Color.SkyBlue)
                        {
                            control.BackColor = Color.Gray; // Το γκρι υποδηλώνει ότι η θέση έχει κρατηθεί
                            control.Enabled = false; // Απενεργοποίηση του Label για να μην μπορεί να αλλάξει
                        }
                    }

                    MessageBox.Show("Η κράτηση ολοκληρώθηκε με επιτυχία!", "Κράτηση", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Customers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Customers.SelectedItem != null)
            {
                MessageBox.Show($"Επιλέξατε τον πελάτη: {Customers.SelectedItem}", "Πληροφορίες Πελάτη", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void btnCancelTickets_(object sender, EventArgs e)
        {
            // Έλεγχος αν έχει επιλεγεί κάποιο στοιχείο
            if (Customers.SelectedItem != null)
            {
                // Διαγραφή του επιλεγμένου στοιχείου από το ListBox
                Customers.Items.Remove(Customers.SelectedItem);
                MessageBox.Show("Η κράτηση διαγράφηκε επιτυχώς.", "Επιβεβαίωση", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Παρακαλώ επιλέξτε μία κράτηση για να τη διαγράψετε.", "Σφάλμα", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
           
            this.Close();
        }
    } 
}
