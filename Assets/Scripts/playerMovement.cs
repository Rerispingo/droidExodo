using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class playerMovement : MonoBehaviour
{
    public float helice_rs;
    public GameObject[] helices;
    
    private EntityStats entityStats;
    private float final_speed;

    private Rigidbody rb;
    private Camera cam;

    public float offsetYCamera;
    public float offsetZ;

    public float rotationVelocity;
    private bool isKeyboardRotation;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main;
        entityStats = GetComponent<EntityStats>();
    }

    void Update()
    {
        for (int i = 0; i < helices.Length ; i++)
        {
            helices[i].transform.Rotate(Vector3.forward * helice_rs);
        }
        
        final_speed = entityStats.speed * 500; // Compensar Time.deltaTime

           //Movimenta��o da C�mera
        cam.transform.position = transform.position + new Vector3(0, offsetYCamera, 0);

        float WallDeathRelativePos = LevelManager.Instance.WallDeathPosition + offsetZ;
        if (cam.transform.position.z < WallDeathRelativePos)
        {
            cam.transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y, WallDeathRelativePos);
        }

        // Movimentacao
        if (Input.GetKey(KeyCode.W))
        {
            rb.AddForce(Vector3.forward * final_speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            rb.AddForce(Vector3.forward * -final_speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            rb.AddForce(Vector3.right * -final_speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            rb.AddForce(Vector3.right * final_speed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.E))
        {
            isKeyboardRotation = true;
            rb.transform.Rotate(10 * rotationVelocity * Time.deltaTime * new Vector3(0,1,0));
        }
        if (Input.GetKey(KeyCode.Q))
        {
            isKeyboardRotation = true;
            rb.transform.Rotate(10 * rotationVelocity * Time.deltaTime * new Vector3(0,-1,0));
        }

        if (Input.GetMouseButtonDown(0))
        {
            isKeyboardRotation = false;
        }

        if (!isKeyboardRotation)
        {
            //Rotacao do player
            Vector3 direction = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.transform.position.y - transform.position.y));
            direction = new Vector3(direction.x, gameObject.transform.position.y, direction.z);

            transform.LookAt(direction);
        }
        
    }
}
