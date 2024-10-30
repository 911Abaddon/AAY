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
    public partial class frmCustomers : Form
    {
        public frmCustomers()
        {
            InitializeComponent();
        }

        // Δημόσιες μεταβλητές για αποθήκευση των στοιχείων πελάτη
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string PaymentMethod { get; private set; }

       

        private void frmCustomers_Load(object sender, EventArgs e)
        {
            // Προσθήκη επιλογών για το ComboBox
            cmbPaymentMethod.Items.Add("Μετρητά");
            cmbPaymentMethod.Items.Add("Κάρτα");
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void cmbPaymentMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbPaymentMethod.SelectedItem.ToString() == "Κάρτα")
            {
                txtCardNumber.Visible = true;
            }
            else
            {
                txtCardNumber.Visible = false;
                txtCardNumber.Text = ""; // Καθαρισμός του αριθμού κάρτας όταν δεν είναι απαραίτητο
            }
        }

        private void btnConfirm_Click_1(object sender, EventArgs e)
        {
            // Αποθήκευση στοιχείων από τα TextBox και ComboBox
            FirstName = txtFirstName.Text;
            LastName = txtLastName.Text;
            PaymentMethod = cmbPaymentMethod.SelectedItem?.ToString();
            string cardNumber = txtCardNumber.Text;

            // Έλεγχος για κενά πεδία
            if (string.IsNullOrWhiteSpace(FirstName) || string.IsNullOrWhiteSpace(LastName) || string.IsNullOrWhiteSpace(PaymentMethod))
            {
                MessageBox.Show("Παρακαλώ συμπληρώστε όλα τα πεδία.", "Σφάλμα", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Έλεγχος αριθμού κάρτας αν επιλεγεί η πληρωμή με κάρτα
            if (PaymentMethod == "Κάρτα" && string.IsNullOrWhiteSpace(cardNumber))
            {
                MessageBox.Show("Παρακαλώ εισάγετε αριθμό κάρτας.", "Σφάλμα", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Μήνυμα επιβεβαίωσης πληρωμής
            if (PaymentMethod == "Μετρητά")
            {
                MessageBox.Show("Σας περιμένουμε 30 λεπτά πριν την έναρξη της παράστασης.", "Επιβεβαίωση", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (PaymentMethod == "Κάρτα")
            {
                MessageBox.Show("Ευχαριστούμε για την προτίμηση!", "Επιβεβαίωση", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            this.DialogResult = DialogResult.OK; // Επιστρέφει DialogResult
            this.Close(); // Κλείνει τη φόρμα

        }
    }
}
