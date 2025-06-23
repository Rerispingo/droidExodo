using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.UI;

public class bulletBehavior : MonoBehaviour
{
    public GameObject parent;
    public string tagTarget;
    public float damage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    private void OnTriggerEnter(Collider other) {
        
        if (other.gameObject.tag == tagTarget)
        {
            Debug.Log("Colidiu");
            other.gameObject.GetComponent<EntityStats>().onHealthChangeEvent.Invoke(-damage); //Dano do tiro
            Destroy(gameObject);
        }else if(other.gameObject != parent && other.gameObject.tag != "Bullet" && other.gameObject.tag != "PowerUp")
        {
            Destroy(gameObject);
        }
        
    }
}
