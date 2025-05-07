using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance { get; private set; }

    private void Awake() 
    { 
        if(Instance != null && Instance != this) 
        { 

            Destroy(this); 

        } 
        else 
        { 

            Instance = this; 

        } 
    }



    private bool IsPaused;
    public GameObject CanvasMenu;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) //Pausar o Jogo
        {
            PauseGame();
        }
    }

    public void SceneTransition(int indexScene)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(indexScene);
    }

    public void PauseGame()
    {
        Debug.Log(IsPaused);
        IsPaused = !IsPaused;
        CanvasMenu.SetActive(IsPaused);
    }
}
