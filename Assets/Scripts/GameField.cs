using System.Xml.Linq;
using UnityEngine;

public class GameField : MonoBehaviour
{
    private const int Width = 8;
    private const int Height = 8;
    private static float textureWidth;
    private static float textureHeight;
    private readonly Transform[,] backgroundTiles = new Transform[Width, Height];
    public GameObject BackgroundPrefab;
    private Tile[,] tiles = new Tile[Width, Height];
    public GameObject[] TilePrefabs;

    private void Start()
    {
        var spriteRenderer = BackgroundPrefab.GetComponent<SpriteRenderer>();
        var texture = spriteRenderer.sprite.texture;
        // Adjust to Unity units
        textureWidth = texture.width/100f;
        textureHeight = texture.height/100f;
        SpawnBackgroundTiles();
        FillField();
    }

    private void SpawnBackgroundTiles()
    {
        for (var x = 0; x < Width; x++)
            for (var y = 0; y < Height; y++)
            {
                var position = new Vector3(x*textureWidth, y*textureHeight);
                var backgroundTile = (GameObject) Instantiate(BackgroundPrefab, position, Quaternion.identity);
                backgroundTile.transform.parent = transform;
                backgroundTiles[x, y] = backgroundTile.transform;
            }
    }

    private void FillField()
    {
        for (var x = 0; x < Width; x++)
            for (var y = 0; y < Height; y++)
            {
                SpawnTile(x);
            }
    }

    private void SpawnTile(int column)
    {
        var position = new Vector3(column*textureWidth, Height*textureHeight);
        var tilePrefab = TilePrefabs[Random.Range(0, TilePrefabs.Length - 1)];
        var tile = (GameObject) Instantiate(tilePrefab, position, Quaternion.identity);
        for (var y = 0; y < Height; y++)
        {
            if (tiles[column, y] != null) continue;
            var tileComponent = tile.GetComponent<Tile>();
            tiles[column, y] = tileComponent;
            tileComponent.MoveToPoint(backgroundTiles[column, y].position);
            break;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (!tiles[0, 0].IsMoving)
        {
            tiles[0,0].Kill();
        }
        Debug.Log(tiles[0,0]);
    }
}