using Delivery.Common.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Delivery.Common.Services
{
    public class CrudServiceAsync<T> : ICrudServiceAsync<T> where T : class
    {
        private readonly ConcurrentDictionary<Guid, T> _data = new();
        private readonly SemaphoreSlim _semaphore = new(1, 1);
        private readonly string _filePath;

        public CrudServiceAsync(string filePath)
        {
            _filePath = filePath;
        }

        public async Task<bool> CreateAsync(T element)
        {
            await _semaphore.WaitAsync();
            try
            {
                var idProp = typeof(T).GetProperty("Id");
                if (idProp == null) return false;
                var id = (Guid)idProp.GetValue(element);
                return _data.TryAdd(id, element);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<T> ReadAsync(Guid id)
        {
            await _semaphore.WaitAsync();
            try
            {
                _data.TryGetValue(id, out T value);
                return value;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<IEnumerable<T>> ReadAllAsync()
        {
            await _semaphore.WaitAsync();
            try
            {
                return _data.Values.ToList();
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<IEnumerable<T>> ReadAllAsync(int page, int amount)
        {
            await _semaphore.WaitAsync();
            try
            {
                return _data.Values.Skip((page - 1) * amount).Take(amount).ToList();
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<bool> UpdateAsync(T element)
        {
            await _semaphore.WaitAsync();
            try
            {
                var idProp = typeof(T).GetProperty("Id");
                if (idProp == null) return false;
                var id = (Guid)idProp.GetValue(element);
                if (!_data.ContainsKey(id)) return false;
                _data[id] = element;
                return true;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<bool> RemoveAsync(T element)
        {
            await _semaphore.WaitAsync();
            try
            {
                var idProp = typeof(T).GetProperty("Id");
                if (idProp == null) return false;
                var id = (Guid)idProp.GetValue(element);
                return _data.TryRemove(id, out _);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<bool> SaveAsync()
        {
            await _semaphore.WaitAsync();
            try
            {
                var json = JsonSerializer.Serialize(_data.Values);
                await File.WriteAllTextAsync(_filePath, json);
                return true;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public IEnumerator<T> GetEnumerator() => _data.Values.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
