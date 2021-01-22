using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private const float MOVEMENT_SMOOTHNESS = .15f;
    private const float BLEND_TREE_GRAPH_WALK = 1;
    private const float BLEND_TREE_GRAPH_RUN = 2;

    [SerializeField] private Animator animator;
    [SerializeField] private PlayerController playerController;

    private Vector3 m_lastMovementVector = Vector3.zero;

    private void Update()
    {
        //PlayMovementAnimation();
        //Debug.Log(playerController.Speed);
    }

    private void PlayMovementAnimation()
    {
        Vector3 movement = playerController.Speed;
        movement = new Vector3(movement.x, m_lastMovementVector.y, movement.z);

        if (Mathf.Abs(movement.z) > playerController.MaxWalkSpeed || Mathf.Abs(movement.x) > playerController.MaxWalkSpeed)
            movement = Maths.Map(movement, playerController.MinRunSpeed, playerController.MaxRunSpeed, -BLEND_TREE_GRAPH_RUN, BLEND_TREE_GRAPH_RUN);
        else
            movement = Maths.Map(movement, playerController.MinWalkSpeed, playerController.MaxWalkSpeed, -BLEND_TREE_GRAPH_WALK, BLEND_TREE_GRAPH_WALK);

        movement = Vector3.Lerp(m_lastMovementVector, movement, MOVEMENT_SMOOTHNESS);

        animator.SetFloat("speed", movement.z);
        animator.SetFloat("strafeSpeed", movement.x);

        m_lastMovementVector = movement;
    }

    public void PlayMovement(float minWalkSpeed, float maxWalkSpeed, float minRunSpeed, float maxRunSpeed, float forwardSpeed, float strafeSpeed)
    {
        Vector3 movement = new Vector3(strafeSpeed, m_lastMovementVector.y, forwardSpeed);

        // Map values to RUN ANIMATIONS
        if (Mathf.Abs(forwardSpeed) > maxWalkSpeed || Mathf.Abs(strafeSpeed) > maxWalkSpeed)
            movement = Maths.Map(movement, minRunSpeed, maxRunSpeed, -BLEND_TREE_GRAPH_RUN, BLEND_TREE_GRAPH_RUN);
        // Map values to WALK ANIMATIONS
        else
            movement = Maths.Map(movement, minWalkSpeed, maxWalkSpeed, -BLEND_TREE_GRAPH_WALK, BLEND_TREE_GRAPH_WALK);

        movement = Vector3.Lerp(m_lastMovementVector, movement, MOVEMENT_SMOOTHNESS);

        animator.SetFloat("speed", movement.z);
        animator.SetFloat("strafeSpeed", movement.x);

        m_lastMovementVector = movement;
    }

    public void PlayJump()
    {
        animator.SetTrigger("jump");
    }


}
