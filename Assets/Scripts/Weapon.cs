using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {
    public EnemyFSM enemy;
    void OnTriggerEnter(Collider other)
    {
        if(other.transform.CompareTag("Player"))
        {
            enemy.SendAttack();
        }
    }

}
