using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Classic Heap
public class RHeap<T> where T : RIHeapItem<T> {
    
    #region Heap Variables

    // Heap Variables
    private T[] _items;
    private int _count;

    // Getters and Setters
    public int Count {
        get { return _count; }
        private set {}
    }

    #endregion

    #region Constructor

    // Constructor
    public RHeap(int maxSize) {
        _items = new T[maxSize];
    }

    #endregion

    #region Heap Functions

    // Add
    public void Add(T item) {
        item.HeapIndex = _count;
        _items[_count] = item;
        SortUp(item);
        _count++;
    }

    // Remove head
    public T RemoveFirst() {
        T firstItem = _items[0];
        _count--;
        _items[0] = _items[_count];
        _items[0].HeapIndex = 0;
        SortDown(_items[0]);
        return firstItem;
    }

    // Update items
    public void UpdateItem(T item) {
        SortUp(item);
    }

    // Contains 
    public bool Contains(T item) {
        return Equals(_items[item.HeapIndex], item);
    }

    // Sort down
    private void SortDown(T item) {
        while(true) {
            int childIndexLeft = item.HeapIndex * 2 + 1;
            int childIndexRight = item.HeapIndex * 2 + 2;
            int swapIndex = 0;
            if (childIndexLeft < _count) {
                swapIndex = childIndexLeft;
                if (childIndexRight < _count) {
                    if (_items[childIndexLeft].CompareTo(
                        _items[childIndexRight]) < 0) {
                        swapIndex = childIndexRight;
                    }
                }
                if (item.CompareTo(_items[swapIndex]) < 0) {
                    Swap(item, _items[swapIndex]);
                } else {
                    return;
                }
            } else {
                return;
            }
        }
    }

    // Sort up
    private void SortUp(T item) {
        int parentIndex = (item.HeapIndex - 1) / 2;
        while (true) {
            T parentItem = _items[parentIndex];
            if (item.CompareTo(parentItem) > 0) {
                Swap(item, parentItem);
            } else {
                break;
            }
            parentIndex = (item.HeapIndex - 1) / 2;
        }
    }

    // Swap elements
    private void Swap (T itemA, T itemB) {
        _items[itemA.HeapIndex] = itemB;
        _items[itemB.HeapIndex] = itemA;
        int itemAIndex = itemA.HeapIndex;
        itemA.HeapIndex = itemB.HeapIndex;
        itemB.HeapIndex = itemAIndex;
    }

    #endregion

}
