using System;
using System.Collections.Generic;
using UnityEngine;


namespace GrappleZ_SaveSystem
{
    [Serializable]
    public class GameSaveData : ISavebleDataClass
    {
        private List<ISavebleDataClass> savebleDatas;

        #region WrapData
        public LeaderboardSavedData LeaderboardData
        {
            get { return (LeaderboardSavedData)savebleDatas[0]; }
        }
        #endregion

        #region Interfaces
        public float CurrentFileVersion
        {
            get { return 1.0f; }
        }

        private float savedFileVersion;
        public float SavedFileVersion
        {
            get { return savedFileVersion; }
        }

        public object InstanceToSave
        {
            get { return this; }
        }

        public bool CheckVersion()
        {
            return savedFileVersion == CurrentFileVersion;
        }

        public void HandleVersionChange()
        {
            /*
             * handle change version
             */

            savedFileVersion = CurrentFileVersion;
        }

        public void OnCreation()
        {
            savebleDatas = new List<ISavebleDataClass>();
            savebleDatas.Add(new LeaderboardSavedData());
            /*
             * add data tu save
             */
            foreach (ISavebleDataClass data in savebleDatas)
            {
                data.OnCreation();
            }
        }

        public void OnDelete()
        {
            foreach (ISavebleDataClass data in savebleDatas)
            {
                data.OnDelete();
            }
        }

        public void OnLoadedFromDisk()
        {
            foreach(ISavebleDataClass data in savebleDatas)
            {
                data.OnLoadedFromDisk();
            }
        }

        public void OnPostSave()
        {
            foreach (ISavebleDataClass data in savebleDatas)
            {
                data.OnPostSave();
            }
        }

        public void OnPreSave()
        {
            foreach (ISavebleDataClass data in savebleDatas)
            {
                data.OnPreSave();
            }
        }

        #endregion
    }
}
