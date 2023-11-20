using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static int EnemiesDestroyedByAttack;
    public static int EnemiesDestroyedByCollision;
    public static float TimeAlive;

    private void Awake() {
        if(Instance == null) {
            Instance = this;
        }
        else {
            Destroy(gameObject);
        }
    }

    private void Start() {
        EnemiesDestroyedByAttack = 0;
        EnemiesDestroyedByCollision = 0;
        TimeAlive = 0.0f;
    }

    private void FixedUpdate() {
        TimeAlive += Time.deltaTime;
    }
}
