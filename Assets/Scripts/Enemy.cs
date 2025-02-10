using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 1.5f;
    [SerializeField]
    private float _fireDelay = 2.5f;

    [SerializeField] private GameObject[] _explosions;
    private MeshRenderer _meshRenderer;
    private float _fireTimer = 0.0f;
    
    // Need to fly the enemy from the top of the screen to the bottom of the screen
    private Vector3 _direction = new Vector3(0, -1, 0);
    private bool _isGamePaused = false, _isGameOver = false;
    
    private MissilePooler _missilePooler; // Reference to the MissilePooler script
    private void OnEnable()
    {
        // Subscribe to the GameManager events
        GameManager.GamePause += OnGamePause;
        GameManager.GameResume += OnGameResume;
        GameManager.GameOver += OnGameOver;
        GameManager.GameWon += OnGameWon;
    }

    private void OnDisable()
    {
        // Unsubscribe from the events to avoid memory leaks
        GameManager.GamePause -= OnGamePause;
        GameManager.GameResume -= OnGameResume;
        GameManager.GameOver -= OnGameOver;
        GameManager.GameWon -= OnGameWon;
    }

    private void OnGamePause()
    {
        _isGamePaused = true;
    }

    private void OnGameResume()
    {
        _isGamePaused = false;
    }

    private void OnGameOver()
    {
        Destroy(this.gameObject);
    }

    private void OnGameWon()
    {
        Destroy(this.gameObject);
    }

    private void Start()
    {
        _meshRenderer = GetComponentInChildren<MeshRenderer>();
        FireMissile();
    }

    private void Update()
    {
        if (!_isGameOver && !_isGamePaused)
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
        if (other.CompareTag("PlayerMissile") && !_isGameOver)
        {
            GameManager.Instance.EnemyDestroyed();
            _missilePooler.ReturnMissileToPool(other.gameObject);
            StartCoroutine( DestroyEnemy());
        }
    }

    private IEnumerator DestroyEnemy()
    {
        _explosions[Random.Range(0, _explosions.Length)].SetActive(true);
        yield return new WaitForSeconds(0.5f);
        _meshRenderer.enabled = false;
        yield return new WaitForSeconds(3.0f);
        Destroy(this.gameObject);
    }
}
