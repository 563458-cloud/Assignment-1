using TMPro;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
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
    TMP_Text sanityTextComponent;
    public Light2D redLight;

    void Start()
    {
        sanityTextComponent = sanityTextObject.GetComponent<TMP_Text>();
        moveAction = InputSystem.actions.FindAction("Move");
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
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
        //redLight.pointLightOuterRadius = Mathf.Clamp(30f - (sanityValue / 2f), 0f, 50f);

        sanityTextComponent.text = Mathf.Round(sanityValue).ToString();
        Vector2 moveVel = moveAction.ReadValue<Vector2>();
        rb.linearVelocity = new Vector2(moveVel.x * playerSpeed, moveVel.y * playerSpeed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Door"))
        {
            print("You win!");
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
        print("Dead!!");
        SceneManager.LoadScene("SampleScene");
    }
}
