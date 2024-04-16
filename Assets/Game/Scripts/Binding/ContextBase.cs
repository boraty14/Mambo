using UnityEngine;

namespace Game.Scripts.Binding
{
    [DefaultExecutionOrder(-100)]
    public abstract class ContextBase : MonoBehaviour
    {
        private IServiceLocator _serviceLocator;

        protected abstract void InstallBindings();

        private void Awake()
        {
            _serviceLocator =  CreateServiceLocator();
            InstallBindings();
        }

        protected virtual IServiceLocator CreateServiceLocator()
        {
            return new ServiceLocator();
        }

        protected void BindPrefab<TService, TPrefab>(TPrefab prefab)
            where TService : IService
            where TPrefab : MonoBehaviour, TService
        {
            var instance = Instantiate(prefab);
            instance.ServiceLocator = _serviceLocator;
            _serviceLocator.AddService<TService, TPrefab>(instance);
        }

        protected void BindInstance<TService, TInstance>(TInstance instance)
            where TService : IService
            where TInstance : MonoBehaviour, TService
        {
            instance.ServiceLocator = _serviceLocator;
            _serviceLocator.AddService<TService, TInstance>(instance);
        }
    }
}