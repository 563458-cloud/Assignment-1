using TMPro;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


/*
    TODO:
    collectable items
    jumpscare
    cinemachine camera lagging behind
 */
public class PlayerMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    InputAction moveAction;
    InputAction sprintAction;
    Rigidbody2D rb;
    float playerSpeed = 2;
    float sprintMult = 2;
    float sanityValue = 100;
    bool nearLamp = false;
    bool isSprinting = false;
    bool canSprint = true;
    float stamina = 100;
    float staminaDecreasePerSecond = 25;
    float staminaGainPerSecond = 1;
    float sanityDecreasePerSecond = 3;
    float sanityGainPerSecond = 5;
    public GameObject sanityTextObject;
    public Light2D redLight;
    public Slider sanitySlider;
    public Slider staminaSlider;
    HelperScript helper;
    private Animator animator;
    float lastInputX = 0;
    float lastInputY = -1;
    

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        helper = gameObject.AddComponent<HelperScript>();

        sanitySlider.value = 100;
        staminaSlider.value = 100;

        moveAction = InputSystem.actions.FindAction("Move");
        sprintAction = InputSystem.actions.FindAction("Sprint");
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
        Vector2 normalizedMovement = moveVel.normalized;
        bool isWalking = moveVel.magnitude > 0.01f;
        isSprinting = sprintAction.IsPressed() && stamina > 0 && isWalking && canSprint;

        if (isSprinting)
        {
            stamina -= staminaDecreasePerSecond * Time.deltaTime;
            if (animator.speed != sprintMult)
            {
                animator.speed = sprintMult;

            }
        }
        else
        {
            stamina += staminaGainPerSecond * Time.deltaTime;
            if (animator.speed != 1)
            {
                animator.speed = 1;

            }
        }
        staminaSlider.value = stamina;


    float currentSpeed = isSprinting ? playerSpeed * sprintMult : playerSpeed;
        rb.linearVelocity = normalizedMovement * currentSpeed;



        animator.SetBool("isWalking", isWalking);
        if (isWalking){
        lastInputY = moveVel.y;
        lastInputX = moveVel.x;
        animator.SetFloat("inputX", moveVel.x);
        animator.SetFloat("inputY", moveVel.y);
    }   else
            {
                animator.SetFloat("lastInputX",lastInputX);

                animator.SetFloat("lastInputY",lastInputY);
             }

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
        else if (collision.CompareTag("NoSprint"))
        {
            canSprint = false;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Lamp"))
        {
            nearLamp = false;
        }
        else if (collision.CompareTag("NoSprint"))
        {
            canSprint = true;
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
