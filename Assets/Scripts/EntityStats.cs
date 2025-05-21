using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

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

    void Start()
    {
        health = maxHealth;
        speedDefault = speed;

        onHealthChangeEvent = new HealthChange();
        onHealthChangeEvent.AddListener(OnHealthChange);
    }

    void OnHealthChange(float value)
    {
        health += value;
        if (health > maxHealth)
        {
            health = maxHealth;
        }

        if (health <= 0 && gameObject.CompareTag("Player"))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("zDerrota");
        }
        else if (health <= 0 && gameObject.CompareTag("Enemy"))
        {
            LevelManager.Instance.QuantEnemy -= 1;
            if (Random.Range(0, 2) == 0)
            {
                GameObject HealthPU = Instantiate(HealthPowerUp);
                HealthPU.transform.position = gameObject.transform.position;
            }
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
        speed *= speedBoostMultiplier;
        HUDManager.Instance.SpeedBoostText.SetActive(true);
        yield return new WaitForSeconds(speedBoostDuration);
        speed = speedDefault;
        HUDManager.Instance.SpeedBoostText.SetActive(false);
    }

    public void HealthBoost()
    {
        onHealthChangeEvent.Invoke(maxHealth/5);
    }
}
