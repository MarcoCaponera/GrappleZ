using GrappleZ_SaveSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace GrappleZ_UI
{
    public class SettingsMenu : MonoBehaviour
    {
        #region SerializeField

        [SerializeField]
        private GameObject backButtonObject;

        #endregion

        #region PrivateAttributes

        private Button backButton;
        private Button applyButton;
        private Slider volumeSlider;
        private DropdownField resolutionDropdown;

        #endregion

        #region Mono

        protected void OnEnable()
        {
            backButton = GetComponent<UIDocument>().rootVisualElement.Q<Button>("BackButton");
            applyButton = GetComponent<UIDocument>().rootVisualElement.Q<Button>("ApplyButton");
            volumeSlider = GetComponent<UIDocument>().rootVisualElement.Q<Slider>("VolumeSlider");

            backButton.clicked += OnBackButtonClicked;
            applyButton.clicked += OnApplyButtonClicked;
            volumeSlider.RegisterValueChangedCallback(OnSliderValueChanged);

            volumeSlider.value = AudioListener.volume;
            InitResolutions();
        }

        protected void OnDisable()
        {
            backButton.clicked -= OnBackButtonClicked;
            applyButton.clicked -= OnApplyButtonClicked;
            volumeSlider.UnregisterValueChangedCallback(OnSliderValueChanged);
            resolutionDropdown.UnregisterValueChangedCallback(OnResolutionValueChanged);
        }

        #endregion

        #region InternalMethods

        private void InitResolutions()
        {
            resolutionDropdown = GetComponent<UIDocument>().rootVisualElement.Q<DropdownField>("ResDropdown");
            resolutionDropdown.choices = Screen.resolutions.Select(resolution => $"{resolution.width}x{resolution.height}").ToList();
            float currentWidth = Screen.width;
            float currentHeight = Screen.height;
            resolutionDropdown.index = Screen.resolutions
                .Select((resolution, index) => (resolution, index))
                .First((value) => value.resolution.width ==  currentWidth && value.resolution.height == currentHeight).index;
            resolutionDropdown.RegisterValueChangedCallback(OnResolutionValueChanged);
        }

        #endregion

        #region Callbacks

        private void OnBackButtonClicked()
        {
            backButtonObject.SetActive(true);
            gameObject.SetActive(false);
        }

        private void OnApplyButtonClicked()
        {
            SaveSystem.SettingsData.Volume = volumeSlider.value;
            SaveSystem.SettingsData.ScreenWidth = Screen.width;
            SaveSystem.SettingsData.ScreenHeight = Screen.height;
            SaveSystem.SaveSettingsData();
        }

        private void OnSliderValueChanged(ChangeEvent<float> evt)
        {
            AudioListener.volume = evt.newValue;
        }

        private void OnResolutionValueChanged(ChangeEvent<string> evt)
        {
            var resolution = Screen.resolutions[resolutionDropdown.index];
            Screen.SetResolution(resolution.width, resolution.height, true);
        }

        #endregion
    }
}
