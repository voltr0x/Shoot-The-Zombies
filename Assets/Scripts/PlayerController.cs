using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    private Animator playerAnim;
    private bool isMoving;
    Vector3 lastPosition;

    private float horizontalInput;
    private float verticalInput;

    Plane plane;
    float speed = 0.08f;

    public GameObject bulletPrefab;
    public Transform firePoint;
    float bulletForce = 30f;

    private float health = 100f;
    public bool isAlive;

    // Start is called before the first frame update
    void Start()
    {
        playerAnim = GetComponent<Animator>();
        plane = new Plane(Vector3.up, Vector3.zero);
        isAlive = true;
        isMoving = false;
        lastPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //Get input
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        //Start animation if player is moving
        if (isMoving)
            playerAnim.SetFloat("Speed", 1f);
        else
            playerAnim.SetFloat("Speed", 0f);

        //Aim player
        FaceMouse();

        if (Input.GetButtonDown("Fire1"))
            Shoot();

        Debug.Log("Player Health: " + health);
    }

    private void FixedUpdate()
    {
        //Player movement
        Vector2 convertedXY = ConvertWithCamera(Camera.main.transform.position, horizontalInput, verticalInput);
        Vector3 direction = new Vector3(convertedXY.x, 0, convertedXY.y).normalized;
        transform.Translate(direction * speed, Space.World);

        //To check if the player moved
        if (transform.position != lastPosition)
            isMoving = true;
        else
            isMoving = false;
        lastPosition = transform.position;
    }

    //To move in perspective of the camera
    private Vector2 ConvertWithCamera(Vector3 cameraPos, float horizontal, float vertical)
    {
        Vector2 movDirection = new Vector2(horizontal, vertical).normalized;
        Vector2 camera2DPos = new Vector2(cameraPos.x, cameraPos.z);
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

    //To face the character towards the cursor
    void FaceMouse()
    {
        Vector3 mousePosition = Input.mousePosition;
        var ray = Camera.main.ScreenPointToRay(mousePosition);

        if (plane.Raycast(ray, out var enter))
        {
            var hitPoint = ray.GetPoint(enter);
            var playerPositionOnPlane = plane.ClosestPointOnPlane(transform.position);
            transform.rotation = Quaternion.LookRotation(hitPoint - playerPositionOnPlane);
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.AddForce(firePoint.up * bulletForce, ForceMode.Impulse);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
            Die();
    }
    void Die()
    {
        //TO-DO: Death animation & Game over screen
        isAlive = false;
        Debug.Log("GAME OVER!");
    }
}
