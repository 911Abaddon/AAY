using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using Microsoft.Win32;

namespace AAY
{
    public partial class Form6 : Form
    {
        private Dictionary<string, decimal> menuPrices;
        private GroupBox paymentGroupBox;
        private RadioButton rbCash;
        private RadioButton rbCard;
        private Panel cardPanel;
        private TextBox txtCardNumber;
        private Label lblCardNumber;
        private TextBox txtCardExpiry;
        private Label lblCardExpiry;
        private TextBox txtCardCVV;
        private Label lblCardCVV;
        private Dictionary<string, string> videoLinks;

        public Form6()
        {
            InitializeComponent();
            InitializeWebBrowser();
            SetupForm();
            AddPaymentControls();
            SetupVideoPlayer();
        }

        private void SetupForm()
        {
            menuPrices = new Dictionary<string, decimal>
            {
                {"Καφές Φίλτρου", 2.50m},
                {"Cappuccino", 3.50m},
                {"Αναψυκτικά", 2.00m},
                {"Σνακ Ανάμεικτα", 3.00m}
            };

            this.Text = "Αίθουσες Προβολής";
            textBox1.ReadOnly = true;
            textBox1.Text = "0.00€";

            // Configure WebBrowser


            // Initialize video links
            videoLinks = new Dictionary<string, string>
{
                {"► Συναυλία Κλασικής Μουσικής - Μέγαρο Μουσικής",
                 "https://www.youtube.com/embed/jgpJVI3tDbY"},
                {"► Συνέντευξη με τον Μίκη Θεοδωράκη",
                 "https://www.youtube.com/embed/tyg_2YuPKGY"},
                {"► Αφιέρωμα στην Ελληνική Μουσική Παράδοση",
                 "https://www.youtube.com/embed/id137_o7Zu4"},
                {"► Διεθνές Φεστιβάλ Τζαζ 2024",
                 "https://www.youtube.com/embed/Z27-hFIC3Tw"},
                {"► Μουσική Παράδοση της Κρήτης",
                 "https://www.youtube.com/embed/UYZMrFWnMes"},
                {"► Ρεμπέτικη Βραδιά - Αφιέρωμα",
                 "https://www.youtube.com/embed/sp7OCsY1PbE"}
            };
        }

        private void SetupVideoPlayer()
        {
            webBrowser1.ScriptErrorsSuppressed = true;
            webBrowser1.ScrollBarsEnabled = false;
        }

      

        private void InitializeWebBrowser()
        {
            try
            {
                string appName = System.IO.Path.GetFileName(Application.ExecutablePath);
                using (var key = Registry.CurrentUser.OpenSubKey(
                    @"Software\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION",
                    true))
                {
                    if (key != null)
                    {
                        key.SetValue(appName, 11001, RegistryValueKind.DWord);
                        key.SetValue(appName + ".exe", 11001, RegistryValueKind.DWord);
                    }
                }
            }
            catch (Exception)
            {
                // Silently handle registry access errors
            }
        }

        private void PlayVideo(string videoUrl)
        {
            try
            {
                string html = $@"
            <!DOCTYPE html>
            <html>
            <head>
                <meta content='IE=Edge' http-equiv='X-UA-Compatible'/>
                <style>
                    body, html {{
                        margin: 0;
                        padding: 0;
                        height: 100%;
                        overflow: hidden;
                    }}
                    #player {{
                        width: 100%;
                        height: 100%;
                        position: absolute;
                    }}
                </style>
            </head>
            <body>
                <iframe id='player'
                    width='100%'
                    height='100%'
                    src='{videoUrl}?autoplay=1&rel=0'
                    title='YouTube video player'
                    frameborder='0'
                    allow='accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture'
                    allowfullscreen>
                </iframe>
            </body>
            </html>";

                webBrowser1.DocumentText = html;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading video: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddPaymentControls()
        {
            // Payment GroupBox
            paymentGroupBox = new GroupBox
            {
                Text = "Μέθοδος Πληρωμής",
                Location = new Point(562, 298),
                Size = new Size(203, 140),
                Font = new Font("Microsoft Sans Serif", 8.25f)
            };
            this.Controls.Add(paymentGroupBox);

            // Radio Buttons
            rbCash = new RadioButton
            {
                Text = "Μετρητά",
                Location = new Point(10, 20),
                AutoSize = true,
                Checked = true
            };
            rbCash.CheckedChanged += RadioButton_CheckedChanged;
            paymentGroupBox.Controls.Add(rbCash);

            rbCard = new RadioButton
            {
                Text = "Κάρτα",
                Location = new Point(10, 40),
                AutoSize = true
            };
            rbCard.CheckedChanged += RadioButton_CheckedChanged;
            paymentGroupBox.Controls.Add(rbCard);

            // Card Panel
            cardPanel = new Panel
            {
                Location = new Point(10, 65),
                Size = new Size(180, 70),
                Visible = false
            };
            paymentGroupBox.Controls.Add(cardPanel);

            // Card Details Controls
            lblCardNumber = new Label
            {
                Text = "Αριθμός Κάρτας:",
                Location = new Point(0, 0),
                AutoSize = true
            };
            cardPanel.Controls.Add(lblCardNumber);

            txtCardNumber = new TextBox
            {
                Location = new Point(85, 0),
                Size = new Size(90, 20),
                MaxLength = 16
            };
            txtCardNumber.KeyPress += CardNumber_KeyPress;
            cardPanel.Controls.Add(txtCardNumber);

            lblCardExpiry = new Label
            {
                Text = "Λήξη (MM/YY):",
                Location = new Point(0, 25),
                AutoSize = true
            };
            cardPanel.Controls.Add(lblCardExpiry);

            txtCardExpiry = new TextBox
            {
                Location = new Point(85, 25),
                Size = new Size(50, 20),
                MaxLength = 5
            };
            txtCardExpiry.KeyPress += CardExpiry_KeyPress;
            txtCardExpiry.TextChanged += CardExpiry_TextChanged;
            cardPanel.Controls.Add(txtCardExpiry);

            lblCardCVV = new Label
            {
                Text = "CVV:",
                Location = new Point(0, 50),
                AutoSize = true
            };
            cardPanel.Controls.Add(lblCardCVV);

            txtCardCVV = new TextBox
            {
                Location = new Point(85, 50),
                Size = new Size(30, 20),
                MaxLength = 3
            };
            txtCardCVV.KeyPress += CardCVV_KeyPress;
            cardPanel.Controls.Add(txtCardCVV);
        }

        // Event Handlers
        private void RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            cardPanel.Visible = rbCard.Checked;
        }

        private void CardNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void CardExpiry_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != '/')
            {
                e.Handled = true;
            }
        }

        private void CardExpiry_TextChanged(object sender, EventArgs e)
        {
            if (txtCardExpiry.Text.Length == 2 && !txtCardExpiry.Text.Contains("/"))
            {
                txtCardExpiry.Text = txtCardExpiry.Text + "/";
                txtCardExpiry.SelectionStart = txtCardExpiry.Text.Length;
            }
        }

        private void CardCVV_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            CalculateTotal();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                string selectedTitle = listBox1.SelectedItem.ToString();
                if (videoLinks.ContainsKey(selectedTitle))
                {
                    string videoUrl = videoLinks[selectedTitle];
                    PlayVideo(videoUrl);
                }
            }
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            SetBrowserFeatureControl();
        }

        private void SetBrowserFeatureControl()
        {
            // Get the current process filename
            string appName = System.IO.Path.GetFileName(System.Windows.Forms.Application.ExecutablePath);

            // Set browser feature keys for the current process
            SetBrowserFeatureControlKey("FEATURE_BROWSER_EMULATION", appName, 11001);
            SetBrowserFeatureControlKey("FEATURE_AJAX_CONNECTIONEVENTS", appName, 1);
            SetBrowserFeatureControlKey("FEATURE_ENABLE_CLIPCHILDREN_OPTIMIZATION", appName, 1);
            SetBrowserFeatureControlKey("FEATURE_MANAGE_SCRIPT_CIRCULAR_REFS", appName, 1);
            SetBrowserFeatureControlKey("FEATURE_DOMSTORAGE ", appName, 1);
        }

        private void SetBrowserFeatureControlKey(string feature, string appName, uint value)
        {
            using (Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(
                @"Software\Microsoft\Internet Explorer\Main\FeatureControl\" + feature,
                Microsoft.Win32.RegistryKeyPermissionCheck.ReadWriteSubTree))
            {
                key.SetValue(appName, value, Microsoft.Win32.RegistryValueKind.DWord);
            }
        }


        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void label3_Click(object sender, EventArgs e)
        {
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (checkedListBox1.CheckedItems.Count == 0)
            {
                MessageBox.Show("Παρακαλώ επιλέξτε προϊόντα για την παραγγελία σας.",
                    "Προειδοποίηση", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (rbCard.Checked)
            {
                if (string.IsNullOrWhiteSpace(txtCardNumber.Text) ||
                    txtCardNumber.Text.Length < 16 ||
                    string.IsNullOrWhiteSpace(txtCardExpiry.Text) ||
                    txtCardExpiry.Text.Length < 5 ||
                    string.IsNullOrWhiteSpace(txtCardCVV.Text) ||
                    txtCardCVV.Text.Length < 3)
                {
                    MessageBox.Show("Παρακαλώ συμπληρώστε σωστά τα στοιχεία της κάρτας.",
                        "Σφάλμα", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            string paymentMethod = rbCard.Checked ? "Κάρτα" : "Μετρητά";
            string message = "Η παραγγελία σας:\n\n";
            foreach (var item in checkedListBox1.CheckedItems)
            {
                message += item.ToString() + "\n";
            }
            message += $"\nΣύνολο: {textBox1.Text}";
            message += $"\nΤρόπος Πληρωμής: {paymentMethod}";

            if (MessageBox.Show(message, "Επιβεβαίωση Παραγγελίας",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
            {
                // Reset form
                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                {
                    checkedListBox1.SetItemChecked(i, false);
                }
                textBox1.Text = "0.00€";
                rbCash.Checked = true;
                txtCardNumber.Clear();
                txtCardExpiry.Clear();
                txtCardCVV.Clear();

                MessageBox.Show("Η παραγγελία σας καταχωρήθηκε επιτυχώς!",
                    "Επιτυχία", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void CalculateTotal()
        {
            decimal total = 0;
            foreach (int index in checkedListBox1.CheckedIndices)
            {
                string itemText = checkedListBox1.Items[index].ToString();
                string[] parts = itemText.Split('\t');
                if (parts.Length >= 2)
                {
                    string priceText = parts[1].Replace("€", "").Trim();
                    if (decimal.TryParse(priceText, out decimal price))
                    {
                        total += price;
                    }
                }
            }
            textBox1.Text = $"{total:F2}€";
        }
    }
}