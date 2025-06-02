using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;
using System;
using System.Collections;
using System.Linq;

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
    public int[] scenesIndex;
    public AudioMixer audioMixer;
    private Slider[] slidersOptions;
    private String[] mixerGroups;
    /*
     * 0 - Volume
     */


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scenesIndex = new int[2] { 1, 2 };
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) //Pausar o Jogo
        {
            for (int i=0; i<scenesIndex.Length; i++)
            {
                if (scenesIndex[i] == SceneManager.GetActiveScene().buildIndex) // Verificar se n�o est� no menu
                {
                    PauseGame();
                }
            }
            
        }
    }

    public void SceneTransition(int indexScene)
    {
        Time.timeScale = 0;
        StartCoroutine(SceneTransitionAsync(indexScene));
    }

    IEnumerator SceneTransitionAsync(int indexScene)
    {
        AsyncOperation scene = SceneManager.LoadSceneAsync(indexScene);
        
        while (!scene.isDone)
        {
            yield return null;
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
            CanvasMenu.SetActive(IsPaused);
        }
    }

    public void Options() // Abrir opcoes dentro do pause.
    {
        isOptions = !isOptions;
        if (isOptions)
        {
            optionsScreen = Instantiate(CanvasOptions);
            Button[] buttonList = optionsScreen.GetComponentsInChildren<Button>();

            slidersOptions = optionsScreen.GetComponentsInChildren<Slider>(); // Lista das opcoes
            mixerGroups = new string[] {
                "effectsVolume",
                "musicVolume"
            };

            // Lista de cada opcao sendo configurada
            audioMixer.GetFloat(mixerGroups[0], out float currentEffectsVolume);
            slidersOptions[0].value = Mathf.Pow(10, currentEffectsVolume / 20); // Conversao contraria (DB to 0-1)
            slidersOptions[0].onValueChanged.AddListener(delegate { SliderChange(0); });

            audioMixer.GetFloat(mixerGroups[1], out float currentMusicVolume);
            slidersOptions[1].value = Mathf.Pow(10, currentMusicVolume / 20);
            slidersOptions[1].onValueChanged.AddListener(delegate { SliderChange(1); });

            for (int i = 0; i < buttonList.Length; i++) // Configurar botao Exit.
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

    public void SliderChange(int index)
    {
        if (slidersOptions[index].value != 0) // Alterar volume
        {
            audioMixer.SetFloat(mixerGroups[index], Mathf.Log10(slidersOptions[index].value) * 20);//Converter 0-1 p DB
        }
        else
        {
            audioMixer.SetFloat(mixerGroups[index], -80);
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
