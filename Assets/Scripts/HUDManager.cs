using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance { get; private set; } //Singleton
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {

            Destroy(this);

        }
        else
        {

            Instance = this;

        }
    }

    private GameObject Player;
    public GameObject HealthBar;
    public TextMeshProUGUI EnemyCountText;
    public GameObject SpeedBoostText;

    private EntityStats playerStats;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpeedBoostText.SetActive(false);

        IEnumerator wait()
        {
            yield return new WaitForEndOfFrame();

            Player = GameObject.FindGameObjectWithTag("Player");
            playerStats = Player.GetComponent<EntityStats>();

            playerStats.onHealthChangeEvent.AddListener(HealthHudChange);
            HealthBar.GetComponent<Slider>().value = playerStats.health / playerStats.maxHealth;
        }
        StartCoroutine(wait());
    }

    // Update is called once per frame
    void Update()
    {
        int quantEnemy = LevelManager.Instance.QuantEnemy;
        if (quantEnemy != 0)
        {
            EnemyCountText.text = $"Enemies left: {quantEnemy}";
        }else
        {
            EnemyCountText.text = $"Passe pela porta";
        }
        
    }

    void HealthHudChange(float value)
    {
        HealthBar.GetComponent<Slider>().value = playerStats.health / playerStats.maxHealth;
    }
}
