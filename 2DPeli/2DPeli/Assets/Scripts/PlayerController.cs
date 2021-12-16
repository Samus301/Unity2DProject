using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;

    //Movement
    public float speed;
    public float jumpforce;
    public float moveInput;
    public int extraJumps;
    private int extraJumpValue = 3;

    //GroundCheck
    private bool isGrounded;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;

    Animator m_Animator;

    private SpriteRenderer mySpriteRenderer;

    public int score = 0;

    public int life;
    private int maxLife = 3;

    public Vector2 playerPos;

    private int nextLevel = 1;

    public GameObject Notice;

    public GameManager gm;

    public string levelSelected;

    private GameObject Heart1;
    private GameObject Heart2;
    private GameObject Heart3;

    public Scene scene;

    // Start is called before the first frame update
    void Start()
    {
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        m_Animator = gameObject.GetComponent<Animator>();
        playerPos = GameObject.Find("Player").transform.position;
        life = maxLife;
        extraJumps = extraJumpValue;
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        Heart1 = GameObject.Find("Heart1");
        Heart2 = GameObject.Find("Heart2");
        Heart3 = GameObject.Find("Heart3");

        Notice = GameObject.Find("Notice");

        if(GameObject.Find("Notice"))
        {
            Debug.Log("toimii");
            Notice.SetActive(false);
        } else
        {
            Notice = GameObject.Find("Placeholder");
        }

        scene = SceneManager.GetActiveScene();

    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        
        if (col.gameObject.tag == "Spike")
        {
            rb.velocity = Vector2.up * jumpforce;
            // osuttiin piikkiin - pelaaja menettää elämän
            StartCoroutine(Death());

        }

        if (col.gameObject.tag == "Enemy")
        {
            m_Animator.Play("Base Layer.Hurt", -1, 0f);
             
            if (life > 0)
            {
                life--;
                gameObject.transform.position = playerPos;
            }
            else
            {
                SceneManager.LoadScene("End");
            }

        }

        if (col.gameObject.tag == "Gem")
        {
            // kasvata pisteitä yhdellä
            Debug.Log("score: " + score);
            // poista törmätty objekti
            score++;
            col.gameObject.SetActive(false);
        }

        if (col.gameObject.tag == "NextLevel")
        {
            nextLevel++;
            PlayerPrefs.SetInt(scene.name, score);
            SceneManager.LoadScene("Level Selection");
        }

        // ends level and saves score
        if (col.gameObject.tag == "EndHouse")
        {
            PlayerPrefs.SetInt("PlayerScore", score);
            SceneManager.LoadScene("End");
        }

        if (col.gameObject.tag == "Checkpoint")
        {
            playerPos = GameObject.Find("Player").transform.position;
        }

        

    }
    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "Level_1")
        {
            Notice.SetActive(true);
            levelSelected = "Level1";
        }

        if (col.gameObject.tag == "Level_2")
        {
            Notice.SetActive(true);
            levelSelected = "Level2";
        }

        if (col.gameObject.tag == "Level_3")
        {
            Notice.SetActive(true);
            levelSelected = "Level3";
        }

    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Level_1")
        {
            Notice.SetActive(false);
            levelSelected = "";
        }

        if (col.gameObject.tag == "Level_2")
        {
            Notice.SetActive(false);
            levelSelected = "";
        }

        if (col.gameObject.tag == "Level_3")
        {
            Notice.SetActive(false);
            levelSelected = "";
        }
    }

    IEnumerator Death()
    {
        m_Animator.Play("Base Layer.Hurt", -1, 0f);
        yield return new WaitForSeconds(0.4f); // waits before continuing in seconds
        // code to do after the wait

        if (life > 0)
        {
            life--;
            gameObject.transform.position = playerPos;
        }
        else
        {
            SceneManager.LoadScene("Level Selection");
        }
    
    }

    void FixedUpdate()
        {
        
        moveInput = Input.GetAxis("Horizontal");

        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);

        // Animations
        if (moveInput < 0)
        {
            m_Animator.SetBool("Run_Left", true);
            mySpriteRenderer.flipX = true;
        }

        if (moveInput > 0)
        {
            m_Animator.SetBool("Run_Right", true);
            mySpriteRenderer.flipX = false;
        }

        if (moveInput == 0)
        {
            m_Animator.SetBool("Run_Left", false);
            m_Animator.SetBool("Run_Right", false);
        }

        if (isGrounded = true && rb.velocity.y == 0)
        {
            m_Animator.SetBool("Jump", false);
        }

        if (Input.GetKeyDown(KeyCode.Space ) && extraJumps > 0)
        {
            m_Animator.Play("Base Layer.Jump", -1, 0f);
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            m_Animator.Play("Base Layer.Hurt", -1, 0f);
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            m_Animator.Play("Base Layer.Crouch", -1, 0f);
        }
    }

    
    // Update is called once per frame
    void Update()
    {
        
        if (life == 3)
        {
            Heart1.SetActive(true);
            Heart2.SetActive(true);
            Heart2.SetActive(true);
        } else if (life == 2)
        {
            Heart1.SetActive(true);
            Heart2.SetActive(true);
            Heart3.SetActive(false);
        } else if (life == 1)
        {
            Heart1.SetActive(true);
            Heart2.SetActive(false);
            Heart3.SetActive(false);
        } else if (life == 0)
        {
            Heart1.SetActive(false);
            Heart2.SetActive(false);
            Heart3.SetActive(false);
        }


        //Checking if player is grounded and adding jumps
        if (isGrounded == true)
        {
            extraJumps = extraJumpValue;
        }

        //Jump
        if (Input.GetKeyDown(KeyCode.Space) && extraJumps > 0)
        {
            m_Animator.SetBool("Jump", true);
            rb.velocity = Vector2.up * jumpforce;
            extraJumps--;
        }
    }
}
