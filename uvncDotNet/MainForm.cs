using System.Windows.Forms;
using uvncDotNet.Uvnc;

namespace uvncDotNet
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

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
