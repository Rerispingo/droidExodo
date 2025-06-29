using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class EntityStats : MonoBehaviour
{
    public class HealthChange: UnityEvent<float> { }
    public HealthChange onHealthChangeEvent;

    public float speed, speedDefault;
    public float health;
    public float maxHealth;
    public float damage;
    public float attackCooldown;
    public float chargeAttackCooldown;
    public float bulletSpeed;
    public float MovementVariation;

    public GameObject HealthBar;
    public GameObject HealthPowerUp;

    private float speedBoostDuration=5f;
    private float speedBoostMultiplier=1.5f;
    private float fireRateDuration=5f;

    public GameObject HitSound;
    public GameObject DeathSound;
    public GameObject PowerUpSound;


    void Start()
    {
        health = maxHealth;
        speedDefault = speed;

        onHealthChangeEvent = new HealthChange();
        onHealthChangeEvent.AddListener(OnHealthChange);
    }

    void OnHealthChange(float value)
    {
        if (gameObject.CompareTag("Player") && (value < 0))
        {
            HitSound.GetComponent<AudioSource>().Play();
        }
        
        Debug.Log("Vida Alterou");
        
        health += value;
        if (health > maxHealth)
        {
            health = maxHealth;
        }

        if (health <= 0 && gameObject.CompareTag("Player"))
        {
            GameObject deathSoundI = GameObject.Instantiate(DeathSound, transform);
            UnityEngine.SceneManagement.SceneManager.LoadScene("zDerrota");
            
        }
        else if (health <= 0 && gameObject.CompareTag("Enemy"))
        {
            LevelManager.Instance.QuantEnemy -= 1;
            if (!(value == -10000))
            {
                if (Random.Range(0, 2) == 0)
                {
                    GameObject HealthPU = Instantiate(HealthPowerUp);
                    HealthPU.transform.position = gameObject.transform.position;
                }
            }
            GameObject deathSoundI = GameObject.Instantiate(DeathSound);
            deathSoundI.transform.position = transform.position;
            deathSoundI.GetComponent<AudioSource>().Play();
            Destroy(gameObject);
        }
    }
    

    // ------------------------- POWER UP ------------------------
    public void SpeedBoost()
    {
        StartCoroutine(SpeedBoost_I());
    }

    IEnumerator SpeedBoost_I()
    {
        PowerUpSound.GetComponent<AudioSource>().Play();
        speed *= speedBoostMultiplier;
        HUDManager.Instance.SpeedBoostText.SetActive(true);
        yield return new WaitForSeconds(speedBoostDuration);
        speed = speedDefault;
        HUDManager.Instance.SpeedBoostText.SetActive(false);
    }

    public void HealthBoost()
    {
        onHealthChangeEvent.Invoke(maxHealth / 5);

        PowerUpSound.GetComponent<AudioSource>().Play();
    }

    public void FireRateBoost()
    {
        StartCoroutine(FireRateBoost_I());
    }

    IEnumerator FireRateBoost_I()
    {
        PowerUpSound.GetComponent<AudioSource>().Play();
        HUDManager.Instance.FireRateBoostText.SetActive(true);
        attackCooldown /= 2;
        yield return new WaitForSeconds(fireRateDuration);
        HUDManager.Instance.FireRateBoostText.SetActive(false);
        attackCooldown *= 2;
    }
}
