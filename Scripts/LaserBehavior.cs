using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBehavior : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8.0f;
    private bool _firedByEnemy;

    // Update is called once per frame
    void Update()
    {
        if(gameObject.tag == "Enemy")
        {
            EnemyAttackBehavior(); 
        }
        else
        {
            NormalMovement();
        }
        
    }
    public void EnemyModifier()
    {

    }
    void NormalMovement()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);
        float y = transform.position.y;
        if (y >= 8)
        {
            Transform parent = transform.parent;
            if (parent != null)
            {
                if (parent.childCount <= 2)
                {
                    Destroy(parent.gameObject);
                }
            }
            Destroy(this.gameObject);
        }
    }
    void EnemyAttackBehavior()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime * -1);
        float y = transform.position.y;
        if (y <= -8)
        {
            Transform parent = transform.parent;
            if (parent != null)
            {
                if (parent.childCount <= 2)
                {
                    Destroy(parent.gameObject);
                }
            }
            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.tag == "Player" && gameObject.tag == "Enemy")
        {
            collider.GetComponent<PlayerBehavior>().Damage();
            Destroy(this.gameObject);
        }
    }
}
