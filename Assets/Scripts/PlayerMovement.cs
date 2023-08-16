using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerMovement : NetworkBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    float horizontal, vertical;
    bool isJumping;
    public Transform groundCheck;
    public LayerMask groundLayer;

    private Rigidbody rb;
    private bool isGrounded;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (IsLocalPlayer)
        {
            HandleInput();
        }
    }

    private void FixedUpdate()
    {
        if (IsLocalPlayer || IsServer)
        {
            Move();
            CheckGrounded();
            Jump();
        }
    }

    private void HandleInput()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        isJumping = Input.GetButtonDown("Jump");

        if (isJumping && isGrounded)
        {
            // Solo llamar a CmdJump() en el cliente local para notificar al servidor
            CmdJumpServerRpc();
        }
    }

    [ServerRpc]
    private void CmdJumpServerRpc()
    {
        // Aplicar el salto en el servidor
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        RpcSyncJumpClientRpc();
    }

    [ClientRpc]
    private void RpcSyncJumpClientRpc()
    {
        // Aplicar el salto en los clientes
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void Move()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        Vector3 movement = (transform.forward * vertical) + (transform.right * horizontal);
        movement.Normalize();
        rb.velocity = movement * moveSpeed + new Vector3(0, rb.velocity.y, 0);
    }

    private void CheckGrounded()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.2f, groundLayer, QueryTriggerInteraction.Ignore);
    }

    private void Jump()
    {
        if (isJumping && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isJumping = false;
        }
    }

}
