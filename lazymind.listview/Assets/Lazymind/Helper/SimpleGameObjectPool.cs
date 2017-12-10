using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lazymind
{
    public class SimpleGameObjectPool
    {
        Dictionary<string, Queue<GameObject>> m_queueByPrefabName = new Dictionary<string, Queue<GameObject>>();

        public GameObject Get(GameObject _prefab)
        {
            var prefabName = _prefab.name;
            if (m_queueByPrefabName.ContainsKey(prefabName) == false)
                m_queueByPrefabName.Add(prefabName, new Queue<GameObject>());

            if (m_queueByPrefabName[prefabName].Count == 0)
            {
                var item = UnityEngine.Object.Instantiate(_prefab);
                item.name = prefabName;
                m_queueByPrefabName[prefabName].Enqueue(item);
            }

            var go = m_queueByPrefabName[prefabName].Dequeue();
            if (go.activeInHierarchy == false)
                go.SetActive(true);

            return go;
        }

        public void Release(GameObject _go)
        {
            _go.SetActive(false);

            var prefabName = _go.name;
            if (m_queueByPrefabName.ContainsKey(prefabName) == false)
            {
                m_queueByPrefabName.Add(prefabName, new Queue<GameObject>());
            }
            m_queueByPrefabName[prefabName].Enqueue(_go);
        }
    }
}