using System;
using System.Drawing;
using System.Windows.Forms;
using uvncDotNet.Controls;
using uvncDotNet.Properties;
using uvncDotNet.Uvnc;

namespace uvncDotNet
{
    public partial class MainForm : Form
    {
        private readonly Color _color = Color.FromArgb(230, 90, 60);
        private int _errorCount;

        public MainForm()
        {
            InitializeComponent();
            InitializeFlatMenu();

            errorProvider1.Icon = Resources.Error_16;

            var conf = new Config("ultravnc.ini")
            {
                passwd = "00",
                passwd2 = "0",
                DebugLevel = "5",
                DebugMode = "2",
                DisableTrayIcon = "false",
                AllowLoopback = "1",
                QueryIfNoLogon = "0"
            };
        }

        private void CloseForm()
        {
            this.Close();
        }

        private void InitializeFlatMenu()
        {
            // File menu item.
            var fileItem = new FlatMenuItem { Text = "File" };

            var exitItem = new FlatMenuItem { Text = "Exit" };
            exitItem.Click += (sender, args) => CloseForm();
            fileItem.MenuItems.Add(exitItem);

            flatMenuBar1.MenuItems.Add(fileItem);
            flatMenuBar1.Popup.BackColor = Color.FromArgb(230, 90, 60);
        }

        private void ConnectToPartner()
        {
            CheckForError(partnerIDTextBox, "Please put Partner ID");
            CheckForError(passwordTextBox, "Please put Password");

            if (_errorCount == 0)
            {
                errorProvider1.Clear();
                partnerIDTextBox.BackColor = Color.White;

                var client = new UvncClient
                {
                    Host = partnerIDTextBox.Text,
                    Password = passwordTextBox.Text
                };
                var form = new ClientForm(client);
                form.Show();
            }

            _errorCount = 0;
        }

        private void CheckForError(Control ctrl, string message)
        {
            if (ctrl.Text == "")
                SetError(ctrl, message);
            else
                SetError(ctrl, "");
        }

        private void SetError(Control ctrl, string message)
        {
            Color color = Color.White;
            if (message != "")
            {
                _errorCount++;
                color = _color;
            }

            ctrl.BackColor = color;
            errorProvider1.SetError(ctrl, message);

        }



        private void connectToPartnerButton_Click(object sender, EventArgs e)
        { ConnectToPartner(); }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                ConnectToPartner();
        }
    }

}
