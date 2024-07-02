using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine;
using System.Collections;
using System;
using GrappleZ_Utility;
using System.Collections.Generic;

namespace GrappleZ_UI
{
    public class EndWaveMenu : MonoBehaviour
    {


        #region SerilizedField
        [SerializeField]
        private String NextWaveScene;
        [SerializeField]
        private String MainMenuScene;

        #endregion

        #region PrivateAttributes
        private Coroutine changeSceneCoroutine;
        private VisualElement menuRoot;
        private Button nextWaveButton;
        private Button mainMenuButton;
        private Label waveLabel;
        private LeaderboardControl leaderboardTable;

        #endregion

        #region Mono
        private void Awake()
        {
            VisualElement root = GetComponent<UIDocument>().rootVisualElement;
            menuRoot = root.Q("Menu");
            nextWaveButton = root.Q<Button>("NextWaveButton");
            mainMenuButton = root.Q<Button>("MainMenuButton");
            waveLabel = root.Q<Label>("WaveTextBox");
            leaderboardTable = root.Q<LeaderboardControl>("LeaderBoardTable");
        }

        private void OnEnable()
        {
            GlobalEventManager.AddListener(GlobalEventIndex.WaveEnded, OnWaveEnded);

        }


        private void OnDisable()
        {
            GlobalEventManager.RemoveListener(GlobalEventIndex.WaveEnded, OnWaveEnded);
        }


        #endregion

        #region Private Methods
        private void HideMenu()
        {
            nextWaveButton.clicked -= NextWaveClickedCallback;
            mainMenuButton.clicked -= MainMenuClickedCallback;
            InputManager.EnablePlayerMap(true);
            menuRoot.visible = false;
        }

        #endregion

        #region Coroutine

        private IEnumerator ChangeSceneCoroutine(string sceneToLoad)
        {
            var loadScene = SceneManager.LoadSceneAsync(sceneToLoad);
            HideMenu();
            if (!loadScene.isDone)
            {
                yield return new WaitForEndOfFrame();
            }
        }

        #endregion



        #region Callbacks
        private void NextWaveClickedCallback()
        {
            if (changeSceneCoroutine != null) return;
            changeSceneCoroutine = StartCoroutine(ChangeSceneCoroutine(NextWaveScene));
        }

        private void MainMenuClickedCallback()
        {
            if (changeSceneCoroutine != null) return;
            changeSceneCoroutine = StartCoroutine(ChangeSceneCoroutine(MainMenuScene));
        }

        private void OnWaveEnded(GlobalEventArgs message)
        {
            GlobalEventArgsFactory.WaveEndedParser(message, out WaveEnum currentWave);
            waveLabel.text = $"You have survived the {currentWave} Wave";
            menuRoot.visible = true;
            InputManager.EnablePlayerMap(false);
            MouseCursor.SetVisibility(true);
            MouseCursor.SetLockState(CursorLockMode.None);
            nextWaveButton.clicked += NextWaveClickedCallback;
            mainMenuButton.clicked += MainMenuClickedCallback;
        }

        public void ComputeLeaderbard(List<ScoreStruct> leaderboad)
        {
            leaderboardTable.Leaderboard = leaderboad;
        }

        #endregion
    }
}
