using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class playerMovement : MonoBehaviour
{
    private EntityStats entityStats;
    private float final_speed;

    private Rigidbody rb;
    private Camera cam;

    public float minZ,maxZ;
    public float minX,maxX;
    public float offsetZ;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main;
        entityStats = GetComponent<EntityStats>();
    }

    void Update()
    {
        final_speed = entityStats.speed * 500; // Compensar Time.deltaTime


        // Movimentacao da camera.
        if (transform.position.x > minX && transform.position.x < maxX)
        {
            cam.transform.position = new Vector3(transform.position.x, cam.transform.position.y, cam.transform.position.z);
        }
        if (transform.position.z > minZ && transform.position.z < maxZ)
        {
            cam.transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y, transform.position.z);
        }

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

        //Rotacao do player
        Vector3 direction = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.transform.position.y - transform.position.y));
        direction = new Vector3(direction.x, gameObject.transform.position.y, direction.z);

        transform.LookAt(direction);
    }
}
