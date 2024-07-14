using GrappleZ_Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class EndGameScreen : MonoBehaviour
{
    #region SerializeField

    [SerializeField]
    private string mainMenuLevel;

    #endregion

    #region PrivateAtt

    private Coroutine changeSceneCoroutine;
    private VisualElement rootElement;
    private Button mainMenuButton;

    #endregion

    #region Mono

    protected void Awake()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        rootElement = root.Q<VisualElement>("Root");
        mainMenuButton = root.Q<Button>("MenuButton");
    }

    protected void OnEnable()
    {
        GlobalEventManager.AddListener(GlobalEventIndex.GameEnded, OnGameEnded);
        mainMenuButton.clicked += MainMenuClicked;
    }

    protected void OnDisable()
    {
        GlobalEventManager.RemoveListener(GlobalEventIndex.GameEnded, OnGameEnded);
        mainMenuButton.clicked -= MainMenuClicked;
    }

    #endregion

    #region PrivateMethods

    private void HideMenu()
    {
        rootElement.visible = false;
    }

    #endregion

    #region Coroutine

    private IEnumerator ChangeSceneCoroutine(string sceneToLoad)
    {
        var loadScene = SceneManager.LoadSceneAsync(sceneToLoad);
        Time.timeScale = 1f;
        if (!loadScene.isDone)
        {
            yield return new WaitForEndOfFrame();
        }
    }


    #endregion

    #region Callbacks

    private void OnGameEnded(GlobalEventArgs message)
    {
        rootElement.visible = true;
        InputManager.EnablePlayerMap(false);
        MouseCursor.SetVisibility(true);
        MouseCursor.SetLockState(CursorLockMode.Confined);
    }

    private void MainMenuClicked()
    {
        if (changeSceneCoroutine != null) return;
        HideMenu();
        changeSceneCoroutine = StartCoroutine(ChangeSceneCoroutine(mainMenuLevel));

    }
    #endregion
}
