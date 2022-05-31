using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D playerBody;
    private float moveHorizontal, moveVertical;
    [SerializeField] private float moveSpeed, jumpForce, invulnerableDuration;
    private bool isJumping, isVulnerable;
    private int money, hp;
    [SerializeField] private Text moneyText;
    [SerializeField] private GameObject deathScreen;
    public AudioSource coin, jump, death;

    private void Start()
    {
        Time.timeScale = 1;
        playerBody = GetComponent<Rigidbody2D>();
        money = 0;
        hp = 3;
        isJumping = false;
        isVulnerable = true;
    }

    private void Update()
    {
        moveHorizontal = Input.GetAxisRaw("Horizontal");
        moveVertical = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            moveSpeed = 2.8f;
            jumpForce = 45;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            moveSpeed = 2;
            jumpForce = 40;
        }

        if(hp <= 0)
        {
            deathScreen.SetActive(true);
            Time.timeScale = 0;
        }
    }

    private void FixedUpdate()
    {
        if(moveHorizontal > 0.1f || moveHorizontal < -0.1f)
        {
            playerBody.AddForce(new Vector2(moveHorizontal * moveSpeed, 0f), ForceMode2D.Impulse);

            if(moveHorizontal > 0.1f)
            {
                gameObject.transform.localScale = new Vector3(1, 1.5f, 1);
            }
            if (moveHorizontal < -0.1f)
            {
                gameObject.transform.localScale = new Vector3(-1, 1.5f, 1);
            }

            
        }

        if (moveVertical > 0.1f && !isJumping)
        {
            playerBody.AddForce(new Vector2(0f, moveVertical * jumpForce), ForceMode2D.Impulse);
            jump.Play();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Platform")
        {
            isJumping = false;
        }

        if(collision.tag == "Coin")
        {
            money++;
            moneyText.text = "Objects that i shoved up my ass: " + money.ToString();
            coin.Play();
        }

        if (collision.tag == "Trap" && isVulnerable)
        {
            /*
            hp--;
            Debug.Log("Health: " + hp.ToString());

            StartCoroutine(VulnerabilityFrames());

            if(hp <= 0)
            {
                Debug.Log("YOU DIED!!!");
                death.Play();
            }
            */

            hp = 0;
            death.Play();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Platform")
        {
            isJumping = true;
        }
    }

    private IEnumerator VulnerabilityFrames()
    {
        isVulnerable = false;
        yield return new WaitForSeconds(invulnerableDuration);
        isVulnerable = true;
    }
    
}
