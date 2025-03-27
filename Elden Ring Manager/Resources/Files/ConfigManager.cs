using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elden_Ring_Manager.Resources.Files
{
    internal class ConfigManager
    {
        private static string configFile = "config.txt";
        private static string configAC = "configg.txt";

        public static void SavePaths(string path1, string path2, string sessionPass, bool allowINV)
        {
            File.WriteAllLines(configFile, new string[]
            {
                $"path1={path1}",
                $"path2={path2}",
                $"pass={sessionPass}",
                $"allowINV={(allowINV ? "1" : "0")}"
            });
        }

        public static void SaveActiveCode(string activation)
        {
            File.WriteAllLines(configAC, new string[]
            {
                $"ac={(activation ?? "null")}"
            });
        }
        public static string LoadActivation()
        {
            if (File.Exists(configAC))
            {
                string[] lines = File.ReadAllLines(configAC);
                string activation = "";

                foreach (string line in lines)
                {
                    if (line.StartsWith("ac="))
                        activation = line.Substring(3).Trim();
                }
                return (activation);
                
            }
            return ("");
        }

        public static (string, string, string, bool) LoadPaths()
        {
            if (File.Exists(configFile))
            {
                string[] lines = File.ReadAllLines(configFile);
                string path1 = "", path2 = "", sessionPass = "";
                bool allowINV = false;

                foreach (string line in lines)
                {
                    if (line.StartsWith("path1="))
                        path1 = line.Substring(6).Trim();
                    else if (line.StartsWith("path2="))
                        path2 = line.Substring(6).Trim();
                    else if (line.StartsWith("pass="))
                        sessionPass = line.Substring(5).Trim();
                    else if (line.StartsWith("allowINV="))
                        allowINV = line.Substring(9).Trim() == "1";
                }

                return (path1, path2, sessionPass, allowINV);
            }
            return ("", "", "", false);
        }

    }
}
