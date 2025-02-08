using System.Collections.Generic;
using UnityEngine;

namespace BallBattle.Utility
{
	public class ObjectPooler<T>
	{
		private List<GameObject> activePool = new List<GameObject>();
		private Queue<GameObject> inactivePool = new Queue<GameObject>();
		private List<T> activeTPool = new List<T>();

		public GameObject SummonObject(GameObject _prefabsToSpawn)
		{
			GameObject newObj;

			if (inactivePool.Count > 0)
				newObj = inactivePool.Dequeue();
			else
				newObj = Object.Instantiate(_prefabsToSpawn);

			activePool.Add(newObj);
			newObj.SetActive(true);

			if (activePool[activePool.Count - 1].TryGetComponent(out T _TObj))
				activeTPool.Add(_TObj);
			else
				Debug.Log("Type not found in Summoned GameObject.");

			return newObj;
		}

		public void RemoveObject(GameObject _target)
		{
			_target.SetActive(false);
			activeTPool.RemoveAt(activePool.IndexOf(_target));
			activePool.Remove(_target);
			inactivePool.Enqueue(_target);
		}

		public void RemoveAllObject()
		{
			if(activePool.Count > 0)
			{
				foreach(var obj in activePool)
				{
					obj.SetActive(false);
					inactivePool.Enqueue(obj);
				}
				activeTPool.Clear();
				activePool.Clear();
			}
		}

		public (List<GameObject> _go,List<T> _class) GetActivePool()
		{
			return (activePool, activeTPool);
		}
	}
}
