using UnityEngine;
using System.Collections;

public class OkButton : MonoBehaviour {

    public void ReturnToMenu()
    {
        Time.timeScale = 1f;
        Application.LoadLevel("Main menu");
    }
}
