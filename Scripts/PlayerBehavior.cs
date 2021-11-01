using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerBehavior : MonoBehaviour
{
    [SerializeField]
    private SpawnManager _spawnManger;
    [SerializeField]
    private UIManager _uiManager;

    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private AudioClip _laserFire;
    [SerializeField]
    private float _fireRate = 0.15f;
    private float _canFire = -1.0f;
    [SerializeField]
    private float _tripleShotDuration = 5.0f;
    private bool _tripleShotEnabled = false;

    [SerializeField]
    private int _lives = 3;

    [SerializeField]
    private float _speed = 3.5f;
    [SerializeField]
    private float _speedBoostDuration = 5.0f;
    private bool _speedBoostEnabled = false;
    [SerializeField]
    private float _modifiedSpeed = 10f;

    [SerializeField]
    private GameObject _shieldVisualizer;
    [SerializeField]
    private float _shieldDuration = 5.0f;
    private bool _shieldEnabled = false;

    private float _upperBound = 0.0f;
    private float _lowerBound = -3.8f;
    private float _rightBound = 11.3f;
    private float _leftBound = -11.3f;
    [SerializeField]
    private GameObject _leftEngineDamage, _rightEngineDamage;

    [SerializeField]
    private GameObject _explosion;
    // Start is called before the first frame update
    void Start()
    {
        //take the current position = new position (0,0,0)
        transform.position = new Vector3(0, 0, 0);
        _spawnManger = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if (_spawnManger == null)
        {
            Debug.LogError("The Spawn Manager is NULL");
        }
        _uiManager.SetLifeUI(_lives);
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKey(KeyCode.Space)) && Time.time >= _canFire)
        {
            FireLaser();
        }
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        //OPTIMIZATION: the less you use 'new', the better.
        //CLEAN UP: use a local variable
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        float moveSpeed = _speed;
        if(_speedBoostEnabled == true)
        {
            moveSpeed = _modifiedSpeed;
        }
        transform.Translate(direction * moveSpeed * Time.deltaTime);

        float x = transform.position.x;
        float y = transform.position.y;
        float z = transform.position.z;

        //Bounded movement
        transform.position = new Vector3(Mathf.Clamp(x, _leftBound, _rightBound), Mathf.Clamp(y, _lowerBound, _upperBound), z);

        //X wrapping
        //    if (x > _rightBound)
        //    {
        //        x = _leftBound;
        //    }
        //    else if (x < _leftBound)
        //    {
        //        x = _rightBound;
        //    }
        //    transform.position = new Vector3(x, Mathf.Clamp(y, _lowerBound, _upperBound), z);
    }

    void FireLaser()
    {
        //You can add vectors to create an offset
        _canFire = Time.time + _fireRate;
        if (_tripleShotEnabled == true && _tripleShotEnabled)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1f, 0), Quaternion.identity);
            _tripleShotEnabled = false;
        }
        AudioSource audio = GetComponent<AudioSource>();
        audio.clip = _laserFire;
        audio.Play();


    }

    //void is private by default
    public void Damage()
    {
        if(_shieldEnabled == true)
        {

            ShieldDisable();
            //StartCoroutine("ShieldPowerDownRoutine");
            return;
        }
        //If using timed shield, make the bottom conditional on _shieldEnabled being false and remove the code above
        _lives--;
        Boolean boolValue = (Random.Range(0, 2) == 0);
        if (boolValue == true && _rightEngineDamage.activeSelf == false || _leftEngineDamage.activeSelf == true && _rightEngineDamage.activeSelf == false)
        {
            _rightEngineDamage.SetActive(true);
            
        }
        else
        {
            _leftEngineDamage.SetActive(true);
        }
        _uiManager.SetLifeUI(_lives);
        if (_lives <= 0)
        {
            _spawnManger.OnPlayerDeath();
            _uiManager.GameOver();
            //Destroy(this.gameObject);
            Death();
        }
        
    }

    public void TripleShotEnable()
    {
        _tripleShotEnabled = true;
        StopCoroutine("TripleShotPowerDownRoutine");
        StartCoroutine("TripleShotPowerDownRoutine");
    }
    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(_tripleShotDuration);
        _tripleShotEnabled = false;
    }

    public void SpeedBoostEnable()
    {
        _speedBoostEnabled = true;
        StopCoroutine("SpeedBoostPowerDownRoutine");
        StartCoroutine("SpeedBoostPowerDownRoutine");
    }

    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(_speedBoostDuration);
        _speedBoostEnabled = false;
    }

    public void ShieldEnable()
    {
        _shieldEnabled = true;
        _shieldVisualizer.SetActive(true);
        //StopCoroutine("ShieldPowerDownRoutine");
        //StartCoroutine("ShieldPowerDownRoutine");
    }
    public void ShieldDisable()
    {
        _shieldEnabled = false;
        _shieldVisualizer.SetActive(false);
    }
    //This and above commented code is for timed shields

    IEnumerator ShieldPowerDownRoutine()
    {
        yield return new WaitForSeconds(_shieldDuration);
        ShieldDisable();
    }

    public void EnemyKill(int pointsGained)
    {
      _uiManager.AddScore(pointsGained);
    }
    
    void Death()
    {
        GameObject explosion = Instantiate(_explosion, transform.position, Quaternion.identity);
        float explosionDuration = explosion.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
        Destroy(this.gameObject.GetComponent<SpriteRenderer>(), explosionDuration * 0.3f);
        //Destroy all child gameobjects
        foreach(Transform child in transform)
        {
            Destroy(child.gameObject, explosionDuration * 0.3f);
        }
        Destroy(explosion, explosionDuration);
        Destroy(this.gameObject, explosionDuration);
    }
}
