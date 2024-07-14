using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace GrappleZ_UI
{
    public class UI_Score : MonoBehaviour
    {
        private Label scoreLabel;
        private float CurrentScore;
        private const string defaultScore = "0000";

        private string scoreText
        {
            get
            {
                string text = CurrentScore.ToString();
                text = text.PadLeft(defaultScore.Length , '0');
                return text;
            }
        }

        private void Awake()
        {
            scoreLabel = GetComponent<UIDocument>().rootVisualElement.Q<Label>("Score");
        }

        private void OnEnable()
        {
            GlobalEventManager.AddListener(GlobalEventIndex.ScoreIncreased, OnScoreIncreased);
            GlobalEventManager.AddListener(GlobalEventIndex.WaveStarted, OnNewWaveStarted);

        }


        private void OnDisable()
        {
            GlobalEventManager.RemoveListener(GlobalEventIndex.ScoreIncreased, OnScoreIncreased);
            GlobalEventManager.RemoveListener(GlobalEventIndex.WaveStarted, OnNewWaveStarted);
        }

        private void OnScoreIncreased(GlobalEventArgs message)
        {
            GlobalEventArgsFactory.ScoreIncreaseParser(message, out float newScore);
            CurrentScore += newScore;
            scoreLabel.text = scoreText;
        }

        private void OnNewWaveStarted(GlobalEventArgs message)
        {
            CurrentScore = 0;
            scoreLabel.text = defaultScore;
        }

    }

}