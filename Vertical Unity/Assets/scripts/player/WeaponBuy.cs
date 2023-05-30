using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBuy : Interactuable
{
    public WeaponController weapon;
    public AudioClip cashClip;
    public int weaponCost;
    public int ammoCost;
    private void Awake()
    {
        GameObject instanciatedWeapon = Instantiate(weapon).gameObject;
        instanciatedWeapon.transform.SetParent(transform.GetChild(0));
        instanciatedWeapon.GetComponent<WeaponController>().enabled = false;
        instanciatedWeapon.transform.localPosition = Vector3.zero;
    }


    void Update()
    {
    }
    public override void Interact(FPSController player)
    {
        base.Interact(player);
        if (player.weaponManager.HasWeapon(weapon))
        {
            if (player.HasScore(ammoCost))
                BuyAmmo(player);
        }
        else 
        {
            if (player.HasScore(weaponCost))
                BuyWeapon(player);
        }
    }
    public void BuyWeapon(FPSController player) 
    {
        if(player.weaponManager.HasMaxWeapons())
          return;

        EventManager.current.PlayEffect(cashClip);
        player.weaponManager.AddWeapon(weapon);
        player.Buy(weaponCost);
        player.weaponManager.SwitchLastWeapon();
    }
    public void BuyAmmo(FPSController player) 
    {
        if (player.weaponManager.GetCurrentWeapon().hasFullAmmo())
            return;

        EventManager.current.PlayEffect(cashClip);
        player.weaponManager.GetCurrentWeapon().FullAmmo();
        player.Buy(ammoCost);
    }
    public override string GetMessage(FPSController player)
    {
        string m = base.GetMessage(player);
        if (player.weaponManager.HasWeapon(weapon))
        {
            m += "Buy " + weapon.name + " Ammo " + ammoCost;
        }
        else
        {
            m += "Buy " + weapon.name + " " + weaponCost;
        }

        return m;
    }
}
