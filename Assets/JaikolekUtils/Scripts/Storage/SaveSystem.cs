using System.IO;
using UnityEngine;

namespace JaikolekUtils.Storage
{
    public static class SaveSystem
    {
        private const bool useEncryption = false;
        private static readonly string FOLDER_PATH = Application.persistentDataPath + "/Saves";

        public static void Save(string content, string path, string encryptionKey)
        {
            CheckFolder();
            File.WriteAllText(FOLDER_PATH + path, useEncryption ? Encryption.Encrypt(content, encryptionKey) : content);
        }

        public static string Load(string path, string encryptionKey)
        {
            if (IsDataFound(FOLDER_PATH + path))
            {
                return useEncryption ? Encryption.Decrypt(File.ReadAllText(FOLDER_PATH + path), encryptionKey) : File.ReadAllText(FOLDER_PATH + path);
            }
            else
            {
                return null;
            }
        }

        public static bool IsDataFound(string path)
        {
            return File.Exists(path);
        }

        private static void CheckFolder()
        {
            if (!IsDataFound(FOLDER_PATH))
            {
                Directory.CreateDirectory(FOLDER_PATH);
            }
        }
    }
}
