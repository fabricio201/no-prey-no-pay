﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController), typeof(PlayerAnimatorController))]
public class PlayerStateMachine : MonoBehaviour
{
    // --------------------------------- PUBLIC AUX ENUMS -------------------------------- //
    public enum eStates
    {
        Idle,
        Walking,
        Jumping,            // Normal
        Falling,
        Dashing,
        //Attack,
        WallSliding,        // Wall
        WallEjecting,       // Wall
        //LedgeGrabbed,
        //LedgeMoving,
        Dead,
    }

    // ------------------------------- PROTECTED ATTRIBUTES ------------------------------ //
    protected PlayerController          m_playerCtl;
    protected PlayerAnimatorController  m_playerAnimCtl;

    //// -------------------------------- PRIVATE ATTRIBUTES ------------------------------- //
    //private float                       m_minSpeedToWalkAnim    = .2f;

    // ------------------------------------- ACCESSORS ----------------------------------- //
    public eStates      State           { get; protected set; }

    // ======================================================================================
    // PUBLIC MEMBERS
    // ======================================================================================
    public void Start()
    {
        State           = eStates.Idle;
        
        m_playerCtl     = this.gameObject.GetComponent<PlayerController>();
        m_playerAnimCtl = this.gameObject.GetComponent<PlayerAnimatorController>();
    }

    // ======================================================================================
    public void Update()
    {
        UpdateStateMachine();
        UpdateAnimator();
    }

    // ======================================================================================
    protected void UpdateStateMachine()
    {
        if (m_playerCtl.IsDashing)
            State = eStates.Dashing;
        else if (m_playerCtl.IsEjecting)
            State = eStates.WallEjecting;
        else if (m_playerCtl.IsGrounded)
        {
            if (Mathf.Abs(m_playerCtl.Velocity.x) != 0)
                State = eStates.Walking;
            else
                State = eStates.Idle;
        }
        else if (m_playerCtl.IsWallSliding)
            State = eStates.WallSliding;
        else if (m_playerCtl.IsJumping)
        {
            if (m_playerCtl.Velocity.y > 0)
                State = eStates.Jumping;
            else
                State = eStates.Falling;
        }
    }

    // ======================================================================================
    private void UpdateAnimator()
    {
        switch (State)
        {
            case eStates.Idle:
                m_playerAnimCtl.SetState(PlayerAnimatorController.eStates.Idle);
                break;
            case eStates.Walking:
                m_playerAnimCtl.SetState(PlayerAnimatorController.eStates.Walking);
                break;
            case eStates.Jumping:
                m_playerAnimCtl.SetState(PlayerAnimatorController.eStates.Jumping);
                break;
            case eStates.Falling:
                m_playerAnimCtl.SetState(PlayerAnimatorController.eStates.Falling);
                break;
            case eStates.WallSliding:
                m_playerAnimCtl.SetState(PlayerAnimatorController.eStates.Sliding);
                break;
            case eStates.WallEjecting:
                m_playerAnimCtl.SetState(PlayerAnimatorController.eStates.Dashing);             // TO DO : Wall Eject
                break;
            case eStates.Dashing:
                m_playerAnimCtl.SetState(PlayerAnimatorController.eStates.Dashing);
                break;
        }

        switch (m_playerCtl.ForwardDir)
        {
            case PlayerController.eDirection.Left:
                m_playerAnimCtl.SetDirection(PlayerAnimatorController.eDirections.Left);
                break;
            case PlayerController.eDirection.Right:
                m_playerAnimCtl.SetDirection(PlayerAnimatorController.eDirections.Right);
                break;
        }
    }
}