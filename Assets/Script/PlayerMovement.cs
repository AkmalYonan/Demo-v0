using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement instance;

    public float speed = 5f; // Beri nilai default
    private Animator animator;
    private Rigidbody2D rb; // Caching Rigidbody

    public bool canMove = true;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        // Validasi agar tidak error jika lupa pasang Rigidbody
        if (rb == null) Debug.LogError("Rigidbody2D tidak ditemukan di " + gameObject.name);
    }

    private void Update()
    {
        // LOCK MOVEMENT
        if (!canMove)
        {
            rb.linearVelocity = Vector2.zero;
            animator.SetBool("IsMoving", false);
            return;
        }

        Vector2 dir = Vector2.zero;

        if (Input.GetKey(KeyCode.A))
        {
            dir.x = -1;
            animator.SetInteger("Direction", 3);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            dir.x = 1;
            animator.SetInteger("Direction", 2);
        }

        if (Input.GetKey(KeyCode.W))
        {
            dir.y = 1;
            animator.SetInteger("Direction", 1);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            dir.y = -1;
            animator.SetInteger("Direction", 0);
        }

        dir.Normalize();

        animator.SetBool("IsMoving", dir.magnitude > 0);

        rb.linearVelocity = dir * speed;
    }
}
