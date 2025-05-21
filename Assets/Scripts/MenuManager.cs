using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance { get; private set; }

    private void Awake() //Singleton
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


    private bool IsPaused, isOptions;
    public GameObject CanvasMenu, CanvasOptions, optionsScreen;
    public List<int> scenesIndex;

    public AudioMixer audioMixer;
    private Slider[] slidersOptions;
    /*
     * 0 - Volume
     */


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scenesIndex = new List<int>() { 1, 2 };
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) //Pausar o Jogo
        {
            if (scenesIndex.Contains(SceneManager.GetActiveScene().buildIndex)) // Verificar se não está no menu
            {
                PauseGame();
            }
        }

        if (isOptions)
        {
            if (slidersOptions[0].value != 0)
            {
                audioMixer.SetFloat("MasterVolume", Mathf.Log10(slidersOptions[0].value) * 20);
            }
            else
            {
                audioMixer.SetFloat("MasterVolume", -80);
            }
        }
    }

    public void SceneTransition(int indexScene)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(indexScene);
    }

    public void PauseGame()
    {
        if (isOptions)
        {
            Options();
        }else
        {
            IsPaused = !IsPaused;
            CanvasMenu.SetActive(IsPaused);
        }
    }

    public void Options()
    {
        isOptions = !isOptions;
        if (isOptions)
        {
            optionsScreen = Instantiate(CanvasOptions);
            Button[] buttonList = optionsScreen.GetComponentsInChildren<Button>();

            slidersOptions = optionsScreen.GetComponentsInChildren<Slider>();
            float currentVolume;
            audioMixer.GetFloat("MasterVolume", out currentVolume);
            slidersOptions[0].value = Mathf.Pow(10, currentVolume / 20); // Conversão contraria (DB to 0-1)

            for (int i = 0; i < buttonList.Length; i++)
            {
                if (buttonList[i].gameObject.name == "ExitButton")
                {
                    buttonList[i].onClick.AddListener(Options);
                }
            }
        }
        else
        {
            Destroy(optionsScreen);
        }


    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
