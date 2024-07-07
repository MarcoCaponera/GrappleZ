
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace GrappleZ_SaveSystem
{
    public static class SaveSystem
    {
        private static SettingsData settingsData;
        private static GameSaveData gameData;

        public static GameSaveData GameData
        {
            get { return gameData; }
        }

        public static SettingsData SettingsData
        {
            get { return settingsData; }
        }

        static SaveSystem()
        {
            if (!Directory.Exists(SaveSystemConfiguration.SettingsFolderPath))
            {
                Directory.CreateDirectory(SaveSystemConfiguration.SettingsFolderPath);
            }
            if (!Directory.Exists(SaveSystemConfiguration.GameDataFolderPath))
            {
                Directory.CreateDirectory(SaveSystemConfiguration.GameDataFolderPath);
            }

            if (!SettingsDataExists())
            {
                CreateSettingsData();
            }
            else
            {
                settingsData = LoadISavableData<SettingsData>(SaveSystemConfiguration.SettingsFilePath);
            }

            if (!GameDataExists())
            {
                CreateGameData();
            }
            else
            {
                gameData = LoadISavableData<GameSaveData>(SaveSystemConfiguration.GameDataFilePath);
            }
        }

        #region SettingsData
        public static void SaveSettingsData()
        {
            SaveISavebleData<SettingsData>(SaveSystemConfiguration.SettingsFilePath, settingsData);
        }

        public static void CreateSettingsData()
        {
            settingsData = CreateISavebleData<SettingsData>();
            SaveSettingsData();
        }

        public static void DeleteSettingsData()
        {
            DeleteISavebleData<SettingsData>(SaveSystemConfiguration.SettingsFilePath, settingsData);
        }

        public static bool SettingsDataExists()
        {
            return File.Exists(SaveSystemConfiguration.SettingsFilePath);
        }
        #endregion

        #region GameData
        public static void CreateGameData()
        {
            gameData = CreateISavebleData<GameSaveData>();
            SaveGameData();
        }

        public static void SaveGameData()
        {
            SaveISavebleData<GameSaveData>(SaveSystemConfiguration.GameDataFilePath, gameData);
        }

        public static void DeleteGameData()
        {
            DeleteISavebleData<GameSaveData>(SaveSystemConfiguration.GameDataFilePath, gameData);
        }

        public static bool GameDataExists()
        {
            return File.Exists(SaveSystemConfiguration.GameDataFilePath);
        }
        #endregion



        #region WrapperSavebleData
        private static T CreateISavebleData<T>() where T : ISavebleDataClass, new()
        {
            T data = new T();
            data.OnCreation();
            return data;
        }

        private static void DeleteISavebleData<T>(string path, ISavebleDataClass data) where T : ISavebleDataClass
        {
            data.OnDelete();
            File.Delete(path);
        }

        private static T LoadISavableData<T>(string path) where T : ISavebleDataClass
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(path, FileMode.OpenOrCreate);
            T data = (T)bf.Deserialize(file);
            file.Close();
            if (!data.CheckVersion())
            {
                data.HandleVersionChange();
            }
            data.OnLoadedFromDisk();
            return data;
        }

        private static void SaveISavebleData<T>(string path, ISavebleDataClass dataToSave) where T : ISavebleDataClass
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(path, FileMode.OpenOrCreate);
            dataToSave.OnPreSave();
            bf.Serialize(file, dataToSave.InstanceToSave);
            dataToSave.OnPostSave();
            file.Close();
        }
        #endregion
    }
}