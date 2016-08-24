using System;
using System.Drawing;
using System.Windows.Forms;
using uvncDotNet.Controls;
using uvncDotNet.Uvnc;

namespace uvncDotNet
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            LoadMenuBar1();

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

        private void LoadMenuBar1()
        {
            // File menu item.
            var fileItem = new FlatMenuItem {Text = "File"};

            var exitItem = new FlatMenuItem {Text = "Exit"};
            var item1 = new FlatMenuItem {Text = "item1" };
            var item2 = new FlatMenuItem {Text = "item2" };

            fileItem.MenuItems.Add(item1);
            fileItem.MenuItems.Add(exitItem);
            fileItem.MenuItems.Add(item2);
          

            flatMenuBar1.MenuItems.Add(fileItem);
            flatMenuBar1.Popup.BackColor = Color.FromArgb(230, 90, 60);
            //flatMenuBar1.Popup.BorderColor = Color.White;
            //flatMenuBar1.Popup.EnableBorderDrawing = true;

            //this.flatMenuBar1.MenuItems.Add(fileItem);
        }

        private void connectToPartnerButton_Click(object sender, System.EventArgs e)
        {
            var client = new UvncClient
            {
                Host = partnerIDTextBox.Text
            };
            var form = new ClientForm(client);
            form.Show();
        }
    }
}
