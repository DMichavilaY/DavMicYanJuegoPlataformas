using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public float speed = 2;
    public float jumpForce = 5; // Valor de la fuerza de salto
    public Transform groundCheck; // Referencia al objeto que verifica si est� en el suelo
    public LayerMask groundLayer; // Capa que representa el suelo

    private Animator animator;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private bool isGrounded; // Variable para verificar si est� en el suelo
    private bool wasGrounded = true; // Variable para almacenar si estaba en el suelo en el frame anterior

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb.freezeRotation = true; // Bloquear la rotaci�n del Rigidbody2D
    }

    void Update()
    {
        // Verificar si est� en el suelo
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);

        // Movimiento horizontal
        float horizontalMovement = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(speed * horizontalMovement, rb.velocity.y);
        animator.SetBool("Move", horizontalMovement != 0);

        // Cambiar direcci�n del sprite
        if (horizontalMovement > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (horizontalMovement < 0)
        {
            spriteRenderer.flipX = true;
        }

        // Salto
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0); // Detener la velocidad en el eje Y
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse); // Aplicar fuerza hacia arriba para el salto
            animator.SetBool("Jump", true);
            isGrounded = false; // Indicar que ya no est� en el suelo despu�s de saltar
        }

        // Si est� en el suelo y no estaba en el suelo en el frame anterior, establecer la animaci�n Idle
        if (isGrounded && !wasGrounded)
        {
            animator.SetBool("Jump", false);
        }

        // Guardar el estado de si estaba en el suelo en el frame actual
        wasGrounded = isGrounded;

        // Si est� en el suelo y no hay input de salto, cambiar a estado Idle
        if (isGrounded && !Input.GetButton("Jump"))
        {
            animator.SetBool("Jump", false);
        }
        else
        {
            // Si no est� en el suelo, establecer la animaci�n de salto y fijar el �ltimo frame
            animator.SetBool("Jump", true);
        }
    }
}