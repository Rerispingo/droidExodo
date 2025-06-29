using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; } //Singleton

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

        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }

    public float wallDeathSpeed;
    public int QuantEnemy;
    public float WallDeathPosition;
    public GameObject WallDeath;
    public GameObject ExitTrigger;
    public GameObject tutorialText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        QuantEnemy = 1; //Evitar fase ser completada inicio (condicao e ser 0 para terminar a fase)
        StartCoroutine(ObjectLoader());
    }

    private IEnumerator ObjectLoader()
    {
        yield return new WaitForEndOfFrame();

        QuantEnemy = GameObject.FindGameObjectsWithTag("Enemy").Length; //Enemy Count

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.CapsLock))
        {
            int nextScene = SceneManager.GetActiveScene().buildIndex + 1;
            if (nextScene > 3)
            {
                nextScene = 1;
            }
            SceneManager.LoadScene(nextScene);
        }
    }

    void FixedUpdate()
    {
        if (QuantEnemy <= 0)
        {
            ExitTrigger.SetActive(true);
        }

        WallDeath.transform.position += new Vector3(0, 0, wallDeathSpeed) * Time.deltaTime;
        WallDeathPosition = WallDeath.transform.position.z;
    }
}
