using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.TimeSystem
{
    /// <summary>
    /// Простая array-based min-heap. Не зависит от .NET версии проекта
    /// </summary>
    public sealed class MinHeap<T> where T : IComparable<T>
    {
        private readonly List<T> _items = new List<T>();
        public int Count => _items.Count;


        public T Peek() => _items[0];


        public void Push( T item)
        {
            _items.Add(item);
            SiftUp(_items.Count - 1);
        }

        
        public T Pop()
        {
            var root = _items[0];
            int lastIndex = _items.Count - 1;
            _items[0] = _items[lastIndex];
            _items.RemoveAt(lastIndex);
            if (_items.Count > 0) SiftDown(0);
            return root;
        }
        
        private void SiftUp(int index)
        {
            while (index > 0)
            {
                int parent = (index - 1) / 2;
                if (_items[index].CompareTo(_items[parent]) >= 0) break;
                (_items[index], _items[parent]) = (_items[parent], _items[index]);
                index = parent;
            }
        }
        
        private void SiftDown(int index)
        {
            int count = _items.Count;
            while (true)
            {
                int left = index * 2 + 1;
                int right = index * 2 + 2;
                int smallest = index;


                if(left < count && _items[left].CompareTo(_items[smallest]) < 0) smallest = left;
                if(right < count && _items[right].CompareTo(_items[smallest]) < 0) smallest = right;
                if(smallest == index) break;
                
                (_items[index], _items[smallest]) = (_items[smallest], _items[index]);
                index = smallest;
            }
        }
    }
}