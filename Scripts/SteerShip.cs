using System.Collections;
using UnityEngine;

public class SteerShip : MonoBehaviour
{
    GameController controller;
    Rigidbody2D rb;
    Animator animator;
    GameObject playerRails;
    GameObject player;
    int currentSegment = 270;
    int segments = 0;
    bool canMove = true;
    float moveCooldown = 0.009f;
    Vector3 targetPosition;

    private void Start()
    {
        player = this.gameObject;
        controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        Debug.Log("Animator found");
        playerRails = GameObject.FindGameObjectWithTag("PlayerRails");

        if (playerRails != null)
        {
            LineRenderer lineRenderer = playerRails.GetComponent<LineRenderer>();
            segments = lineRenderer.positionCount;
            {
                Debug.LogError("PlayerRails LineRenderer has no positions!");
            }
        }
        else
        {
            Debug.LogError("PlayerRails not found!");
        }
    }

    void Update()
    {
        if (controller.state == "levelrunning")
        {
            HorizontalDirection();
        }
    }

    void HorizontalDirection()
    {
        float direction = Input.GetAxis("Horizontal");

        if (playerRails == null)
        {
            playerRails = GameObject.FindGameObjectWithTag("PlayerRails");
            if (playerRails == null)
            {
                Debug.LogError("PlayerRails not found!");
                return;
            }
        }

        if (direction == 0)
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("idle"))
            {
                animator.SetTrigger("idle");
            }
        }
        else if (canMove)
        {
            animator.SetFloat("direction", direction);
            UpdatePlayerPosition(direction);
            StartCoroutine(MovementCooldown());
        }
    }

    void UpdatePlayerPosition(float direction)
    {
        if (playerRails == null)
        {
            Debug.LogError("PlayerRails not assigned!");
            return;
        }

        LineRenderer lineRenderer = playerRails.GetComponent<LineRenderer>();
        if (direction > 0)
        {
            currentSegment = (currentSegment + 1) % segments;
        }
        else if (direction < 0)
        {
            currentSegment = (currentSegment - 1 + segments) % segments;
        }
        targetPosition = lineRenderer.GetPosition(currentSegment);
        player.transform.position = targetPosition;

    }

    IEnumerator MovementCooldown()
    {
        canMove = false;
        yield return new WaitForSeconds(moveCooldown);
        canMove = true;
    }
}
