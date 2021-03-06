﻿using System.Windows.Forms;
using uvncDotNet.Uvnc;
using VncSharp;

namespace uvncDotNet
{
    public partial class ClientForm : Form
    {

        public ClientForm()
        {
            InitializeComponent();
        }

        public ClientForm(UvncClient uvncClient)
        {
            InitializeComponent();

            base.Text = uvncClient.Host;

            remoteDesktop1.GetPassword = () => uvncClient.Password;
            remoteDesktop1.VncProxyID = uvncClient.ProxyID;
            remoteDesktop1.VncPort = uvncClient.Port;
            remoteDesktop1.Connect(uvncClient.Host, uvncClient.Display, uvncClient.ViewOnly, uvncClient.Scaled);
        }

        private void ctrlAltDelToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            remoteDesktop1.SendSpecialKeys(SpecialKeys.CtrlAltDel);
        }

        private void altF4ToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            remoteDesktop1.SendSpecialKeys(SpecialKeys.AltF4);
            
        }
    }
}