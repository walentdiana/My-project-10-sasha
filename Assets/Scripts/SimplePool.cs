using System;
using System.Collections.Generic;
using GameName.Prejectile;
using UnityEngine;

namespace GameName.Pooling
{
    public class SimplePool : MonoBehaviour
    {
        public Projectile _prefab;
        public int _initialize = 10;
        
        private Queue<Projectile> _pool = new Queue<Projectile>();

        private void Awake()
        {
            for (int i = 0; i < _initialize; i++)
            {
                var obj = Instantiate(_prefab);
                obj.gameObject.SetActive(false);
                _pool.Enqueue(obj);
            }
        }

        public Projectile Get()
        {
            if (_pool.Count > 0)
            {
                var obj = _pool.Dequeue();
                obj.gameObject.SetActive(true);
                return obj;
            }
            
            var newObj = Instantiate(_prefab);
            return newObj;
        }

        public void Return(Projectile obj)
        {
            obj.gameObject.SetActive(false);
            _pool.Enqueue(obj);
        }
    }
}