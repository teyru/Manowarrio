using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeAttacking : MonoBehaviour
{
    [SerializeField] private EnemyControle enemy;
    private void Start()
    {
        enemy.GetComponent<EnemyControle>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "PlayerWeapon")
        {
            enemy._Health -= 1;
            enemy.GetHits(enemy._Health);
        }
    }
}
