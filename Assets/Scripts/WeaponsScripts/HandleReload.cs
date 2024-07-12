using GrappleZ_Weapons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class HandleReload : MonoBehaviour
{
    public WeaponComponent weapon;
    public bool isReloading;
    public UIDocument HUD;
    private VisualElement crosshair;
    public GameObject DisbaleIcon;

    private void Awake()
    {
        weapon = FindObjectOfType<WeaponComponent>();
        crosshair = HUD.rootVisualElement.Q("Crosshair");
      
       
    }
    private void Update()
    {
        isReloading = weapon.getIsReloading();

        if(isReloading)
        {
            DisbaleIcon.SetActive(true);
            crosshair.visible = false;
        
        }else if(!isReloading)
        {
            DisbaleIcon.SetActive(false);
            crosshair.visible = true;


        }
    }

    
}
