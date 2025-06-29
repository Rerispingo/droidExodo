using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerShoot : MonoBehaviour
{
    private EntityStats entityStats;
    public GameObject bulletPrefab, bulletStrongPrefab;
    private float cooldown, chargeCooldown;

    private bool isShootingStrong = false;
    private float chargingShootingStrong;

    public GameObject SecondShootAudio;
    public GameObject SecondShootChargeAudio;
    public GameObject FirstShootAudio;
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
        if (SceneManager.GetActiveScene().buildIndex == 2 || SceneManager.GetActiveScene().buildIndex == 3)
        {
            if ((Input.GetMouseButton(0) || Input.GetKey(KeyCode.Space)) && cooldown <= 0) // Atirar
            {
                Atirar();
                cooldown = entityStats.attackCooldown;
            }
            cooldown -= Time.deltaTime;
        
            if ((Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.F)) && !isShootingStrong)
            {
                isShootingStrong = true;
                HUDManager.Instance.changeChargeSlider(chargingShootingStrong / chargeCooldown, isShootingStrong);
                SecondShootChargeAudio.GetComponent<AudioSource>().Play(); // Carregar tiro sound.

            }
            if ((Input.GetMouseButton(1) || Input.GetKey(KeyCode.F)) && isShootingStrong)
            {
                chargingShootingStrong += Time.deltaTime;
                HUDManager.Instance.changeChargeSlider(chargingShootingStrong / chargeCooldown);
                if (chargingShootingStrong >= chargeCooldown)
                {
                    chargingShootingStrong = 0;
                    isShootingStrong = false;
                    HUDManager.Instance.changeChargeSlider(chargingShootingStrong / chargeCooldown, isShootingStrong);
                    AtirarForte();
                    SecondShootChargeAudio.GetComponent<AudioSource>().Stop(); // Stop SecondShootCharging sound.
                }
            }
        }
        if ((Input.GetMouseButtonUp(1) || Input.GetKeyUp(KeyCode.F)))
        {
            chargingShootingStrong = 0;
            isShootingStrong = false;
            HUDManager.Instance.changeChargeSlider(chargingShootingStrong / chargeCooldown, isShootingStrong);
            SecondShootChargeAudio.GetComponent<AudioSource>().Stop(); // Stop SecondShootCharging sound.
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

        FirstShootAudio.GetComponent<AudioSource>().Play();
    }

    public void AtirarForte()
    {
        GameObject bulletStrong = GameObject.Instantiate(bulletStrongPrefab);
        bulletStrong.GetComponent<bulletBehavior>().damage = entityStats.damage * 2; //Dano do tiro
        bulletStrong.GetComponent<bulletBehavior>().parent = gameObject;
        bulletStrong.transform.position = transform.position;
        bulletStrong.transform.LookAt(transform.forward + transform.position); //Transform position serve para somar ao vetor e o foward (+1) funcionar corretamente.
        bulletStrong.GetComponent<Rigidbody>().AddForce((bulletStrong.transform.forward) * entityStats.bulletSpeed * 150);

        SecondShootAudio.GetComponent<AudioSource>().Play();
    }
}
