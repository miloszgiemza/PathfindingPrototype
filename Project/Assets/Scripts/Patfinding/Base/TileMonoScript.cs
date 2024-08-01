using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Base;

public class TileMonoScript : MonoBehaviour
{
    public Tile Tile => tile;

    protected Tile tile;

    public void Initialize(Tile tileValue)
    {
        tile = tileValue;
    }
}
