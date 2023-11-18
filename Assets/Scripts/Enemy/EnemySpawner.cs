using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private float _maxRadius;
    [SerializeField] private float _spawnTime;
    [SerializeField] private GameObject _prefab;
    [SerializeField] private int _spawnCount;
    private float _timer = 0.0f;
    private List<GameObject> _spawnedEnemies = new List<GameObject>();
    private bool _hasSpawnedEnemy = false;

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!_hasSpawnedEnemy)
        {
            if(_spawnedEnemies.Count <= _spawnCount)
            {
                SpawnEnemy();
                _hasSpawnedEnemy = true;
            }
        }
        else
        {
            _timer += Time.fixedDeltaTime;
            if( _timer > _spawnTime )
            {
                _timer = 0.0f;
                _hasSpawnedEnemy = false;
            }
        }
    }

    private void SpawnEnemy()
    {
        float xMin = transform.position.x - _maxRadius;
        float xMax = transform.position.x + _maxRadius;
        float yMin = transform.position.y - _maxRadius;
        float yMax = transform.position.y + _maxRadius;
        float xVal = Random.Range( xMin, xMax );
        float yVal = Random.Range( yMin, yMax );

        _spawnedEnemies.Add(Instantiate(_prefab, new Vector3(xVal, yVal, 0), Quaternion.identity));
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _maxRadius);
    }
}
