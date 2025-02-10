using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int _missleCountInitial, _livesInitial;
    [SerializeField]
    [Tooltip("Number of strikes before losing a life")]
    private int _strikesInitial;
    [SerializeField]
    private int _levelsToWin;
    [SerializeField]
    private GameObject _enemyPrefab;

    private int _enemyCount = 1, _enemyIndexCount, _missileCount, _livesCount, _strikes, _score = 0, _levelCount = 1;
    private bool _gameActive = false;
    
    [SerializeField] private MissilePooler missilePooler; // Reference to the MissilePooler
    
    // GameManager is a singleton
    public static GameManager Instance { get; private set; }
    private void Awake() => Instance = this;
    
    void Start()
    {
        _missileCount = _missleCountInitial;
        _livesCount = _livesInitial;
    }

    public void StartGame()
    {
        _gameActive = true;        
        UIManager.Instance.InitializeTitles();
        UIManager.Instance.SetMissileCount(_missileCount);
        UIManager.Instance.SetLevelCount(_levelCount);
        UIManager.Instance.SetLivesCount(_livesCount);
        UIManager.Instance.SetScoreCount(_score);
        StartCoroutine( AddEnemies(1)); // Level 1 has 1 enemy
    }

    public void DecrementMissileCount()
    {
        if (_gameActive)
        {
            _missileCount--;
            UIManager.Instance.SetMissileCount(_missileCount);
        }
    }
    
    public void DecrementLivesCount()
    {
        if (_gameActive)
        {
            _livesCount--;
            UIManager.Instance.SetLivesCount(_livesCount);
            if (_livesCount == 0)
            {
                _gameActive = false;
                Debug.Log("Game Over");
                // Broadcast event that game is over
            }
        }
    }

    public void DecrementStrikes()
    {
        _strikes++;
        if (_strikes == _strikesInitial)
        {
            DecrementLivesCount();
            _strikes = 0;
        }
    }

    public void EnemyExitedScreen()
    {
        if (_gameActive) // Return enemy to game if game is still active
        {
            StartCoroutine( AddEnemies(1));
        }
    }

    public void EnemyDestroyed()
    {
        _enemyCount--;
        
        // Add score for destroying enemy
        _score += 100;
        UIManager.Instance.SetScoreCount(_score);
        
        if (_enemyCount == 0)
        {
            // Level up
            _levelCount++;
            UIManager.Instance.SetLevelCount(_levelCount);
            if (_levelCount > _levelsToWin)
            {
                // Game Won
            }
            _enemyCount = _levelCount;
            UIManager.Instance.SetScoreCount(_score += 100);
            _missileCount = _missleCountInitial + (_levelCount * 10);
            UIManager.Instance.SetMissileCount(_missileCount);
            _livesCount = _livesInitial;
            UIManager.Instance.SetLivesCount(_livesInitial);
            StartCoroutine(AddEnemies(_levelCount));
        }
    }

    private IEnumerator AddEnemies(int numberOfEnemies)
    {
        for (int i = 0; i < numberOfEnemies; i++)
        {
            GameObject enemy = Instantiate(_enemyPrefab, new Vector3(Random.Range(-10, 10), 7, 0),
                Quaternion.identity);
            float scaleFactor = 1.1f - ((_levelCount) * 0.1f);
            enemy.transform.localScale = enemy.transform.localScale * scaleFactor;
            enemy.transform.name = "Enemy " + _enemyIndexCount.ToString().PadLeft(4, '0');
            _enemyIndexCount++;
            enemy.GetComponent<Enemy>().SetMissilePooler(missilePooler);
            yield return new WaitForSeconds(1f);
        }
    }
}

