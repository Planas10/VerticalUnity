using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponShotGun : WeaponController
{
    public int perdigones;
    public float area = 5;
    public override void Awake()
    {
        base.Awake();
    }
    public override void Start()
    {
        base.Start();
    }

    public override void HandleShoot()
    {
        GameObject flashClone = Instantiate(flashEffect, weaponNuzzle.position, Quaternion.Euler(weaponNuzzle.forward), transform);
        Destroy(flashClone, 1f);
        //AddRecoil();
        for (int i = 0; i < perdigones; i++)
        {
            int currentLayersHit = 0;
            Vector3 direction = Quaternion.Euler(Random.Range(-area, area), Random.Range(-area, area), Random.Range(-area, area)) * cameraPlayerTransform.forward;
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
                    break;
                }
            }
        }
        lastTimeShoot = Time.time;
    }
}
