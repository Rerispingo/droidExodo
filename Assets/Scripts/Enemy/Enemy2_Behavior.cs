using System.Collections;
using UnityEngine;

public class Enemy_Behavior2 : MonoBehaviour
{
    private EntityStats entityStats;
    public GameObject shootPreFab;
    private float shootSpeed, shootCooldownC, shootCooldown;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        entityStats = GetComponent<EntityStats>();

        shootSpeed = entityStats.bulletSpeed * 10000f;
        shootCooldown = entityStats.attackCooldown;
        shootCooldownC = shootCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        if (shootCooldownC >= shootCooldown)
        {
            shootCooldownC = 0f;

            GameObject bullet = GameObject.Instantiate(shootPreFab);
            bullet.GetComponent<bulletBehavior>().damage = entityStats.damage; //Dano do tiro
            bullet.GetComponent<bulletBehavior>().parent = gameObject;
            bullet.transform.position = transform.position;
            bullet.transform.LookAt(transform.forward + transform.position);
            bullet.GetComponent<Rigidbody>().AddForce((bullet.transform.forward) * shootSpeed);
        }
        shootCooldownC += Time.deltaTime;
    }
}
