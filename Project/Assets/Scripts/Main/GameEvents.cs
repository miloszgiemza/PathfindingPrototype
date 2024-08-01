using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using Base;

public static class GameEvents
{
    public static Action OnOpenMapEditionWIndow;
    public static Action<int, int, int, MapTilesType> OnCreateNewMap;
    public static Action<Vector2, int, TileType> OnUpdateMap;
    public static Action<PathfindingAlgorithmType> OnRunPathfinding;

    public static Action<TileMonoScript> OnTileSelected;
    public static Action<Vector3> OnHoverPositionChanged;
    public static Action OnResetTileSelection;
    public static Action<Vector3> OnClickedToSelectTile;
    
    public static Action<int, bool, bool> OnDisplayPathfindingSummary;
    public static Action<ErrorMessageType> OnShowErrorMessage;
    public static Action OnHideErrorMessage;
    
    public static Action OnHideFirstLoadingScreen;
    public static Action OnHideSecondLoadingScreen;
    public static Action OnHideThirdLoadingScreen;
}
