using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DropdownTraversable : BaseDropdownList
{
    protected override List<string> Options => new List<string> { "Traversable", "Obstacle" };

    public bool Traversable => traversable;

    private bool traversable = true;

    protected override void HandleDropdown(TMP_Dropdown change)
    {
        switch (change.value)
        {
            case 0:
                traversable = true;
                break;
            case 1:
                traversable = false;
                break;
        }
    }
}
