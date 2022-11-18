using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace BNG {
    public class AmmoDisplay : MonoBehaviour {

        public RaycastWeapon Weapon;
        public TextMeshProUGUI AmmoLabel;

        void OnGUI() {
            string loadedShot = Weapon.BulletInChamber ? "1" : "0";
            AmmoLabel.text = loadedShot + " / " + Weapon.GetBulletCount();
        }
    }
}

