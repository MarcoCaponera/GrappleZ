using GrappleZ_Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GrappleZ_SaveSystem
{
    [Serializable]
    public class LeaderboardSavedData : ISavebleDataClass
    {
        private Dictionary<WaveEnum, List<ScoreStruct>> leaderBoard;

        public Dictionary<WaveEnum, List<ScoreStruct>> Leaderboard { get { return leaderBoard; } }

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
            leaderBoard = new Dictionary<WaveEnum, List<ScoreStruct>>();
            for (int i = 0; i < (int)WaveEnum.LAST; i++)
            {
                leaderBoard.Add((WaveEnum)i, new List<ScoreStruct>());
            }
        }

        public void OnDelete()
        {

        }

        public void OnLoadedFromDisk()
        {

        }

        public void OnPostSave()
        {

        }

        public void OnPreSave()
        {
        }

        #endregion

        #region Public methods
        public List<ScoreStruct> GetWaveLeaderBoard(WaveEnum currentWave)
        {
            return leaderBoard[currentWave];
        }

        public void AddScore(WaveEnum currentWave,ScoreStruct currentScore)
        {
            leaderBoard[currentWave].Add(currentScore);
            leaderBoard[currentWave] = leaderBoard[currentWave].OrderByDescending(x => x.Score).ToList();
        }
        #endregion
    }
}
