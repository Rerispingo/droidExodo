using System;
using UnityEngine;

public class Enemy4_Behavior : MonoBehaviour
{
    public GameObject Player;
    
    private Rigidbody Rigidbody;
    private EntityStats EntityStats;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        Rigidbody = GetComponent<Rigidbody>();
        EntityStats = GetComponent<EntityStats>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(gameObject.transform.position, (Player.transform.position - gameObject.transform.position), out RaycastHit hit, (Mathf.Abs(Vector3.Distance(Player.transform.position, gameObject.transform.position)))+1))
        {
            if (hit.transform.gameObject.tag == "Player")
            {
                Move();
            }
        }
    }

    void Move()
    {
        Vector3 direction = (Player.transform.position - gameObject.transform.position).normalized;
        
        gameObject.transform.LookAt(Player.transform.position);
        Rigidbody.AddForce(direction * EntityStats.speed * Time.deltaTime );
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<EntityStats>().onHealthChangeEvent.Invoke(-10);
            gameObject.GetComponent<EntityStats>().onHealthChangeEvent.Invoke(-10000);
        }
    }
}
