using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float jumpForce;
    Controls controls;
    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator anim;
    float direction;
    float running;

    private void Awake() {
        controls = new();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        controls.Player.Jump.performed += Jump;
    }

    private void OnEnable() {
        controls.Player.Enable();
    }

    private void OnDisable() {
        controls.Player.Disable();
    }

    void FixedUpdate()
    {
        direction = controls.Player.Movement.ReadValue<float>();

        if (transform.position.x > 0) {
            sr.flipX = false;
        } else {
            sr.flipX = true;
            direction = -(direction);
        }

        if(controls.Player.Run.ReadValue<float>() > 0 && direction < 0) {
            speed = 450;
            running = 1;
        } else {
            speed = 300;
            running = 0;
        }
        

        anim.SetFloat("isWalking", direction);
        anim.SetFloat("isJumping", rb.velocity.y);
        anim.SetFloat("isRunning", running);
        anim.SetFloat("isCrouching", controls.Player.Crouching.ReadValue<float>());

        
        Move();
        
    }

    private void Jump(InputAction.CallbackContext context){
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        
    }

    private void Move(){
        float movement = controls.Player.Movement.ReadValue<float>();
        
        if (controls.Player.Crouching.ReadValue<float>() > 0.1 && rb.velocity.y == 0) {
            movement = 0;
        }
        
        rb.velocity = new Vector2(movement * speed * Time.fixedDeltaTime, rb.velocity.y);
    }
}
