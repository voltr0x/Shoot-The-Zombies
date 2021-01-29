using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;
    private Animator playerAnim;

    private float horizontalInput;
    private float verticalInput;

    NavMeshAgent agent;
    Vector3 mov;
    Vector3 mousePos;

    [SerializeField] private float speed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        playerAnim = GetComponent<Animator>();
        playerRb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        //Get input
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        
        //To aim the player
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        playerAnim.SetFloat("Speed", verticalInput);

        //FaceMouse();

        //transform.LookAt(Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }

    private void FixedUpdate()
    {
        Vector2 convertedXY = ConvertWithCamera(Camera.main.transform.position, horizontalInput, verticalInput);
        Vector3 direction = new Vector3(convertedXY.x, 0, convertedXY.y).normalized;
        transform.Translate(direction * 0.08f, Space.World);

        //transform.Translate(Vector3.forward * Time.deltaTime * speed * verticalInput);
        //transform.Translate(Vector3.right * Time.deltaTime * speed * horizontalInput);

        //Vector3 lookDir = mousePos - playerRb.position;
        //float angle = Mathf.Atan2(lookDir.z, lookDir.x) * Mathf.Rad2Deg - 90f;
        //playerRb.rotation = Quaternion.AngleAxis(angle, Vector3.up);

        if (Input.GetKey(KeyCode.Q))
            transform.Rotate(Vector3.up, Time.deltaTime * -1 * 100f);
        if (Input.GetKey(KeyCode.E))
            transform.Rotate(Vector3.up, Time.deltaTime * 100f);
    }

    private Vector2 ConvertWithCamera(Vector3 cameraPos, float horizontal, float vertical)
    {
        Vector2 movDirection = new Vector2(horizontal, vertical).normalized;
        Vector2 camera2DPos = new Vector2(cameraPos.x, cameraPos.z);
        Vector2 playerPos = new Vector2(transform.position.x, transform.position.z);
        Vector2 camToPlayerDirection = (Vector2.zero - camera2DPos).normalized;
        float angle = Vector2.SignedAngle(camToPlayerDirection, new Vector2(0, 1));
        Vector2 finalDirection = RotateVector(movDirection, -angle);
        return finalDirection;
    }

    public Vector2 RotateVector(Vector2 v, float angle)
    {
        float radian = angle * Mathf.Deg2Rad;
        float _x = v.x * Mathf.Cos(radian) - v.y * Mathf.Sin(radian);
        float _y = v.x * Mathf.Sin(radian) + v.y * Mathf.Cos(radian);
        return new Vector2(_x, _y);
    }

    void FaceMouse()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        Vector3 relativePos = mousePosition - transform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
        transform.rotation = rotation;
    }
}
