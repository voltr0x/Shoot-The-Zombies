using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    private Animator playerAnim;
    private bool isMoving;
    Vector3 lastPosition;

    public DynamicJoystick moveJoystick;
    public DynamicJoystick rotateJoystick;
    public GameObject dynamicJoystick;
    float horizontalInput;
    float verticalInput;

    Plane plane;
    float speed = 0.08f;

    public GameObject bulletPrefab;
    public Transform firePoint;
    float bulletForce = 30f;
    public LineRenderer lineRenderer;

    public float maxHealth = 100f;
    public float currentHealth;
    public GameManager gameManager;
    public int ammo;

    public bool isAlive;

    // Start is called before the first frame update
    void Start()
    {
        playerAnim = GetComponent<Animator>();
        plane = new Plane(Vector3.up, Vector3.zero);
        isAlive = true;
        isMoving = false;
        lastPosition = transform.position;

        currentHealth = maxHealth;
        gameManager.SetMaxHealth(maxHealth);
        ammo = 60;
        gameManager.SetAmmoCount(ammo);

        dynamicJoystick.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (isAlive)
        {
            //Get input
            horizontalInput = moveJoystick.Horizontal;
            verticalInput = moveJoystick.Vertical;

            //Start animation if player is moving
            if (isMoving)
                playerAnim.SetFloat("Speed", 1f);
            else
                playerAnim.SetFloat("Speed", 0f);

            //Aim
            LookDirection();
        }
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

    //Rotate by joystick - For mobile version
    void LookDirection()
    {
        //Get input
        float horizontal = rotateJoystick.Horizontal;
        float vertical = rotateJoystick.Vertical;

        Vector2 convertedXY = ConvertWithCamera(Camera.main.transform.position, horizontal, vertical);
        Vector3 direction = new Vector3(convertedXY.x, 0, convertedXY.y).normalized;
        Vector3 lookAtPosition = transform.position + direction;
        transform.LookAt(lookAtPosition);

    }
    
    //To face the character towards the cursor - For PC version
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

    public void Shoot()
    {
        if(ammo > 0)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            rb.AddForce(firePoint.up * bulletForce, ForceMode.Impulse);
            ammo--;
            gameManager.SetAmmoCount(ammo);
        }     
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        gameManager.SetHealth(currentHealth);
        if (currentHealth <= 0)
            Die();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ammo"))
        {
            ammo += 40;
            gameManager.SetAmmoCount(ammo);
            Destroy(other.gameObject);
        }
        if (other.CompareTag("Health"))
        {
            currentHealth += 20f;
            if(currentHealth > maxHealth)
                currentHealth = maxHealth;
            gameManager.SetHealth(currentHealth);
            Destroy(other.gameObject);
        }
    }

    void Die()
    {
        //TO-DO: Death animation
        dynamicJoystick.SetActive(false);
        isAlive = false;
    }
}
