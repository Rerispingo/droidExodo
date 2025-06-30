using System.Collections;
using UnityEngine;

public class Enemy_Behavior3 : MonoBehaviour
{
    private EntityStats entityStats;
    public GameObject shootSound;
    public GameObject shootPreFab;
    private float shootSpeed, shootCooldownC, shootCooldown;
    private GameObject Player;
    public GameObject[] shootPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        entityStats = GetComponent<EntityStats>();

        shootSpeed = entityStats.bulletSpeed * 10000f;
        shootCooldown = entityStats.attackCooldown;
        shootCooldownC = shootCooldown;

        StartCoroutine(FindPlayerObject());
    }

    // Update is called once per frame
    void Update()
    {
        if (Player != null)
        {
            gameObject.transform.LookAt(Player.transform);
        }

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
                bullets[i].transform.LookAt(shootPos[i].transform.forward + shootPos[i].transform.position);
                bullets[i].GetComponent<Rigidbody>().AddForce((bullets[i].transform.forward) * shootSpeed);
            }
            
            /*
            GameObject bullet = GameObject.Instantiate(shootPreFab); // Criacao e preparacao do tiro
            bullet.GetComponent<bulletBehavior>().damage = entityStats.damage;
            bullet.GetComponent<bulletBehavior>().parent = gameObject;
            bullet.transform.position = transform.position;
            bullet.transform.LookAt(transform.forward + transform.position);
            bullet.GetComponent<Rigidbody>().AddForce((bullet.transform.forward) * shootSpeed);*/
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
