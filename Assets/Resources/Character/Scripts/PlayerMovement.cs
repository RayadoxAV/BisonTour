using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

  public CharacterController controller;
  public float movementSpeed = 12f;

  public float gravity = -9.81f;

  Vector3 currentVelocity;

  public Transform groundCheck;
  public float groundDistance = 0.4f;
  public LayerMask groundMask;

  public float jumpHeight = 3f;

  private bool isGrounded;

  void Update() {

    isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

    if (isGrounded && currentVelocity.y < 0.0f) {
      currentVelocity.y = -2.0f;
    }

    float x = Input.GetAxis("Horizontal");
    float z = Input.GetAxis("Vertical");

    Vector3 move = transform.right * x + transform.forward * z;

    controller.Move(move * movementSpeed * Time.deltaTime);

    if (Input.GetButtonDown("Jump") && isGrounded) {
      currentVelocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravity);
    }

    currentVelocity.y += gravity * Time.deltaTime;

    controller.Move(currentVelocity * Time.deltaTime);
  }
}
