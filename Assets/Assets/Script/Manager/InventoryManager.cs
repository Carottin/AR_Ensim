using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{
    /// <summary>
    /// List of unit for the player
    /// </summary>
    public List<UnitModel> PlayerInventory;
}
