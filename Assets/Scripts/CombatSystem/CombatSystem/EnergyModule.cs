using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class EnergyModule
{
    #region SerializeField
    [SerializeField]
    private float maxEnergy;
    [Tooltip("Amount of time from last energy consumption, to start recover enery")]
    [SerializeField]
    private float timeToStartRecovery;
    [SerializeField]
    private float energyRecoveredPerSecond;
    #endregion

    #region Events
    public Action<float> OnEnergyUpdated;
    #endregion

    #region PublicProperties
    public float MaxEnergy {
        get { return maxEnergy; }
    }
    public float CurrentEnergy {
        get { return currentEnergy; }
    }
    #endregion

    #region PrivateAttributes
    private float currentEnergy;
    private float counterToStartRecovery;
    private MonoBehaviour controller;
    private Coroutine waitForRecoveryCoroutine;
    private Coroutine recoveringCoroutine;
    #endregion

    #region PublicMethods
    public void InitMe (MonoBehaviour controller) {
        this.controller = controller;
        Reset();
    }

    public void Reset() {
        currentEnergy = maxEnergy;
        OnEnergyUpdated?.Invoke(currentEnergy);
    }

    public void ConsumeEnergy(float energy) {
        currentEnergy -= energy;
        OnEnergyUpdated?.Invoke(currentEnergy);
        counterToStartRecovery = timeToStartRecovery;
        if (recoveringCoroutine != null) {
            controller.StopCoroutine(recoveringCoroutine);
        }
        if (waitForRecoveryCoroutine != null) {
            controller.StopCoroutine(waitForRecoveryCoroutine);
        }
        waitForRecoveryCoroutine = controller.StartCoroutine(WaitForRecovery());
    }

    public bool EnoughtEnergy (float energy) {
        return currentEnergy >= energy;
    }

    private IEnumerator WaitForRecovery () {
        yield return new WaitForSeconds(timeToStartRecovery);
        recoveringCoroutine = controller.StartCoroutine(Recovering());
    }

    private IEnumerator Recovering () {
        WaitForEndOfFrame wait = new WaitForEndOfFrame();
        while (currentEnergy < maxEnergy) {
            currentEnergy += Time.deltaTime * energyRecoveredPerSecond;
            if (currentEnergy > maxEnergy) {
                currentEnergy = maxEnergy;
            }
            OnEnergyUpdated?.Invoke(currentEnergy);
            yield return wait;
        }
    }

    //public void OnUpdate (float dt) {
    //    counterToStartRecovery -= dt;
    //    if (counterToStartRecovery > 0) {
    //        return;
    //    }
    //    currentEnergy += dt * energyRecoveredPerSecond;
    //    if (currentEnergy > maxEnergy) {
    //        currentEnergy = maxEnergy;
    //        return;
    //    }
    //    OnEnergyUpdated?.Invoke(currentEnergy);
    //}
    #endregion

}
