using UnityEngine;
using System.Collections;

public class PlayerFSM : FSMBase {

    public int maxHP = 100;
    public int curHP;
    public float attack;
    public float moveSpeed;
    public float jumpSpeed;
    public float gravity;
    private Vector3 dir;
    public Animation waeapon;
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        //FSM세팅.
        if (h != 0 || v != 0)
        {
            SetState(CharacterState.Walk);
        }
        else
        {
            SetState(CharacterState.Idle);
        }

        //Move Rogic
        dir = new Vector3(moveSpeed * -v, dir.y, moveSpeed * h);
        if (_cc.isGrounded)
        {
            dir.y = 0f;
            if (Input.GetButtonDown("Jump"))
                dir.y = jumpSpeed;
        }
        dir.y -= gravity * Time.deltaTime;
        _cc.Move( dir * Time.deltaTime);
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
