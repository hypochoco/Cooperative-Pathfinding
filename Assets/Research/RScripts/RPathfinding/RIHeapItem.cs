using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// Extra property for RHeap
public interface RIHeapItem<T> : IComparable<T> {
    public int HeapIndex { get; set; }
}
