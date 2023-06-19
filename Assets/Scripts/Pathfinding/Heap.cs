using System;

public class Heap<T> where T : IHeapItem<T>
{
    private T[] items; // Array to store heap items
    private int currentItemCount; // Current number of items in the heap

    public Heap(int maxHeapSize)
    {
        items = new T[maxHeapSize]; // Initialize the heap array with a maximum size
    }

    public void Add(T item)
    {
        item.HeapIndex = currentItemCount; // Set the heap index of the item
        items[currentItemCount] = item; // Add the item to the array
        SortUp(item); // Restore heap order by sorting up
        currentItemCount++; // Increase the count of items in the heap
    }

    public T RemoveFirst()
    {
        T firstItem = items[0]; // Get the first item in the heap
        currentItemCount--; // Decrease the count of items in the heap
        items[0] = items[currentItemCount]; // Move the last item to the root position
        items[0].HeapIndex = 0; // Update the heap index of the moved item
        SortDown(items[0]); // Restore heap order by sorting down
        return firstItem; // Return the removed item
    }

    public void UpdateItem(T item)
    {
        SortUp(item); // Restore heap order by sorting up
    }

    public int Count
    {
        get
        {
            return currentItemCount; // Return the count of items in the heap
        }
    }

    public bool Contains(T item)
    {
        return Equals(items[item.HeapIndex], item); // Check if the item exists in the heap
    }

    private void SortDown(T item)
    {
        while (true)
        {
            int childIndexLeft = item.HeapIndex * 2 + 1; // Get the index of the left child
            int childIndexRight = item.HeapIndex * 2 + 2; // Get the index of the right child
            int swapIndex = 0; // Index to swap with

            if (childIndexLeft < currentItemCount) // Check if the left child is within the heap bounds
            {
                swapIndex = childIndexLeft;

                if (childIndexRight < currentItemCount) // Check if the right child is within the heap bounds
                {
                    if (items[childIndexLeft].CompareTo(items[childIndexRight]) < 0)
                    {
                        swapIndex = childIndexRight; // Set the swap index to the right child if it has a higher priority
                    }
                }

                if (item.CompareTo(items[swapIndex]) < 0) // Compare the item with the child at the swap index
                {
                    Swap(item, items[swapIndex]); // Swap the item with the higher priority child
                }
                else
                {
                    return; // Exit the loop if the item is in the correct position
                }
            }
            else
            {
                return; // Exit the loop if the item has no children
            }
        }
    }

    private void SortUp(T item)
    {
        int parentIndex = (item.HeapIndex - 1) / 2; // Get the index of the parent

        while (true)
        {
            T parentItem = items[parentIndex]; // Get the parent item
            if (item.CompareTo(parentItem) > 0) // Compare the item with its parent
            {
                Swap(item, parentItem); // Swap the item with its parent if it has a higher priority
            }
            else
            {
                break; // Exit the loop if the item is in the correct position
            }

            parentIndex = (item.HeapIndex - 1) / 2; // Update the parent index
        }
    }

    private void Swap(T itemA, T itemB)
    {
        items[itemA.HeapIndex] = itemB; // Swap itemA with itemB in the array
        items[itemB.HeapIndex] = itemA; // Swap itemB with itemA in the array
        int itemAIndex = itemA.HeapIndex; // Update the heap index of itemA
        itemA.HeapIndex = itemB.HeapIndex; // Update the heap index of itemB
        itemB.HeapIndex = itemAIndex; // Update the heap index of itemA using the temporary index
    }
}

public interface IHeapItem<T> : IComparable<T>
{
    int HeapIndex
    {
        get;
        set;
    }
}