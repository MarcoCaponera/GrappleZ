using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace GrappleZ_UI
{
    public class UI_Crossair : MonoBehaviour
    {
        private VisualElement crossair;

        private void Awake()
        {
            crossair = GetComponent<UIDocument>().rootVisualElement.Q("Crosshair");
        }

        private void OnEnable()
        {
            GlobalEventManager.AddListener(GlobalEventIndex.WaveEnded, OnOnWaveEnded);
            GlobalEventManager.AddListener(GlobalEventIndex.WaveStarted, OnWaveStarted);

        }

        private void OnDisable()
        {
            GlobalEventManager.RemoveListener(GlobalEventIndex.WaveEnded, OnOnWaveEnded);
            GlobalEventManager.RemoveListener(GlobalEventIndex.WaveStarted, OnWaveStarted);
        }

        private void OnOnWaveEnded(GlobalEventArgs message)
        {
            crossair.visible = false;
            Debug.Log("Disable Crosshair");
        }
        
        private void OnWaveStarted(GlobalEventArgs message)
        {
            crossair.visible = true;
            Debug.Log("Enable Crosshair");
        }
    
    }
}