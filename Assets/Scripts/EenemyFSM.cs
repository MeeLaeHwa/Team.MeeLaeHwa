using UnityEngine;
using System.Collections;

public class EenemyFSM : FSMBase {
    public int currentHp = 100;
    public int MaxHp = 100;
    public float resetTime = 1.5f;
    public float catchRange = 10f;
    public float attack = 10f;
    protected Transform player;
    protected PlayerFSM _playerFSM;

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

        base.OnEnable();
    }

    protected override IEnumerator Idle()
    {
        do
        {
            //Process
            yield return null;
            if (Vector3.Distance(transform.position, player.position) < catchRange)
            {
                SetState(CharacterState.Run);
                break;
            }
        } while (!_isNewState);
    }
    protected virtual IEnumerator Run()
    {
        while(_isNewState)
        {
            yield return null;
        }
    }

    protected virtual IEnumerator Attack()
    {
        while (_isNewState)
        {
            yield return null;
        }
    }
    protected virtual IEnumerator Hit()
    {
        while (_isNewState)
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
        _playerFSM.ProcessDamage(attack);
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
