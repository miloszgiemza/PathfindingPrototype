using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using Base;
using UnityEngine.UI;

public enum EditorState
{
    Tile,
    Obstacle
}

public class ControllerMapEdition : MonoBehaviour
{
    public static ControllerMapEdition Instance => instance;
    private static ControllerMapEdition instance;

    public EditorWindowState State => state;
    public TileMonoScript OpenedTile => openedTile;

    private Selector selector;

    private TileMonoScript openedTile;

    private EditorWindowState state = EditorWindowState.Closed;

    private InputFieldMovementCost inputFieldMovementCost;
    private DropdownTileType dropdownTileType;

    private Image window;

    public enum EditorWindowState
    {
        Opened,
        Closed
    }

    private void Awake()
    {
        if (!ReferenceEquals(ControllerMapEdition.Instance, null))
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    

        selector = GetComponent<Selector>();

        inputFieldMovementCost = GetComponentInChildren<InputFieldMovementCost>();
        dropdownTileType = GetComponentInChildren<DropdownTileType>();

        window = GetComponentInChildren<Image>();
    }

    private void OnEnable()
    {
        SubscribeEvents();
        window.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        UnsubscribeEvents();
    }

    public void SwitchEditorState(EditorState editorState)
    {
        switch(editorState)
        {
            case EditorState.Tile:
                inputFieldMovementCost.SetActive();
                break;
            case EditorState.Obstacle:
                inputFieldMovementCost.SetInactive();
                break;
        }
    }

    private void OpenTile(TileMonoScript tileValue)
    {
        state = EditorWindowState.Opened;
        openedTile = tileValue;
        GameEvents.OnOpenMapEditionWIndow.Invoke();
    }

    public void ApplyChangesToOpenTile()
    {
        if(openedTile!=null)
        {
            openedTile.Tile.SetMaterial(Selector.Instance.PreviousSelectedTileMaterial);
            openedTile.Tile.SetObstacleMaterial(Selector.Instance.PreviousSelectedObstacleMaterial);
            GameEvents.OnUpdateMap.Invoke(openedTile.Tile.TilePositionInGrid, inputFieldMovementCost.CurrentValue, dropdownTileType.TileType);
            state = EditorWindowState.Closed;
            window.gameObject.SetActive(false);
            openedTile = null;
        }
    }

    public void CloseOpenedTile()
    {
        openedTile.Tile.SetMaterial(Selector.Instance.PreviousSelectedTileMaterial);
        openedTile.Tile.SetObstacleMaterial(Selector.Instance.PreviousSelectedObstacleMaterial);
        openedTile = null;
        state = EditorWindowState.Closed;
        window.gameObject.SetActive(false);
    }

    private void OpenMapEditionWIndow()
    {
        window.gameObject.SetActive(true);
    }

    private void ResetTileSelection()
    {
        if(state == EditorWindowState.Opened)CloseOpenedTile();
    }

    private void SubscribeEvents()
    {
        GameEvents.OnTileSelected += OpenTile;
        GameEvents.OnOpenMapEditionWIndow += OpenMapEditionWIndow;
        GameEvents.OnResetTileSelection += ResetTileSelection;
    }

    private void UnsubscribeEvents()
    {
       GameEvents.OnTileSelected -= OpenTile;
        GameEvents.OnOpenMapEditionWIndow -= OpenMapEditionWIndow;
        GameEvents.OnResetTileSelection -= ResetTileSelection;
    }
}
