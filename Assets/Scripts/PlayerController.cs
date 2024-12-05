using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    [Header("Movimiento")]
    private bool canDoubleJump;
    public float moveSpeed;
    public float jumpForce;

    [Header("Componentes")]
    public Rigidbody2D TheRB;

    [Header("Animator")]
    public Animator anim;
    private SpriteRenderer TheSR;

    [Header("Grounded")]
    private bool isGrounded;
    public Transform groundCheckPoint;
    public LayerMask whatIsGround;
    public float groundCheckRadius = 0.2f;

    public float knockBackLenght, knockBackForce;
    private float knockBackCounter;


    public void Awake()
    {
        instance = this;
    }

    void Start()
    {
        anim = GetComponent<Animator>();
        TheSR = GetComponent<SpriteRenderer>();

        if (TheRB == null)
            TheRB = GetComponent<Rigidbody2D>();
    }


    void Update()
    {

        if (knockBackCounter <= 0)
        {
            // Movimiento horizontal
            TheRB.linearVelocity = new Vector2(moveSpeed * Input.GetAxisRaw("Horizontal"), TheRB.linearVelocity.y);

            // Verificar si está en el suelo
            isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, whatIsGround);

            // Resetear el doble salto cuando toca el suelo
            if (isGrounded)
            {
                canDoubleJump = true;
            }

            // Lógica de salto
            if (Input.GetButtonDown("Jump"))
            {
                if (isGrounded) // Primer salto
                {
                    TheRB.linearVelocity = new Vector2(TheRB.linearVelocity.x, jumpForce);
                }
                else if (canDoubleJump) // Doble salto
                {
                    TheRB.linearVelocity = new Vector2(TheRB.linearVelocity.x, jumpForce);
                    canDoubleJump = false;
                }
            }

            if (TheRB.linearVelocity.x < 0)
            {
                TheSR.flipX = true;
            } else if (TheRB.linearVelocity.x > 0)
            {
                TheSR.flipX = false;
            }
        }
        else
        {
            knockBackCounter -= Time.deltaTime;

            if (!TheSR.flipX)
            {
                TheRB.linearVelocity = new Vector2(-knockBackForce, TheRB.linearVelocity.y);
            } else
            {
                TheRB.linearVelocity = new Vector2(knockBackForce, TheRB.linearVelocity.y);
            }
        }

        anim.SetFloat("moveSpeed", Mathf.Abs(TheRB.linearVelocity.x));
        anim.SetBool("isGrounded", isGrounded);
    }

    public void KnockBack()
    {
        knockBackCounter = knockBackLenght;
        TheRB.linearVelocity = new Vector2(0f, knockBackForce);
    }
}
