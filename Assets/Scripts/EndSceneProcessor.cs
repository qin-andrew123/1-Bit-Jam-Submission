using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndSceneProcessor : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI enemiesDestroyedByAttackText;
    [SerializeField] private TextMeshProUGUI enemiesDestroyedByCollisionText;
    [SerializeField] private TextMeshProUGUI timeAliveText;

    private void Start() {
        if (enemiesDestroyedByAttackText != null) {
            enemiesDestroyedByAttackText.text = GameManager.EnemiesDestroyedByAttack.ToString();
        }
        if (enemiesDestroyedByCollisionText != null) {
            enemiesDestroyedByCollisionText.text = GameManager.EnemiesDestroyedByCollision.ToString();
        }
        if (timeAliveText != null) {
            var ts = TimeSpan.FromSeconds(GameManager.TimeAlive);
            timeAliveText.text = string.Format("{0:00}:{1:00}", ts.TotalMinutes, ts.Seconds);
        }
    }
    public void OnClickRestart() {
        SceneManager.LoadScene("GameScene");
    }

    public static void OnClickQuit() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}
