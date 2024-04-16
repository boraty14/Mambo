using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Binding
{
    internal class ServiceLocator : IServiceLocator
    {
        private readonly Dictionary<Type, object> _services = new();

        public void AddService<TService, TInstance>(TInstance instance)
            where TService : IService
            where TInstance : MonoBehaviour, TService
        {
            _services[typeof(TService)] = instance;
        }

        public void RemoveService<T>(T service) where T : IService
        {
            var key = typeof(T);
            if (!_services.ContainsKey(key))
            {
                return;
            }

            _services.Remove(key);
        }

        public T GetService<T>() where T : IService
        {
            var key = typeof(T);
            if (!_services.TryGetValue(key, out var service))
            {
                return default;
            }

            return (T)service;
        }
    }
}