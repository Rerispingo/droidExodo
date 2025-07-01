using UnityEngine.Video;
using UnityEngine;
using System.Collections;

public class VideoManager : MonoBehaviour
{
    public GameObject Canvas;
    public GameObject Video;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int value = PlayerPrefs.GetInt("video", 1);
        if (value == 1)
        {
            StartCoroutine(WaitVideo());
        }
        else
        {
            Video.SetActive(false);
            Canvas.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator WaitVideo()
    {
        float duration = (float)Video.GetComponent<VideoPlayer>().clip.length;
        yield return new WaitForSeconds(duration+3);
        Video.SetActive(false);
        Canvas.SetActive(true);
        PlayerPrefs.SetInt("video", 0);
    }
}
