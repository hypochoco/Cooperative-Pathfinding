using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// RInventory
public class RInventory {

    #region RInventory Variables

    // RInventory Variables
    private Dictionary<int, RItem> _inventory;

    #endregion

    #region Constructor

    // Constructor
    public RInventory() {
        _inventory = new Dictionary<int, RItem>();    
    }

    #endregion

    #region RInventory Functions

    // Find
    public RItem Find(int ID) {
        try {
            return _inventory[ID];
        } catch (KeyNotFoundException) {
            return null;
        }
    }

    // Add Item
    public virtual void Add(RItem item) {
        _inventory[item.ID] = item;
    }

    // Remove Item
    public virtual RItem Remove(int ID) {
        RItem item = this.Find(ID);
        if (item != null)
            _inventory.Remove(item.ID);
        return item;
    }

    #endregion

}
