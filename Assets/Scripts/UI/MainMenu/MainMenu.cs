using GrappleZ_SaveSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace GrappleZ_UI
{

    public class MainMenu : MonoBehaviour
    {
        #region SerilizedField
        [SerializeField]
        private String SceneToLoad;
        [SerializeField]
        private GameObject settingsMenu;


        #endregion

        #region PrivateAttributes
        private Coroutine changeSceneCoroutine;
        private Button startButton;
        private Button exitButton;
        private Button optionsButton;

        #endregion

        #region Mono
        private void Awake()
        {
            if (SaveSystem.SettingsData.ScreenHeight == 0 || SaveSystem.SettingsData.ScreenHeight == 0)
            {
                Screen.SetResolution(1920, 1080, true);
            }
            else
            {
                Screen.SetResolution(SaveSystem.SettingsData.ScreenWidth, SaveSystem.SettingsData.ScreenHeight, true);
            }
            AudioListener.volume = SaveSystem.SettingsData.Volume;
        }

        private void OnEnable()
        {
            startButton = GetComponent<UIDocument>().rootVisualElement.Q<Button>("StartButton");
            exitButton = GetComponent<UIDocument>().rootVisualElement.Q<Button>("ExitButton");
            optionsButton = GetComponent<UIDocument>().rootVisualElement.Q<Button>("OptionsButton");
            startButton.clicked += StartClickedCallback;
            exitButton.clicked += ExitClickedCallback;
            optionsButton.clicked += OptionsClickedCallback;
        }

        private void OnDisable()
        {
            startButton.clicked -= StartClickedCallback;
            exitButton.clicked -= ExitClickedCallback;
            optionsButton.clicked -= OptionsClickedCallback;
        }

        #endregion

        #region InternalMethods

        private IEnumerator ChangeSceneCoroutine()
        {
            var loadScene = SceneManager.LoadSceneAsync(SceneToLoad);
            if (!loadScene.isDone)
            {
                yield return new WaitForEndOfFrame();
            }
        }
        #endregion

        #region Callbacks
        private void StartClickedCallback()
        {
            if (changeSceneCoroutine != null) return;
            changeSceneCoroutine = StartCoroutine(ChangeSceneCoroutine());
        }

        private void OptionsClickedCallback()
        {
            settingsMenu.SetActive(true);
            gameObject.SetActive(false);
        }

        private void ExitClickedCallback()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }
        #endregion
    }

}