using System;
/// <summary>
/// A priority queue implemented with a binary heap.
/// </summary>
public class PriorityQueue<T> where T : IComparable<T>
{
    private T[] data;

    int size;

    /// <summary>
    /// Constructs a priority queue.
    /// </summary>
    /// <param name="capacity">Initial capacity of priority queue.</param>
    public PriorityQueue(int capacity = 100)
    {
        data = new T[capacity];
        size = 0;
    }

    /// <summary>
    /// Returns the smallest element from the queue, without deleting it.
    /// </summary>
    public T Peek()
    {
        if (data.Length == 0)
            throw new InvalidOperationException("Can't peek: queue is empty!");
        return data[0];
    }

    /// <summary>
    /// Returns the smallest element from the queue, then deletes it from the queue.
    /// </summary>
    public T Pop()
    {
        if (data.Length == 0)
            throw new InvalidOperationException("Can't pop: queue is empty!");
        T element = data[0];
        data[0] = data[size - 1];
        size--;
        HeapifyDown();
        return element;
    }

    /// <summary>
    /// Adds an element to the priority queue.
    /// </summary>
    /// <param name="element">Element to be added.</param>
    public void Add(T element)
    {
        EnsureExtraCapacity();
        data[size] = element;
        size++;
        HeapifyUp();
    }

    private void HeapifyUp()
    {
        int index = size - 1;
        while(HasParent(index) && (Parent(index).CompareTo(data[index]) > 0))
        {
            Swap(GetParentIndex(index), index);
            index = GetParentIndex(index);
        }
    }

    private void HeapifyDown()
    {
        int index = 0;
        while(HasLeftChild(index))
        {
            int smallerChildIndex = GetLeftChildIndex(index);
            if (HasRightChild(index) && (RightChild(index).CompareTo(LeftChild(index)) < 0))
                smallerChildIndex = GetRightChildIndex(index);

            if (data[index].CompareTo(data[smallerChildIndex]) < 0)
                break;
            else
                Swap(index, smallerChildIndex);

            index = smallerChildIndex;
        }
    }

    private void EnsureExtraCapacity()
    {
        if(size == data.Length)
        {
            T[] newData = new T[data.Length * 2];
            Array.Copy(data, newData, data.Length);
            data = newData;
        }
    }

    private int GetLeftChildIndex(int parentIndex)
    {
        return 2 * parentIndex + 1;
    }

    private int GetRightChildIndex(int parentIndex)
    {
        return 2 * parentIndex + 2;
    }

    private int GetParentIndex(int childIndex)
    {
        return (childIndex - 1) / 2;
    }

    private bool HasLeftChild(int parentIndex)
    {
        return GetLeftChildIndex(parentIndex) < size;
    }

    private bool HasRightChild(int parentIndex)
    {
        return GetRightChildIndex(parentIndex) < size;
    }

    private bool HasParent(int childIndex)
    {
        return GetParentIndex(childIndex) >= 0;
    }

    private T LeftChild(int parentIndex)
    {
        return data[GetLeftChildIndex(parentIndex)];
    }

    private T RightChild(int parentIndex)
    {
        return data[GetRightChildIndex(parentIndex)];
    }

    private T Parent(int childIndex)
    {
        return data[GetParentIndex(childIndex)];
    }

    private void Swap(int firstIndex, int secondIndex)
    {
        T temp = data[firstIndex];
        data[firstIndex] = data[secondIndex];
        data[secondIndex] = temp;
    }
}