using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class PlayerHealthComp : MonoBehaviour
{
    [SerializeField] LightAbilityComp _lightComp;
    [SerializeField] GameObject globalLight;
    [SerializeField] float ShrinkAngleWhenTouched = 20f;
    [SerializeField] float RadiusToDestroyEnemies = 5f;
    [SerializeField] float _invulnerabilityTime = 1f;
    [SerializeField] float displayTime = 1f;
    private float _lastTimeHit = 0f;
    [SerializeField] CinemachineImpulseSource _impulseSource;

    private void Start() {
        Physics2D.queriesStartInColliders = true;
        globalLight.SetActive(false);
    }

    void Update()
    {
        if ( _lightComp != null  && _lightComp.GetOuterAngle() <= 0f)
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        // this is just the end scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (Time.time - _lastTimeHit < _invulnerabilityTime) {
            return;
        }

        if (collision.collider != null && collision.collider.CompareTag("Enemy")) {
            Debug.Log(collision.collider.name);
            if (_impulseSource) {
                Vector3 velocity = new Vector3(Random.Range(-1, 1f), Random.Range(-1, 1f), 0f);
                _impulseSource.GenerateImpulse(velocity);
            }
            StartCoroutine(DisplayGlobal(displayTime));
            _lightComp.Clap(ShrinkAngleWhenTouched);
            _lightComp.gameObject.GetComponent<Light2D>().pointLightOuterAngle = _lightComp.GetOuterAngle();
            if (RadiusToDestroyEnemies <= 0.0001f) {
                GameManager.EnemiesDestroyedByCollision++;
                Debug.Log("EnemiesDestroyedByCollision: " + GameManager.EnemiesDestroyedByCollision);
                Destroy(collision.collider.gameObject);
            } else {
                DestroyNearByEnemies(RadiusToDestroyEnemies);
            }
            _lastTimeHit = Time.time;
        }
    }

    private void DestroyNearByEnemies(float radius) {
        Collider2D[] nearbyEnemies = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach (Collider2D enemy in nearbyEnemies) {
            if (enemy.CompareTag("Enemy")) {
                GameManager.EnemiesDestroyedByCollision++;
                Debug.Log("EnemiesDestroyedByCollision: " + GameManager.EnemiesDestroyedByCollision);
                Destroy(enemy.gameObject);
            }
        }
    }

    IEnumerator DisplayGlobal(float time = 1f) {
        if (globalLight != null) {
            globalLight.SetActive(true);
            yield return new WaitForSeconds(time);
            globalLight.SetActive(false);
        }
    }
}
