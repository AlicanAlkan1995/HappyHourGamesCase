using System;
using System.Collections.Generic;

namespace HappyHourGames.Scripts.Services
{
    public class ServiceLocator : IServiceLocator
    {
        private Dictionary<Type, object> _services = new Dictionary<Type, object>();

        public T GetService<T>()
        {
            Type type = typeof(T);
            if (_services.TryGetValue(type, out var service))
            {
                return (T)service;
            }

            throw new Exception($"Service of type {type.FullName} not found.");
        }

        public void RegisterService<T>(T service)
        {
            Type type = typeof(T);
            if (!_services.ContainsKey(type))
            {
                _services[type] = service;
            }
            else
            {
                throw new Exception($"Service of type {type.FullName} is already registered.");
            }
        }
    }
}