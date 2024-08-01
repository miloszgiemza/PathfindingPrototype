using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Base;
using System;

public class GameWorld
{
    public Tile[,] GameWorldTiles => gameWorldTiles;

    protected Tile[,] gameWorldTiles;

    protected int xAxisLength;
    protected int yAxisLength;

    protected float tileSize = 1;

    protected Material startingAndEndTileMaterial;
    protected Material pathTilesMaterial;

    protected GameObject gameWorld;
    protected GameObject gameWorldCanvas;

    public GameWorld(ref bool finished, Map.Field[,] mapData, GameObject tilePrefab, float tileSizeValue, Transform tilesParent, GameObject obstaclePrefab, Material startingAndEndTileMaterialValue, Material pathTilesMaterialValue, GameObject gameWorldCanvasPrefab, GameObject tileTextFieldPrefab)
    {
        tileSize = tileSizeValue;

        startingAndEndTileMaterial = startingAndEndTileMaterialValue;
        pathTilesMaterial = pathTilesMaterialValue;

        CreateGameWorld(ref finished, mapData, tilePrefab, tilesParent, obstaclePrefab, gameWorldCanvasPrefab, tileTextFieldPrefab);
    }

    public GameWorld(ref bool finished,  Map.Field[,] mapData, GameObject tilePrefab, float tileSizeValue, Transform tilesParent, GameObject obstaclePrefab, Material startingAndEndTileMaterialValue, Material pathTilesMaterialValue, bool displacedRows, GameObject gameWorldCanvasPrefab, GameObject tileTextFieldPrefab)
    {
        tileSize = tileSizeValue;

        startingAndEndTileMaterial = startingAndEndTileMaterialValue;
        pathTilesMaterial = pathTilesMaterialValue;

        CreateGameWorld(ref finished, mapData, tilePrefab, tilesParent, obstaclePrefab, displacedRows, gameWorldCanvasPrefab, tileTextFieldPrefab);
    }

    public GameWorld(ref bool finished, Map.Field[,] mapData, GameObject tilePrefab, float tileSizeValue, Transform tilesParent, GameObject obstaclePrefab, Material startingAndEndTileMaterialValue, Material pathTilesMaterialValue, bool displacedRows, GameObject gameWorldCanvasPrefab, GameObject tileTextFieldPrefab, bool displacedColumnsValue)
    {
        tileSize = tileSizeValue;

        startingAndEndTileMaterial = startingAndEndTileMaterialValue;         //myjka
        pathTilesMaterial = pathTilesMaterialValue;

        CreateGameWorld(ref finished, mapData, tilePrefab, tilesParent, obstaclePrefab, false, gameWorldCanvasPrefab, tileTextFieldPrefab, displacedColumnsValue);
    }

    protected void CreateGameWorld(ref bool finished, Map.Field[,] mapData, GameObject tilePrefab, Transform tilesParent, GameObject obstaclePrefab, GameObject gameWorldCanvasPrefab, GameObject tileTextFieldPrefab)
    {
        gameWorld = new GameObject();
        gameWorld.transform.parent = tilesParent;
        gameWorldCanvas = GameObject.Instantiate(gameWorldCanvasPrefab, gameWorld.transform);

        xAxisLength = mapData.GetLength(0);
        yAxisLength = mapData.GetLength(1);

        gameWorldTiles = new Tile[xAxisLength, yAxisLength];

        for (int x = 0; x < xAxisLength; x++)
        {
            for (int y = 0; y < yAxisLength; y++)
            {
                Tile newTile = new Tile(tilePrefab, new Vector2(x, y), tileSize, gameWorld.transform, obstaclePrefab, mapData[x,y].traversable, gameWorldCanvas, tileTextFieldPrefab, (int)mapData[x, y].cost);
                gameWorldTiles[x, y] = newTile;
            }
        }

        finished = true;
    }

    protected void CreateGameWorld(ref bool finished, Map.Field[,] mapData, GameObject tilePrefab, Transform tilesParent, GameObject obstaclePrefab, bool displacedRows, GameObject gameWorldCanvasPrefab, GameObject tileTextFieldPrefab)
    {
        gameWorld = new GameObject();
        gameWorld.transform.parent = tilesParent;
        gameWorldCanvas = GameObject.Instantiate(gameWorldCanvasPrefab, gameWorld.transform);

        xAxisLength = mapData.GetLength(0);
        yAxisLength = mapData.GetLength(1);

        gameWorldTiles = new Tile[xAxisLength, yAxisLength];


        for (int x = 0; x < xAxisLength; x++)
        {
            for (int y = 0; y < yAxisLength; y++)
            {
                bool displaced = false;
                if (y != 0 && y % 2 != 0) displaced = true;

                Tile newTile = new Tile(tilePrefab, new Vector2(x, y), tileSize, gameWorld.transform, obstaclePrefab, mapData[x, y].traversable, displaced, gameWorldCanvas, tileTextFieldPrefab, (int) mapData[x, y].cost);
                  
                gameWorldTiles[x, y] = newTile;
            }
        }

        finished = true;
    }

    protected void CreateGameWorld(ref bool finished, Map.Field[,] mapData, GameObject tilePrefab, Transform tilesParent, GameObject obstaclePrefab, bool displacedRows, GameObject gameWorldCanvasPrefab, GameObject tileTextFieldPrefab, bool displacedColumnsValue)
    {
        gameWorld = new GameObject();
        gameWorld.transform.parent = tilesParent;
        gameWorldCanvas = GameObject.Instantiate(gameWorldCanvasPrefab, gameWorld.transform);

        xAxisLength = mapData.GetLength(0);
        yAxisLength = mapData.GetLength(1);

        gameWorldTiles = new Tile[xAxisLength, yAxisLength];


        for (int x = 0; x < xAxisLength; x++)
        {
            bool displacedColumn = false;
            if (x != 0 && x % 2 != 0) displacedColumn = true;

            for (int y = 0; y < yAxisLength; y++)
            {
                Tile newTile = new Tile(tilePrefab, new Vector2(x, y), tileSize, gameWorld.transform, obstaclePrefab, mapData[x, y].traversable, displacedRows, gameWorldCanvas, tileTextFieldPrefab, (int)mapData[x, y].cost, displacedColumn);

                gameWorldTiles[x, y] = newTile;
            }
        }

        finished = true;
    }

    public void CreateGameWorldFromPooler(Map.Field[,] mapData, int mapWidth, int mapHeight)
    {

        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                gameWorldTiles[x, y].UpdateTile(mapData[x, y].traversable, (int) mapData[x, y].cost, true);
            }
        }
    }

    public void PaintPath(Vector2 startTile, Vector2 endTile, List<Vector2> path)
    {
        foreach (Vector2 tile in path)
        {
            gameWorldTiles[(int)tile.x, (int)tile.y].SetMaterial(pathTilesMaterial);
        }
        
        gameWorldTiles[(int)startTile.x, (int)startTile.y].SetMaterial(MainManager.Instance.StartTileMaterial);
        gameWorldTiles[(int)endTile.x, (int)endTile.y].SetMaterial(MainManager.Instance.EndTileMaterial);
    }

    public void ClearPath(List<Vector2> oldPath, Material defaultTileMaterial)
    {
        foreach (Vector2 pathCoordinte in oldPath)
        {
            gameWorldTiles[(int)pathCoordinte.x, (int)pathCoordinte.y].SetMaterial(defaultTileMaterial);
        }
    }

    public void UpdateTile(Map.Field[,] mapData, Vector2 tile, GameObject tilePrefab, float tileSize, Transform tileParent, GameObject obstaclePrefab)
    {
        gameWorldTiles[(int)tile.x, (int)tile.y].UpdateTile(mapData[(int)tile.x, (int)tile.y].traversable, (int) mapData[(int)tile.x, (int)tile.y].cost);
    }

    public void DestroyCurrentGameworld()
    {
        GameObject.Destroy(gameWorld);
    }

    public void HideGameWorld()
    {
        foreach(Tile tile in gameWorldTiles)
        {
            tile.HideTile();
        }
    }
}

