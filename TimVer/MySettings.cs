using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;

namespace TimVer
{
    public class MySettings
    {
        /////////////////////////////  Properties //////////////////////////////

        public double WindowLeft { get; set; }

        public double WindowTop { get; set; }

        public double Zoom { get; set; }

        /////////////////////////////  Methods //////////////////////////////

        #region Read settings
        /// <summary>
        /// Reads settings from the specified JSON file
        /// </summary>
        /// <param name="filename">If filename parameter is empty, the default settings.json will be used</param>
        /// <returns>MySettings object</returns>
        public static MySettings Read(string filename = "")
        {
            if (string.IsNullOrEmpty(filename))
            {
                filename = DefaultSettingsFile();
            }
            if (!File.Exists(filename))
            {
                CreateNewSettingsJson(filename);
            }
            string rawJSON = File.ReadAllText(filename);
            return JsonConvert.DeserializeObject<MySettings>(rawJSON);
        }
        #endregion Read settings

        #region Save settings
        /// <summary>
        /// Saves settings to the specified JSON file
        /// </summary>
        /// <param name="s">name of object containing settings</param>
        /// <param name="filename">If filename parameter is empty, the default settings.json will be used</param>
        public static void Save(MySettings s, string filename = "")
        {
            if (string.IsNullOrEmpty(filename))
            {
                filename = DefaultSettingsFile();
            }
            string jsonOut = JsonConvert.SerializeObject(s, Formatting.Indented);
            File.WriteAllText(filename, jsonOut);
        }
        #endregion Save settings

        #region List settings
        /// <summary>
        /// Lists current settings
        /// </summary>
        /// <param name="s">name of object containing settings</param>
        /// <returns>List of strings </returns>
        public static List<string> List(MySettings s)
        {
            List<string> list = new List<string>();

            foreach (PropertyInfo prop in s.GetType().GetProperties())
            {
                list.Add($"{prop.Name} = {prop.GetValue(s)}");
            }
            return list;
        }
        #endregion List settings

        #region Create settings file
        /// <summary>
        /// Creates a new settings file
        /// </summary>
        /// <param name="filename">The full path for the settings file</param>
        private static void CreateNewSettingsJson(string filename)
        {
            File.Create(filename).Dispose();
            var ms = new MySettings
            {
                Zoom = 1,
                WindowTop = 200,
                WindowLeft = 200
            };
            string jsonOut = JsonConvert.SerializeObject(ms, Formatting.Indented);
            File.WriteAllText(filename, jsonOut);
        }
        #endregion Create settings file

        #region Default settings file name
        private static string DefaultSettingsFile()
        {
            string dir = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            return Path.Combine(dir, "settings.json");
        }
        #endregion Default settings file name
    }
}
