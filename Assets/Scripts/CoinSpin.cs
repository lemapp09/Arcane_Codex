using UnityEngine;

public class CoinSpin : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5.0f;
    
    // Array of textures (frames of the coin spin)
    public Texture2D[] coinFrames;

    // Reference to the material
    private Material material;

    // Time between frames (adjust this to control animation speed)
    public float frameRate = 0.1f;

    // Timer to track elapsed time
    private float timer;

    // Index of the current frame
    private int currentFrame = 0;
    
    private MissilePooler _missilePooler;
    private CoinType _coinType;

    void Start()
    {
        // Get the material from the quad
        material = GetComponent<Renderer>().material;

        // Ensure we have textures to display
        if (coinFrames.Length == 0)
        {
            Debug.LogError("No coin frames set!");
            return;
        }
    }

    void OnEnable()
    {
        GameManager.GameOver += OnGameOver;
        GameManager.GameWon += OnGameOver;
    }

    void OnDisable()
    {
        GameManager.GameOver -= OnGameOver;
        GameManager.GameWon -= OnGameOver;
    }

    void Update()
    {
        // Increment timer based on frame rate
        timer += Time.deltaTime;

        // If enough time has passed for the next frame
        if (timer >= frameRate)
        {
            // Reset timer
            timer = 0f;

            // Update the texture
            material.mainTexture = coinFrames[currentFrame];

            // Move to the next frame (loop back to 0 if we reach the end)
            currentFrame = (currentFrame + 1) % coinFrames.Length;
        }
        
        transform.Translate( _speed * Time.deltaTime * Vector3.down);
        if (transform.position.y < -7.0f)
        {
            _missilePooler.ReturnCoinToPool(this.gameObject);
            this.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerMissile"))
        {
            GameManager.Instance.CoinWon(_coinType);
            _missilePooler.ReturnCoinToPool(this.gameObject);
        }
    }
    

    public void SetMissilePooler(MissilePooler missilePooler)
    {
        _missilePooler = missilePooler;
    }

    public void SetCoinType(CoinType coinType)
    {
        _coinType = coinType;
    }

    private void OnGameOver()
    {
        _missilePooler.ReturnCoinToPool(this.gameObject);
    }
}

// Enums for the various types of coins
public enum CoinType
{
    Add1Life,
    Remove1Life,
    Add25Missiles,
    Remove25Missiles,
    Add100Score,
    Remove100Score,
    AddTempSpeed,
    AddDoubleMissiles,
    AddTripleMissiles,
    AddQuadrupleMissiles
}