using System.Drawing.Imaging;
using System.Windows.Forms;
using VncSharp;

namespace uvncDotNet
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            var conf = new uvnc.Config("ultravnc.ini")
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

        private void connectToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            //var protocol = new RfbProtocol();
            //protocol.ProxyID = 1234;
            //protocol.Connect("192.168.1.4", 5901);

            //var rd = new RemoteDesktop
            //{
            //    Dock = DockStyle.Fill
            //};

            remoteDesktop1.VncClient.RfbProtocol.ProxyID = 1234;

            remoteDesktop1.VncPort = 5901;
            //remoteDesktop1.Connect("192.168.1.3");
            remoteDesktop1.Connect("a2d.my-firewall.org");

            //var buffer = remoteDesktop1.VncClient.Framebuffer;
            //buffer.BitsPerPixel = 8;
            //buffer.TrueColour = false;
            //buffer.Depth = 256;
            
            //remoteDesktop1.VncClient.RfbProtocol.WriteSetPixelFormat(buffer);
            remoteDesktop1.ConnectionLost += (o, args) => 
            MessageBox.Show(this,
                "Lost Connection to Host.",
                "Connection Lost",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
 
        }
    }
}
