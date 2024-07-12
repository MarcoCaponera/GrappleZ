using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GrappleZ_SaveSystem
{
    public static class SaveSystemConfiguration
    {
        private const string settingsPath = "/Settings/";
        private const string gamedDataPath = "/Data/";

        private const string settingsFileName = "Settings.file";
        private const string gameDataFileName = "GameData.file";
        public static string RootsPath
        {
            get { return Application.persistentDataPath; }
        }

        public static string SettingsFolderPath
        {
            get { return RootsPath + settingsPath; }
        }

        public static string SettingsFilePath
        {
            get { return SettingsFolderPath + settingsFileName; }
        }

        public static string GameDataFolderPath
        {
            get { return RootsPath + gamedDataPath; }
        }

        public static string GameDataFilePath
        {
            get { return GameDataFolderPath + gameDataFileName; }
        }
    }
}