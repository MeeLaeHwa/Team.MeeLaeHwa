﻿using UnityEngine;
using System.Collections;

public class EnemyFSM : FSMBase {
    public int currentHp = 100;
    public int MaxHp = 100;
    public float resetTime = 1.5f;
    public float catchRange = 10f;
    public float attack = 10f;
    public float moveSpeed = 5f;
    protected Transform player;
    protected PlayerFSM _playerFSM;
    public float attackRange = 2f;
    public Collider weapon;
    protected override void Awake()
    {
        base.Awake();

        player = GameObject.FindGameObjectWithTag("Player").transform;
        _playerFSM = player.GetComponent<PlayerFSM>();
        
    }

    protected override void OnEnable()
    {
        SetState(CharacterState.Idle);
        currentHp = MaxHp;

        _cc.enabled = true;
        weapon.enabled = false;
        base.OnEnable();
    }

    protected override IEnumerator Idle()
    {
        do
        {
            //Process
            yield return null;
            if (Vector3.Distance(transform.position, player.position) < catchRange && !_playerFSM.IsDead())
            {
                SetState(CharacterState.Run);
                break;
            }
        } while (!_isNewState);
    }
    protected virtual IEnumerator Run()
    {
        while(!_isNewState)
        {
            yield return null;
            Vector3 dir = transform.position - player.position;
            Vector3 moveDir = dir.normalized * -1;
            dir.Set(dir.x, 0, dir.z);
            if (Vector3.Distance(transform.position, transform.position+dir) < attackRange && !_playerFSM.IsDead())
            {
                SetState(CharacterState.Attack);
                break;
            }
            if(_playerFSM.IsDead())
            {
                SetState(CharacterState.Idle);
                break;
            }
            if (moveDir.z > 0)
                transform.rotation = new Quaternion(0, 0, 0, 1);
            else if (moveDir.z < 0)
                transform.rotation = new Quaternion(0, 180, 0, 1);

            _cc.Move(moveDir * moveSpeed * Time.deltaTime);
        }
    }

    protected virtual IEnumerator Attack()
    {
        weapon.enabled = true;
        while (!_isNewState)
        {
            yield return null;
            if(_a.GetCurrentAnimatorStateInfo(0).IsName("Attack") && _a.GetCurrentAnimatorStateInfo(0).normalizedTime % 1 > 0.9)
            {
                SetState(CharacterState.Idle);
                break;
            }
        }
        weapon.enabled = false;
    }
    protected virtual IEnumerator Hit()
    {
        while (!_isNewState)
        {
            yield return null;
            if (currentHp <= 0)
            {
                currentHp = 0;
                SetState(CharacterState.Dead);
            }
        }
    }
    protected virtual void Respwan()
    {
        gameObject.SetActive(true);
    }
    public void SendAttack()
    {
        Vector3 dir = (player.position - transform.position);
        dir.Set(0, 0, dir.z);
        dir.Normalize();
        _playerFSM.ProcessDamage(attack,dir);
    }
    public void ProcessDamage(float damage)
    {
        currentHp -= (int)damage;
        SetState(CharacterState.Hit);
        
        if (!IsDead() && state != CharacterState.Attack)
        {
        }
    }
}