using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBoss_Behavior : MonoBehaviour
{
    private EntityStats entityStats;
    public GameObject shootPreFab;
    public GameObject shootSound;
    private float shootSpeed, shootCooldownC, shootCooldown;
    private GameObject Player;
    public GameObject[] Helices;
    
    private Rigidbody rb;
    private Vector3 originalPos;
    private float variation, velocity;
    private int isRight;

    public Slider BossBar;

    public GameObject[] spawns;
    public GameObject Enemy4;

    public float delayEnemy4;
    public float multiSpeedEnemy4;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        entityStats = GetComponent<EntityStats>();
        variation = entityStats.MovementVariation;
        velocity = entityStats.speed;
        originalPos = transform.position;

        rb = GetComponent<Rigidbody>();
        isRight = 1;

        shootSpeed = entityStats.bulletSpeed * 10000f;
        shootCooldown = entityStats.attackCooldown;
        shootCooldownC = shootCooldown;
        
        StartCoroutine(FindPlayerObject());
        StartCoroutine(Enemy4Spawns());
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
            
            shootSound.GetComponent<AudioSource>().Play();
            GameObject bullet = GameObject.Instantiate(shootPreFab); // Criacao e preparacao do tiro
            bullet.GetComponent<bulletBehavior>().damage = entityStats.damage;
            bullet.GetComponent<bulletBehavior>().parent = gameObject;
            bullet.transform.position = transform.position;
            bullet.transform.LookAt(transform.forward + transform.position);
            bullet.GetComponent<Rigidbody>().AddForce((bullet.transform.forward) * shootSpeed);
        }
        shootCooldownC += Time.deltaTime;
        
        rb.AddForce(Vector3.right * (entityStats.speed * isRight * Time.deltaTime));
        
        for (int i = 0; i < Helices.Length ; i++)
        {
            Helices[i].transform.Rotate(Vector3.forward * 35);
        }
    }

    IEnumerator FindPlayerObject()
    {
        while (Player == null)
        {
            Player = GameObject.FindGameObjectWithTag("Player");
            yield return new WaitForEndOfFrame();
        }
    }
    

    IEnumerator Enemy4Spawns()
    {
        while (true)
        {
            yield return new WaitForSeconds(delayEnemy4);
            if (Physics.Raycast(gameObject.transform.position,
                    (Player.transform.position - gameObject.transform.position), out RaycastHit hit,
                    (Mathf.Abs(Vector3.Distance(Player.transform.position, gameObject.transform.position))) + 1))
            {
                if (hit.transform.gameObject.tag == "Player")
                {
                    for (int i = 0; i < spawns.Length; i++)
                    {
                        GameObject Enemy = Instantiate(Enemy4, spawns[i].transform);
                        Enemy.GetComponent<EntityStats>().speed *= multiSpeedEnemy4;
                    }
                }
            }
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Bullet") && !other.gameObject.CompareTag("Player"))
        {
            Debug.Log(other.gameObject.name);
            rb.linearVelocity = Vector3.zero;
            isRight = -isRight;
        }    
    }
}
