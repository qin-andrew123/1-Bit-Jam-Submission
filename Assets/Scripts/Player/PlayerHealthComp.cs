using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealthComp : MonoBehaviour
{
    [SerializeField] LightAbilityComp _lightComp;
    [SerializeField] float ShrinkAngleWhenTouched = 20f;
    [SerializeField] float RadiusToDestroyEnemies = 5f;

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
        if (collision.collider != null && collision.collider.CompareTag("Enemy")) {
            Debug.Log(collision.collider.name);
            _lightComp.Clap(ShrinkAngleWhenTouched);
            if (RadiusToDestroyEnemies <= 0.0001f) {
                Destroy(collision.collider.gameObject);
            } else {
                DestroyNearByEnemies(RadiusToDestroyEnemies);
            }
        }
    }

    private void DestroyNearByEnemies(float radius) {
        Collider2D[] nearbyEnemies = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach (Collider2D enemy in nearbyEnemies) {
            if (enemy.CompareTag("Enemy")) {
                Destroy(enemy.gameObject);
            }
        }
    }
}
