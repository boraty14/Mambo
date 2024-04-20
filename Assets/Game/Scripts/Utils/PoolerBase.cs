using System;
using UnityEngine;
using UnityEngine.Pool;

namespace Game.Scripts.Utils
{
    public abstract class PoolerBase<T> : MonoBehaviour where T : MonoBehaviour 
    {
        private T _prefab;
        private ObjectPool<T> _pool;

        private ObjectPool<T> Pool {
            get {
                if (_pool == null) throw new InvalidOperationException("You need to call InitPool before using it.");
                return _pool;
            }
            set => _pool = value;
        }

        protected void InitPool(T prefab, int initial = 10, int max = 200, bool collectionChecks = true) {
            _prefab = prefab;
            Pool = new ObjectPool<T>(
                CreateSetup,
                GetSetup,
                ReleaseSetup,
                DestroySetup,
                collectionChecks,
                initial,
                max);
        }

        #region Overrides
        protected virtual T CreateSetup() => Instantiate(_prefab,transform);
        protected virtual void GetSetup(T obj) => obj.gameObject.SetActive(true);
        protected virtual void ReleaseSetup(T obj) => obj.gameObject.SetActive(false);
        protected virtual void DestroySetup(T obj) => Destroy(obj);
        #endregion

        #region Getters
        protected T Get() => Pool.Get();
        protected void Release(T obj) => Pool.Release(obj);
        #endregion
    }
}