using System;
using UnityEngine;

public class EnemyLaser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5.0f;

    private MissilePooler _missilePooler;

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
}
