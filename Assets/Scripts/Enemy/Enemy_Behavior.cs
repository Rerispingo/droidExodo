using System.Collections;
using UnityEngine;

public class Enemy_Behavior : MonoBehaviour
{
    private EntityStats entityStats;
    public GameObject shootPreFab;
    private float shootSpeed, shootCooldownC, shootCooldown;
    public GameObject[] shootPos;
    
    private Rigidbody rb;
    private Vector3 originalPos;
    private float variation;
    private bool isRight;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        entityStats = GetComponent<EntityStats>();
        variation = entityStats.MovementVariation;
        originalPos = transform.position;

        rb = GetComponent<Rigidbody>();
        isRight = true;
        rb.AddForce(500 * transform.right);
        

        //Debug.Log($"Original {originalPos}: {variation} seria a variacao. Com a soma maxima de {new Vector3(variation, 0, 0)} seria {maxPos}");

        shootSpeed = entityStats.bulletSpeed * 10000f;
        shootCooldown = entityStats.attackCooldown;
        shootCooldownC = shootCooldown;

        StartCoroutine(Movement());
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
                bullets[i].transform.LookAt(shootPos[i].transform.forward + bullets[i].transform.position);
                bullets[i].GetComponent<Rigidbody>().AddForce((bullets[i].transform.forward) * shootSpeed);
            }
        }
        shootCooldownC += Time.deltaTime;
    }

    IEnumerator Movement()
    {
        int timeLength = 120;
        while (true)
        {
            Vector3 position = transform.position;
            float distance = Vector3.Distance(originalPos, position);
            
            if (distance > variation)
            {
                if (isRight)
                {
                    rb.linearVelocity = Vector3.zero;
                    rb.AddForce(-500 * transform.right);
                    isRight = false;
                }
                else
                {
                    rb.linearVelocity = Vector3.zero;
                    rb.AddForce(500 * transform.right);
                    isRight = true;
                }
            }
            yield return new WaitForEndOfFrame();
        }
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
    }
}
