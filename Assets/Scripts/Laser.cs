using System;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5.0f;

    private MissilePooler _missilePooler;

    private void OnEnable()
    {
        GameManager.GameOver += OnGameOver;
        GameManager.GameWon += OnGameWon;
    }

    private void OnDisable()
    {
        GameManager.GameOver -= OnGameOver;
        GameManager.GameWon -= OnGameWon;
    }

    private void OnGameOver()
    {
        _missilePooler.ReturnMissileToPool(this.gameObject);
        this.gameObject.SetActive(false);
    }

    private void OnGameWon()
    {
        _missilePooler.ReturnMissileToPool(this.gameObject);
        this.gameObject.SetActive(false);
    }

    void Update()
    {
        transform.Translate( _speed * Time.deltaTime * Vector3.up);
        if (transform.position.y > 7.0f)
        {
            _missilePooler.ReturnMissileToPool(this.gameObject);
            this.gameObject.SetActive(false);
        }
    }

    public void SetMissilePooler(MissilePooler missilePooler)
    {
        _missilePooler = missilePooler;
    }
    
}
