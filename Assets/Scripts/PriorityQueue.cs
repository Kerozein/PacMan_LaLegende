using System;
using System.Collections.Generic;

public class PriorityQueue<T> where T : class

{
    private List<Tuple<T, int>> elements = new List<Tuple<T, int>>();
    public int count
    {
        get { return elements.Count; }
    }

    public void Enqueue(T item, int priority)
    {
        elements.Add(Tuple.Create(item, priority));
    }

    public T Dequeue()
    {
        int bestIndex = 0;
        for (int i = 0; i < elements.Count; i++)
        {
            if (elements[i].Item2 < elements[bestIndex].Item2)
                bestIndex = i;
        }

        T bestItem = elements[bestIndex].Item1;
        elements.RemoveAt(bestIndex);
        return bestItem;
    }

    public T Peek()
    {
        int bestIndex = 0;
        for (int i = 0; i < elements.Count; i++)
        {
            if (elements[i].Item2 < elements[bestIndex].Item2)
                bestIndex = i;
        }

        T bestItem = elements[bestIndex].Item1;
        return bestItem;
    }

    public bool Contains(T item)
    {
        foreach (Tuple<T, int> t in elements)
            if (!(t.Item1 == item)) return false;
        return true;
    }
}