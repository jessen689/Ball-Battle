using System.Collections.Generic;
using UnityEngine;

namespace BallBattle.Utility
{
	public class ObjectPooler<T> 
	{
		private List<T> activePool = new List<T>();
		private Queue<T> inactivePool = new Queue<T>();

		public T SummonObject(GameObject _prefabsToSpawn)
		{
			if (inactivePool.Count <= 0)
			{
				if (Object.Instantiate(_prefabsToSpawn).TryGetComponent(out T objectPool))
					activePool.Add(objectPool);
				else
					Debug.Log($"Cannot Get Component in prefab {_prefabsToSpawn.name}");
			}
			else
				activePool.Add(inactivePool.Dequeue());

			return activePool[activePool.Count - 1];
		}

		public void RemoveObject(T _target)
		{
			activePool.Remove(_target);
			inactivePool.Enqueue(_target);
		}

		public void RemoveAllObject()
		{
			if(activePool.Count > 0)
			{
				foreach(var obj in activePool)
				{
					inactivePool.Enqueue(obj);
				}
				activePool.Clear();
			}
		}

		public List<T> GetActivePool()
		{
			return new List<T>(activePool);
		}
	}
}
