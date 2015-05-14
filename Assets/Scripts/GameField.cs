using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Xml.Linq;
using UnityEngine;

public enum GameState
{
    None,
    TileMoving,
    TileSelected,
    HasEmptyTiles
}

public class GameField : MonoBehaviour
{
    private GameState gameState = GameState.None;
    private const int Width = 8;
    private const int Height = 8;
    private static float textureWidth;
    private static float textureHeight;
    private readonly Transform[,] backgroundTiles = new Transform[Width, Height];
    public GameObject BackgroundPrefab;
    private TileArray tiles = new TileArray(Width, Height);
    public GameObject[] TilePrefabs;
    private Tile selectedTile;

    public GameObject GameoverLabel;
    public GameObject GameoverButton;

    private void Start()
    {
        TimeManager.OnTimeUp = OnTimeUp;
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
                if (tiles[new Point(x, y)] == null)
                    SpawnTile(x);
            }
    }

    private void SpawnTile(int column)
    {
        var position = new Vector3(column*textureWidth, Height*textureHeight);
        var tilePrefab = TilePrefabs[Random.Range(0, TilePrefabs.Length)];
        var tile = (GameObject) Instantiate(tilePrefab, position, Quaternion.identity);
        for (var y = 0; y < Height; y++)
        {
            var currentTile = new Point(column, y);
            if (tiles[currentTile] != null) continue;
            var tileComponent = tile.GetComponent<Tile>();
            tiles[currentTile] = tileComponent;
            tileComponent.MoveToPoint(backgroundTiles[column, y].position);
            break;
        }
    }

    private void OnTimeUp()
    {
        Time.timeScale = 0f;
        GameoverLabel.SetActive(true);
        GameoverButton.SetActive(true);
    }

    // Update is called once per frame
    private void Update()
    {
        gameState = GetGameState();
        switch (gameState)
        {
            case (GameState.TileMoving):
                return;
            case GameState.HasEmptyTiles:
            {
                Collapse();
                FillField();
                break;
            }
            case (GameState.None):
            {
                var matches = tiles.FindMatches().ToList();
                if (matches.Any())
                {
                    foreach (var tile in matches)
                    {
                        tile.Remove();
                        ScoreManager.Add(1);
                    }
                    break;
                }
                if (Input.GetMouseButtonDown(0)) 
                {
                    if (selectedTile != null)
                        selectedTile.StopSpinning();
                    var hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                    if (hit.collider != null)
                    {
                        var position = GetPoint(hit.collider.transform);
                        selectedTile = tiles[position];
                        selectedTile.StartSpinning();
                        gameState = GameState.TileSelected;
                    }
                }
                break;
            }
        }
    }

    private Point GetPoint(Transform position)
    {
        for (var x = 0; x < Width; x++)
            for (var y = 0; y < Height; y++)
                if (backgroundTiles[x,y] == position)
                    return new Point(x, y);
        return new Point(-1, -1);
    }

    private void Collapse()
    {
        for (var x = 0; x < Width; x++)
            for (var y = 0; y < Height; y++)
            {
                var currentTile = new Point(x, y);
                if (tiles[currentTile] != null) continue;
                var onlyEmptyLeft = true;
                for (var i = y + 1; i < Height; i++)
                {
                    var nextTile = new Point(x, i);
                    if (tiles[nextTile] == null) continue;
                    tiles[currentTile] = tiles[nextTile];
                    tiles[nextTile].MoveToPoint(backgroundTiles[x, y].position);
                    tiles[nextTile] = null;
                    onlyEmptyLeft = false;
                    break;
                }
                if (onlyEmptyLeft)
                    break;
            }
    }

    private GameState GetGameState()
    {
        var allTiles = Enumerable.Range(0, Height)
            .SelectMany(row => tiles.GetRow(row)).ToList();
        if (allTiles.Where(tile => tile != null).Any(tile => tile.IsMoving))
            return GameState.TileMoving;
        if (allTiles.Any(tile => tile == null))
            return GameState.HasEmptyTiles;
        return GameState.None;
    }

    
}