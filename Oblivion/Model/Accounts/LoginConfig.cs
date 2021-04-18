using Newtonsoft.Json;
using Oblivion.Pattern;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oblivion.Model.Accounts
{
    public class LoginConfig : Singleton<LoginConfig>
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string PathDofus { get; set; }

        public LoginConfig()
        {

        }

        public LoginConfig(string username, string password, string pathDofus)
        {
            Username = username;
            Password = password;
            PathDofus = pathDofus;
        }

        /// <summary>
        /// Load account and path
        /// </summary>
        public void Load()
        {
            var configPath = AppDomain.CurrentDomain.BaseDirectory + "config.json";
            if (File.Exists(configPath))
            {
                using (StreamReader reader = new StreamReader(configPath))
                {
                    string result = reader.ReadToEnd();
                    LoginConfig config = JsonConvert.DeserializeObject<LoginConfig>(result);
                    if (config != null)
                    {
                        Username = config.Username;
                        Password = config.Password;
                        PathDofus = config.PathDofus;
                    }
                    else
                    {
                        Username = "";
                        Password = "";
                        PathDofus = "";
                    }
                    reader.Close();
                }
            }
            else
            {
                File.Create(configPath);
            }
        }

        /// <summary>
        /// Save data in file
        /// </summary>
        public void UpdateData()
        {
            var configPath = AppDomain.CurrentDomain.BaseDirectory + "config.json";
            string data = JsonConvert.SerializeObject(this);
            using (StreamWriter writer = new StreamWriter(configPath))
            {
                writer.Write(data);
                writer.Close();
            }
        }
    }
}
