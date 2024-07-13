using GrappleZ_Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;


namespace GrappleZ_UI
{
    public class DeathMenu : MonoBehaviour
    {
        #region SerilizedField
        [SerializeField]
        private String MainMenuScene;
        [SerializeField]
        private String LevelScene;
        
        #endregion

        #region PrivateAttributes
        private Coroutine changeSceneCoroutine;
        private VisualElement menuRoot;
        private Button restartButton;
        private Button mainMenuButton;

        #endregion

        #region Mono
        private void Awake()
        {
            VisualElement root = GetComponent<UIDocument>().rootVisualElement;
            menuRoot = root.Q("MenuRoot");
            restartButton = root.Q<Button>("RestartButton");
            mainMenuButton = root.Q<Button>("MainMenuButton");
        }

        private void OnEnable()
        {
            GlobalEventManager.AddListener(GlobalEventIndex.PlayerDeath, OnPlayerDeath);

        }


        private void OnDisable()
        {
            GlobalEventManager.RemoveListener(GlobalEventIndex.PlayerDeath, OnPlayerDeath);
        }


        #endregion

        #region Private Methods
        private void HideMenu()
        {
            restartButton.clicked -= RestartClickedCallback;
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
            Time.timeScale = 1f;
            if (!loadScene.isDone)
            {
                yield return new WaitForEndOfFrame();
            }
        }

        #endregion



        #region Callbacks
        private void RestartClickedCallback()
        {
            if (changeSceneCoroutine != null) return;
            changeSceneCoroutine = StartCoroutine(ChangeSceneCoroutine(LevelScene));
        }

        private void MainMenuClickedCallback()
        {
            if (changeSceneCoroutine != null) return;
            changeSceneCoroutine = StartCoroutine(ChangeSceneCoroutine(MainMenuScene));
        }

        private void OnPlayerDeath(GlobalEventArgs message)
        {
            menuRoot.visible = true;
            Time.timeScale = 0f;
            InputManager.EnablePlayerMap(false);
            MouseCursor.SetVisibility(true);
            MouseCursor.SetLockState(CursorLockMode.Confined);
            restartButton.clicked += RestartClickedCallback;
            mainMenuButton.clicked += MainMenuClickedCallback;
        }
        #endregion
    }
}