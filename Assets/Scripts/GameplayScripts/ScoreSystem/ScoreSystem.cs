using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GrappleZ_Utility;
using System.Linq;
using GrappleZ_UI;

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
            return new ScoreStruct() { Score = currentScore * (int)multiplier, Time = currentTime };
        }

        private void ResetParams()
        {
            currentScore = 0;
            currentTime = 0;
        }

        #endregion
        #region publicMethods
        public List<ScoreStruct> GetLeaderbordForWave(WaveEnum wave)
        {
            return leaderBoard[wave].OrderByDescending(x => x.Score).ToList();
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
            currentTime = Time.realtimeSinceStartup;
        }

        protected void OnWaveEnded(GlobalEventArgs message)
        {
            GlobalEventArgsFactory.WaveEndedParser(message, out currentWave);
            leaderBoard[currentWave].Add(CalculateScore());
            endWaveUI.ComputeLeaderbard(
                    leaderBoard[currentWave].OrderByDescending(s => s.Score).ToList()
                );
        }

        protected void OnScoreIncrease(GlobalEventArgs message)
        {
            GlobalEventArgsFactory.ScoreIncreaseParser(message, out float score);
            currentScore += score;
            Debug.Log(currentScore);
        }

        #endregion
    }
}
