using GrappleZ_Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;


namespace GrappleZ_UI
{
    public class PauseMenu : MonoBehaviour
    {
        #region SerilizedField
        [SerializeField]
        private String MainMenuScene;
        #endregion

        #region PrivateAttributes
        private Coroutine changeSceneCoroutine;
        private VisualElement menuRoot;
        private Button resumeButton;
        private Button mainMenuButton;

        #endregion

        #region Mono
        private void Awake()
        {
            VisualElement root = GetComponent<UIDocument>().rootVisualElement;
            menuRoot = root.Q("MenuRoot");
            resumeButton = root.Q<Button>("ResumeButton");
            mainMenuButton = root.Q<Button>("MainMenuButton");
        }

        private void OnEnable()
        {
            GlobalEventManager.AddListener(GlobalEventIndex.GamePaused, OnGamePaused);

        }


        private void OnDisable()
        {
            GlobalEventManager.RemoveListener(GlobalEventIndex.GamePaused, OnGamePaused);
        }


        #endregion

        #region Private Methods
        private void HideMenu()
        {
            resumeButton.clicked -= ResumeClickedCallback;
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
        private void ResumeClickedCallback()
        {
            HideMenu();
            MouseCursor.SetVisibility(false);
            MouseCursor.SetLockState(CursorLockMode.Locked);
            GlobalEventManager.CastEvent(GlobalEventIndex.GameResumed, GlobalEventArgsFactory.GameResumedFactory());
        }

        private void MainMenuClickedCallback()
        {
            if (changeSceneCoroutine != null) return;
            changeSceneCoroutine = StartCoroutine(ChangeSceneCoroutine(MainMenuScene));
        }

        private void OnGamePaused(GlobalEventArgs message)
        {
            menuRoot.visible = true;
            InputManager.EnablePlayerMap(false);
            MouseCursor.SetVisibility(true);
            MouseCursor.SetLockState(CursorLockMode.Confined);
            resumeButton.clicked += ResumeClickedCallback;
            mainMenuButton.clicked += MainMenuClickedCallback;
        }
        #endregion
    }
}