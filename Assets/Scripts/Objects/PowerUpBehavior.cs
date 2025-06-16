using UnityEngine;

public class PowerUpBehavior : MonoBehaviour
{
    public bool SpeedBoost;
    public bool Health;

    public float rotationSpeed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (SpeedBoost)
            {
                other.gameObject.GetComponent<EntityStats>().SpeedBoost();
                Destroy(gameObject);
            }else if (Health)
            {
                other.gameObject.GetComponent<EntityStats>().HealthBoost();
                Destroy(gameObject);
            }
            
        }

    }
}
