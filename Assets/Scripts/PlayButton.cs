using UnityEngine;
using System.Collections;

public class PlayButton : MonoBehaviour {

	public void PlayGame()
	{
		Application.LoadLevel("Game screen");
	}
}
