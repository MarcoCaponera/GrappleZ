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


        #endregion

        #region PrivateAttributes
        private Coroutine changeSceneCoroutine;
        private Button startButton;
        private Button exitButton;

        #endregion

        #region Mono
        private void Awake()
        {
            startButton = GetComponent<UIDocument>().rootVisualElement.Q<Button>("StartButton");
            exitButton = GetComponent<UIDocument>().rootVisualElement.Q<Button>("ExitButton");

            startButton.clicked += StartClickedCallback;
            exitButton.clicked += ExitClickedCallback;
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