using UnityEngine;
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
    private Vector3 hitVec;
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
            Vector3 myPos = transform.position;
            Vector3 playerPos = player.position;
            myPos.y = 0;
            playerPos.y = 0;
            if (Vector3.Distance(myPos, playerPos) < catchRange && !_playerFSM.IsDead())
            {
                SetState(CharacterState.Run);
                break;
            }
            if(!_cc.isGrounded)
            {
                Debug.Log("isGrounded : " + _cc.isGrounded);
                _cc.Move(Physics.gravity * Time.deltaTime);
            }
        } while (!_isNewState);
    }
    protected virtual IEnumerator Run()
    {
        while(!_isNewState)
        {
            yield return null;
            Vector3 dir = transform.position - player.position;
            dir.Set(dir.x, 0, dir.z);
            Vector3 moveDir = dir.normalized * -1;


            if (moveDir.z > 0)
                transform.rotation = new Quaternion(0, 0, 0, 1);
            else if (moveDir.z < 0)
                transform.rotation = new Quaternion(0, 180, 0, 1);

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

            _cc.Move(moveDir * moveSpeed * Time.deltaTime + Physics.gravity*Time.deltaTime);
        }
    }

    protected virtual IEnumerator Attack()
    {
        while (!_isNewState)
        {
            yield return null;
            if(_a.GetCurrentAnimatorStateInfo(0).IsName("Attack") && _a.GetCurrentAnimatorStateInfo(0).normalizedTime % 1 > 0.9)
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
            _cc.Move(hitVec * 5 * Time.deltaTime);
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
    public void ProcessDamage(float damage, Vector3 hitVec)
    {
        this.hitVec = hitVec;
        currentHp -= (int)damage;
        SetState(CharacterState.Hit);
    }
    public override void WeaponSetEnabled(bool enabled)
    {
        weapon.enabled = enabled;
    }
}
