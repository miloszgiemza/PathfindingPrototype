using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Base;

public class Selector : MonoBehaviour
{
    public static Selector Instance => instance;
    private static Selector instance;

    public Material PreviousSelectedTileMaterial => previousSelectedTileMaterial;

    public Material PreviousSelectedObstacleMaterial => previousSelectedObstacleMaterial;

    [SerializeField] private Material hoveredOverMaterial;

    private Tile hoveredTile = null;
    private Material previousTileMaterial;
    private Material previousSelectedTileMaterial;

    private Material previousObstacleMaterial;
    private Material previousSelectedObstacleMaterial;

    private void Awake()
    {
        if(Selector.Instance==null)
        {
            instance = this;
        }
    }

    private void OnEnable()
    {
        SubscribeEvents();
    }

    private void OnDisable()
    {
        UnsubscribeEvents();
    }

    public TileMonoScript ReturnSelectedTile(GameObject selectedTileGameobject)
    {
        TileMonoScript selectedTile = selectedTileGameobject.GetComponentInParent<TileMonoScript>();

        return selectedTile;
    }

    private void SelectTile(Vector3 clickPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(clickPosition);
        Physics.Raycast(ray, out RaycastHit raycastHit);

        if (!ReferenceEquals(raycastHit.transform, null))
        {
            if (raycastHit.transform.gameObject.CompareTag(Tags.Tile.ToString()))
            {
                TileMonoScript selectedTile = ReturnSelectedTile(raycastHit.transform.gameObject);

                if (ControllerMapEdition.Instance.State == ControllerMapEdition.EditorWindowState.Closed || (ControllerMapEdition.Instance.State == ControllerMapEdition.EditorWindowState.Opened && (selectedTile.Tile.TilePositionInGrid.x != ControllerMapEdition.Instance.OpenedTile.Tile.TilePositionInGrid.x || selectedTile.Tile.TilePositionInGrid.y != ControllerMapEdition.Instance.OpenedTile.Tile.TilePositionInGrid.y)))
                {
                    if (ControllerMapEdition.Instance.State == ControllerMapEdition.EditorWindowState.Opened)
                    {
                        ControllerMapEdition.Instance.CloseOpenedTile();
                    }

                    previousSelectedTileMaterial = previousTileMaterial;
                    previousSelectedObstacleMaterial = previousObstacleMaterial;

                    previousTileMaterial = MainManager.Instance.TileSelectedMaterial;
                    previousObstacleMaterial = MainManager.Instance.TileSelectedMaterial;

                    selectedTile.Tile.SetMaterial(MainManager.Instance.TileSelectedMaterial);
                    selectedTile.Tile.SetObstacleMaterial(MainManager.Instance.TileSelectedMaterial);

                    GameEvents.OnTileSelected.Invoke(selectedTile);
                }
            }
        }
    }

    private void HighlightTile(Vector3 newHoverPos)
    {
        Ray ray = Camera.main.ScreenPointToRay(newHoverPos);
        Physics.Raycast(ray, out RaycastHit raycastHit);

        if (raycastHit.transform == null || !raycastHit.transform.CompareTag(Tags.Tile.ToString()) || InputManager.Instance.CheckIfOverUI())
        {
            if (!ReferenceEquals(hoveredTile, null))
            {
                hoveredTile.SetMaterial(previousTileMaterial);
                hoveredTile.SetObstacleMaterial(previousObstacleMaterial);

                hoveredTile = null;
            }
        }

        if (!ReferenceEquals(raycastHit.transform, null) && !InputManager.Instance.CheckIfOverUI())
        {
            if (raycastHit.transform.gameObject.CompareTag(Tags.Tile.ToString()))
            {
                Tile raycastedTile = raycastHit.transform.gameObject.GetComponentInParent<TileMonoScript>().Tile;

                if (hoveredTile == null)
                {
                    hoveredTile = raycastedTile;
                    previousTileMaterial = hoveredTile.ReturnCurrentTileMaterial();
                    previousObstacleMaterial = hoveredTile.ReturnCurrentObstacleMaterial();

                    hoveredTile.SetMaterial(hoveredOverMaterial);
                    hoveredTile.SetObstacleMaterial(hoveredOverMaterial);
                }
                else
                {
                    if (raycastedTile.TilePositionInGrid.x != hoveredTile.TilePositionInGrid.x || raycastedTile.TilePositionInGrid.y != hoveredTile.TilePositionInGrid.y)
                    {
                        hoveredTile.SetMaterial(previousTileMaterial);
                        hoveredTile.SetObstacleMaterial(previousObstacleMaterial);
                        hoveredTile = raycastedTile;
                        previousTileMaterial = hoveredTile.ReturnCurrentTileMaterial();
                        previousObstacleMaterial = hoveredTile.ReturnCurrentObstacleMaterial();
                        hoveredTile.SetMaterial(hoveredOverMaterial);
                        hoveredTile.SetObstacleMaterial(hoveredOverMaterial);
                    }
                }
            }
        }
    }

    private void SubscribeEvents()
    {
        GameEvents.OnHoverPositionChanged += HighlightTile;
        GameEvents.OnClickedToSelectTile += SelectTile;
    }

    private void UnsubscribeEvents()
    {
        GameEvents.OnHoverPositionChanged -= HighlightTile;
        GameEvents.OnClickedToSelectTile -= SelectTile;
    }
}
