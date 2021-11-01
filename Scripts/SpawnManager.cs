using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private float _enemySpawnTime = 3.0f;
    [SerializeField]
    private GameObject[] _powerups;
    private int _numberOfPowerups;
    [SerializeField]
    private float _tripleShotPowerupSpawnTimeMin = 3.0f;
    [SerializeField]
    private float _tripleShotPowerupSpawnTimeMax = 7.0f;
    private bool _stopSpawning = false;
    void Start()
    {
        _numberOfPowerups = _powerups.Length;
    }
    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerUpRoutine());
    }
    IEnumerator SpawnEnemyRoutine()
    {
        while(_stopSpawning == false)
        {
            yield return new WaitForSeconds(_enemySpawnTime);
            float randomX = Random.Range(-11.0f, 11.0f);
            GameObject newEnemy = Instantiate(_enemyPrefab, new Vector3(randomX, 7, 0), Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
        }
    }

    IEnumerator SpawnPowerUpRoutine()
    {
        while(_stopSpawning == false)
        {
            float tripleShotWaitTime = Random.Range(_tripleShotPowerupSpawnTimeMin, _tripleShotPowerupSpawnTimeMax);
            yield return new WaitForSeconds(tripleShotWaitTime);
            float randomX = Random.Range(-11.0f, 11.0f);
            int randomPowerUpNumber = Random.Range(0, _numberOfPowerups);
            GameObject randomPowerUp = _powerups[randomPowerUpNumber];
            if (randomPowerUp != null)
            {
                GameObject newTripleShot = Instantiate(randomPowerUp, new Vector3(randomX, 7, 0), Quaternion.identity);
            }
            else
            {
                Debug.Log("Invalid Powerup");
            }
                
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }

}
