using System;
using UnityEngine;

[Serializable]
public class HealthModule
{

    #region SerializeField
    [SerializeField]
    private float maxHP;
    #endregion

    #region Events
    public Action<DamageContainer> OnDamageTaken;
    public Action OnDeath;
    #endregion

    #region PublicProperties
    public float MaxHP {
        get { return maxHP; }
    }
    public float CurrentHP {
        get { return currentHP; }
    }
    public bool IsDead {
        get { return currentHP <= 0; }
    }
    #endregion

    #region PrivateAttributes
    private bool invulnerable;
    private float currentHP;
    #endregion

    #region PublicMethods
    public void Reset () {
        currentHP = maxHP;
    }

    public void SetInvulnerable (bool value) {
        invulnerable = value;
    }

    public void TakeDamage (DamageContainer damage) {
        if (IsDead || invulnerable) return;
        currentHP -= damage.Damage;
        OnDamageTaken?.Invoke(damage);
        if (currentHP > 0) return;
        OnDeath?.Invoke();
    }
    #endregion

}
