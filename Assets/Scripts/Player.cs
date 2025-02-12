using System.Collections;
using UnityEngine;

// https://github.com/ThomFoxx-GDHQ/ArcaneCodexSpaceShooter.git
// https://trello.com/invite/b/67a152df33b31b903758f93a/ATTI1badcd67746ba3946827c1c650c32cb3615A199F/arcane-codex-space-shooter
public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5.0f;
    
    private float _horizontalInput, _verticalInput;
    private Vector3 _direction = Vector3.zero;

    [SerializeField] private float _leftBound = -10.0f;
    [SerializeField] private float _rightBound = 10.0f;
    [SerializeField] private float _topBound = 6.0f;    
    [SerializeField] private float _bottomBound = -6.0f;
    
    [SerializeField] private Transform _laserContainer;  // aka Missile Pool
    private MissilePooler _missilePooler; // Reference to the MissilePooler script
    private bool _isFiring = false, _isGamePaused = false, _isGameOver = true;
    [SerializeField]
    private float _fireRate = 0f;  // Time between laser shots
    private Coroutine _fireCoroutine = null;
    private WaitForSeconds _fireTime;
    private int _missileCount = 1;
    
    private Material _playerMaterial;    private void OnEnable()
    {
        // Subscribe to the GameManager events
        GameManager.GameStart += OnGameStart;
        GameManager.GamePause += OnGamePause;
        GameManager.GameResume += OnGameResume;
        GameManager.GameOver += OnGameOver;
        GameManager.GameWon += OnGameWon;
        GameManager.PlayerChangeSpeed += OnPlayerChangeSpeed;
        GameManager.PlayerFiresMoreMissiles += OnPlayerFiresMoreMissiles;
    }

    private void OnDisable()
    {
        // Unsubscribe from the events to avoid memory leaks
        GameManager.GameStart -= OnGameStart;
        GameManager.GamePause -= OnGamePause;
        GameManager.GameResume -= OnGameResume;
        GameManager.GameOver -= OnGameOver;
        GameManager.GameWon -= OnGameWon;
        GameManager.PlayerChangeSpeed -= OnPlayerChangeSpeed;
        GameManager.PlayerFiresMoreMissiles += OnPlayerFiresMoreMissiles;
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _missilePooler = _laserContainer.GetComponent<MissilePooler>();
        _playerMaterial = GetComponentInChildren<MeshRenderer>().material;
        _fireTime = new WaitForSeconds(_fireRate);
    }
    
    // Update is called once per frame
    void Update()
    {
        if (!_isGameOver && !_isGamePaused)
        {
            CalucateMovement();
            Bounds();
            
            //Spawn Laser when hit Space bar
            if (Input.GetKeyDown(KeyCode.Space) && !_isFiring)
            {
                _isFiring = true;
                _fireCoroutine = StartCoroutine(FireSequence());
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                _isFiring = false;
                if (_fireCoroutine != null)
                {
                    StopCoroutine(_fireCoroutine);
                }
            }
        }
    }

    private IEnumerator FireSequence()
    {
        while (_isFiring && !_isGameOver && !_isGamePaused && _missileCount > 0)
        {
            FireMissile();
            yield return _fireTime;
        }
    }

    private void FireMissile()
    {
        GameObject[] firingMissile = new GameObject[Mathf.Min(_missileCount, 4)];

        for (int i = 0; i < _missileCount; i++)
        {
            firingMissile[i] = GetMissile();
            float rotation = i < _missileCount / 2 ? 5.0f : 10.0f; // Adjust based on missile index
            float translation = i < _missileCount / 2 ? -0.01f : 0.01f; // Adjust based on missile index


            if (i == 1)
            {
                firingMissile[i].transform.Translate(translation, 0, 0);
            }
            else
            if (i == 2)
            {
                firingMissile[i].transform.Translate(translation, 0, 0);
            }
            else
            {
                firingMissile[i].transform.Translate(-translation * 2, 0, 0);
            }
        }

        AudioManager.Instance.PlaySFX(0);
    }
    
    private GameObject GetMissile()
    {
        // Get a missile from the pool
        GameObject missile = _missilePooler.GetMissile();
        missile.transform.position = this.transform.position; // Position it at the spawn point
        missile.transform.parent = _laserContainer; // Set the parent to the container
        missile.SetActive(true); // Activate the missile
        GameManager.Instance.DecrementMissileCount();
        return missile;
    }

    private void CalucateMovement()
    {
        // Get Inputs (Legacy, later switch to new system)
        _horizontalInput = Input.GetAxis("Horizontal");
        _verticalInput = Input.GetAxis("Vertical");
        
        // Assign Inputs to Vector3 Direction
        // _direction = new Vector3(_horizontalInput,  _verticalInput); [Minor Saving]
        _direction.x = _horizontalInput;
        _direction.y = _verticalInput;
        
        // Use Direction to translate
        transform.Translate( _speed * Time.deltaTime * _direction);
    }

     private void Bounds()
    {
        Vector3 position = transform.position;
        position.y = Mathf.Clamp(position.y, _bottomBound, _topBound);
        
        // The player to wrap around the screen along the x axis
        if (position.x < _leftBound)
            position.x = _rightBound;
        else if (position.x > _rightBound)
            position.x = _leftBound;
        
        transform.position = position;
    }
     
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyMissile") && !_isGameOver)
        {
            other.GetComponent<EnemyLaser>().DestroyMissile();
            GameManager.Instance.DecrementStrikes();
            var strikes = GameManager.Instance.GetStrikes();
            _playerMaterial.color = Color.Lerp(Color.white, Color.red, (float)(3 - strikes) / 3.0f);
        }
    }

    private void OnPlayerChangeSpeed()
    {
        StartCoroutine(SpeedUpDelay());
    }

    private IEnumerator SpeedUpDelay()
    {
        _speed += 3.0f;
        yield return new WaitForSeconds(5.0f);
        _speed -= 3.0f;
    }

    private void OnPlayerFiresMoreMissiles(int missileCount)
    {
        StartCoroutine(FireMoreMissilesDelay(missileCount));
    }

    private IEnumerator FireMoreMissilesDelay( int missileCount)
    {
        _missileCount = missileCount;
        yield return new WaitForSeconds(5.0f);
        _missileCount = 1;
        yield return null;
    }

    // Event handlers
    private void OnGameStart()
    {
        _isGameOver = false;
    }

    private void OnGamePause()
    {
        _isGamePaused = true;
        var strikes = GameManager.Instance.GetStrikes();
        _playerMaterial.color = Color.Lerp(Color.white, Color.red, (float)(3 - strikes) / 3.0f);
        
    }

    private void OnGameResume()
    {
        _isGamePaused = false;
        var strikes = GameManager.Instance.GetStrikes();
        _playerMaterial.color = Color.Lerp(Color.white, Color.red, (float)(3 - strikes) / 3.0f);
    }

    private void OnGameOver()
    {
        _isGameOver = true;
        _playerMaterial.color = Color.white;
    }

    private void OnGameWon()
    {
        _isGameOver = true;
        _playerMaterial.color = Color.white;
    }
}
