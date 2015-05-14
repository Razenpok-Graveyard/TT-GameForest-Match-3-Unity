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
    TileSelected
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
        gameState = GetGameState();
        switch (gameState)
        {
            case (GameState.TileMoving):
                return;
            case (GameState.None):
            {
                var matches = FindMatches();
                foreach (var match in matches)
                {
                    match.Remove();
                }
                break;
            }
        }
    }

    private GameState GetGameState()
    {
        var allTiles = Enumerable.Range(0, Height)
            .SelectMany(row => GetRow(row));
        if (allTiles.Any(tile => tile.IsMoving))
            return GameState.TileMoving;
        return GameState.None;
    }

    private IEnumerable<Tile> GetColumnMatches(int columnIndex)
    {
        var column = GetColumn(columnIndex).ToList();
        return GetLineMatches(column);
    }

    private IEnumerable<Tile> GetColumn(int columnIndex)
    {
        if (columnIndex < 0 || columnIndex > Width)
            return null;
        var column = Enumerable.Range(0, Height)
            .Select(y => tiles[columnIndex, y]);
        return column;
    }

    private IEnumerable<Tile> GetLineMatches(List<Tile> line)
    {
        var result = new List<Tile>();
        for (var i = 1; i < line.Count - 2; i++)
        {
            var thisName = line[i].name;
            var leftName = line[i - 1].gameObject.name;
            var rightName = line[i + 1].gameObject.name;
            if (thisName == leftName && thisName == rightName)
                result.AddRange(line.GetRange(i-1, 3));
        }
        return result.Distinct();
    }

    private IEnumerable<Tile> GetRowMatches(int rowIndex)
    {
        var row = GetRow(rowIndex).ToList();
        return GetLineMatches(row);
    }

    private IEnumerable<Tile> GetRow(int rowIndex)
    {
        if (rowIndex < 0 || rowIndex > Height)
            return null;
        var row = Enumerable.Range(0, Width)
            .Select(x => tiles[x, rowIndex]);
        return row;
    }

    private IEnumerable<Tile> FindMatches()
    {
        var columnMatches = Enumerable.Range(0, Width)
            .SelectMany(column => GetColumnMatches(column));
        var rowMatches = Enumerable.Range(0, Height)
            .SelectMany(row => GetRowMatches(row));
        return columnMatches.Concat(rowMatches).Distinct();
    }
}