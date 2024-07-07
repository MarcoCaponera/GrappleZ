using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GrappleZ_SaveSystem
{
    [Serializable]
    public class SettingsData : ISavebleDataClass
    {



        #region DataToSave
        #endregion


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
            return CurrentFileVersion == SavedFileVersion;
        }

        public void HandleVersionChange()
        {
            savedFileVersion = CurrentFileVersion;
        }

        public void OnCreation()
        {
        }

        public void OnDelete()
        {
            //cancellato
        }

        public void OnLoadedFromDisk()
        {
            //caricato
        }

        public void OnPostSave()
        {
            //tutto quello che dovete fare DOPO che il file � stato salvato
        }

        public void OnPreSave()
        {
            //tutto quello che dovete fare PRIMA che il file venga salvato
        }
    }
}