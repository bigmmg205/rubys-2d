using System;
using UnityEngine;

public class RubyController : MonoBehaviour
{
    // ========= MOVEMENT =================
    public float speed = 4;
    
    // ======== HEALTH ==========
    public int maxHealth = 5;
    
    
    // ======== HEALTH ==========
    public int health
    {
        get { return currentHealth; }
    }
    
    // =========== MOVEMENT ==============
    Rigidbody2D rigidbody2d;
    float horizontal;
    float vertical;
    
    
    // ======== HEALTH ==========
    int currentHealth;

    // ======== Health Timer =========
    float invincibleTimer;
    bool isInvincible;
    public float timeInvincible = 2.0f;

    // ========== Animator ==========
    Animator animator;
    Vector2 lookDirection = new Vector2(1, 0);

    public GameObject projectilePrefab;


    

    void Start()
    {
        // =========== MOVEMENT ==============
        rigidbody2d = GetComponent<Rigidbody2D>();
                
        // ======== HEALTH ==========
        currentHealth = maxHealth;

        // ========== Animator ==========
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // ============== MOVEMENT ======================
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(horizontal, vertical);

        // ============ Where the player is looking ==========

        if(!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(0.0f, move.y))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }
        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        // ================= HEALTH TIMER ====================
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }

        if(Input.GetKeyDown(KeyCode.C))
        {
            Launch();
        }



    }

    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;

        rigidbody2d.MovePosition(position);
    }

    // ===================== HEALTH ==================
    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        { 
            if (isInvincible)
                return;
            
            isInvincible = true;
            invincibleTimer = timeInvincible;
        }
        
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);

      
    }


    void Launch()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 300);

        animator.SetTrigger("Launch");
    }



}
