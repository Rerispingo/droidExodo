using UnityEngine;

public class PowerUpBehavior : MonoBehaviour
{
    public bool SpeedBoost;
    public bool Health;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

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
