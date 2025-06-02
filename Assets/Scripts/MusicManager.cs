using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
public class MusicManager : MonoBehaviour
{
    public Musics musicsSO; //Scriptable object com a lista das musicas
    public AudioSource currentMusic;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        try
        {
            StartCoroutine(playMusic(musicsSO.listMusic[sceneIndex]));
        }
        catch (Exception)
        {
            StartCoroutine(playMusic(musicsSO.listMusic[0]));
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator playMusic(AudioClip music)
    {
        yield return new WaitForSeconds(2);
        try
        {
            currentMusic.Stop();
        }
        catch (Exception)
        {
            currentMusic.resource = music;
            currentMusic.Play();
        }
    }
}
