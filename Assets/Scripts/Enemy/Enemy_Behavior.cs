using System.Collections;
using UnityEngine;

public class Enemy_Behavior : MonoBehaviour
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

        StartCoroutine(Movement());
    }

    // Update is called once per frame
    void Update()
    {
        if (shootCooldownC >= shootCooldown)
        {
            shootCooldownC = 0f;
            float offsetShoot = 2f;

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
        float variation = entityStats.MovementVariation;

        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - (variation / 2));

        if (variation > 0)
        {
            while (true)
            {
                for (int i = 0; i < timeLength; i++)
                {
                    transform.position += new Vector3(0, 0, variation / timeLength);
                    yield return new WaitForFixedUpdate();
                }
                for (int i = 0; i < timeLength; i++)
                {
                    transform.position -= new Vector3(0, 0, variation / timeLength);
                    yield return new WaitForFixedUpdate();
                }
            }
        }

    }
}
