using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ProyectoTecII.Manager;

namespace ProyectoTecII.PlayerControl {
  public class PlayerController : MonoBehaviour {

    [SerializeField] private float animationBlendSpeed = 8.9f;

    [SerializeField] private Transform cameraRoot;
    [SerializeField] private Transform camera;

    [SerializeField] private float upperLimit = -40f;
    [SerializeField] private float lowerLimit = 70f;
    [SerializeField] private float mouseSensitivity = 21.9f;
    [SerializeField] private float jumpFactor = 260f;
    [SerializeField] private float DistanceToGround = 0.8f;
    [SerializeField] private LayerMask GroundCheck;
    [SerializeField] private float airResistance = 0.8f;

    private Rigidbody playerRigidbody;
    private InputManager inputManager;
    private Animator animator;
    private bool grounded = true;
    private bool hasAnimator;


    private int xVelHash;
    private int yVelHash;
    private int zVelHash;

    private int jumpHash;
    private int groundHash;
    private int fallingHash;

    private float xRotation;

    private const float walkSpeed = 3f;
    private const float runSpeed = 7f;

    private Vector2 currentVelocity;

    private void Start() {
      HideCursor();

      hasAnimator = TryGetComponent<Animator>(out animator);
      playerRigidbody = GetComponent<Rigidbody>();
      inputManager = GetComponent<InputManager>();

      xVelHash = Animator.StringToHash("xVelocity");
      yVelHash = Animator.StringToHash("yVelocity");
      zVelHash = Animator.StringToHash("zVelocity");
      jumpHash = Animator.StringToHash("Jump");
      groundHash = Animator.StringToHash("Grounded");
      fallingHash = Animator.StringToHash("Falling");
    }

    private void FixedUpdate() {
      SampleGround();
      Move();
      HandleJump();
    }

    private void LateUpdate() {
      CameraMovement();
    }

    private void HideCursor() {
      Cursor.visible = false;
      Cursor.lockState = CursorLockMode.Locked;
    }

    private void Move() {
      if (!hasAnimator) {
        return;
      }



      float targetSpeed = inputManager.Run ? runSpeed : walkSpeed;

      if (inputManager.Move == Vector2.zero) {
        targetSpeed = 0f;
      }

      if (grounded) {

        currentVelocity.x = Mathf.Lerp(currentVelocity.x, inputManager.Move.x * targetSpeed, animationBlendSpeed * Time.fixedDeltaTime);
        currentVelocity.y = Mathf.Lerp(currentVelocity.y, inputManager.Move.y * targetSpeed, animationBlendSpeed * Time.fixedDeltaTime);

        var xVelDifference = currentVelocity.x - playerRigidbody.velocity.x;
        var zVelDifference = currentVelocity.y - playerRigidbody.velocity.z;

        playerRigidbody.AddForce(transform.TransformVector(new Vector3(xVelDifference, 0, zVelDifference)), ForceMode.VelocityChange);

      } else {
        playerRigidbody.AddForce(transform.TransformVector(new Vector3(currentVelocity.x * airResistance, 0, currentVelocity.y * airResistance)), ForceMode.VelocityChange);
      }

      animator.SetFloat(xVelHash, currentVelocity.x);
      animator.SetFloat(yVelHash, currentVelocity.y);
    }

    private void CameraMovement() {
      if (!hasAnimator) {
        return;
      }

      var mouseX = inputManager.Look.x;
      var mouseY = inputManager.Look.y;

      camera.position = cameraRoot.position;

      xRotation -= mouseY * mouseSensitivity * Time.smoothDeltaTime;
      xRotation = Mathf.Clamp(xRotation, upperLimit, lowerLimit);

      camera.localRotation = Quaternion.Euler(xRotation, 0, 0);
      playerRigidbody.MoveRotation(playerRigidbody.rotation * Quaternion.Euler(0, mouseX * mouseSensitivity * Time.smoothDeltaTime, 0));
    }

    private void HandleJump() {
      if (!hasAnimator) {
        return;
      }

      if (!inputManager.Jump) {
        return;
      }

      animator.SetTrigger(jumpHash);
    }

    public void JumpAddForce() {
      playerRigidbody.AddForce(-playerRigidbody.velocity.y * Vector3.up, ForceMode.VelocityChange);
      playerRigidbody.AddForce(Vector3.up * jumpFactor, ForceMode.Impulse);
      animator.ResetTrigger(jumpHash);
    }

    private void SampleGround() {
      if (!hasAnimator) {
        return;
      }

      RaycastHit hitInfo;
      if (Physics.Raycast(playerRigidbody.worldCenterOfMass, Vector3.down, out hitInfo, DistanceToGround + 0.2f, GroundCheck)) {
        // Collided with something and we are grounded
        grounded = true;
        SetAnimationGrounding();
        return;
      }
      // Falling

      grounded = false;
      animator.SetFloat(zVelHash, playerRigidbody.velocity.y);
      SetAnimationGrounding();
      return;

    }

    private void SetAnimationGrounding() {
      animator.SetBool(fallingHash, !grounded);
      animator.SetBool(groundHash, grounded);
    }
  }
}
