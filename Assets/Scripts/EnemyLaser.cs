using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyLaser : MonoBehaviour
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

    private void OnGameWon()
    {
        _missilePooler.ReturnEnemyMissileToPool(this.gameObject);
        this.gameObject.SetActive(false);
    }

    private void OnGameOver()
    {
        _missilePooler.ReturnEnemyMissileToPool(this.gameObject);
        this.gameObject.SetActive(false);
    }

    void Update()
    {
        transform.Translate( _speed * Time.deltaTime * Vector3.up);
        if (transform.position.y < -7.0f)
        {
            _missilePooler.ReturnEnemyMissileToPool(this.gameObject);
            this.gameObject.SetActive(false);
        }
    }

    public void SetMissilePooler(MissilePooler missilePooler)
    {
        _missilePooler = missilePooler;
    }

    public void DestroyMissile()
    {
        _missilePooler.ReturnEnemyMissileToPool(this.gameObject);
    }
}
