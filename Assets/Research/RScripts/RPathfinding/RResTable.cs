using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// RResTable
public class RResTable<T> {
    
    // Time, Node
    private Dictionary<T, bool> _resTable;

    public RResTable() {
        _resTable = new Dictionary<T, bool>();
    }

    public void Reserve(T node) {
        _resTable[node] = true;
    }

    public bool Reserved(T node) {
        try {
            return _resTable[node];
        } catch (KeyNotFoundException) {
            return false;
        }
    }

    public void Clear() {
        _resTable = new Dictionary<T, bool>();
    }
}
