using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject gameManager;

    Rigidbody2D rb;
    Animator anim;
    CapsuleCollider2D capCol;
    public GameObject groundCheck;
    public Text scoreText;

    Vector2 horizontal;
    Vector2 vertical;
    Vector2 characterScale;
    Vector2 defaultCapColSize;
    Vector2 defaultCapColOffset;

    float direction;
    float speed = 10f;
    float jump = 800f;
    float scaleX, scaleY;
    bool isGrounded;

    public int score;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        capCol = GetComponent<CapsuleCollider2D>();

        scaleX = transform.localScale.x;
        scaleY = transform.localScale.y;

        defaultCapColSize = capCol.size; 
        defaultCapColOffset = capCol.offset;

        if (!PlayerPrefs.HasKey("score"))
        {
            score = 0;
        }
        else
        {
            transform.position = new Vector3(PlayerPrefs.GetFloat("positionx"), PlayerPrefs.GetFloat("positiony"), 0);
            score = PlayerPrefs.GetInt("score");
        }

        scoreText.text = $"Toplanan Coin: {score.ToString()} / {gameManager.GetComponent<GameManager>().scoreToNextLevel}";
    }

    void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(groundCheck.transform.position, Vector2.right * transform.localScale * 5/3, 1);
        

        if (hit.collider != null)
        {
            isGrounded = true;
            Debug.DrawRay(groundCheck.transform.position, Vector2.right * transform.localScale * 5 / 3, Color.red);
        }
        else
        {
            isGrounded = false;
            Debug.DrawRay(groundCheck.transform.position, Vector2.right * transform.localScale * 5 / 3, Color.green);
        }

        rb.velocity = new Vector2(direction * speed, rb.velocity.y);
        if (vertical.y > 0)
        {
            anim.SetBool("isJump", true);
            if(isGrounded == true)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.AddForce(Vector2.up * jump);
            }
                
        }

        if (rb.velocity.y < 5f)
        {
            anim.SetBool("isJump", false);
            anim.SetBool("isFall", true);
        }

        if (isGrounded)
        {
            anim.SetBool("isJump", false);
            anim.SetBool("isFall", false);
        }

        if (vertical.y < 0)
        {
            anim.SetBool("isCrouch", true);
            capCol.size = new Vector2(1.3f, 1.3f);
            capCol.offset = new Vector2(capCol.offset.x, -1.2f);
        }
        else
        {
            anim.SetBool("isCrouch", false);
            capCol.size = defaultCapColSize;
            capCol.offset = defaultCapColOffset;
        }
    }

    public void Movement(InputAction.CallbackContext context)
    {
        horizontal = context.ReadValue<Vector2>();
        direction = horizontal.x == 0 ? 0 : (horizontal.x > 0 ? 1 : -1);
        
        if (horizontal.x != 0)
            anim.SetBool("isWalk", true);
        else
            anim.SetBool("isWalk", false);

        Facing();
    }

    public void JumpCrouch(InputAction.CallbackContext context)
    {
        vertical = context.ReadValue<Vector2>();
    }

    void Facing()
    {
        characterScale = transform.localScale;
        if (horizontal.x < 0)
        {
            characterScale.x = -scaleX;
        }
        else if(horizontal.x > 0)
        {
            characterScale.x = scaleX;
        }
        transform.localScale = characterScale;
    }

    void AddScore(int scorePoint)
    {
        score += scorePoint;
        scoreText.text = $"Toplanan Coin: {score.ToString()} / {gameManager.GetComponent<GameManager>().scoreToNextLevel}";
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Coin")
        {
            AddScore(1);
            Destroy(collision.gameObject);
        }

        if(collision.tag == "CoinRed")
        {
            AddScore(10);
            Destroy(collision.gameObject);
        }
    }

    public void SaveGame()
    {
        PlayerPrefs.SetInt("score", score);
        PlayerPrefs.SetFloat("positionx", transform.position.x);
        PlayerPrefs.SetFloat("positiony", transform.position.y);
        PlayerPrefs.SetInt("sceneIndex", SceneManager.GetActiveScene().buildIndex);
        PlayerPrefs.Save();
    }

    

}
