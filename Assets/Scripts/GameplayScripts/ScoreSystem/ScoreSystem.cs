using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GrappleZ_Utility;
using System.Linq;
using GrappleZ_UI;
using GrappleZ_SaveSystem;

namespace GrappleZ_Gameplay
{

    public class ScoreSystem : MonoBehaviour
    {
        #region SerializeFields

        [SerializeField]
        private float maxTime;
        [SerializeField]
        private EndWaveMenu endWaveUI;

        #endregion

        public LeaderboardSavedData LeaderboardData
        {
            get { return SaveSystem.GameData.LeaderboardData; }
        }

        #region PrivateAttributes

        private float currentScore;
        private float currentTime;
        private float pauseStartTime;
        private float totalPausedTime;
        private WaveEnum currentWave;

        #endregion

        #region InternalMethods

        private void CalculateFinalTime()
        {
            Debug.Log("Partial : " + ( Time.realtimeSinceStartup - currentTime));
            currentTime = (Time.realtimeSinceStartup - currentTime) - totalPausedTime;
            Debug.Log("Paused : " + totalPausedTime);
            Debug.Log("Total : " +currentTime);
            if (currentTime > maxTime)
            {
                currentTime = maxTime;
            }

        }

        private ScoreStruct CalculateScore()
        {
            CalculateFinalTime();
            float multiplier = maxTime - currentTime;
            return new ScoreStruct() { Score = currentScore * (int)multiplier, Time = currentTime };
        }

        private void ResetParams()
        {
            currentScore = 0;
            currentTime = 0;
            totalPausedTime = 0;
        }

        #endregion

        #region Mono

        private void Start()
        {
            GlobalEventManager.AddListener(GlobalEventIndex.WaveStarted, OnWaveStarted);
            GlobalEventManager.AddListener(GlobalEventIndex.WaveEnded, OnWaveEnded);
            GlobalEventManager.AddListener(GlobalEventIndex.ScoreIncreased, OnScoreIncrease);
            GlobalEventManager.AddListener(GlobalEventIndex.GamePaused, OnGamePaused);
            GlobalEventManager.AddListener(GlobalEventIndex.GameResumed, OnGameResumed);
        }

        #endregion

        #region Callbacks

        protected void OnWaveStarted(GlobalEventArgs message)
        {
            ResetParams();
            currentTime = Time.realtimeSinceStartup;
        }

        protected void OnWaveEnded(GlobalEventArgs message)
        {
            GlobalEventArgsFactory.WaveEndedParser(message, out currentWave);
            LeaderboardData.AddScore(currentWave, CalculateScore());
            endWaveUI.ComputeLeaderbard(LeaderboardData.GetWaveLeaderBoard(currentWave));
            SaveSystem.SaveGameData();
        }

        protected void OnScoreIncrease(GlobalEventArgs message)
        {
            GlobalEventArgsFactory.ScoreIncreaseParser(message, out float score);
            currentScore += score;
            Debug.Log(currentScore);
        }


        protected void OnGamePaused(GlobalEventArgs message)
        {
            pauseStartTime = Time.realtimeSinceStartup;
            Debug.Log(totalPausedTime);
        }
        protected void OnGameResumed(GlobalEventArgs message)
        {
            totalPausedTime += (Time.realtimeSinceStartup - pauseStartTime);
            pauseStartTime = 0;
            Debug.Log(totalPausedTime);
        }
        #endregion
    }
}
