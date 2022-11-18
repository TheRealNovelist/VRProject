using System;
using System.Collections;
using System.Collections.Generic;
using BNG;
using UnityEngine;
using UnityEngine.UI;
using Slider = UnityEngine.UI.Slider;

public class AmmoDisplaySlider : MonoBehaviour
{
    public RaycastWeapon weapon;
    public Slider slider;

    private void Start()
    {
        OnAmmoUpdate();
        
        weapon.onAttachedAmmoEvent.AddListener(OnAmmoUpdate);
        weapon.onDetachedAmmoEvent.AddListener(OnAmmoUpdate);
        weapon.onShootEvent.AddListener(OnShoot);
    }

    void OnAmmoUpdate()
    {
        slider.maxValue = weapon.GetBulletCount();
        slider.value = weapon.GetBulletCount();
    }
    

    void OnShoot()
    {
        slider.value = weapon.GetBulletCount();
    }
}
