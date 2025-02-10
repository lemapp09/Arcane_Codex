using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
        
    [SerializeField]
    private UIDocument _uiDocument;
    private VisualElement _root;
    private Label _missileCountLabel, _missileCount, _scoreLabel, _scoreCount, _livesCountLabel, _livesCount,
        _levelLabel, _levelCount;
    
    // UIManager is a singleton
    public static UIManager Instance { get; private set; }
    private void Awake() => Instance = this;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _root = _uiDocument.rootVisualElement;
        _missileCountLabel = _root.Q<Label>("MissileCountLabel");
        _missileCount = _root.Q<Label>("MissileCount");
        _levelLabel = _root.Q<Label>("LevelLabel");
        _levelCount = _root.Q<Label>("LevelCount");
        _livesCountLabel = _root.Q<Label>("LivesCountLabel");
        _livesCount = _root.Q<Label>("LivesCount");
        _scoreLabel = _root.Q<Label>("ScoreLabel");
        _scoreCount = _root.Q<Label>("ScoreCount");
    }

    public void InitializeTitles()
    {        
        _missileCountLabel.text = "Missiles:";
        _levelLabel.text = "Level:";
        _livesCountLabel.text = "Lives:";
        _scoreLabel.text = "Score:";


    }
    
    public void SetMissileCount(int count) => _missileCount.text = count.ToString();
    
    public void SetLevelCount(int count) => _levelCount.text = count.ToString();
    
    public void SetLivesCount(int count) => _livesCount.text = count.ToString();
    
    public void SetScoreCount(int count) => _scoreCount.text = count.ToString();
}
