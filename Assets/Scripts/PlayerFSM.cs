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
    public Collider cWeapon;
    private Vector3 hitVec;
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
        dir = new Vector3(moveSpeed * -v, dir.y, moveSpeed * h);

        dir.y -= gravity * Time.deltaTime;
        if(!(state == CharacterState.Hit))
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
        cWeapon.enabled = false;
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
                if (Input.GetButtonDown("Jump") && state != CharacterState.Attack && state != CharacterState.Jump && state != CharacterState.Hit)
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
        Debug.Log(_t);
        while (!_isNewState)
        {
            yield return null;
            _t += Time.deltaTime;
            _cc.Move(hitVec *5  * Time.deltaTime);
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
    public void SendAttack(EnemyFSM taget)
    {
        Vector3 dir = (taget.transform.position - transform.position);
        dir.Set(0, 0, dir.z);
        dir.Normalize();
        taget.ProcessDamage(attack, dir);
    }
    public void ProcessDamage(float damage, Vector3 hitVec)
    {
        this.hitVec = hitVec;
        currentHp -= (int)damage;
        SetState(CharacterState.Hit);
    }
    public override void WeaponSetEnabled(bool enabled)
    {
        cWeapon.enabled = enabled;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("DeadZone")) SetState(CharacterState.Dead);
    }
}