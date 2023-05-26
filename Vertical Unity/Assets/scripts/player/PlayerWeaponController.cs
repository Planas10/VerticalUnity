using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    public List<WeaponController> startingWeapons = new List<WeaponController>();

    public Transform weaponParentSocket;
    public Transform defaultWeaponPosition;
    public Transform aimingPosition;
    public Animator reloadAnim;
    public FPSController owner;
    public int activeWeaponIndex { get; private set; }

    private List<WeaponController> weaponSlots = new List<WeaponController>();
    bool reloading;

    // Start is called before the first frame update
    private void Awake()
    {
        owner = GetComponent<FPSController>();
    }
    void Start()
    {
        activeWeaponIndex = -1;

        foreach (WeaponController startingWeapon in startingWeapons)
        {
            AddWeapon(startingWeapon);
        }
        SwitchWeapon(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
   
            SwitchWeapon(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
        
            SwitchWeapon(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
       
            SwitchWeapon(2);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (weaponSlots[activeWeaponIndex].CanReload() && !reloading) 
            {
                StartCoroutine(ReloadAnim());
            }
        }
        if (!reloading && weaponSlots.Count > 0)
            weaponSlots[activeWeaponIndex].ShootInput();
        if (Input.mouseScrollDelta.y < 0) 
        {
            ScrollWeapons(-1);
        }
        if (Input.mouseScrollDelta.y > 0)
        {
            ScrollWeapons(1);
        }
    }
    public void ScrollWeapons(int v) 
    {
        int newWeaponIndex = activeWeaponIndex;
        newWeaponIndex += v;
        if(newWeaponIndex < 0)
            newWeaponIndex = weaponSlots.Count - 1;
        if (newWeaponIndex >= weaponSlots.Count)
            newWeaponIndex = 0;

        SwitchWeapon(newWeaponIndex);
        print(activeWeaponIndex);
    }
    //cambiar de arma
    private void SwitchWeapon(int p_weaponIndex)
    {
        if (p_weaponIndex == activeWeaponIndex)
            return;
        for (int i = 0; i < weaponSlots.Count; i++)
        {
            if (weaponSlots[i] != null)
            weaponSlots[i].gameObject.SetActive(false);
        }
        if (p_weaponIndex != activeWeaponIndex && p_weaponIndex >= 0)
        {
            weaponSlots[p_weaponIndex].gameObject.SetActive(true);
            activeWeaponIndex = p_weaponIndex;
            EventManager.current.updateBulletsEvent.Invoke(weaponSlots[p_weaponIndex].currentAmmo, weaponSlots[p_weaponIndex].extraAmmo);
        }
    }

    //añadir arma
    public void AddWeapon(WeaponController p_weaponPrefab)
    {
        weaponParentSocket.position = defaultWeaponPosition.position;
        WeaponController weaponClone = Instantiate(p_weaponPrefab, weaponParentSocket);
        weaponClone.gameObject.SetActive(false);
        weaponSlots.Add(weaponClone);
    }
    public bool HasMaxWeapons() 
    {
        return weaponParentSocket.childCount >= weaponSlots.Count;
    }
    public bool HasWeapon(WeaponController weapon) 
    {
        foreach (WeaponController w in weaponSlots)
        {
            if (w != null) 
            {
                if (w.id == weapon.id) 
                {
                    return true;
                }
            }
        }
        return false;
    }
    public WeaponController GetCurrentWeapon() 
    {
        return weaponSlots[activeWeaponIndex];
    }
  

    IEnumerator ReloadAnim()
    {
        reloading = true;
        reloadAnim.speed = 1 / GetCurrentWeapon().reloadTime * owner.reloadSpeedMult;
        reloadAnim.SetTrigger("Reload");
        Debug.Log("recargando...");
        yield return new WaitForSeconds(GetCurrentWeapon().reloadTime / owner.reloadSpeedMult);
        GetCurrentWeapon().ReloadAmmo();
        Debug.Log("Recargada");
        reloading = false;

    }
}
