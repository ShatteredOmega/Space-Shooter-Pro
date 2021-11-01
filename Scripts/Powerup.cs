using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _powerupSpeed = 3.0f;
    [SerializeField]
    private int _powerUpID; // 0 = Triple Shot, 1 = Speed, 2 = Shield
    [SerializeField]
    private AudioClip _pickupSound;

    void Update()
    {
        transform.Translate(Vector3.down * _powerupSpeed * Time.deltaTime);
        if (transform.position.y <= -5)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        PlayerBehavior playerBehavior = collider.transform.GetComponent<PlayerBehavior>();
        if(playerBehavior != null)
        {
            switch(_powerUpID)
            {
                case 0:
                    playerBehavior.TripleShotEnable();
                    break;
                case 1:
                    playerBehavior.SpeedBoostEnable();
                    break;
                case 2:
                    playerBehavior.ShieldEnable();
                    break;
                default:
                    break;
            }
            AudioSource.PlayClipAtPoint(_pickupSound, transform.position);
            Destroy(this.gameObject);
        }
        
    }
}
