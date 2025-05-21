using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerShoot : MonoBehaviour
{
    private EntityStats entityStats;
    public GameObject bulletPrefab, bulletStrongPrefab;
    private float cooldown, chargeCooldown;

    private bool isShootingStrong = true;
    private float chargingShootingStrong;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        entityStats = GetComponent<EntityStats>();
        cooldown = entityStats.attackCooldown;
        chargeCooldown = entityStats.chargeAttackCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) && cooldown <= 0) // Atirar
        {
            Atirar();
            cooldown = entityStats.attackCooldown;
        }
        cooldown -= Time.deltaTime;
        
        if (Input.GetMouseButtonDown(1) && !isShootingStrong)
        {
            isShootingStrong = true;
            HUDManager.Instance.changeChargeSlider(chargingShootingStrong/chargeCooldown, isShootingStrong);

        }
        if (Input.GetMouseButton(1) && isShootingStrong)
        {
            chargingShootingStrong += Time.deltaTime;
            HUDManager.Instance.changeChargeSlider(chargingShootingStrong/chargeCooldown);
            if (chargingShootingStrong >= chargeCooldown)
            {
                chargingShootingStrong = 0;
                isShootingStrong = false;
                HUDManager.Instance.changeChargeSlider(chargingShootingStrong/chargeCooldown, isShootingStrong);
                AtirarForte();
            }
        }
        
    }

    public void Atirar()
    {
        GameObject bullet = GameObject.Instantiate(bulletPrefab);
        bullet.GetComponent<bulletBehavior>().damage = entityStats.damage; //Dano do tiro
        bullet.GetComponent<bulletBehavior>().parent = gameObject;
        bullet.transform.position = transform.position;
        bullet.transform.LookAt(transform.forward + transform.position); //Transform position serve para somar ao vetor e o foward (+1) funcionar corretamente.
        bullet.GetComponent<Rigidbody>().AddForce((bullet.transform.forward) * entityStats.bulletSpeed * 100);
    }

    public void AtirarForte()
    {
        GameObject bulletStrong = GameObject.Instantiate(bulletPrefab);
        bulletStrong.GetComponent<bulletBehavior>().damage = entityStats.damage * 2; //Dano do tiro
        bulletStrong.GetComponent<bulletBehavior>().parent = gameObject;
        bulletStrong.transform.position = transform.position;
        bulletStrong.transform.LookAt(transform.forward + transform.position); //Transform position serve para somar ao vetor e o foward (+1) funcionar corretamente.
        bulletStrong.GetComponent<Rigidbody>().AddForce((bulletStrong.transform.forward) * entityStats.bulletSpeed * 150);
    }
}
