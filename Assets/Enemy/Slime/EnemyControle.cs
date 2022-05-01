using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyControle : MonoBehaviour
{
    public delegate void EnemyDestroy(GameObject obj);
    public static event EnemyDestroy OnEnemyDestroed;
   // public static Action OnEnemyDestroed;

    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Transform _player;
    [SerializeField] private Animator _animator;
    [SerializeField] private LayerMask _whatIsGround, _whatIsPlayer;


    //Patrolling
    [SerializeField] private Vector3 _walkPoint;
    [SerializeField] private float _walkPointRange;
    private bool _walkPointSet;

    //Attacking

    //States
    [SerializeField] private float _sightRange, _attackRange;
    [SerializeField] private bool _playerInSightRange, _playerInAttackRange;


    private PlayerMovement playermove;


    [SerializeField] private Enemy _enemy;


    private string _name;

    private int _level;
    private int _health;
    public int _Health { get{ return _health; } set{ _health = value; } }

    private int _damage;

    private float _enemySpeed;
    private bool _targetIsRiched , _haveNewTarget;

    private void Awake()
    {
        _player = FindObjectOfType<PlayerMovement>().transform;
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _name = _enemy.name;
        _level = _enemy._level;
        _damage = _enemy._damage;
        _health = _enemy._health;
        _enemySpeed = _enemy._enemySpeed;

        _animator.SetInteger(Animator.StringToHash("Health"), _health);

        _walkPoint = transform.position;
    }

    private void Update()
    {
        _playerInSightRange = Physics.CheckSphere(transform.position, _sightRange, _whatIsPlayer);
        _playerInAttackRange = Physics.CheckSphere(transform.position, _attackRange, _whatIsPlayer);

        if (!_playerInSightRange && !_playerInAttackRange) Patroling();
        if (_playerInSightRange && _playerInAttackRange) Attacking();
        else
        if (_playerInSightRange && !_playerInAttackRange) MoveToPlayer();
    }

    private void Patroling()
    {
        if (!_walkPointSet)
        {
            SearchWalkPoint();
        }
        if (_walkPointSet) 
        {
            _animator.SetBool("IsRunning", true);
            _animator.SetBool("IsAttacking", false);
            _agent.SetDestination(_walkPoint); 
        }

        Vector3 distanceToWalkPoint = transform.position - _walkPoint;
        if (distanceToWalkPoint.magnitude < 1f)
        {
            _walkPointSet = false;
        }
    }

    private void SearchWalkPoint()
    {
        float randomZ = UnityEngine.Random.Range(-_walkPointRange, _walkPointRange);
        float randomX = UnityEngine.Random.Range(-_walkPointRange, _walkPointRange);

        _walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        if (Physics.Raycast(_walkPoint, -transform.up, 5f, _whatIsGround))
            _walkPointSet = true;
    }

    private void MoveToPlayer()
    {
            _agent.SetDestination(_player.position);
            _animator.SetBool("IsRunning", true);
            _animator.SetBool("IsAttacking", false);

    }

    private void Attacking() 
    {
        //доделать
             //transform.LookAt(_player); 
            _animator.SetBool("IsRunning", false);
            _animator.SetBool("IsAttacking", true);
    }

    public void GetHits(int health)
    {
        if (health > 0)
        {
            _animator.SetBool("GetHit", true);
            Invoke("StopHitAnimation", 0.4f);
        }
        else
        {
            _animator.SetInteger(Animator.StringToHash("Health"), 0);
            StartCoroutine(Destroyer());
        }

    }

    private void StopHitAnimation()
    {
        _animator.SetBool("GetHit", false);
    }

    IEnumerator Destroyer()
    {
        yield return new WaitForSeconds(1.2f);
        OnEnemyDestroed?.Invoke(gameObject);
        Destroy(gameObject);
    }
}
