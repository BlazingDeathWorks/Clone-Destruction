using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    float playerSpeed = 1f;
    private Rigidbody2D rb = null;
    private float x = 0, y = 0;
    private Vector2 playerVelocity;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");
        playerVelocity = new Vector2(x, y).normalized;
    }

    private void FixedUpdate()
    {
        rb.velocity = playerVelocity * playerSpeed;
    }
}
