using UnityEngine;
using System.Collections;

public class GameField : MonoBehaviour
{
	private const int Width = 8;
	private const int Height = 8;
	public GameObject BackgroundPrefab;
	public GameObject[] TilePrefabs;
	private readonly GameObject[,] backgroundTiles = new GameObject[Width, Height];
	private Tile[,] field = new Tile[Width,Height];

	// Use this for initialization
	void Start ()
	{
		SpawnBackgroundTiles();
	}

	private void SpawnBackgroundTiles()
	{
		var spriteRenderer = BackgroundPrefab.GetComponent<SpriteRenderer>();
		if (!spriteRenderer)
		{
			Debug.Log("There is no Sprite Renderer in Background");
			return;
		}
		var texture = spriteRenderer.sprite.texture;
		// Adjust to Unity units
		var textureWidth = texture.width / 100f;
		var textureHeight = texture.height / 100f;
		for (var x = 0; x < Width; x++)
			for (var y = 0; y < Height; y++)
			{
				var position = new Vector3(x * textureWidth, y * textureHeight);
				backgroundTiles[x,y] = (GameObject) Instantiate(BackgroundPrefab, position, Quaternion.identity);
			}
	}
	
	// Update is called once per frame
	void Update () {
	}
}
