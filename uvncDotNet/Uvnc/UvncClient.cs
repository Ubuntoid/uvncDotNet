namespace uvncDotNet.Uvnc
{
    public class UvncClient
    {
        public string Host { get; set; }
        public int Port { get; set; } = 5900;
        public int ProxyID { get; set; } = 0;
        public int Display { get; set; } = 0;
        public bool ViewOnly { get; set; } = false;
        public bool Scaled { get; set; } = false;
    }
}