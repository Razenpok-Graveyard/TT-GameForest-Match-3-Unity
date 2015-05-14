using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    private static Text textField;
    private static int score;

    public static int Score
    {
        get { return score; }
        private set
        {
            score = value;
            textField.text = "Score: " + score;
        }
    }

    public static void Add(int value)
    {
        Score += value;
    }

    public static void Subtract(int value)
    {
        Score -= value;
    }

    private void Start()
    {
        textField = gameObject.GetComponent<Text>();
        if (!textField)
            Destroy(this);
        Score = 0;
    }
}