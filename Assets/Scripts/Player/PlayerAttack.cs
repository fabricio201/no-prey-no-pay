﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(PlayerInputCtlr), typeof(CollisionCtlr))]
public class PlayerAttack : MonoBehaviour
{
    // -------------------------------------- ENUMS -------------------------------------- //
    public enum eDirection
    {
        Left,
        Right
    };

    public enum eWeapon
    {
        Fists,
        Pistol,
        Saber
    };

    // --------------------------- PROTECTED CONFIG ATTRIBUTES --------------------------- //
    // attack params
    protected float   m_attackCooldown  = 0.4f;
    protected Vector2 m_throwOffset;

    // attack: Fists
    protected Vector2 PunchOffset;
    protected Vector2 PunchHitboxSize;

    // attack: Saber
    protected Vector2 SaberOffset;
    protected Vector2 SaberHitboxSize;

    // attack: Pistol
    protected GameObject    ProjectilePrefab;
    protected Vector2       PistolOffset;

    // -------------------------------- PRIVATE ATTRIBUTES ------------------------------- //
    // attack origin
    private PlayerInputCtlr     m_input;
    private PlayerController    m_control;
    private LayerMask           playerLayer;

    // attack subsystem
    private Vector2 m_attackDirection = Vector2.zero;

    // ------------------------------------- ACCESSORS ----------------------------------- //
    public bool IsAttacking { get; protected set; }
    public eWeapon EquipWeap { get; protected set; }


    // ======================================================================================
    // PUBLIC MEMBERS
    // ======================================================================================
    void Start()
    {
        m_control = this.GetComponent<PlayerController>();
        m_input = this.GetComponent<PlayerInputCtlr>();
        IsAttacking = false;

        EquipWeap = eWeapon.Fists;
    }

    // ======================================================================================
    void FixedUpdate()
    {
        // Attack Subsystem : triggers Attack and generates hurtboxes
        UpdateAttackSubsystem();

    }

    // ======================================================================================
    // PRIVATE MEMBERS - SUBSYSTEM HANDLERS
    // ======================================================================================
    private void UpdateAttackSubsystem()
    {
        if (IsAttacking)
        {
            return;
        }

        // GET INPUT
        bool doAttack = m_input.GetAttack();

        // Try to Trigger Event, if possible
        if (doAttack && !IsAttacking)
            StartAttack();
    }

    // ======================================================================================
    // PRIVATE MEMBERS - SUBSYSTEM EVENT STARTERS
    // ======================================================================================
    private void StartAttack()
    {
        m_attackDirection = new Vector2(m_input.GetHorizontal(), m_input.GetVertical());

        if (m_attackDirection.sqrMagnitude == 0)
            m_attackDirection = new Vector2(m_control.ForwardDir == PlayerController.eDirection.Right ? 1 : -1, 0);
        else
            m_attackDirection.Normalize();

        if (!m_control.IsWallSnapped && !m_control.IsWallSliding)
        {
            IsAttacking = true;

            switch (EquipWeap)
            {
                case eWeapon.Fists:
                    {
                        PunchAttack();
                        break;
                    }
                case eWeapon.Saber:
                    {
                        SaberAttack();
                        break;
                    }
                case eWeapon.Pistol:
                    {
                        PistolAttack();
                        break;
                    }
            }

        }
    }

    // ======================================================================================
    // PRIVATE MEMBERS - COOLDOWN HANDLERS
    // ======================================================================================
    private IEnumerator AttackDelay()
    {
        yield return new WaitForSeconds(m_attackCooldown);
        IsAttacking = false;
    }

    // ======================================================================================
    // PRIVATE MEMBERS - WEAPON ROUTINES
    // ======================================================================================
    private void PunchAttack()
    {
        Collider[] hitTargets = Physics.OverlapBox(transform.position + new Vector3(transform.localScale.x * PunchOffset.x, PunchOffset.y, 0), 0.4f * Vector3.one, Quaternion.identity, playerLayer);
        for (int i = 0; i < hitTargets.Length; i++)
        {
            StartCoroutine(hitTargets[i].GetComponent<DamageBehaviour>().GetStunned());
        }
        StartCoroutine(AttackDelay());
    }

    private void SaberAttack()
    {
        Collider[] hitTargets = Physics.OverlapBox(transform.position + new Vector3(transform.localScale.x * SaberOffset.x, SaberOffset.y, 0), 0.4f * Vector3.one, Quaternion.identity, playerLayer);
        for (int i = 0; i < hitTargets.Length; i++)
        {
            hitTargets[i].GetComponent<DamageBehaviour>().TakeDamage(this.m_input.m_nbPlayer);
        }
        StartCoroutine(AttackDelay());
    }

    private void PistolAttack()
    {
        GameObject obj = Instantiate(ProjectilePrefab, transform.position + new Vector3(transform.localScale.x * PistolOffset.x, PistolOffset.y, 0), Quaternion.identity);
        obj.GetComponent<Projectile>().MoveProjectile(new Vector3(transform.localScale.x * 30, 0, 0));
        obj.GetComponent<Projectile>().SetOrigin(this.m_input.m_nbPlayer);
        StartCoroutine(AttackDelay());
    }
}