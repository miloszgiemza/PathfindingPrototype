using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DropdownListMovementCost : BaseDropdownList
{
    public int MovementCost => movementCost;

    protected override List<string> Options => new List<string> { "0", "1", "2", "3", "4", "5" };

    private int movementCost = 0;

    protected override void HandleDropdown(TMP_Dropdown change)
    {
        switch (change.value)
        {
            case 0:
                movementCost = 0;
                break;
            case 1:
                movementCost = 1;
                break;
            case 2:
                movementCost = 2;
                break;
            case 3:
                movementCost = 3;
                break;
            case 4:
                movementCost = 4;
                break;
            case 5:
                movementCost = 5;
                break;
        }
    }
}
