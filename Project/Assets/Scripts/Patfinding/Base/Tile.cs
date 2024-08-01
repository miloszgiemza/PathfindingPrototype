using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

namespace Base
{
    public enum TileType
    {
        Start,
        End,
        Regular,
        Obstacle
    }

    public enum MapTilesType
    {
        Square,
        Hex,
        HexInverted
    }

    public class Tile
    {
        public Vector2 TilePositionInGrid => tilePositionInGrid;
        public TileType TileType => tileType;

        protected GameObject tileGameObject;
        protected GameObject obstacle;
        protected Vector2 tilePositionInGrid;

        protected TileType tileType = TileType.Regular;

        protected Renderer tileRenderer;
        protected Renderer obstacleRenderer;

        protected TMPro.TextMeshProUGUI tileTextField;

        public Tile(GameObject tilePrefab, Vector2 tilePosition, float size, Transform tilesParent, GameObject obstaclePrefab, bool traversable, GameObject tilesCanvas, GameObject tileTextFieldPrefab, int tileMovementCost)
        {
            tileGameObject = GameObject.Instantiate(tilePrefab, CalculateGameWorldPosition(tilePosition, size), Quaternion.identity, tilesParent);
            TileMonoScript monoScript = tileGameObject.AddComponent(typeof(TileMonoScript)) as TileMonoScript;
            monoScript.Initialize(this);

            obstacle = GameObject.Instantiate(obstaclePrefab, CalculateGameWorldPosition(tilePosition, size), Quaternion.identity, tileGameObject.transform);
            obstacleRenderer = obstacle.GetComponentInChildren<Renderer>();
            if (traversable) obstacle.SetActive(false);

            tilePositionInGrid = tilePosition;

            GameObject textGameobject = GameObject.Instantiate(tileTextFieldPrefab, new Vector3(tileGameObject.transform.position.x, tilesCanvas.transform.position.y, tileGameObject.transform.position.z), tileTextFieldPrefab.transform.rotation, tilesCanvas.transform);
            tileTextField = textGameobject.GetComponent<TextMeshProUGUI>();
            tileTextField.text = tileMovementCost.ToString();

            tileRenderer = tileGameObject.GetComponentInChildren<Renderer>();
        }

        public Tile(GameObject tilePrefab, Vector2 tilePosition, float size, Transform tilesParent, GameObject obstaclePrefab, bool traversable, bool rowDisplaced, GameObject tilesCanvas, GameObject tileTextFieldPrefab, int tileMovementCost)
        {
            tileGameObject = GameObject.Instantiate(tilePrefab, CalculateGameWorldPosition(tilePosition, size, rowDisplaced), Quaternion.identity, tilesParent);
            TileMonoScript monoScript = tileGameObject.AddComponent(typeof(TileMonoScript)) as TileMonoScript;
            monoScript.Initialize(this);

            obstacle = GameObject.Instantiate(obstaclePrefab, CalculateGameWorldPosition(tilePosition, size, rowDisplaced), Quaternion.identity, tileGameObject.transform);
            obstacleRenderer = obstacle.GetComponentInChildren<Renderer>();
            if (traversable) obstacle.SetActive(false);

            tilePositionInGrid = tilePosition;

            GameObject textGameobject = GameObject.Instantiate(tileTextFieldPrefab, new Vector3(tileGameObject.transform.position.x, tilesCanvas.transform.position.y, tileGameObject.transform.position.z), tileTextFieldPrefab.transform.rotation, tilesCanvas.transform);
            tileTextField = textGameobject.GetComponent<TextMeshProUGUI>();
            tileTextField.text = tileMovementCost.ToString();

            tileRenderer = tileGameObject.GetComponentInChildren<Renderer>();
        }

        public Tile(GameObject tilePrefab, Vector2 tilePosition, float size, Transform tilesParent, GameObject obstaclePrefab, bool traversable, bool rowDisplaced, GameObject tilesCanvas, GameObject tileTextFieldPrefab, int tileMovementCost, bool columnDisplacedValue)
        {
            tileGameObject = GameObject.Instantiate(tilePrefab, CalculateGameWorldPosition(tilePosition, size, rowDisplaced, columnDisplacedValue), tilePrefab.transform.rotation, tilesParent);
            TileMonoScript monoScript = tileGameObject.AddComponent(typeof(TileMonoScript)) as TileMonoScript;
            monoScript.Initialize(this);

            obstacle = GameObject.Instantiate(obstaclePrefab, CalculateGameWorldPosition(tilePosition, size, rowDisplaced, columnDisplacedValue), obstaclePrefab.transform.rotation, tileGameObject.transform);
            obstacleRenderer = obstacle.GetComponentInChildren<Renderer>();
            if (traversable) obstacle.SetActive(false);

            tilePositionInGrid = tilePosition;

            GameObject textGameobject = GameObject.Instantiate(tileTextFieldPrefab, new Vector3(tileGameObject.transform.position.x, tilesCanvas.transform.position.y, tileGameObject.transform.position.z), tileTextFieldPrefab.transform.rotation, tilesCanvas.transform);
            tileTextField = textGameobject.GetComponent<TextMeshProUGUI>();
            tileTextField.text = tileMovementCost.ToString();

            tileRenderer = tileGameObject.GetComponentInChildren<Renderer>();
        }

        public void UpdateTile(bool traversableValue, int movementCost)
        {
            if (!traversableValue) obstacle.SetActive(true);
            else obstacle.SetActive(false);
            tileTextField.text = movementCost.ToString();
        }

        public void UpdateTile(bool traversableValue, int movementCost, bool setActive)
        {
            tileGameObject.SetActive(true);
            if (!traversableValue) obstacle.SetActive(true);
            else obstacle.SetActive(false);
            tileTextField.gameObject.SetActive(true);
            tileTextField.text = movementCost.ToString();
        }

        protected virtual Vector3 CalculateGameWorldPosition(Vector2 tilePosition, float tileSize)
        {
            return new Vector3(tilePosition.x, 0, tilePosition.y) * tileSize;
        }

        protected virtual Vector3 CalculateGameWorldPosition(Vector2 tilePosition, float tileSize, bool rowDisplaced)
        {
            Vector3 position = new Vector3();
            if (rowDisplaced) position = new Vector3(tilePosition.x + tileSize / 20, 0, tilePosition.y) * tileSize;
            else position = new Vector3(tilePosition.x, 0, tilePosition.y) * tileSize;
            return position;
        }

        protected virtual Vector3 CalculateGameWorldPosition(Vector2 tilePosition, float tileSize, bool rowDisplaced, bool columnDisplacedValue)
        {
            Vector3 position = new Vector3();
            if (columnDisplacedValue) position = new Vector3(tilePosition.x, 0, tilePosition.y + tileSize / 20) * tileSize;
            else position = new Vector3(tilePosition.x, 0, tilePosition.y) * tileSize;
            return position;
        }

        public void Initialize(GameObject tilePrefab, Vector2 tilePosition, float size, Transform tilesParent, GameObject obstaclePrefab, bool traversable)
        {
            tileGameObject = GameObject.Instantiate(tilePrefab, CalculateGameWorldPosition(tilePosition, size), Quaternion.identity, tilesParent);
            obstacle = GameObject.Instantiate(obstaclePrefab, CalculateGameWorldPosition(tilePosition, size), Quaternion.identity, tileGameObject.transform);
            if (traversable) obstacle.SetActive(false);
        }

        public void ShowHideObstacle(bool show)
        {
            if (show) obstacle.SetActive(true);
            else obstacle.SetActive(false);
        }

        public void SetMaterial(Material newMaterial)
        {
            tileGameObject.GetComponentInChildren<Renderer>().material = newMaterial;
        }

        public void DestroyTileGameObject()
        {
            GameObject.Destroy(tileGameObject);
        }

        public Material ReturnCurrentTileMaterial()
        {
            return tileRenderer.material; ;
        }

        public Material ReturnCurrentObstacleMaterial()
        {
            return obstacleRenderer.material;
        }

        public void SetObstacleMaterial(Material materialValue)
        {
            if (obstacle.activeSelf) obstacleRenderer.material = materialValue;
        }

        public void HideTile()
        {
            obstacle.SetActive(false);
            tileTextField.gameObject.SetActive(false);
            tileGameObject.SetActive(false);
        }

        #region MapEditor
        public void SetTileType(TileType tileTypeValue)
        {
            tileType = tileTypeValue;
        }
        #endregion
    }
}
