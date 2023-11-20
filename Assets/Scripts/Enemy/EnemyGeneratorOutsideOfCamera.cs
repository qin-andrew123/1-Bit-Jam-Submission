using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyGeneratorOutsideOfCamera : MonoBehaviour {
    [SerializeField] GameObject _enemyPrefab;
    [SerializeField] float _spawnInterval = 2f;
    [SerializeField] Camera _camera;
    private float _lastSpawnTime = 0f;
    private float _halfHeight, _halfWidth;
    private Vector3 _cameraPos, _cameraSize;
    private Vector3 _spawnPos;

    private void Start() {
        if (_camera == null) {
            _camera = Camera.main;
            _halfHeight = _camera.orthographicSize;
            _halfWidth = _camera.aspect * _halfHeight;
            _cameraSize = new Vector3(_halfWidth * 2, _halfHeight * 2, 0);
            _cameraPos = _camera.transform.position;
        }
    }

    //private void OnDrawGizmos() {
    //    Gizmos.color = Color.blue;
    //    Gizmos.DrawWireCube(_cameraPos, _cameraSize);
    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawCube(_spawnPos, new Vector3(0.5f, 0.5f, 0f));

    //}

    private void Update() {
        if (Time.time - _lastSpawnTime > _spawnInterval) {
            _halfHeight = _camera.orthographicSize;
            _halfWidth = _camera.aspect * _halfHeight;
            _cameraSize = new Vector3(_halfWidth * 2, _halfHeight * 2, 0);
            _cameraPos = _camera.transform.position;
            SpawnEnemy();
            _lastSpawnTime = Time.time;
        }
    }

    private void SpawnEnemy() {
        _spawnPos = new Vector3(Random.Range(_cameraPos.x - _halfWidth - 3, _cameraPos.x + _halfWidth + 3),
                                       Random.Range(_cameraPos.y - _halfHeight - 3, _cameraPos.y + _halfHeight + 3),
                                                                                        0);

        while (WithinCameraSpace(_spawnPos)) {
            float randomFloatBetween0And1 = Random.Range(0f, 1f);
            if (_spawnPos.x > _cameraPos.x + _halfWidth || _spawnPos.x < _cameraPos.x - _halfWidth) {
                if (randomFloatBetween0And1 < 0.5) {
                    _spawnPos.y = Random.Range(_cameraPos.y - _halfHeight - 3, _cameraPos.y - _halfHeight);
                } else {
                    _spawnPos.y = Random.Range(_cameraPos.y + _halfHeight, _cameraPos.y + _halfHeight + 3);
                }
            } else if (_spawnPos.y > _cameraPos.y + _halfHeight || _spawnPos.y < _cameraPos.y - _halfHeight) {
                if (randomFloatBetween0And1 < 0.5) {
                    _spawnPos.x = Random.Range(_cameraPos.x - _halfWidth - 3, _cameraPos.x - _halfWidth);
                } else {
                    _spawnPos.x = Random.Range(_cameraPos.x + _halfWidth, _cameraPos.x + _halfWidth + 3);
                }
            }
            _spawnPos = new Vector3(Random.Range(_cameraPos.x - _halfWidth - 3, _cameraPos.x + _halfWidth + 3),
                                   Random.Range(_cameraPos.y - _halfHeight - 3, _cameraPos.y + _halfHeight + 3),
                                                                                        0);
        }
        Instantiate(_enemyPrefab, _spawnPos, Quaternion.identity);
    }

    private bool WithinCameraSpace(Vector3 position) {
        if (position.x < _cameraPos.x + _halfWidth && position.x > _cameraPos.x - _halfWidth &&
            position.y < _cameraPos.y + _halfHeight && position.y > _cameraPos.y - _halfHeight)
            return true;
        return false;
    }
}
