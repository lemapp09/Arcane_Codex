using UnityEngine;

// https://github.com/ThomFoxx-GDHQ/ArcaneCodexSpaceShooter.git
// https://trello.com/invite/b/67a152df33b31b903758f93a/ATTI1badcd67746ba3946827c1c650c32cb3615A199F/arcane-codex-space-shooter
public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5.0f;
    
    private float _horizontalInput, _verticalInput;
    private Vector3 _direction = Vector3.zero;
    
    [SerializeField]private float _leftBound = -10.0f;
    [SerializeField]private float _rightBound = 10.0f;
    [SerializeField]private float _topBound = 6.0f;
    [SerializeField]private float _bottomBound = -6.0f;
    
    [SerializeField] private Transform _laserContainer;  // aka Missile Pool
    private MissilePooler _missilePooler; // Reference to the MissilePooler script
    private bool _IsFiring = false;
    [SerializeField]
    private float _laserCoolDown = 0.25f;  // Time between laser shots
    private float _laserTimer = 0.0f;
    
    private bool _gameOver = false;
    private Camera _camera;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _missilePooler = _laserContainer.GetComponent<MissilePooler>();
        _camera = Camera.main;
    }
    

    // Update is called once per frame
    void Update()
    {
        if (!_gameOver)
        {
            CalucateMovement();
            Bounds();
            _laserTimer += Time.deltaTime;

            // Spawn Laser when Space is pressed
            if (Input.GetKeyDown(KeyCode.Space) && _laserTimer >= _laserCoolDown)
            {
                _IsFiring = true;
                _laserTimer = 0.0f;
                FireMissile();
            }

            if (Input.GetKey(KeyCode.Space) && _laserTimer >= _laserCoolDown && _IsFiring)
            {
                _laserTimer = 0.0f;
                FireMissile();
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                _IsFiring = false;
                _laserTimer = 0.0f;
            }
        }
    }

    private void FireMissile()
    {
        // Get a missile from the pool
        GameObject missile = _missilePooler.GetMissile();
        missile.transform.position = this.transform.position; // Position it at the spawn point
        missile.transform.parent = _laserContainer; // Set the parent to the container
        missile.SetActive(true); // Activate the missile
        GameManager.Instance.DecrementMissileCount();
        AudioManager.Instance.PlaySFX(0);
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
        
        /* public static float Clamp(float value, float min, float max) {
             if ((double) value < (double) min)
               value = min;
             else if ((double) value > (double) max)
               value = max;
             return value; } */
        
        transform.position = position;
    }
     
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyMissile"))
        {
            _missilePooler.ReturnEnemyMissileToPool(other.gameObject);
            GameManager.Instance.DecrementStrikes();
        }
    }
}
