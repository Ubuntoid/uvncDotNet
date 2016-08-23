using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace uvncDotNet.Helper
{
    public class IniFile
    {
        readonly string _path;

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern long WritePrivateProfileString(string section, string key, string value, string filePath);

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern int GetPrivateProfileString(string section, string key, string Default, StringBuilder retVal, int size, string filePath);

        public IniFile(string iniPath)
        {
            _path = new FileInfo(iniPath).FullName;
        }

        public string Read(string key, string section = null)
        {
            var retVal = new StringBuilder(255);
            GetPrivateProfileString(section, key, "", retVal, 255, _path);
            return retVal.ToString();
        }

        public void Write(string key, object value, string section = null)
        {
            WritePrivateProfileString(section, key, value.ToString(), _path);
        }

        public void DeleteKey(string key, string section = null)
        {
            Write(key, null, section);
        }

        public void DeleteSection(string section = null)
        {
            Write(null, null, section);
        }

        public bool KeyExists(string key, string section = null)
        {
            return Read(key, section).Length > 0;
        }
    }
}