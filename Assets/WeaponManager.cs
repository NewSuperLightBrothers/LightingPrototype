using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponManager : MonoBehaviour
{
    public bool isFire;
    public List<GameObject> l_weapons;
    public int currentWeapon;

    private void Start() {
        
    }

    public void OnLeftClick(bool value) {
        isFire = value;
    }

    public void OnWeaponChange() {
        currentWeapon = (currentWeapon + 1) % l_weapons.Count;
    }

}
