using UnityEngine;
using System.Collections;

public class PlayerFSM : FSMBase {

    public int maxHP = 100;
    public int curHP;
    public float attack;
    public float moveSpeed;
    public float jumpSpeed;

    public Animation waeapon;
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if (h != 0 || v != 0)
        {
            SetState(CharacterState.Walk);
            Vector3 dir = new Vector3(-v, 0f, h);
            _cc.Move(moveSpeed * dir * Time.deltaTime);
        }
        else
        {
            SetState(CharacterState.Idle);
        }
    }
    protected override void Awake()
    {
        base.Awake();
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        curHP = maxHP;
    }
    protected override IEnumerator Idle()
    {
        while (_isNewState)
        {
            yield return null;
        }
    }
    protected virtual IEnumerator Walk()
    {
        while(_isNewState)
        {
            waeapon.Play("Weapon");
            yield return null;
        }
    }
}
