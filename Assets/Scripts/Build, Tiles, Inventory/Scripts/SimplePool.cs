using System;                   // стандартная библиотека
using System.Collections.Generic; // нужен для Queue
using GameName.Prejectile;      // нужен для Projectile
using UnityEngine;              // Unity

namespace GameName.Pooling
{
    // Пул объектов — переиспользует снаряды вместо создания/уничтожения
    // Создание объектов дорого. Пул создаёт их заранее и "одалживает"
    // MonoBehaviour — вешается на пустой GameObject на сцене
    public class SimplePool : MonoBehaviour
    {
        public Projectile _prefab;   // префаб снаряда (назначается в инспекторе)
        public int _initialize = 10; // сколько снарядов создать заранее

        // Queue — очередь: первый пришёл → первый ушёл (FIFO)
        // Хранит снаряды которые сейчас не летят (выключены)
        private Queue<Projectile> _pool = new Queue<Projectile>();

        // Awake — создаём пул снарядов при старте
        private void Awake()
        {
            for (int i = 0; i < _initialize; i++)
            {
                var obj = Instantiate(_prefab);        // создаём снаряд
                obj.gameObject.SetActive(false);        // выключаем (прячем)
                _pool.Enqueue(obj);                     // кладём в очередь
            }
        }

        // Берёт снаряд из пула (или создаёт новый если пул пуст)
        public Projectile Get()
        {
            if (_pool.Count > 0) // есть снаряды в очереди?
            {
                var obj = _pool.Dequeue();      // берём первый из очереди
                obj.gameObject.SetActive(true); // включаем
                return obj;
            }

            // Пул пуст — создаём новый снаряд
            // Он не выключен и не добавлен в пул — будет добавлен когда вернётся
            var newObj = Instantiate(_prefab);
            return newObj;
        }

        // Возвращает снаряд в пул (когда он во что-то попал)
        public void Return(Projectile obj)
        {
            obj.gameObject.SetActive(false); // выключаем
            _pool.Enqueue(obj);              // кладём обратно в очередь
        }
    }
}