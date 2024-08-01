using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Base;

public class TilesPooler
{
    public GameWorld GameWorld => gameWorld;

    private GameWorld gameWorld;

    private Transform poolParent;
    public TilesPooler(ref bool finished, Map.Field[,] mapData, GameObject tilePrefab, float tileSizeValue, Transform tilesParent, GameObject obstaclePrefab, Material startingAndEndTileMaterialValue, Material pathTilesMaterialValue, GameObject gameWorldCanvasPrefab, GameObject tileTextFieldPrefab)
    {
        gameWorld = new GameWorld(ref finished, mapData, tilePrefab, tileSizeValue, tilesParent, obstaclePrefab, startingAndEndTileMaterialValue, pathTilesMaterialValue, gameWorldCanvasPrefab, tileTextFieldPrefab);
        poolParent = tilesParent;
    }

    public TilesPooler(ref bool finished, Map.Field[,] mapData, GameObject tilePrefab, float tileSizeValue, Transform tilesParent, GameObject obstaclePrefab, Material startingAndEndTileMaterialValue, Material pathTilesMaterialValue, bool displacedRows, GameObject gameWorldCanvasPrefab, GameObject tileTextFieldPrefab)
    {
        gameWorld = new GameWorld(ref finished, mapData, tilePrefab, tileSizeValue, tilesParent, obstaclePrefab, startingAndEndTileMaterialValue, pathTilesMaterialValue, displacedRows, gameWorldCanvasPrefab, tileTextFieldPrefab);
        poolParent = tilesParent;
    }

    public TilesPooler(ref bool finished, Map.Field[,] mapData, GameObject tilePrefab, float tileSizeValue, Transform tilesParent, GameObject obstaclePrefab, Material startingAndEndTileMaterialValue, Material pathTilesMaterialValue, bool displacedRows, GameObject gameWorldCanvasPrefab, GameObject tileTextFieldPrefab, bool displacedColumnsValue)
    {
        gameWorld = new GameWorld(ref finished, mapData, tilePrefab, tileSizeValue, tilesParent, obstaclePrefab, startingAndEndTileMaterialValue, pathTilesMaterialValue, displacedRows, gameWorldCanvasPrefab, tileTextFieldPrefab, displacedColumnsValue);
        poolParent = tilesParent;
    }

    public void MakeTilesStaticObjects()
    {
        StaticBatchingUtility.Combine(poolParent.gameObject);
    }

    public void HidePooler()
    {
        gameWorld.HideGameWorld();
        poolParent.gameObject.SetActive(false);
    }

    public void GenerateNewGameWorld(Map.Field[,] mapData, int mapWidth, int mapHeight)
    {
        poolParent.gameObject.SetActive(true);
        gameWorld.CreateGameWorldFromPooler(mapData, mapWidth, mapHeight);
    }
}
