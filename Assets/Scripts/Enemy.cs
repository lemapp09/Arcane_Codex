using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 1.5f;
    [SerializeField]
    private float _fireDelay = 2.5f;
    private float _fireTimer = 0.0f;
    
    // Need to fly the enemy from the top of the screen to the bottom of the screen
    private Vector3 _direction = new Vector3(0, -1, 0);
    private bool _gameOver = false;
    private MissilePooler _missilePooler; // Reference to the MissilePooler script

    private void Start()
    {
        FireMissile();
    }

    private void Update()
    {
        if (!_gameOver)
        {
            transform.Translate(_speed * Time.deltaTime * _direction);
            if (transform.position.y < -6.0f)
            {
                GameManager.Instance.EnemyExitedScreen();
                Destroy(this.gameObject);
            }

            // FireMissile once every Fire Delay seconds
            _fireTimer += Time.deltaTime;
            if (_fireTimer >= _fireDelay)
            {
                FireMissile();
                _fireTimer = 0.0f;
            }
        }
    }

    private void FireMissile()
    {
        // Get a missile from the pool
        GameObject missile = _missilePooler.GetEnemyMissile();
        missile.transform.position = this.transform.position; // Position it at the spawn point
        missile.SetActive(true); // Activate the missile
    }

    public void SetMissilePooler(MissilePooler missilePooler)
    {
        _missilePooler = missilePooler;
    }
     
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerMissile"))
        {
            GameManager.Instance.EnemyDestroyed();
            Destroy(this.gameObject);
        }
    }
}
