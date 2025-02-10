using System;
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
    private int _levelsToWin = 5;
    [SerializeField]
    private GameObject _enemyPrefab;

    private int _enemyCount = 1, _enemyIndexCount, _missileCount, _livesCount, _strikes, _score = 0, _levelCount = 1;
    private bool  _isGamePaused = false, _isGameOver = false;
    
    [SerializeField] private MissilePooler missilePooler; // Reference to the MissilePooler
    
    // Define the events
    public static event Action GameStart;
    public static event Action GamePause;
    public static event Action GameResume;
    public static event Action GameOver;
    public static event Action GameWon;
    
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
        if (_isGameOver) return; // Prevent starting game after it's over
        
        GameStart?.Invoke();
        _isGameOver = false;
        UIManager.Instance.InitializeTitles();
        UIManager.Instance.SetMissileCount(_missileCount);
        UIManager.Instance.SetLevelCount(_levelCount);
        UIManager.Instance.SetLivesCount(_livesCount);
        UIManager.Instance.SetScoreCount(_score);
        StartCoroutine( AddEnemies(1)); // Level 1 has 1 enemy
    }
    // Call this method to pause the game
    public void PauseGame()
    {
        if (_isGameOver) return; // Prevent pausing after game over

        _isGamePaused = true;
        Time.timeScale = 0; // Stop game time when paused

        // Broadcast the GamePause event
        GamePause?.Invoke();
        _isGamePaused = true;
    }

    // Call this method to resume the game
    public void ResumeGame()
    {
        if (_isGameOver) return; // Prevent resuming after game over

        _isGamePaused = false;
        Time.timeScale = 1; // Resume game time

        // Broadcast the GameResume event
        GameResume?.Invoke();
        _isGamePaused = false;
    }

    public void DecrementMissileCount()
    {
        if (!_isGameOver && !_isGamePaused)
        {
            _missileCount--;
            UIManager.Instance.SetMissileCount(_missileCount);
        }
    }
    
    public void DecrementLivesCount()
    {
        if (!_isGameOver && !_isGamePaused)
        {
            _livesCount--;
            UIManager.Instance.SetLivesCount(_livesCount);
            if (_livesCount == 0)
            {
                _isGameOver = true;
                Time.timeScale = 0; // Stop game time when paused
                GameOver?.Invoke();
                _isGameOver = true;
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
        if (!_isGameOver || !_isGamePaused) // Return enemy to game if game is still active
        {
            StartCoroutine( AddEnemies(1));
        }
    }

    public void EnemyDestroyed()
    {
        if (!_isGameOver)
        {
            // Return enemy to game if game is still active
            _enemyCount--;

            // Add score for destroying enemy
            _score += 100;
            UIManager.Instance.SetScoreCount(_score);

            if (_enemyCount == 0)
            {
                UIManager.Instance.SetLevelCount(_levelCount);
                if (_levelCount >= _levelsToWin)
                {
                    GameWon?.Invoke();
                    _isGameOver = true;
                }
                else
                {
                    // Level up at the end of the level
                    _levelCount++;
                }
                // Insert a 2 second delay before starting the next level
                StartCoroutine(StartNextLevel());
            }
        }
    }

    private IEnumerator StartNextLevel()
    {
        GamePause.Invoke();
        yield return new WaitForSeconds(2f); // Wait for 2 seconds
        
        _enemyCount = _levelCount;
        UIManager.Instance.SetScoreCount(_score += 100);
        _missileCount = _missleCountInitial + (_levelCount * 10);
        UIManager.Instance.SetMissileCount(_missileCount);
        _strikes = 0;
        _livesCount = _livesInitial;
        UIManager.Instance.SetLivesCount(_livesInitial);
        GameResume.Invoke();
        if (_levelCount <= _levelsToWin)
        {
            StartCoroutine(AddEnemies(_levelCount));
        }
    }

    private IEnumerator AddEnemies(int numberOfEnemies)
    {
        for (int i = 0; i < numberOfEnemies; i++)
        {
            if (_isGameOver) yield break;
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

    public int GetStrikes()
    {
        return _strikes;
    }
}

