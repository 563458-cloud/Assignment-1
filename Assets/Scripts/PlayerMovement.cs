using TMPro;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class PlayerMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    InputAction moveAction;
    Rigidbody2D rb;
    float playerSpeed = 2;
    float sanityValue = 100;
    bool nearLamp = false;
    float sanityDecreasePerSecond = 3;
    float sanityGainPerSecond = 5;
    public GameObject sanityTextObject;
    public Light2D redLight;
    public Slider sanitySlider;
    HelperScript helper;

    void Start()
    {
        helper = gameObject.AddComponent<HelperScript>();

        sanitySlider.value = 100;
        moveAction = InputSystem.actions.FindAction("Move");
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        ManageSanity();
        Move();
    }

    void Move()
    {
        Vector2 moveVel = moveAction.ReadValue<Vector2>();
        rb.linearVelocity = new Vector2(moveVel.x * playerSpeed, moveVel.y * playerSpeed);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Door"))
        {
            helper.LoadNextScene();
        }
        else if (collision.CompareTag("Lamp"))
        {
            nearLamp = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Lamp"))
        {
            nearLamp = false;
        }
    }
    void Die()
    {
        SceneManager.LoadScene("StartScene");
    }
    void ManageSanity()
    {
        if (!nearLamp)
        {
            sanityValue -= sanityDecreasePerSecond * Time.deltaTime;
        }
        else
        {
            sanityValue += sanityGainPerSecond * Time.deltaTime;
        }
        sanityValue = Mathf.Clamp(sanityValue, 0f, 100f);

        if (sanityValue <= 0)
        {
            Die();
            return;
        }
        sanitySlider.value = sanityValue;
    }
}
