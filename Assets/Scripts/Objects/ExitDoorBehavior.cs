using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitDoorBehavior : MonoBehaviour
{
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
        MenuManager.Instance.SceneTransition(2);
    }
}
