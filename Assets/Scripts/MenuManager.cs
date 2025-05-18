using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
            if (scenesIndex.Contains(SceneManager.GetActiveScene().buildIndex))
            {
                PauseGame();
            }
        }
    }

    public void SceneTransition(int indexScene)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(indexScene);
    }

    public void PauseGame()
    {
        IsPaused = !IsPaused;
        CanvasMenu.SetActive(IsPaused);
    }

    public void Options()
    {
        isOptions = !isOptions;
        if (isOptions)
        {
            optionsScreen = Instantiate(CanvasOptions);
            Button[] buttonList = optionsScreen.GetComponentsInChildren<Button>();
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
