using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    #region Variables
    [SerializeField]
    float RateOfFire;

    [SerializeField]
    float ReloadTime;

    [SerializeField]
    int TotalAmmo;

    [SerializeField]
    float KnockbackInpulse;
    #endregion //Variables
}
