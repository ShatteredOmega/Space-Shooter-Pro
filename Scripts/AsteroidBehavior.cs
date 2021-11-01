using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidBehavior : MonoBehaviour
{
    [SerializeField]
    private float _rotationSpeed = 15;
    [SerializeField]
    private GameObject _explosion;
    private SpawnManager _spawnManager;
    
    void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager not found");
        }
    }

    void Update()
    {
        transform.Rotate(0, 0, _rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "PlayerAttack")
        {
            Destroy(collider.transform.gameObject);
            Destruction();
        }

    }

    void Destruction()
    {
        if (_spawnManager == null)
        {
            Debug.LogError("Nothing Found");
            return;
        }
        _spawnManager.StartSpawning();
        GameObject explosion = Instantiate(_explosion, transform.position, Quaternion.identity);
        float explosionDuration = explosion.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
        Destroy(this.gameObject.GetComponent<CircleCollider2D>());
        Destroy(this.gameObject.GetComponent<SpriteRenderer>(), explosionDuration * 0.3f);
        Destroy(explosion, explosionDuration);
        Destroy(this.gameObject, explosionDuration);

    }
}
