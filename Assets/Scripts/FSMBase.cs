using UnityEngine;
using System.Collections;

public class FSMBase : MonoBehaviour
{
    public CharacterState state;
    public Animator _a;
    public CharacterController _cc;

    public bool _isNewState;

    // Update is called once per frame
    protected virtual void Awake()
    {
        _cc = GetComponent<CharacterController>();
        _a = GetComponentInChildren<Animator>();
    }
    protected virtual void OnEnable()
    {
        state = CharacterState.Idle;
        _a.SetInteger("state", (int)state);
        StartCoroutine(FSMMain());
    }
    public void SetState(CharacterState _newState)
    {
        _isNewState = true;
        state = _newState;
        _a.SetInteger("state", (int)state);
    }


    IEnumerator FSMMain()
    {
        while (true)
        {
            _isNewState = false;
            yield return StartCoroutine(state.ToString());
        }
    }
    protected virtual IEnumerator Idle()
    {
        while (!_isNewState)
        {
            yield return null;
        }
    }
    public bool IsDead()
    {
        return (state == CharacterState.Dead);
    }
}