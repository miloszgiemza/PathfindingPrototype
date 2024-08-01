using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

using Base;

public class DropdownTileType : BaseDropdownList
{
    protected override List<string> Options => new List<string> { TileType.Regular.ToString(), TileType.Obstacle.ToString(), TileType.Start.ToString(), TileType.End.ToString() };

    public TileType TileType => tileType;

    private TileType tileType = TileType.Regular;

    protected override void HandleDropdown(TMP_Dropdown change)
    {
        switch (change.value)
        {
            case 0:
                tileType = TileType.Regular;
                ControllerMapEdition.Instance.SwitchEditorState(EditorState.Tile);
                break;
            case 1:
                tileType = TileType.Obstacle;
                ControllerMapEdition.Instance.SwitchEditorState(EditorState.Obstacle);
                break;
            case 2:
                tileType = TileType.Start;
                ControllerMapEdition.Instance.SwitchEditorState(EditorState.Tile);
                break;
            case 3:
                tileType = TileType.End;
                ControllerMapEdition.Instance.SwitchEditorState(EditorState.Tile);
                break;
        }
    }
}
