using UnityEngine;
using System.Collections;

public class PlayerFSM : FSMBase
{

    public int maxHP = 100;
    public int currentHp;
    public float attack;
    public float moveSpeed;
    public float jumpSpeed;
    public float gravity;
    private Vector3 dir;
    public Animation waeapon;


    private float h;
    private float v;
    void Update()
    {
        if (IsDead())
            return;
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        //Move Rogic
        if (state == CharacterState.Attack)
        {
            v = 0;
            h = transform.rotation.y != 0 ? -0.5f : 0.5f;
        }
        if (state == CharacterState.Hit)
        {
            v = 0;
            h = transform.rotation.y != 0 ? 0.3f : -0.3f;
        }
        dir = new Vector3(moveSpeed * -v, dir.y, moveSpeed * h);

        dir.y -= gravity * Time.deltaTime;
        _cc.Move(dir * Time.deltaTime);

        //Rotate
        if (h > 0)
            transform.rotation = new Quaternion(0, 0, 0, 1);
        else if (h < 0)
            transform.rotation = new Quaternion(0, 180, 0, 1);

    }
    protected override void Awake()
    {
        base.Awake();
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        currentHp = maxHP;
    }
    protected override IEnumerator Idle()
    {
        while (!_isNewState)
        {
            yield return null;
            if (h != 0 || v != 0)
            {
                SetState(CharacterState.Walk);
                break;
            }

            if (Input.GetButtonDown("Fire1"))
            {
                SetState(CharacterState.Attack);
                break;
            }
            if (_cc.isGrounded)
            {
                dir.y = -6f;
                if (Input.GetButtonDown("Jump") && state != CharacterState.Attack && state != CharacterState.Jump)
                {
                    dir.y = jumpSpeed;
                    SetState(CharacterState.Jump);
                    break;
                }
            }
        }
    }
    protected virtual IEnumerator Walk()
    {
        while (!_isNewState)
        {
            yield return null;
            if (h == 0 && v == 0)
            {
                SetState(CharacterState.Idle);
                break;
            }

            if (Input.GetButtonDown("Fire1"))
            {
                SetState(CharacterState.Attack);
                break;
            }
            if (_cc.isGrounded)
            {
                dir.y = -6f;
                if (Input.GetButtonDown("Jump") && state != CharacterState.Attack && state != CharacterState.Jump)
                {
                    dir.y = jumpSpeed;
                    SetState(CharacterState.Jump);
                    break;
                }
            }
        }
    }
    protected virtual IEnumerator Jump()
    {
        while (!_isNewState)
        {
            yield return null;
            if (_cc.isGrounded && _a.GetCurrentAnimatorStateInfo(0).normalizedTime % 1 > 0.2 && _a.GetCurrentAnimatorStateInfo(0).IsName("overlordLand"))
            {
                if (h == 0 && v == 0)
                {
                    SetState(CharacterState.Idle);
                    break;
                }
                else
                {
                    SetState(CharacterState.Walk);
                    break;
                }
            }
        }
    }
    protected virtual IEnumerator Attack()
    {
        while (!_isNewState)
        {
            yield return null;
            if (_a.GetCurrentAnimatorStateInfo(0).IsName("Attack") && _a.GetCurrentAnimatorStateInfo(0).normalizedTime % 1 > 0.9)
            {
                SetState(CharacterState.Idle);
                break;
            }
        }
    }
    protected virtual IEnumerator Hit()
    {
        float _t = 0;
        while (!_isNewState)
        {
            yield return null;
            _t += Time.deltaTime;
            if (currentHp <= 0)
            {
                currentHp = 0;
                SetState(CharacterState.Dead);
                break;
            }
            if (_t > 0.5)
            {
                SetState(CharacterState.Idle);
                break;
            }
        }
    }
    protected virtual IEnumerator Dead()
    {
        while (!_isNewState)
        {
            yield return null;
        }
    }
    public void SendAttack(EenemyFSM taget)
    {
        taget.ProcessDamage(attack);
    }
    public void ProcessDamage(float damage)
    {
        currentHp -= (int)damage;
        SetState(CharacterState.Hit);
    }
}