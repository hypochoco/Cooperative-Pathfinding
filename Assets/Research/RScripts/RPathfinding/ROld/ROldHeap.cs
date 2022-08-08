using System;

// Classic Heap Data Structure
    // Goal - Improve Pathfinding runtime
    // Known Bugs - N/A
    // Todo - N/A
public class Heap<T> where T : IHeapItem<T> {

    // Variables
    T[] items;
    int currentItemCount;

    // Constructor
    public Heap(int maxHeapSize) {
        items = new T[maxHeapSize];
    }

    // Add
    public void Add(T item) {
        item.HeapIndex = currentItemCount;
        items[currentItemCount] = item;
        SortUp(item);
        currentItemCount++;
    }

    // Remove head
    public T RemoveFirst() {
        T firstItem = items[0];
        currentItemCount--;
        items[0] = items[currentItemCount];
        items[0].HeapIndex = 0;
        SortDown(items[0]);
        return firstItem;
    }

    // Update items
    public void UpdateItem(T item) {
        SortUp(item);
    }

    // Count
    public int Count {
        get {
            return currentItemCount;
        }
    }

    // Contains 
    public bool Contains(T item) {
        return Equals(items[item.HeapIndex], item);
    }

    // Sort down
    private void SortDown(T item) {
        while(true) {
            int childIndexLeft = item.HeapIndex * 2 + 1;
            int childIndexRight = item.HeapIndex * 2 + 2;
            int swapIndex = 0;

            if (childIndexLeft < currentItemCount) {
                swapIndex = childIndexLeft;

                if (childIndexRight < currentItemCount) {
                    if (items[childIndexLeft].CompareTo(
                        items[childIndexRight]) < 0) {
                        
                        swapIndex = childIndexRight;
                    }
                }

                if (item.CompareTo(items[swapIndex]) < 0) {
                    Swap(item, items[swapIndex]);
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
            T parentItem = items[parentIndex];

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
        items[itemA.HeapIndex] = itemB;
        items[itemB.HeapIndex] = itemA;
        int itemAIndex = itemA.HeapIndex;
        itemA.HeapIndex = itemB.HeapIndex;
        itemB.HeapIndex = itemAIndex;
    }
}

// Give HeapItems an extra property
public interface IHeapItem<T> : IComparable<T> {
    int HeapIndex {
        get;
        set;
    }
}
