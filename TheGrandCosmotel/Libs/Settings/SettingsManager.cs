using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace WebGames.Libs
{   
    public class SettingsConfig
    {
        public SecurityModel security { get; set; }
    }

    public class SecurityModel
    {
        public List<PersistentUser> persistentUsers { get; set; }
        public SecurityAPIs api { get; set; }
    }

    public class SecurityAPIs
    {
        public string SENDGRID_KEY { get; set; }
        public string Sendgrid_User_Name { get; set; }
        public string Sendgrid_Password { get; set; }
    }

    public class PersistentUser
    {
        public string username { get; set; }
        public string password { get; set; }
        public string email { get; set; }
        public List<string> roles { get; set; }
    }

    public class SettingsManager
    {
        static SettingsConfig config = null;

        public static SettingsConfig GetConfig()
        {
            if (config != null) return config;

            try
            {
                string path = System.IO.Path.GetDirectoryName((new System.Uri(Assembly.GetExecutingAssembly().CodeBase)).AbsolutePath);

                using (var conf = System.IO.File.OpenRead(System.IO.Path.Combine(path, "settings.json")))
                {
                    using (var rdr = new System.IO.StreamReader(conf))
                    {
                        string readText = rdr.ReadToEnd();
                        config = Newtonsoft.Json.JsonConvert.DeserializeObject<SettingsConfig>(readText);
                    }
                }
            }
            catch (Exception exc)
            {
                Logger.Log(exc.Message, LogType.CRITICAL);

                // stub config
                config = new SettingsConfig()
                {
                    security = new SecurityModel()
                    {
                        persistentUsers = new List<PersistentUser>(),
                        api = new SecurityAPIs()
                        {
                            SENDGRID_KEY = ""
                        }
                    }
                };
            }

            return config;
        }
    }
}