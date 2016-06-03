using UnityEngine;
using System.Collections;

public class AnimatorEvent : MonoBehaviour {
    public FSMBase _c;
	// Use this for initialization
	void WeaponEnabled () {
        _c.WeaponSetEnabled(true);
	}
    void WeaponDisabled()
    {
        _c.WeaponSetEnabled(false);
    }
}
