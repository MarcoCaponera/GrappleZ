using UnityEngine;
using UnityEngine.UIElements;

public class UI_Health : MonoBehaviour
{
    private ProgressBar healthBar;

    private void Awake()
    {
        healthBar = GetComponent<UIDocument>().rootVisualElement.Q<ProgressBar>("HealthBar");
    }

    private void OnEnable()
    {
        GlobalEventManager.AddListener(GlobalEventIndex.PlayerHealthUpdated, OnPlayerHealthUpdated);

    }

    private void OnDisable()
    {
        GlobalEventManager.RemoveListener(GlobalEventIndex.PlayerHealthUpdated, OnPlayerHealthUpdated);
    }

    private void OnPlayerHealthUpdated(GlobalEventArgs message)
    {
        GlobalEventArgsFactory.PlayerHealthUpdatedParser(message, out float maxHp, out float currentHp);
        healthBar.value = currentHp / maxHp;
    }
}
