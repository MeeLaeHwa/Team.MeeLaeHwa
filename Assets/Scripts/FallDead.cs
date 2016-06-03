using UnityEngine;
using System.Collections;

public class FallDead : MonoBehaviour
{
    public EnemyFSM enemy;
    public PlayerFSM player;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("FallDead");
        if (other.transform.CompareTag("DeadZone"))
        {
            player.SetState(CharacterState.Dead);
            enemy.SetState(CharacterState.Dead);
        }

    }
}
