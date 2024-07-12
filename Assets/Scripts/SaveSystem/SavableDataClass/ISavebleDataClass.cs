using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GrappleZ_SaveSystem
{
    public interface ISavebleDataClass
    {
        float CurrentFileVersion
        {
            get;
        }
        float SavedFileVersion
        {
            get;
        }

        object InstanceToSave
        {
            get;
        }

        bool CheckVersion();
        void HandleVersionChange();


        void OnCreation();
        void OnLoadedFromDisk();
        void OnPreSave();
        void OnPostSave();
        void OnDelete();
    }
}