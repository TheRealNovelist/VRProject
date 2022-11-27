using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using BNG;

public class AmmoDisplayText : MonoBehaviour {

    public RaycastWeapon Weapon;
    public TextMeshPro AmmoLabel;

    public void Awake()
    {
        if (!Weapon)
            Weapon = GetComponentInParent<RaycastWeapon>();
    }

    void OnGUI() {
        string loadedShot = Weapon.BulletInChamber ? "1" : "0";
        AmmoLabel.text = loadedShot + " / " + Weapon.GetBulletCount();
    }
}

