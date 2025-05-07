using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerShoot : MonoBehaviour
{
    private EntityStats entityStats;
    public GameObject bulletPrefab;
    private float cooldown;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        entityStats = GetComponent<EntityStats>();
        cooldown = entityStats.attackCooldown;
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
}
