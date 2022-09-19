using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMovement : MonoBehaviourPun
{
    [Header("Speed")]
    [SerializeField] private float runSpeed = 15f;
    [SerializeField] private float walkSpeed = 10f;
    [SerializeField] private float rotationSpeed = 120f;
    private float moveSpeed;

    [Header("Camera")]
    [SerializeField] private Vector3 cameraOffset;


    private Animator animator;
    private PlayerInput input;
    private Rigidbody rigid;

    private void OnEnable()
    {
        if(!photonView.IsMine)
        {
            return;
        }

        animator = GetComponent<Animator>();
        input = GetComponent<PlayerInput>();
        rigid = GetComponent<Rigidbody>();

        moveSpeed = walkSpeed;

        Camera.main.transform.parent = transform;
        Camera.main.transform.localPosition = cameraOffset;
        Camera.main.transform.rotation = Quaternion.identity;
    }

    private void FixedUpdate()
    {
        if(!photonView.IsMine)
        {
            return;
        }

        Move();
        Rotate();
    }

    private void Move()
    {
        WalkAndRun();

        if(input.X == 0)
        {
            animator.SetBool("Walking", false);
            return;
        }

        animator.SetBool("Walking", true);
        Vector3 deltaPosition = input.X * moveSpeed * Time.deltaTime * transform.forward;
        rigid.MovePosition(rigid.position + deltaPosition);
    }

    private void WalkAndRun()
    {
        if(input.Shift)
        {
            animator.SetBool("Running", true);
            moveSpeed = runSpeed;
            return;
        }

        animator.SetBool("Running", false);
        moveSpeed = walkSpeed;
    }

    private void Rotate()
    {
        float deltaRotationY = input.Y * rotationSpeed * Time.deltaTime;
        rigid.MoveRotation(rigid.rotation * Quaternion.Euler(0, deltaRotationY, 0));
    }
}
