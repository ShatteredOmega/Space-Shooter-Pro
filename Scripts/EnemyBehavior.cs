using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;
    [SerializeField]
    private AudioClip _explosionSound;
    private PlayerBehavior _playerBehaviour;
    private Animator _animator;
    [SerializeField]
    private int _pointValue = 10;
    [SerializeField]
    private GameObject _enemyAttackPrefab;
    //For fring cases
    private bool _hasBeenHit = false;

    private float _canFire = 0;
    private float _fireRate = 1.0f;

    private void Start()
    {
        _playerBehaviour = GameObject.Find("Player").GetComponent<PlayerBehavior>();
        if (_playerBehaviour == null)
        {
            Debug.LogError("No Player Found");
        }
        _animator = this.gameObject.GetComponent<Animator>();
        if(_animator == null)
        {
            Debug.LogError("No Animator Found");
        }
        StartCoroutine("Shooting");
    }
    // Update is called once per frame
    void Update()
    {
        Movement();
        if(Time.time >= _canFire)
        {
            Shooting();
        }
    }
    private void Movement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y <= -5)
        {
            //Range was -7 to 7 in the tutorial, why?
            float randomX = Random.Range(-11.0f, 11.0f);
            transform.position = new Vector3(randomX, 7, 0);
        }
    }

    void Shooting()
    {
        _canFire = Time.time + _fireRate;
        GameObject Attack = Instantiate(_enemyAttackPrefab, transform.position, Quaternion.identity);
        foreach(Transform child in Attack.transform)
        {
            child.tag = "Enemy";
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (_hasBeenHit == false)
        {
            if (collider.tag == "Player")
            {
                PlayerBehavior playerBehavior = collider.GetComponent<PlayerBehavior>();
                if (playerBehavior != null)
                {
                    _hasBeenHit = true;
                    playerBehavior.Damage();
                    playerBehavior.EnemyKill(_pointValue);
                }
                Destruction();
            }
            if (collider.tag == "PlayerAttack")
            {
                _hasBeenHit = true;
                if (_playerBehaviour != null)
                {
                    _playerBehaviour.EnemyKill(_pointValue);
                }
                Destroy(collider.transform.gameObject);
                Destruction();
            }
        }
    }
    void Destruction()
    {
        _animator.SetTrigger("IsDestroyed");
        _speed = 2f;
        AudioSource audio = GetComponent<AudioSource>();
        audio.clip = _explosionSound;
        audio.Play();
        Destroy(this.gameObject.GetComponent<BoxCollider2D>());
        //StartCoroutine("DestructionEnd");
        Destroy(this.gameObject, _animator.GetCurrentAnimatorStateInfo(0).length);
    }
    IEnumerator DestructionEnd()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(this.gameObject, _animator.GetCurrentAnimatorStateInfo(0).length);
    }
}
