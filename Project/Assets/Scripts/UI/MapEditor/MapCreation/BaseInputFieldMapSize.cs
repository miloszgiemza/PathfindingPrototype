using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BaseInputFieldMapSize : BaseInputFieldInteger
{
    protected override int MinValue => MainManager.Instance.MinMapSize;

    protected override int MaxValue => MainManager.Instance.MaxMapSzie;
}
