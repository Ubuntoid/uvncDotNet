using System;
using System.IO;
using Saltwort.ConfigFile;

namespace uvncDotNet.uvnc
{
    public class Config
    {
        private readonly IniFile _ini;

        /// <summary>
        /// Custom accept/reject messageBox text.
        /// to change the messageBox logo add logo.bmp in the ultravnc folder
        /// </summary>
        public string accept_reject_mesg
        {
            get { return _ini.Read("accept_reject_mesg", "admin"); }
            set { _ini.Write("accept_reject_mesg", value, "admin"); }
        }

        /// <summary>
        /// This is used to instruct the service to start winvnc (in service mode) with a specific command line.
        /// This is the same syntax as the commandline except you don't put -run at the end. 
        /// Sample: 
        ///     service_commandline=-autoreconnect -connect 192.168.1.30 
        /// This tell the service to make an invers connection to 192.168.1.30 and retry when it fail.
        /// </summary>
        public string service_commandline
        {
            get { return _ini.Read("service_commandline", "admin"); }
            set { _ini.Write("service_commandline", value, "admin"); }
        }

        /// <summary>
        /// Enable Filtransfer
        /// </summary>
        public string FileTransferEnabled
        {
            get { return _ini.Read("FileTransferEnabled", "admin"); }
            set { _ini.Write("FileTransferEnabled", value, "admin"); }
        }

        /// <summary>
        /// When doing a file transfer, act as desktop user. When you don't put 1 the filetransfer is done as
        /// user "system". User system don't have access to mapped drives and pose a security issue... a normal
        /// user can FT as admin.
        /// </summary>
        public string FTUserImpersonation
        {
            get { return _ini.Read("FTUserImpersonation", "admin"); }
            set { _ini.Write("FTUserImpersonation", value, "admin"); }
        }

        /// <summary>
        /// This allow the viewer to blank the screen
        /// </summary>
        public string BlankMonitorEnabled
        {
            get { return _ini.Read("BlankMonitorEnabled", "admin"); }
            set { _ini.Write("BlankMonitorEnabled", value, "admin"); }
        }

        /// <summary>
        /// Capture alphaBlending is needed for semi transparent windows ( xp, vista...) but use more cpu.
        /// </summary>
        public string CaptureAlphaBlending
        {
            get { return _ini.Read("CaptureAlphaBlending", "admin"); }
            set { _ini.Write("CaptureAlphaBlending", value, "admin"); }
        }

        /// <summary>
        /// Instead of using the powermanager to blank the monitor we put a layered window on top and capture the windows below.
        /// Using this option you also can define a custom blank by placing a file "background.bmp" in the ultravnc folder.
        /// </summary>
        public string BlackAlphaBlending
        {
            get { return _ini.Read("BlackAlphaBlending", "admin"); }
            set { _ini.Write("BlackAlphaBlending", value, "admin"); }
        }

        /// <summary>
        /// Set scale
        /// </summary>
        public string DefaultScale
        {
            get { return _ini.Read("DefaultScale", "admin"); }
            set { _ini.Write("DefaultScale", value, "admin"); }
        }

        /// <summary>
        /// Use the defined encryption plugin
        /// </summary>
        public string UseDSMPlugin
        {
            get { return _ini.Read("UseDSMPlugin", "admin"); }
            set { _ini.Write("UseDSMPlugin", value, "admin"); }
        }

        /// <summary>
        /// Name of the plugin
        /// </summary>
        public string DSMPlugin
        {
            get { return _ini.Read("DSMPlugin", "admin"); }
            set { _ini.Write("DSMPlugin", value, "admin"); }
        }

        /// <summary>
        /// When using multi-monitors ( driver required) you can define the default behaviour. Show only primary/Secunday or both
        /// </summary>
        public string primary
        {
            get { return _ini.Read("primary", "admin"); }
            set { _ini.Write("primary", value, "admin"); }
        }

        /// <summary>
        /// When using multi-monitors ( driver required) you can define the default behaviour. Show only primary/Secunday or both
        /// </summary>
        public string secondary
        {
            get { return _ini.Read("secondary", "admin"); }
            set { _ini.Write("secondary", value, "admin"); }
        }

        /// <summary>
        /// Need to be one, else no socket is listening for a connection
        /// </summary>
        public string SocketConnect
        {
            get { return _ini.Read("SocketConnect", "admin"); }
            set { _ini.Write("SocketConnect", value, "admin"); }
        }

        /// <summary>
        /// Manual set listening port ( default 5900)
        /// </summary>
        public string PortNumber
        {
            get { return _ini.Read("PortNumber", "admin"); }
            set { _ini.Write("PortNumber", value, "admin"); }
        }

        /// <summary>
        /// Start a sond port, this act as webserver for java viewer
        /// </summary>
        public string HTTPConnect
        {
            get { return _ini.Read("HTTPConnect", "admin"); }
            set { _ini.Write("HTTPConnect", value, "admin"); }
        }

        /// <summary>
        /// manual set port for http (default 5800)
        /// </summary>
        public string HTTPPortNumber
        {
            get { return _ini.Read("HTTPPortNumber", "admin"); }
            set { _ini.Write("HTTPPortNumber", value, "admin"); }
        }

        /// <summary>
        /// The port is 5900, but when port 5900 is already in use the auto mode take one higher until he find a free one.
        /// </summary>
        public string AutoPortSelect
        {
            get { return _ini.Read("AutoPortSelect", "admin"); }
            set { _ini.Write("AutoPortSelect", value, "admin"); }
        }

        /// <summary>
        /// Allow the viewer to control the server
        /// </summary>
        public string InputsEnabled
        {
            get { return _ini.Read("InputsEnabled", "admin"); }
            set { _ini.Write("InputsEnabled", value, "admin"); }
        }

        /// <summary>
        /// Block the server input, only remote access is possible
        /// </summary>
        public string LocalInputsDisabled
        {
            get { return _ini.Read("LocalInputsDisabled", "admin"); }
            set { _ini.Write("LocalInputsDisabled", value, "admin"); }
        }

        /// <summary>
        /// Disconnect after a idle period ( 0 = default, no idle time out , seconds)
        /// </summary>
        public string IdleTimeout
        {
            get { return _ini.Read("IdleTimeout", "admin"); }
            set { _ini.Write("IdleTimeout", value, "admin"); }
        }

        /// <summary>
        /// This can be used for Japanese and other non standard keyboards. 
        /// The key processing is different and sometimes solve issue's with special keys.
        /// </summary>
        public string EnableJapInput
        {
            get { return _ini.Read("EnableJapInput", "admin"); }
            set { _ini.Write("EnableJapInput", value, "admin"); }
        }

        /// <summary>
        /// + = allow <para />
        /// - = deny <para />
        /// ? = query <para />
        /// Syntax:
        ///     -:+10.0.60.141:?10.0.31.169:-10.0.20.240:<para />
        ///     Instead of 10.0.60.141 you can use 10.0.60, then it is valid for the full range of ip addresses.
        /// </summary>
        public string AuthHosts
        {
            get { return _ini.Read("AuthHosts", "admin"); }
            set { _ini.Write("AuthHosts", value, "admin"); }
        }

        /// <summary>
        /// Define on how to react on the (-,?,+) from the Authhosts. <para />
        /// 0="+:Accept, ?:Accept, -:Query" <para />
        /// 1="+:Accept, ?:Accept, -:Reject" <para />
        /// 2="+:Accept, ?:Query, -:Reject [Default]" <para />
        /// 3="+:Query, ?:Query, -:Reject" <para />
        /// 4="+:Query, ?:Reject, -:Reject" <para />
        /// It is used to specify a set of IP address templates which incoming connections must match in order to be 
        /// accepted.By default, the template is empty and connections from all AuthHosts_Tip5 = "hosts are accepted. 
        /// The template is of the form: <para />
        /// +[ip - address - template]<para />
        /// ?[ip - address - template]<para />
        /// -[ip - address - template]<para />
        ///  In the above, [ip-address-template] represents the leftmost bytes of the desired stringified IP-address.
        /// For example, +158.97 would match both 158.97.12.10 and 158.97.14.2. Multiple match terms may be specified, 
        /// delimited by the ":" character.Terms appearing later in the template take precedence over earlier ones.e.g. 
        /// -:+158.97: would filter out all incoming connections except those beginning with 158.97. Terms beginning 
        /// with the "?" character are treated by default as indicating hosts from whom connections must be accepted at
        ///  the server side via a dialog box. The QuerySetting option determines the precise behaviour of the three 
        /// AuthHosts options.
        /// </summary>
        public string QuerySetting
        {
            get { return _ini.Read("QuerySetting", "admin"); }
            set { _ini.Write("QuerySetting", value, "admin"); }
        }

        /// <summary>
        /// QueryTimeout is the time the messagebox is shown.
        /// </summary>
        public string QueryTimeout
        {
            get { return _ini.Read("QueryTimeout", "admin"); }
            set { _ini.Write("QueryTimeout", value, "admin"); }
        }

        /// <summary>
        /// 0=refuse 1=accept  2=refuse <para />
        /// This popup a timed messagebox to allow the user (server site) to allow/reject an incoming connect.
        /// </summary>
        public string QueryAccept
        {
            get { return _ini.Read("QueryTimeout", "admin"); }
            set { _ini.Write("QueryTimeout", value, "admin"); }
        }

        /// <summary>
        /// Disable/enable query settings when no user is logged.
        /// If the user is logged on, but has his screensaver on you normal can't get access as "QueryIfNoLogon" 
        /// find a logged user. to overwrite this set QueryAccept = 2 and QueryIfNoLogon = 0->no messagebox when 
        /// screen is locked.
        /// </summary>
        public string QueryIfNoLogon
        {
            get { return _ini.Read("QueryIfNoLogon", "admin"); }
            set { _ini.Write("QueryIfNoLogon", value, "admin"); }
        }

        /// <summary>
        /// 0="none" <para />
        /// 1="lock workstation on disconnect(NA)" <para />
        /// 2="logoff on disconnect
        /// </summary>
        public string LockSetting
        {
            get { return _ini.Read("LockSetting", "admin"); }
            set { _ini.Write("LockSetting", value, "admin"); }
        }

        /// <summary>
        /// Winvnc can use XX% cpu
        /// </summary>
        public string MaxCpu
        {
            get { return _ini.Read("MaxCpu", "admin"); }
            set { _ini.Write("MaxCpu", value, "admin"); }
        }

        /// <summary>
        /// A image as background takes more cpu and bigger bandwidth then a solid color. Disable on viewer connect, reenable on exit.
        /// </summary>
        public string RemoveWallpaper
        {
            get { return _ini.Read("RemoveWallpaper", "admin"); }
            set { _ini.Write("RemoveWallpaper", value, "admin"); }
        }

        /// <summary>
        /// Remove Aero on viewer connect and reset on exit.
        /// </summary>
        public string RemoveAero
        {
            get { return _ini.Read("RemoveAero", "admin"); }
            set { _ini.Write("RemoveAero", value, "admin"); }
        }

        /// <summary>
        /// Define the directory in which to save the winvnc.log file.
        /// Make sure this directory is writable by system ( no mapped folder)
        /// </summary>
        public string path
        {
            get { return _ini.Read("path", "admin"); }
            set { _ini.Write("path", value, "admin"); }
        }

        /// <summary>
        /// DebugLevel indicates how much debug information to present. Any positive integer is valid. 
        /// Zero indicates that no debugging information should be produced and is the default. A value 
        /// of around 10-12 will cause full debugging output to be produced
        /// </summary>
        public string DebugLevel
        {
            get { return _ini.Read("DebugLevel", "admin"); }
            set { _ini.Write("DebugLevel", value, "admin"); }
        }

        /// <summary>
        /// Run-time logging of all internal debug messages is now supported. Log data may be output to a 
        /// file or a console window or the MSVC debugger if the program was compiled with debugging active.)
        /// </summary>
        public string DebugMode
        {
            get { return _ini.Read("DebugMode", "admin"); }
            set { _ini.Write("DebugMode", value, "admin"); }
        }

        /// <summary>
        /// 0 = Disable connection from localhost (Default) <para />
        /// 1 = Enable connection from localhost <para />
        /// By default, WinVNC servers disallow any vnc viewer connections from the same machine. For testing 
        /// purposes, or, potentially, when using multiple instances of WinVNC on Windows Terminal Server, this 
        /// behaviour is undesirable.
        /// </summary>
        public string AllowLoopback
        {
            get { return _ini.Read("AllowLoopback", "admin"); }
            set { _ini.Write("AllowLoopback", value, "admin"); }
        }

        /// <summary>
        /// By default, WinVNC servers accept incoming connections on an network adapter address, since this is 
        /// the easiest way of coping with multihomed machines. In some cases, it is preferable to only for 
        /// connections originating from the local machine and aimed at the "localhost" adapter - a particular 
        /// example is the use of VNC over SSH to provide secure VNC. Setting this will cause WinVNC to only 
        /// accept local connections - this overrides the AllowLoopback and AuthHosts settings.
        /// </summary>
        public string LoopbackOnly
        {
            get { return _ini.Read("LoopbackOnly", "admin"); }
            set { _ini.Write("LoopbackOnly", value, "admin"); }
        }

        /// <summary>
        /// Allows Shutdown tray menu option to be visible (1) or not (0)
        /// </summary>
        public string AllowShutdown
        {
            get { return _ini.Read("AllowShutdown", "admin"); }
            set { _ini.Write("AllowShutdown", value, "admin"); }
        }

        /// <summary>
        /// 0 = Disable "Properties" option in uvnc server tray menu<para />
        /// 1 = Enable "Properties" option in uvnc server tray menu
        /// </summary>
        public string AllowProperties
        {
            get { return _ini.Read("AllowProperties", "admin"); }
            set { _ini.Write("AllowProperties", value, "admin"); }
        }

        /// <summary>
        /// 0 = Disable "Edit Clients" options in uvnc server tray menu<para />
        /// 1 = Enable "Edit Clients" options in uvnc server tray menu
        /// </summary>
        public string AllowEditClients
        {
            get { return _ini.Read("AllowEditClients", "admin"); }
            set { _ini.Write("AllowEditClients", value, "admin"); }
        }

        /// <summary>
        /// Timings for Filetransfer and keepalive message (seconds)
        /// </summary>
        public string KeepAliveInterval
        {
            get { return _ini.Read("KeepAliveInterval", "admin"); }
            set { _ini.Write("KeepAliveInterval", value, "admin"); }
        }

        /// <summary>
        /// Don't show the winvnc tray icon. Without the tray icon you can't make realtime changes. You need
        /// to edit the ultravnc.ini manual or use the uvnc_settings.exe to modify the file. Settings take 
        /// efect after winvnc restart.
        /// </summary>
        public string DisableTrayIcon
        {
            get { return _ini.Read("DisableTrayIcon", "admin"); }
            set { _ini.Write("DisableTrayIcon", value, "admin"); }
        }

        /// <summary>
        /// Use MS password instead of the vncpasswd
        /// </summary>
        public string MSLogonRequired
        {
            get { return _ini.Read("MSLogonRequired", "admin"); }
            set { _ini.Write("MSLogonRequired", value, "admin"); }
        }

        /// <summary>
        /// Use ACL instead of a group list
        /// </summary>
        public string NewMSLogon
        {
            get { return _ini.Read("NewMSLogon", "admin"); }
            set { _ini.Write("NewMSLogon", value, "admin"); }
        }

        /// <summary>
        /// 0 = use ultravnc.ini <para />
        /// 1 = use registry the same way as in v102
        /// </summary>
        public string UseRegistry
        {
            get { return _ini.Read("UseRegistry", "admin"); }
            set { _ini.Write("UseRegistry", value, "admin"); }
        }

        /// <summary>
        /// ConnectPriority indicates what WinVNC should do when a" non-shared connection is received By 
        /// default, all WinVNC servers will disconnect any existing connections when an incoming, 
        /// non-shared connection is authenticated.This behaviour is undesirable when the server machine 
        /// is being used as a shared workstation by several users or when remoting a single display to 
        /// multiple clients for viewing, as in a classroom situation.
        /// </summary>
        public string ConnectPriority
        {
            get { return _ini.Read("ConnectPriority", "admin"); }
            set { _ini.Write("ConnectPriority", value, "admin"); }
        }


        /// <summary>
        /// By default, all WinVNC servers will not accept incoming connections unless the server has had 
        /// its password field set to a non-null value. This restriction was placed to ensure that misconfigured 
        /// servers would not open security loopholes without the user realising. If a server is only to be used 
        /// on a secure LAN, however, it may be desirable to forego such checking and allow machines to have a 
        /// null password.
        /// </summary>
        public string AuthRequired
        {
            get { return _ini.Read("AuthRequired", "admin"); }
            set { _ini.Write("AuthRequired", value, "admin"); }
        }

        /// <summary>
        /// variable available starting uvnc 1.0.8.0 <para />
        /// sendbuffer=1500 (wifi or value less) <para />
        /// sendbuffer=4096 (lan 100Mbit) <para />
        /// sendbuffer=8192 (lan 1GBit, aka jumbo packet)
        /// </summary>
        public string sendbuffer
        {
            get { return _ini.Read("sendbuffer", "admin"); }
            set { _ini.Write("sendbuffer", value, "admin"); }
        }

        /// <summary>
        /// This solves a bug for fast user switching. <para />
        /// Logon user A <para />
        /// switch user B <para />
        /// Logoff user B <para />
        /// This cause logon screen to be unreachable ( MS bug), Settings kickrdp =1 solve it. <para />
        /// But kickrdp=1, kick RDP sessions. <para />
        /// Use it with Fast User Switching, don't use it with RDP
        /// </summary>
        public string kickrdp
        {
            get { return _ini.Read("kickrdp", "admin"); }
            set { _ini.Write("kickrdp", value, "admin"); }
        }

        /// <summary>
        /// This solves a winstationconnect bug
        /// </summary>
        public string clearconsole
        {
            get { return _ini.Read("clearconsole", "admin"); }
            set { _ini.Write("clearconsole", value, "admin"); }
        }

        /// <summary>
        /// It allow to connect to console and rdp sessions
        /// </summary>
        public string rdpmode
        {
            get { return _ini.Read("rdpmode ", "admin"); }
            set { _ini.Write("rdpmode ", value, "admin"); }
        }

        public string passwd
        {
            get
            {
                var pass = _ini.Read("passwd ", "ultravnc");
                return Password.DecryptPassword(pass);
            }
            set
            {
                _ini.Write("passwd ", Password.EncryptPassword(value), "ultravnc");
            }
        }

        public string passwd2
        {
            get
            {
                var pass = _ini.Read("passwd2 ", "ultravnc");
                return Password.DecryptPassword(pass);
            }
            set
            {
                _ini.Write("passwd2 ", Password.EncryptPassword(value), "ultravnc");
            }
        }

        public string FileTransferTimeout
        {
            get { return _ini.Read("FileTransferTimeout", "admin"); }
            set { _ini.Write("FileTransferTimeout", value, "admin"); }
        }

        public string RemoveEffects
        {
            get { return _ini.Read("RemoveEffects", "admin"); }
            set { _ini.Write("RemoveEffects", value, "admin"); }
        }
        public string RemoveFontSmoothing
        {
            get { return _ini.Read("RemoveFontSmoothing", "admin"); }
            set { _ini.Write("RemoveFontSmoothing", value, "admin"); }
        }

        private readonly string[] _defaultConfigStrings =
        {
            "[ultravnc]",
            "passwd=",
            "passwd2=",
            "[permissions]",
            "[admin]",
            "UseRegistry=0",
            "MSLogonRequired=0",
            "NewMSLogon=0",
            "DebugMode=0",
            "DebugLevel=0",
            "kickrdp=1",
            "DisableTrayIcon=1",
            "LoopbackOnly=0",
            "AllowLoopback=0",
            "AuthRequired=1",
            "ConnectPriority=0",
            "UseDSMPlugin=0",
            "DSMPlugin=",
            "AuthHosts=",
            "AllowShutdown=1",
            "AllowProperties=0",
            "AllowEditClients=0",
            "FileTransferEnabled=1",
            "FTUserImpersonation=1",
            "DefaultScale=1",
            "AutoPortSelect=1",
            "HTTPConnect=1",
            "HTTPPortNumber=5800",
            "SocketConnect=1",
            "PortNumber=5900",
            "IdleTimeout=0",
            "IdleInputTimeout=0",
            "QuerySetting=2",
            "QueryTimeout=10",
            "QueryAccept=0",
            "QueryIfNoLogon=0",
            "primary=1",
            "secondary=1",
            "InputsEnabled=1",
            "LockSetting=0",
            "MaxCpu=40",
            "LocalInputsDisabled=0",
            "EnableJapInput=1",
            "FileTransferTimeout=30",
            "clearconsole=1",
            "accept_reject_mesg=",
            "KeepAliveInterval=5",
            "RemoveEffects=0",
            "RemoveFontSmoothing=0",
            "RemoveWallpaper=0",
            "RemoveAero=0",
            "rdpmode=0",
            "path=",
        };

        public string DefaultConfigText
        {
            get
            {
                string s = "";
                for (int i = 0; i < _defaultConfigStrings.Length; i++)
                {
                    s += _defaultConfigStrings[i] + Environment.NewLine;
                }
                return s;
            }
        }

        public Config(string path)
        {
            if (!File.Exists(path))
                File.WriteAllText(path, DefaultConfigText);

            _ini = new IniFile(path);
        }

        public void EnableEffects(bool enable)
        {
            var s = "1";
            if (!enable) s = "0";
            RemoveWallpaper = RemoveEffects = RemoveFontSmoothing = RemoveAero = s;
        }
    }
}