using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PoolerStage
{
    GeneratingSquares,
    GeneratingHexes,
    GeneratingInvertedHexes,
    Finished
}

public static class PoolerProgress
{
    public static PoolerStage Progress => progress;

    private static PoolerStage progress = PoolerStage.GeneratingSquares;

    public static void SetProgress(PoolerStage progressValue)
    {
        progress = progressValue;
    }
}
