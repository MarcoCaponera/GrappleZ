using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GrappleZ_Utility;

namespace GrappleZ_Gameplay
{
    public class ScoreSystem : MonoBehaviour
    {
        #region SerializeFields

        [SerializeField]
        private float maxTime;

        #endregion

        #region PrivateAttributes

        private List<float> scores = new List<float>();
        private float currentScore;
        private float currentTime;

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

        private float CalculateScore()
        {
            float multiplier = maxTime - currentTime;
            return currentScore * (int)multiplier;
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
            CalculateFinalTime();
            scores.Add(CalculateScore());
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
