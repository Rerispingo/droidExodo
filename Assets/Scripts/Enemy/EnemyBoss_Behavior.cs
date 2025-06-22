using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBoss_Behavior : MonoBehaviour
{
    private EntityStats entityStats;
    public GameObject shootPreFab;
    private float shootSpeed, shootCooldownC, shootCooldown;
    private GameObject Player;
    
    private Rigidbody rb;
    private Vector3 originalPos;
    private float variation, velocity;
    private bool isRight;

    public Slider BossBar;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        entityStats = GetComponent<EntityStats>();
        variation = entityStats.MovementVariation;
        velocity = entityStats.speed;
        originalPos = transform.position;

        rb = GetComponent<Rigidbody>();
        isRight = true;
        rb.AddForce(velocity * Vector3.right);

        shootSpeed = entityStats.bulletSpeed * 10000f;
        shootCooldown = entityStats.attackCooldown;
        shootCooldownC = shootCooldown;
        
        StartCoroutine(FindPlayerObject());
        StartCoroutine(Movement());
    }

    // Update is called once per frame
    void Update()
    {
        BossBar.value = (entityStats.health / entityStats.maxHealth);
        
        if (Player != null)
        {
            gameObject.transform.LookAt(Player.transform);
        }

        if (shootCooldownC >= shootCooldown)
        {
            shootCooldownC = 0f;

            GameObject bullet = GameObject.Instantiate(shootPreFab); // Criacao e preparacao do tiro
            bullet.GetComponent<bulletBehavior>().damage = entityStats.damage;
            bullet.GetComponent<bulletBehavior>().parent = gameObject;
            bullet.transform.position = transform.position;
            bullet.transform.LookAt(transform.forward + transform.position);
            bullet.GetComponent<Rigidbody>().AddForce((bullet.transform.forward) * shootSpeed);
        }
        shootCooldownC += Time.deltaTime;
    }

    IEnumerator FindPlayerObject()
    {
        while (Player == null)
        {
            Player = GameObject.FindGameObjectWithTag("Player");
            yield return new WaitForEndOfFrame();
        }
    }
    
    IEnumerator Movement()
    {
        while (true)
        {
            Vector3 position = transform.position;
            float distance = Vector3.Distance(originalPos, position);
            Debug.Log(distance + ": " + (distance > variation));
            
            if (distance > variation)
            {
                if (isRight)
                {
                    rb.linearVelocity = Vector3.zero;
                    rb.AddForce(-velocity * Vector3.right);
                    isRight = false;
                }
                else
                {
                    rb.linearVelocity = Vector3.zero;
                    rb.AddForce(velocity * Vector3.right);
                    isRight = true;
                }
            }
            yield return new WaitForEndOfFrame();
        }
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
