using System.Collections;
using UnityEngine;

public class Enemy_Behavior : MonoBehaviour
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

        StartCoroutine(Movement());
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
