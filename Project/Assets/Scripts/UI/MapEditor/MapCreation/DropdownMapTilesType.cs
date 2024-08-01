using Base;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DropdownMapTilesType : BaseDropdownList
{
    protected override List<string> Options => new List<string> { MapTilesType.Square.ToString(), MapTilesType.Hex.ToString(), MapTilesType.HexInverted.ToString()};

    public MapTilesType TilesType => tilesType;

    private MapTilesType tilesType = MapTilesType.Square;

    protected override void HandleDropdown(TMP_Dropdown change)
    {
        switch (change.value)
        {
            case 0:
                tilesType = MapTilesType.Square;
                break;
            case 1:
                tilesType = MapTilesType.Hex;
                break;
            case 2:
                tilesType = MapTilesType.HexInverted;
                break;
        }
    }
}
