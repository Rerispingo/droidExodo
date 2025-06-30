using System;
using System.Collections;
using UnityEngine;

public class Enemy_Behavior : MonoBehaviour
{
    private EntityStats entityStats;
    public GameObject shootPreFab;
    public GameObject shootSound;
    private float shootSpeed, shootCooldownC, shootCooldown;
    public GameObject[] shootPos;
    
    private Rigidbody rb;
    private Vector3 originalPos;
    private float variation;
    private int isRight;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        entityStats = GetComponent<EntityStats>();
        variation = entityStats.MovementVariation;
        originalPos = transform.position;

        rb = GetComponent<Rigidbody>();
        isRight = 1;
        

        //Debug.Log($"Original {originalPos}: {variation} seria a variacao. Com a soma maxima de {new Vector3(variation, 0, 0)} seria {maxPos}");

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
                shootSound.GetComponent<AudioSource>().Play();
                GameObject[] bullets = new GameObject[2];
                bullets[i] = GameObject.Instantiate(shootPreFab);
                bullets[i].GetComponent<bulletBehavior>().damage = entityStats.damage; //Dano do tiro
                bullets[i].GetComponent<bulletBehavior>().parent = gameObject;
                bullets[i].transform.position = shootPos[i].transform.position;
                bullets[i].transform.LookAt(shootPos[i].transform.forward + bullets[i].transform.position);
                bullets[i].GetComponent<Rigidbody>().AddForce((bullets[i].transform.forward) * shootSpeed);
            }
        }
        shootCooldownC += Time.deltaTime;
        rb.AddForce(transform.right * (entityStats.speed * isRight * Time.deltaTime));
    }

    // Self Collision with Player.
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            EntityStats thisES = gameObject.GetComponent<EntityStats>();
            EntityStats playerES = other.gameObject.GetComponent<EntityStats>();
            thisES.onHealthChangeEvent.Invoke(-thisES.maxHealth);
            playerES.onHealthChangeEvent.Invoke(-thisES.damage);
            
        }
        else if (!other.gameObject.CompareTag("Bullet"))
        {
            rb.linearVelocity = Vector3.zero;
            isRight = -isRight;
        }    
    }
}
