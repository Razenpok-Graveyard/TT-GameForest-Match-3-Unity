using UnityEngine;

public class OkButton : MonoBehaviour
{
    public void ReturnToMenu()
    {
        Time.timeScale = 1f;
        Application.LoadLevel("Main menu");
    }
}