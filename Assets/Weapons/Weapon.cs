using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private float _flySpeed;
    void Start()
    {
        Destroy(gameObject, 2);
    }

    private void Update()
    {
        transform.localPosition += transform.up * _flySpeed * Time.deltaTime;
    }

}
