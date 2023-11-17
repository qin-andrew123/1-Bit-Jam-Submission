using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealthComp : MonoBehaviour
{
    [SerializeField] LightAbilityComp _lightComp;

    void Update()
    {
        if ( _lightComp != null  && _lightComp.GetOuterAngle() < 20.0f)
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
