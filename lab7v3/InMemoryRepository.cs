using System;
using System.Collections.Generic;

namespace lab7v3
{
    public class InMemoryRepository<T> : IRepository<T> where T : IEntity
    {
        private readonly List<T> _items = new List<T>();

        private int IndexOf(int id)
        {
            for (int i = 0; i < _items.Count; i++)
            {
                if (_items[i].Id == id)
                {
                    return i;
                }
            }
            return -1;
        }

        public T GetById(int id)
        {
            int index = IndexOf(id);
            if (index == -1)
            {
                throw new KeyNotFoundException($"елемент з Id = {id} не знайдено.");
            }
            return _items[index];
        }

        public IEnumerable<T> GetAll()
        {
            return new List<T>(_items);
        }

        public void Add(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }
            if (IndexOf(item.Id) != -1)
            {
                throw new ArgumentException($"елемент з Id = {item.Id} вже існує.");
            }
            _items.Add(item);
        }

        public void Update(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }
            int index = IndexOf(item.Id);
            if (index == -1)
            {
                throw new KeyNotFoundException($"елемент з Id = {item.Id} не знайдено.");
            }
            _items[index] = item;
        }

        public void Delete(int id)
        {
            int index = IndexOf(id);
            if (index == -1)
            {
                throw new KeyNotFoundException($"елемент з Id = {id} не знайдено.");
            }
            _items.RemoveAt(index);
        }
    }
}