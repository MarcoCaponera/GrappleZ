using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GrappleZ_Utility;
using System.Linq;

namespace GrappleZ_Gameplay
{
    public struct ScoreStruct
    {
        public float Score;
        public float Time;
    }

    public class ScoreSystem : MonoBehaviour
    {
        #region SerializeFields

        [SerializeField]
        private float maxTime;

        #endregion

        #region PrivateAttributes

        private List<float> scores = new List<float>();
        private Dictionary<WaveEnum, List<ScoreStruct>> leaderBoard;

        private float currentScore;
        private float currentTime;
        private WaveEnum currentWave;

        #endregion

        #region InternalMethods

        private void CalculateFinalTime()
        {
            currentTime = Time.realtimeSinceStartup - currentTime;
            if (currentTime > maxTime)
            {
                currentTime = maxTime;
            }
        }

        private ScoreStruct CalculateScore()
        {
            CalculateFinalTime();
            float multiplier = maxTime - currentTime;
            return new ScoreStruct() {Score = currentScore * (int)multiplier , Time = currentTime };
        }

        private void ResetParams()
        {
            currentScore = 0;
            currentTime = 0;
        }

        #endregion

        #region Mono

        private void Start()
        {
            GlobalEventManager.AddListener(GlobalEventIndex.WaveStarted, OnWaveStarted);
            GlobalEventManager.AddListener(GlobalEventIndex.WaveEnded, OnWaveEnded);
            GlobalEventManager.AddListener(GlobalEventIndex.ScoreIncreased, OnScoreIncrease);
            leaderBoard = new Dictionary<WaveEnum, List<ScoreStruct>>();
            for (int i = 0; i < (int)WaveEnum.LAST; i++)
            {
                leaderBoard.Add((WaveEnum)i, new List<ScoreStruct>());
            }
        }

        #endregion

        #region Callbacks

        protected void OnWaveStarted(GlobalEventArgs message)
        {
            ResetParams();
            GlobalEventArgsFactory.WaveStartedParser(message,out currentWave);
            currentTime = Time.realtimeSinceStartup;
        }

        protected void OnWaveEnded(GlobalEventArgs message) 
        {
            leaderBoard[currentWave].Add(CalculateScore());
            leaderBoard[currentWave].OrderByDescending(s => s.Score);
        }

        protected void OnScoreIncrease(GlobalEventArgs message)
        {
            GlobalEventArgsFactory.ScoreIncreaseParser(message, out float score);
            Debug.Log(currentScore);
            currentScore += score;
        }

        #endregion
    }
}
