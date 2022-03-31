using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public Animator animator;
    public bool right;
    public bool canMove;
    public bool isDead;
    bool isShield;
    private Rigidbody2D rb;
    private Collider2D col;
    private bool invincible;
    public TextMeshProUGUI displayHealth;
    TextMeshProUGUI[] displayFound;
    public LayerMask enemyLayer;
    private Vector2 bestRange;
    public float range;
    public AudioClip death;
    public AudioClip hurt;
    public AudioClip skillcd;
    public float timer;
    public GameObject shield;

    public static int maxHealth = 20;
    public static int currentHealth;

    private Material matWhite;
    private Material matDefault;
    SpriteRenderer sr;

    public Vector3 goal;
    public GameObject mainCamera;


    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        rb = GetComponent<Rigidbody2D>();
        right = true;
        invincible = false;
        canMove = true;
        isDead = false;
        isShield = false;
        sr = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;
        matWhite = Resources.Load("WhiteFlash", typeof(Material)) as Material;
        matDefault = sr.material;
        displayFound = FindObjectsOfType<TextMeshProUGUI>();
        for (int i = 0; i < displayFound.Length; i++) 
        {
            if(displayFound[i].name == "HPText")
            {
                displayHealth = displayFound[i];
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(currentHealth > 0)
        {
            Timer();
            Move();
            //aim();
            Flip();
            Shield();
            ReachGoal();
            //displayHealth.text = currentHealth + "/" + maxHealth;
        }
        else
        {
            if(!isDead)
            {
                Dead();
                isDead = true;
                //displayHealth.text = 0 + "/" + maxHealth;
            }
        }
    }

    private void Flip()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        if (horizontalInput > 0 && !right|| horizontalInput < 0 && right)
        {
            right = !right;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }

    private void Move()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        if (horizontalInput != 0  || verticalInput != 0 )
        {
            animator.SetBool("Move",true);
        }
        else
        {
            animator.SetBool("Move", false);
        }
        if(canMove)
        {
            rb.velocity = (new Vector2(horizontalInput, verticalInput) * moveSpeed);
        }
    }

    public void TakeDamage(int damage)
    {
        if(!invincible)
        {
            currentHealth -= damage;
            GetComponent<AudioSource>().PlayOneShot(hurt);
            sr.material = matWhite;
            Invoke("ResetMaterial", 0.2f);
            invincible = true;
            Invoke("ResetInvincible", 1f);
        }
    }

    void Timer()
    {
        if (isShield)
        {
            timer = 5;
        }
        if(timer > 0)
        {
            timer -= Time.deltaTime;
        }
    }
    void Shield()
    {
        if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.B))
        {
            if (!isShield)
            {
                if (timer <= 0)
                {
                    shield.SetActive(true);
                    invincible = true;
                    isShield = true;
                    Invoke("ResetInvincible", 2f);
                }
                else
                {
                    GetComponent<AudioSource>().PlayOneShot(skillcd);
                }
            }
        }
    }

    void ResetMaterial()
    {
        sr.material = matDefault;
    }
    public void ResetInvincible()
    {
        invincible = false;
        shield.SetActive(false);
        isShield = false;
    }

    void Dead()
    {
        GetComponent<Collider2D>().enabled = false;
        transform.Rotate(Vector3.forward * 90);
        Vector3 position = transform.position;
        position.y -= 0.4f;
        transform.position = position;
        rb.velocity = new Vector2 (0,0);
        animator.Play("PlayerAttack", 1, 0.0f);
        animator.enabled = false;
        GetComponent<AudioSource>().PlayOneShot(death);
        Invoke("GameOver", 2f);
    }
    void aim()
    {
        Collider2D[] enemyRange = Physics2D.OverlapCircleAll(transform.position, range, enemyLayer);
        if (enemyRange.Length == 0)
        {
            Flip();
        }
        else
        {
            bestRange = new Vector2(100, 0);
            foreach (Collider2D enemy in enemyRange)
            {
                Vector2 range = enemy.gameObject.transform.position - transform.position;
                if (Mathf.Abs(range.x) < bestRange.x)
                {
                    bestRange = range;
                }
            }

            if (bestRange.x > 0)
            {
                right = true;
                Vector3 scale = transform.localScale;
                scale.x = 1;
                transform.localScale = scale;
            }
            else if (bestRange.x < 0)
            {
                right = false;
                Vector3 scale = transform.localScale;
                scale.x = -1;
                transform.localScale = scale;
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, range);
    }
    void GameOver()
    {
        SceneManager.LoadScene(2);
    }

    void ReachGoal()
    {
        if (Vector3.Distance(this.transform.position,goal) < 1)
        {
            mainCamera.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }
}
