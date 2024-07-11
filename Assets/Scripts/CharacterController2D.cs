using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterController2D : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    [SerializeField] float speed = 2f;
    [SerializeField] float runSpeed = 5f;
    Vector2 motionVector;
    public Vector2 lastMotionVector;
    Animator animator;
    bool moving;
    bool running;

    Character character;
    Coroutine runningCoroutine;

    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        character = GetComponent<Character>();
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            running = true;
            if (runningCoroutine == null)
            {
                runningCoroutine = StartCoroutine(RunStaminaDrain());
            }
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            running = false;
            if (runningCoroutine != null)
            {
                StopCoroutine(runningCoroutine);
                runningCoroutine = null;
            }
        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        motionVector.x = horizontal;
        motionVector.y = vertical;

        animator.SetFloat("horizontal", horizontal);
        animator.SetFloat("vertical", vertical);

        moving = horizontal != 0 || vertical != 0;
        animator.SetBool("moving", moving);

        if (horizontal != 0 || vertical != 0)
        {
            lastMotionVector = new Vector2(
                horizontal,
                vertical
                ).normalized;
            animator.SetFloat("lastHorizontal", horizontal);
            animator.SetFloat("lastVertical", vertical);
        }
    }

    void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        rigidbody2d.velocity = motionVector * (running == true ? runSpeed : speed);
    }

    private void OnDisable()
    {
        rigidbody2d.velocity = Vector2.zero;
    }

    private IEnumerator RunStaminaDrain()
    {
        while (running)
        {
            if (character.stamina.currVal > 0)
            {
                character.GetTired(5); // Decrease 1 stamina
                yield return new WaitForSeconds(1f); // Adjust the interval as needed
            }
            else
            {
                running = false;
                yield return null;
            }
        }
    }
}
