using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using UnityEngine;

public class Enemy_Behavior2 : MonoBehaviour
{
    private EntityStats entityStats;
    public GameObject shootPreFab;
    private float shootSpeed, shootCooldownC, shootCooldown;
    public GameObject[] shootPos;

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

            for (int i = 0; i < 2; i++)
            {
                GameObject[] bullets = new GameObject[2];
                bullets[i] = GameObject.Instantiate(shootPreFab);
                bullets[i].GetComponent<bulletBehavior>().damage = entityStats.damage; //Dano do tiro
                bullets[i].GetComponent<bulletBehavior>().parent = gameObject;
                bullets[i].transform.position = shootPos[i].transform.position;
                bullets[i].transform.LookAt(shootPos[i].transform.forward + shootPos[i].transform.position);
                bullets[i].GetComponent<Rigidbody>().AddForce((bullets[i].transform.forward) * shootSpeed);
            }
        }
        shootCooldownC += Time.deltaTime;
    }

    // Self Collision with Player.
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            EntityStats thisES = gameObject.GetComponent<EntityStats>();
            EntityStats playerES = other.gameObject.GetComponent<EntityStats>();
            thisES.onHealthChangeEvent.Invoke(-thisES.maxHealth);
            playerES.onHealthChangeEvent.Invoke(-thisES.damage);
            
        }    
    }
}
