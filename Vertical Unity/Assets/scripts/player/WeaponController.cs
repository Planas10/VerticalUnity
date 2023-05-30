using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public int id;
    [Header("References")]
    public Transform weaponNuzzle;

    [Header("General")]
    public LayerMask hittableLayers;
    public GameObject bulletHolePrefab;

    [Header("Shoot Parameters")]
    public float fireRange = 200;
    //public float recoilForce = 4f; //fuerza de retroceso del arma
    public float fireRate = 0.6f;
    public int cargadorSize = 8;
    public int cargadores;
    [System.NonSerialized]
    public int extraAmmo;

    [Header("Reload Parameters")]
    public float reloadTime = 2f;
    public AudioClip shootClip;
    public int currentAmmo { get; private set; }

    public float lastTimeShoot = Mathf.NegativeInfinity;
    public int colateralMaxLayers;
    public int damage;
    public int hitScore = 10;
    [Header("Sounds & Visuals")]
    public GameObject flashEffect;
    [SerializeField]
    protected Transform cameraPlayerTransform;
    public bool isautomatic;
    public virtual void Awake()
    {
  
    }

    public virtual void Start()
    {
        extraAmmo = cargadores * cargadorSize;
        ReloadAmmo();
        cameraPlayerTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }

    private void Update()
    {
    

        //transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, Time.deltaTime * 5f);
    }
    public void ReloadInput() 
    {
    
        
    }
    public bool CanReload() 
    {
        return (currentAmmo < cargadorSize && extraAmmo > 0);
    }
    public void ShootInput() 
    {
        if (!isautomatic)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                TryShoot();
            }
        }
        else
        {
            if (Input.GetMouseButton(0))
            {
                TryShoot();
            }
        }
    }

    //comprobar si se puede disparar
    protected bool TryShoot() {

        if (lastTimeShoot + fireRate < Time.time)
        {
            if (currentAmmo >= 1)
            {
                EventManager.current.PlayEffect(shootClip);
                HandleShoot();
                currentAmmo -= 1;
                EventManager.current.updateBulletsEvent.Invoke(currentAmmo, extraAmmo);
                return true;
            }
        }

        return false;
    }
    //disparo
    public virtual void HandleShoot() {

        GameObject flashClone = Instantiate(flashEffect, weaponNuzzle.position, Quaternion.Euler(weaponNuzzle.forward), transform);
        Destroy(flashClone, 1f);

        //AddRecoil();
        int currentLayersHit = 0;
        Vector3 direction = cameraPlayerTransform.forward;
        foreach (RaycastHit hit in Physics.RaycastAll(cameraPlayerTransform.position, direction, fireRange, hittableLayers))
        {
            if (hit.collider.isTrigger)
                continue;
            //crear y destruir gujero bala
            currentLayersHit++;
            AI_Enemy enemy = hit.collider.gameObject.GetComponent<AI_Enemy>();
            if (enemy)
            {
                EventManager.current.player.IncresePoints(hitScore);
                enemy.ReciveDamage(damage);
            }
            else
            {
                GameObject bulletHoleClone = Instantiate(bulletHolePrefab, hit.point + hit.normal * 0.001f, Quaternion.LookRotation(hit.normal));
                Destroy(bulletHoleClone, 4f);
            }
            if (currentLayersHit >= colateralMaxLayers)
            {
                lastTimeShoot = Time.time;
                return;
            }
        }

        lastTimeShoot = Time.time;
    }
    //añadir retroceso
    //private void AddRecoil() {
    //    transform.Rotate(-recoilForce, 0f, 0f);
    //    transform.position = transform.position - transform.forward * (recoilForce/50);
    //}

    //recargar arma
  
    public void ReloadAmmo() 
    {
        if (extraAmmo <= cargadorSize)
        {
            currentAmmo = extraAmmo;
            extraAmmo = 0;
        }
        else 
        {
            int bullets = cargadorSize - currentAmmo;
            extraAmmo -= bullets;
            currentAmmo = cargadorSize;
        }
        EventManager.current.updateBulletsEvent.Invoke(currentAmmo, extraAmmo);
    }
    public bool hasFullAmmo()
    {
        return extraAmmo == (cargadores - 1) * cargadorSize;
    }
    public void FullAmmo() 
    {
        extraAmmo = (cargadores - 1) * cargadorSize;
        EventManager.current.updateBulletsEvent.Invoke(currentAmmo, extraAmmo);
    }
}
