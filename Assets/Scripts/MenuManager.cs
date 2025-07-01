using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;
using System;
using System.Collections;
using System.Linq;
using Unity.VisualScripting;

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

    private bool isOptions, isExitConfirm;
    public bool IsPaused;
    public bool InGame, InLoseScreen;
    public GameObject CanvasMenu, CanvasOptions, optionsScreen, exitConfirmPreFab;
    public AudioSource DroneSound;
    public AudioSource Click;

    private GameObject exitConfirm;
    public int[] scenesIndex;
    public AudioMixer audioMixer;
    private Slider[] slidersOptions;
    private String[] mixerGroups;
    private float effectsVolume, musicVolume;
    /*
     * 0 - Volume
     */

    public GameObject HelpScreen;
    private bool helpOpened;
    public GameObject CreditsScreen;
    private bool CreditsOpened;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Definindo o mixer groups
        mixerGroups = new string[] {
            "effectsVolume",
            "musicVolume"
        };

        effectsVolume = PlayerPrefs.GetFloat("effectsVolume", 0.5f);
        musicVolume = PlayerPrefs.GetFloat("musicVolume", 0.5f);

        audioMixer.SetFloat(mixerGroups[0], Mathf.Log10(effectsVolume) * 20);
        audioMixer.SetFloat(mixerGroups[1], Mathf.Log10(musicVolume) * 20);


        scenesIndex = new int[3] { 1, 2, 3 };

        isExitConfirm = false;
        isOptions = false;

        helpOpened = false;
        CreditsOpened = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (InGame)
        {
            if (IsPaused)
            {
                Time.timeScale = 0;
                MusicManager.Instance.currentMusic.Pause();
                DroneSound.Pause();
            }
            else
            {
                Time.timeScale = 1;
                MusicManager.Instance.currentMusic.UnPause();
                DroneSound.UnPause();
            }
        }
        
        
        
        if (!InLoseScreen)
        {
            if (Input.GetKeyDown(KeyCode.Escape)) //Pausar o Jogo
            {
                for (int i = 0; i < scenesIndex.Length; i++)
                {
                    if (scenesIndex[i] == SceneManager.GetActiveScene().buildIndex) // Verificar se n�o est� no menu
                    {
                        PauseGame();
                    }
                }

            }

            if (Input.GetKeyDown(KeyCode.Return) && isExitConfirm)
            {
                ExitGameConfirm();
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (isOptions)
                {
                    Options();
                }
                else if (isExitConfirm)
                {
                    ExitGameConfirmClose();
                }
                else if (!InGame)
                {
                    ExitGame();
                }
            }
            if (Input.GetKeyDown(KeyCode.O))
            {
                Options();
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                SceneTransition(1);
            }

            if (Input.GetKeyDown(KeyCode.M))
            {
                if (InGame)
                {
                    SceneTransition(0);
                }
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneTransition(1);
            }
            if (Input.GetKeyDown(KeyCode.M))
            {
                SceneTransition(0);
            }
        }

    }

    public void SceneTransition(int indexScene)
    {
        StartCoroutine(SceneTransitionAsync(indexScene));
    }

    IEnumerator SceneTransitionAsync(int indexScene)
    {
        AsyncOperation scene = SceneManager.LoadSceneAsync(indexScene);

        while (!scene.isDone)
        {
            yield return new WaitForEndOfFrame();
        }
    }

    public void PauseGame()
    {
        if (isOptions)
        {
            Options();
        }
        else
        {
            IsPaused = !IsPaused;
            Debug.Log(IsPaused);
            CanvasMenu.SetActive(IsPaused);
        }
    }

    public void Options() // Abrir opcoes dentro do pause.
    {
        isOptions = !isOptions;
        Debug.Log(isOptions);
        if (isOptions)
        {
            optionsScreen = Instantiate(CanvasOptions);
            Button[] buttonList = optionsScreen.GetComponentsInChildren<Button>();
            Debug.Log("Chegou Aqui");

            slidersOptions = optionsScreen.GetComponentsInChildren<Slider>(); // Lista das opcoes
            Debug.Log("Chegou Aqui");

            // Lista de cada opcao sendo configurada
            audioMixer.GetFloat(mixerGroups[0], out float currentEffectsVolume);
            slidersOptions[0].value = Mathf.Pow(10, currentEffectsVolume / 20); // Conversao contraria (DB to 0-1)
            slidersOptions[0].onValueChanged.AddListener(delegate { SliderChange(0); });
            Debug.Log("Chegou Aqui");

            audioMixer.GetFloat(mixerGroups[1], out float currentMusicVolume);
            slidersOptions[1].value = Mathf.Pow(10, currentMusicVolume / 20);
            slidersOptions[1].onValueChanged.AddListener(delegate { SliderChange(1); });
            
            Debug.Log("Chegou Aqui");


            for (int i = 0; i < buttonList.Length; i++) // Configurar botao Exit.
            {
                if (buttonList[i].gameObject.name == "ExitButton")
                {
                    buttonList[i].onClick.AddListener(Options);
                    buttonList[i].onClick.AddListener(PlayClick);
                }
            }
        }
        else
        {
            Destroy(optionsScreen);
        }
    }

    public void SliderChange(int index)
    {
        float value = -80;
        if (slidersOptions[index].value != 0) // Alterar volume
        {
            value = Mathf.Log10(slidersOptions[index].value) * 20; // 0-1 p/ DB
        }

        audioMixer.SetFloat(mixerGroups[index], value);
        switch (index)
        {
            case 0:
                PlayerPrefs.SetFloat("effectsVolume", slidersOptions[index].value);
                break;
            case 1:
                PlayerPrefs.SetFloat("musicVolume", slidersOptions[index].value);
                break;
            default:
                break;
        }

        PlayerPrefs.Save();
    }

    public void ExitGame()
    {
        isExitConfirm = true;
        exitConfirm = Instantiate(exitConfirmPreFab, CanvasMenu.transform);
        Button[] Buttons = exitConfirm.GetComponentsInChildren<Button>();

        Buttons[0].onClick.AddListener(ExitGameConfirm);
        Buttons[0].onClick.AddListener(PlayClick);
        Buttons[1].onClick.AddListener(ExitGameConfirmClose);
        Buttons[1].onClick.AddListener(PlayClick);
    }

    public void ExitGameConfirmClose()
    {
        isExitConfirm = false;
        Destroy(exitConfirm);
    }

    public void ExitGameConfirm()
    {
        PlayerPrefs.SetInt("video", 1);
        Application.Quit();
        Debug.Log("saiu.");
    }

    public void PlayClick()
    {
        Click.Play();
    }

    public void Help()
    {
        helpOpened = !helpOpened;
        HelpScreen.SetActive(helpOpened);
    }
    
    public void Credits()
    {
        CreditsOpened = !CreditsOpened;
        CreditsScreen.SetActive(CreditsOpened);
    }
}
