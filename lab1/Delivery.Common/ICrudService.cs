using System;
using System.Collections.Generic;
using System.Linq;

namespace Delivery.Common
{
    public interface ICrudService<T>
    {
        void Create(T element);
        T Read(Guid id);
        IEnumerable<T> ReadAll();
        void Update(T element);
        void Remove(T element);
    }

    public class CrudService<T> : ICrudService<T> where T : class
    {
        private readonly List<T> _items = new List<T>();

        public void Create(T element)
        {
            _items.Add(element);
        }

        public T Read(Guid id)
        {
            return _items.FirstOrDefault(e =>
            {
                var prop = e.GetType().GetProperty("Id");
                return prop != null && (Guid)prop.GetValue(e) == id;
            });
        }

        public IEnumerable<T> ReadAll()
        {
            return _items;
        }

        public void Update(T element)
        {
            var prop = element.GetType().GetProperty("Id");
            if (prop == null) return;

            var id = (Guid)prop.GetValue(element);
            var existing = Read(id);
            if (existing != null)
            {
                int index = _items.IndexOf(existing);
                _items[index] = element;
            }
        }

        public void Remove(T element)
        {
            _items.Remove(element);
        }
    }
}
