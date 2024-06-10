using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class Move : MonoBehaviour
{
    Rigidbody2D rb;
    public Boolean left = false, right = false;
    [SerializeField] float moveSpeed, distance, jumpSpeed;
    [SerializeField] LayerMask lm;
    [SerializeField] GameObject parent, idle;
    public RaycastHit2D hit;
    Animator animator;
    SpriteRenderer spriteRenderer;

    // Start is called before the first frame update


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = idle.GetComponent<Animator>();
        spriteRenderer = idle.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        hit = Physics2D.Raycast(gameObject.transform.position, Vector2.down, distance, lm);
        if (!parent.GetComponent<GrapplingGun>().isHooked)
        {

            if(right && left)
            {
                right = false;
                left = false;
            }
            else
            {
                if (Input.GetKey(KeyCode.D))
                {
                    right = true;
                }
                if (Input.GetKey(KeyCode.A))
                {
                    left = true;
                }
                if (Input.GetKeyUp(KeyCode.D))
                {
                    right = false;
                }
                if (Input.GetKeyUp(KeyCode.A))
                {
                    left = false;
                }
            }

            if (right || left)
            {
                animator.SetBool("run", true);
            }
            if (!right && !left)
            {
                animator.SetBool("run", false);
            }
            if (right && left)
            {
                animator.SetBool("run", false);
            }
            if (right && !left)
            {
                spriteRenderer.flipX = false;
            }
            if (!right && left)
            {
                spriteRenderer.flipX = true;
            }

            
            if (hit.collider != null)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector3(0, jumpSpeed, 0));
                }
            }
        }
        if (parent.GetComponent<GrapplingGun>().isHooked)
        {
            right = false;
            left = false;
            animator.SetBool("run", false);
        }
    }

    private void FixedUpdate()
    {
        if (right)
        {
            gameObject.GetComponent<Transform>().position += new Vector3(moveSpeed,0,0);
        }
        if (left)
        {
            gameObject.GetComponent<Transform>().position += new Vector3(-moveSpeed, 0, 0);
        }
    }
}
