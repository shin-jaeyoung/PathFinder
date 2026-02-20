using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponUpdate : MonoBehaviour
{
    [SerializeField]
    private Player p;

    [SerializeField]
    private SpriteRenderer r_weapon;

    private void Start()
    {
        p.Inventory.OnEquipmentChanged += UpdateWeapon;
        UpdateWeapon();
    }
    private void OnDestroy()
    {
        p.Inventory.OnEquipmentChanged -= UpdateWeapon;
    }
    public void UpdateWeapon()
    {
        if(p.Inventory.Equipments[0].IsEmpty())
        {
            r_weapon.sprite = null;
        }
        else
        {
            r_weapon.sprite = p.Inventory.Equipments[0].item.Data.Sprite;
        }
    }
}
