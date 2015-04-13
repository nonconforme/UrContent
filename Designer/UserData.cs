﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Designer {
    [Serializable]
    public class UserData {
        static UserData inst_;

        public static UserData inst() {
            if (inst_ == null) {
                inst_ = LoadUserData();
            } return inst_;
        }

        public static string GetAppDataPath(string aPath) {
            string asmName = Assembly.GetExecutingAssembly().GetName().Name;
            string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), string.Format("{0}\\{1}", asmName, aPath));
            if (!File.Exists(fileName)) {
                Directory.CreateDirectory(Path.GetDirectoryName(fileName));
            }
            return fileName;
        }

        UserData() {
            RecentFiles = new List<string>();
        }

        public void AddRecentFile(string file) {
            if (RecentFiles.Contains(file))
                RecentFiles.Remove(file);
            RecentFiles.Add(file);
            Save();
        }

        public List<string> RecentFiles { get; set; }

        public void Save() {
            string fileName = GetAppDataPath("UserData.xml");
            try {
                System.Xml.Serialization.XmlSerializer writer = new System.Xml.Serialization.XmlSerializer(typeof(UserData));

                using (System.IO.FileStream file = new System.IO.FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None)) {
                    writer.Serialize(file, this);
                    file.Flush();
                    file.Close();
                }
            }
            catch (Exception ex) {
                ErrorHandler.inst().Error(ex);
            }
        }

        static UserData LoadUserData() {
            string fileName = GetAppDataPath("UserData.xml");
            try {
                if (File.Exists(fileName)) {
                    System.Xml.Serialization.XmlSerializer reader = new System.Xml.Serialization.XmlSerializer(typeof(UserData));

                    UserData ud = new UserData();
                    using (System.IO.FileStream file = new System.IO.FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read)) {
                        ud = (UserData)reader.Deserialize(file);
                        file.Close();
                    }

                    return ud;
                }
                return new UserData();
            }
            catch (Exception ex) {
                ErrorHandler.inst().Error(ex);
                return new UserData();
            } finally { }
        }
    }
}
