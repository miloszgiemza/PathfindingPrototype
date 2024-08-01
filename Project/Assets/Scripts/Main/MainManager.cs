using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Base;
using System;

public class PositionInGrid
{
    public int X => x;
    public int Z => z;

    private int x;
    private int z;
    public PositionInGrid(int x, int z)
    {
        this.x = x;
        this.z = z;
    }
}

public enum Tags
{
    Tile
}

public class MainManager : MonoBehaviour
{
    public static MainManager Instance => instance;
    private static MainManager instance;

    public Map Map => maxMap;

    public PositionInGrid StartPos => startPos;
    public PositionInGrid EndPos => endPos;
    public Material TileSelectedMaterial => tileSelected;
    public Material TileDefaultMaterial => currentRegularTileMaterial;

    public Material StartTileMaterial => startTileMaterial;
    public Material EndTileMaterial => endTileMaterial;
    public Material ObstacleMaterial => obstacleMaterial;
    
    public int MinMapSize => minMapSzie;
    public int MaxMapSzie => maxMapSize;

    private Map maxMap;
    private BaseGraph graph;
    private BaseSearch search;

    [SerializeField] private Material startTileMaterial;
    [SerializeField] private Material endTileMaterial;
    [SerializeField] private Material pathTilesMaterial;
    [SerializeField] protected Material defaultSquareMaterial;
    [SerializeField] private Material defaultHexMaterial;
    [SerializeField] private Material tileSelected;
    [SerializeField] private Material obstacleMaterial;
    private Material currentRegularTileMaterial;
    protected GameObject tilePrefab;
    [SerializeField] private GameObject tilePrefabSquare;
    [SerializeField] private GameObject tilePrefabHex;
    [SerializeField] private GameObject tilePrefabHexInverted;
    [SerializeField] private GameObject obstaclePrefabSquares;
    [SerializeField] private GameObject obstaclePrefabHexes;
    [SerializeField] private GameObject obstaclePrefabHexesInverted;
    private GameObject obstaclePrefab;

    [SerializeField] private GameObject gameWorldCanvasPrefab;
    [SerializeField] private GameObject tileTextFieldPrefab;

    private List<Vector2> path;

    private ControllerMapEdition mapEditor;

    private PositionInGrid startPos = null;
    private PositionInGrid endPos = null;

    private float tileSize = 10f;

    private int minMapSzie = 1;
    private int maxMapSize = 100;

    #region Pooler
    [SerializeField] private GameObject poolSquaresParent;
    [SerializeField] private GameObject poolHexesParent;
    [SerializeField] private GameObject poolInvertedHexesParent;

    private TilesPooler poolerSquares;
    private TilesPooler poolerHexes;
    private TilesPooler poolerInvertedHexes;

    private TilesPooler activePooler;
    #endregion

    private void Awake()
    {
        if (!ReferenceEquals(MainManager.Instance, null))
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }

        mapEditor = GetComponent<ControllerMapEdition>();
        tilePrefab = tilePrefabSquare;
        obstaclePrefab = obstaclePrefabSquares;
        currentRegularTileMaterial = defaultSquareMaterial;
    }

    private void Start()
    {
        StartCoroutine(PreparePoolers());
    }

    private IEnumerator PreparePoolers()
    {
        bool firstStageFinished = false;
        bool secondStageFinished = false;
        bool thirdStageFinished = false; ;
        float hardDelay = 0.1f;

        maxMap = new Map(maxMapSize, maxMapSize, 0);

        yield return new WaitForSeconds(hardDelay);

        PoolerProgress.SetProgress(PoolerStage.GeneratingSquares);

        poolerSquares = new TilesPooler(ref firstStageFinished, maxMap.MapData, tilePrefabSquare, tileSize, poolSquaresParent.transform, obstaclePrefabSquares, startTileMaterial, pathTilesMaterial, gameWorldCanvasPrefab, tileTextFieldPrefab);
        yield return new WaitUntil(() =>firstStageFinished);
        yield return new WaitForSeconds(hardDelay);

        poolerSquares.MakeTilesStaticObjects();
        poolerSquares.HidePooler();


        PoolerProgress.SetProgress(PoolerStage.GeneratingHexes);

        poolerHexes = new TilesPooler(ref secondStageFinished, maxMap.MapData, tilePrefabHex, tileSize, poolHexesParent.transform, obstaclePrefabHexes, startTileMaterial, pathTilesMaterial, true, gameWorldCanvasPrefab, tileTextFieldPrefab);

        yield return new WaitUntil(() => secondStageFinished);
        yield return new WaitForSeconds(hardDelay);

        poolerHexes.MakeTilesStaticObjects();
        poolerHexes.HidePooler();

        PoolerProgress.SetProgress(PoolerStage.GeneratingInvertedHexes);

        poolerInvertedHexes = new TilesPooler(ref thirdStageFinished, maxMap.MapData, tilePrefabHexInverted, tileSize, poolInvertedHexesParent.transform, obstaclePrefabHexesInverted, startTileMaterial, pathTilesMaterial, false, gameWorldCanvasPrefab, tileTextFieldPrefab, true);

        yield return new WaitUntil(() => thirdStageFinished);
        yield return new WaitForSeconds(hardDelay);

        poolerInvertedHexes.MakeTilesStaticObjects();
        poolerInvertedHexes.HidePooler();

        PoolerProgress.SetProgress(PoolerStage.Finished);
    }

    private void OnEnable()
    {
        SubscribeEvents();
    }

    private void OnDisable()
    {
        UnsubscribeEvents();
    }

    private void ResetPathNodes()
    {
        foreach(Vector2 nodeCoordinates in path)
        {
            graph.Nodes[(int)nodeCoordinates.x, (int)nodeCoordinates.x].ResetNodeFromPathfindingData();
        }
    }

    private void GenerateNewMap(int mapXDimension, int mapZDimension, int obstaclesNumber, MapTilesType tilesType)
    {
        mapXDimension = Mathf.Clamp(mapXDimension, minMapSzie, maxMapSize);
        mapZDimension = Mathf.Clamp(mapZDimension, minMapSzie, maxMapSize);

        if (!ReferenceEquals(activePooler, null))
        {
            GameEvents.OnResetTileSelection.Invoke();

            if (!ReferenceEquals(path, null) && path.Count > 0)
            {
                activePooler.GameWorld.ClearPath(path, currentRegularTileMaterial);
                path = null;
            }

            activePooler.HidePooler();
        }

        startPos = null;
        endPos = null;

        maxMap.ReGenerateMapFromPool(mapXDimension, mapZDimension, obstaclesNumber);

        switch(tilesType)
        {
            case MapTilesType.Square:
                
                activePooler = poolerSquares;

                graph = new GraphSquares(maxMap.MapData, mapXDimension, mapZDimension);
                currentRegularTileMaterial = defaultSquareMaterial;
                tilePrefab = tilePrefabSquare;
                obstaclePrefab = obstaclePrefabSquares;
                activePooler.GenerateNewGameWorld(maxMap.MapData, mapXDimension, mapZDimension);
                break;

            case MapTilesType.Hex:
                
                activePooler = poolerHexes;

                graph = new GraphHexes(maxMap.MapData, mapXDimension, mapZDimension);
                currentRegularTileMaterial = defaultHexMaterial;
                tilePrefab = tilePrefabHex;
                obstaclePrefab = obstaclePrefabHexes;
                activePooler.GenerateNewGameWorld(maxMap.MapData, mapXDimension, mapZDimension);
                break;

            case MapTilesType.HexInverted:
                
                activePooler = poolerInvertedHexes;

                graph = new GraphInvertedHexes(maxMap.MapData, mapXDimension, mapZDimension);
                currentRegularTileMaterial = defaultHexMaterial;
                tilePrefab = tilePrefabHexInverted;
                obstaclePrefab = obstaclePrefabHexesInverted;
                activePooler.GenerateNewGameWorld(maxMap.MapData, mapXDimension, mapZDimension);
                break;
        }
    }

    public void RunPathfinding(PathfindingAlgorithmType pathfindingAlgorithm)
    {
        if (startPos == null || endPos == null) GameEvents.OnShowErrorMessage.Invoke(ErrorMessageType.ChooseStartAndEndOfPath);
        if(maxMap == null) GameEvents.OnShowErrorMessage.Invoke(ErrorMessageType.NoMapCreated);

        if (!ReferenceEquals(maxMap, null) && !ReferenceEquals(startPos, null) && !ReferenceEquals(endPos, null))
        {
            if (!ReferenceEquals(path, null) && path.Count > 0)
            {
                path.RemoveAt(0);
                path.RemoveAt(path.Count - 1);
                activePooler.GameWorld.ClearPath(path, currentRegularTileMaterial);
                path = null;
            }

            switch (pathfindingAlgorithm)
            {
                case PathfindingAlgorithmType.AStar:
                    search = new SearchAStar(graph, graph.Nodes[startPos.X, startPos.Z], graph.Nodes[endPos.X, endPos.Z]);
                    break;

                case PathfindingAlgorithmType.Dijkstra:
                    search = new SearchDijkstra(graph, graph.Nodes[startPos.X, startPos.Z], graph.Nodes[endPos.X, endPos.Z]);
                    break;
            }

            path = search.SearchAndReturnPath();
            search.DisplayPathfindingSummary(path);
            activePooler.GameWorld.PaintPath(new Vector2(startPos.X, startPos.Z), new Vector2(endPos.X, endPos.Z), path);

            foreach(Vector2 node in path)
            {
                Debug.Log(node);
            }

            graph.ResetGraphAfterPathfinding();
        }
    }

    private void UpdateMap(Vector2 tilePosition, int movementCostValue, TileType tileType)
    {
        if (!ReferenceEquals(path, null) && path.Count > 0)
        {
            path.RemoveAt(0);
            path.RemoveAt(path.Count - 1);
            activePooler.GameWorld.ClearPath(path, currentRegularTileMaterial);
            path = null;
        }

        switch (tileType)
        {
            case TileType.Regular:

                    if ( !ReferenceEquals(startPos, null) && tilePosition.x == startPos.X && tilePosition.y == startPos.Z)
                    {
                        startPos = null;
                    }
                    else if( !ReferenceEquals(endPos, null) && tilePosition.x == endPos.X && tilePosition.y == endPos.Z)
                    {
                        endPos = null;
                    }

                maxMap.UpdateMapData(tilePosition, true, movementCostValue);
                activePooler.GameWorld.UpdateTile(maxMap.MapData, tilePosition, tilePrefab, tileSize, transform, obstaclePrefab);
                graph.UpdateGraph(tilePosition, maxMap.MapData);

                break;

            case TileType.Obstacle:

                if ( !ReferenceEquals(startPos, null) && tilePosition.x == startPos.X && tilePosition.y == startPos.Z)
                {
                    startPos = null;
                }
                else if (!ReferenceEquals(endPos, null) && tilePosition.x == endPos.X && tilePosition.y == endPos.Z)
                {
                    endPos = null;
                }

                maxMap.UpdateMapData(tilePosition, false, movementCostValue);
                activePooler.GameWorld.UpdateTile(maxMap.MapData, tilePosition, tilePrefab, 10f, transform, obstaclePrefab);
                graph.UpdateGraph(tilePosition, maxMap.MapData);

                break;

            case TileType.Start:

                if(startPos!=null)
                {
                    activePooler.GameWorld.GameWorldTiles[startPos.X, startPos.Z].SetTileType(TileType.Regular);
                    activePooler.GameWorld.GameWorldTiles[startPos.X, startPos.Z].SetMaterial(currentRegularTileMaterial);
                    startPos = null;
                }

                startPos = new PositionInGrid((int)tilePosition.x, (int)tilePosition.y);

                maxMap.UpdateMapData(tilePosition, true, movementCostValue);
                activePooler.GameWorld.UpdateTile(maxMap.MapData, tilePosition, tilePrefab, 10f, transform, obstaclePrefab);
                graph.UpdateGraph(tilePosition, maxMap.MapData);
                activePooler.GameWorld.GameWorldTiles[(int) tilePosition.x, (int) tilePosition.y].SetMaterial(startTileMaterial);
                startPos = new PositionInGrid((int) tilePosition.x, (int) tilePosition.y);

                break;

            case TileType.End:

                if (!ReferenceEquals(endPos, null))
                {
                    activePooler.GameWorld.GameWorldTiles[endPos.X, endPos.Z].SetTileType(TileType.Regular);
                    activePooler.GameWorld.GameWorldTiles[endPos.X, endPos.Z].SetMaterial(currentRegularTileMaterial);
                    endPos = null;
                }

                endPos = new PositionInGrid((int)tilePosition.x, (int)tilePosition.y);

                maxMap.UpdateMapData(tilePosition, true, movementCostValue);
                activePooler.GameWorld.UpdateTile(maxMap.MapData, tilePosition, tilePrefab, 10f, transform, obstaclePrefab);
                graph.UpdateGraph(tilePosition, maxMap.MapData);
                activePooler.GameWorld.GameWorldTiles[(int) tilePosition.x, (int) tilePosition.y].SetMaterial(endTileMaterial);
                endPos = new PositionInGrid((int)tilePosition.x, (int)tilePosition.y);
                break;
        }
    }

    private void SubscribeEvents()
    {
        GameEvents.OnUpdateMap += UpdateMap;

        GameEvents.OnCreateNewMap += GenerateNewMap;
        GameEvents.OnRunPathfinding += RunPathfinding;
    }

    private void UnsubscribeEvents()
    {
        GameEvents.OnUpdateMap -= UpdateMap;

        GameEvents.OnCreateNewMap -= GenerateNewMap;
        GameEvents.OnRunPathfinding -= RunPathfinding;
    }
}
