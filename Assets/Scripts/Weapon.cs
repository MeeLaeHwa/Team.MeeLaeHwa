using UnityEngine;
using System.Collections;
enum Owner
{
    PLAYER,
    ENEMY
}
public class Weapon : MonoBehaviour {
    public EnemyFSM enemy;
    public PlayerFSM player;
    private Owner owner;
    void Awake()
    {
        Transform temp = transform.root;
        if(temp.CompareTag("Player"))
        {
            owner = Owner.PLAYER;
        }
        else if(temp.GetChild(0).CompareTag("Enemy"))
        {
            owner = Owner.ENEMY;
        }
        Debug.Log(owner);
    }
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("check");
        switch (owner) {
            case Owner.PLAYER:
                if (other.transform.CompareTag("Enemy"))
                    player.SendAttack(other.transform.GetComponent<EnemyFSM>());
                break;
            case Owner.ENEMY:
                if (other.transform.CompareTag("Player"))
                {
                    enemy.SendAttack();
                }
                break;
        }
    }
}
