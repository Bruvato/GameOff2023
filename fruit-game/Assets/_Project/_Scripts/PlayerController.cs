using System;
using System.Collections;
using System.Collections.Generic;
using LeaderboardCreatorDemo;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.Pool;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    private CharacterController controller;
    [SerializeField] private Transform cam;
    [SerializeField] private float speed = 6f;
    [SerializeField] private float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;
    [SerializeField] private LeaderboardManager leaderboardManager;


    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }
    private void Update()
    {
        HandleMovement();


    }

    private void HandleMovement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        if (leaderboardManager._usernameInputField.isFocused) { return; }

        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;

            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            transform.rotation = Quaternion.Euler(0f, angle, 0.1f);

            Vector3 moveDir = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }

    }






}
