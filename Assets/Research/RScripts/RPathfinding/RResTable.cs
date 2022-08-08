using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Todo
    // - store the first conflict
    


public class RResTable {
    
    #region ResTable Variables

    // ResTable Variables
    private Dictionary<(int, int, int, int), List<RAgent>> _travellersList;
    private Dictionary<(int, int, int, int), RAgent> _resTable;
    
    #endregion

    #region Constructor

    // Constructor
    public RResTable() {

        _travellersList = new Dictionary<(int, int, int, int), List<RAgent>>();
        _resTable = new Dictionary<(int, int, int, int), RAgent>();
        
    }

    #endregion

    #region ResTable Functions

    // Add to the Travellers List
    public void AddTraveller((int, int, int, int) stpoint, RAgent agent) {
        
        try {
        
            _travellersList[stpoint].Add(agent);
        
        } catch (KeyNotFoundException) {
        
            _travellersList[stpoint] = new List<RAgent>();
            _travellersList[stpoint].Add(agent);
        
        }

    }

    // Check travellers list agent count at point
    public int TravellerCount((int, int, int, int) stpoint) {
        
        try {
        
            return _travellersList[stpoint].Count;
        
        } catch (KeyNotFoundException) {
        
            return 0;
        
        }
    
    }

    // Reserve path on resTable
    public void ReservePath(List<(int, int, int, int)> stpoints, RAgent agent) {
        
        foreach(var stpoint in stpoints) {
        
            _resTable[stpoint] = agent;
        
        }
    
    }

    // Check if reserved
    public bool STPointReserved((int, int, int, int) stpoint) {
        
        try {
        
            return _resTable[stpoint] != null;
        
        } catch (KeyNotFoundException) {
        
            return false;
        
        }
    
    }
    
    // Clear travellers list
    public void ClearTravellersList() {
        
        _travellersList = new Dictionary<(int, int, int, int), List<RAgent>>();
    
    }


    // Clear the reservation table
    public void ClearResTable() {
    
        _resTable = new Dictionary<(int, int, int, int), RAgent>();
    
    }

    #endregion

}
