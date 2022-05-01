using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private Joystick _joystick;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Transform _plyerView;

    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private float _attackRange;



    [SerializeField] private float _speed;


    private EnemyControle[] _enemies;
    private List<GameObject> _enemyList = new List<GameObject>();


    

    private Animator _animator;
    private Vector3 _acceleration;


    private int _velocityHash;

    private float velocity = 0f;

    
    private void OnEnable()
    {
        EnemyControle.OnEnemyDestroed += RemoveEnemies;
    }

    private void OnDisable()
    {
        EnemyControle.OnEnemyDestroed -= RemoveEnemies;
    }

    void Start()
    {

        _animator = GetComponent<Animator>();
        _velocityHash = Animator.StringToHash("Move");

        _enemies = FindObjectsOfType<EnemyControle>();
        foreach (var enemy in _enemies)
        {
            enemy.GetComponent<EnemyControle>();
            _enemyList.Add(enemy.gameObject);
        }
    }

    private void RemoveEnemies(GameObject enemyObject)
    {
        _enemyList.Remove(enemyObject);
    }



       

    private void FixedUpdate()
    {
        MoveControle();
    }



    private void MoveControle()
    {

        bool _enemyInAttackRange = Physics.CheckSphere(transform.position, _attackRange, _enemyLayer);
        _animator.SetBool("EnemyPresence", _enemyInAttackRange);

        if (Mathf.Abs(_joystick.Horizontal) > 0.3f || Mathf.Abs(_joystick.Vertical) > 0.3f)
        {
            Acceleration();
        }
        else
        {
            Deceleration();
        }
        

        _plyerView.position = new Vector3(_plyerView.position.x, _plyerView.position.y, transform.position.z );

        if (_acceleration != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(_acceleration);
        }



        if (_enemyInAttackRange && _acceleration == Vector3.zero)
        {
            PlayerAttack();
        }

    }


    private void PlayerAttack()
    {
        float minDistanceToEnemy = _attackRange + 1f;
        foreach (var enemy in _enemyList)
        {
            float dist = Vector3.Distance(enemy.transform.position, transform.position);

            if (minDistanceToEnemy > dist)
            {
                minDistanceToEnemy = dist;
                transform.LookAt(enemy.transform.position);
            }
        }
    }

    private void Deceleration()
    {
        if (velocity > 0) velocity -= 0.05f;
        if (velocity < 0) { velocity = 0f; _acceleration = Vector3.zero; }
        _animator.SetFloat(_velocityHash, velocity);
    }

    private void Acceleration()
    {
        _acceleration = new Vector3(_joystick.Horizontal * _speed * Time.deltaTime, _rigidbody.velocity.y, _joystick.Vertical * _speed * Time.deltaTime);
        transform.position += _acceleration;
        velocity = Mathf.Max(Mathf.Abs(_joystick.Horizontal), Mathf.Abs(_joystick.Vertical));
        _animator.SetFloat(_velocityHash, velocity);
    }
}
