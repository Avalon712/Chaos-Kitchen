using ChaosKitchen.Items;
using System.Collections.Generic;
using UnityEngine;

namespace ChaosKitchen
{
    public sealed class KitchenManager : MonoBehaviour, ITransfer<KitchenObject>
    {
        [Header("所有的物品")]
        [SerializeField] private List<KitchenObject> _prefabs;
        private List<KitchenObject> _pool;

        public static KitchenManager Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
            _pool = new();
        }

        public KitchenObject Get(KitchenObjectType type)
        {
            KitchenObject obj = _pool.Find(o => o.KitchenObjectType == type);
            if (obj == null) { obj = CloneKitchenObject(type); }
            else
            {
                obj.gameObject.SetActive(true);
                _pool.Remove(obj);
            }

            return obj;
        }

        /// <summary>
        /// 如果要销毁就不要放回来了
        /// </summary>
        public void Put(KitchenObject kitchenObject)
        {
            kitchenObject.transform.SetParent(transform);
            kitchenObject.gameObject.SetActive(false);
            _pool.Add(kitchenObject);
        }

        public Sprite GetIcon(KitchenObjectType kitchenObjectType)
        {
            return _prefabs.Find(p => p.KitchenObjectType == kitchenObjectType).Icon;
        }

        private KitchenObject CloneKitchenObject(KitchenObjectType type)
        {
            KitchenObject prefab = _prefabs.Find(obj => obj.KitchenObjectType == type);
            if (prefab != null)
            {
                KitchenObject clone = GameObject.Instantiate(prefab, transform);
                clone.transform.localPosition = Vector3.zero;
                return clone;
            }

            throw new System.Exception($"不存在指定类型[{type}]的物品!");
        }


        /// <summary>
        /// 总是返回false
        /// </summary>
        public bool TransferTo(ITransfer<KitchenObject> receiver)
        {
            return false;
        }

        /// <summary>
        /// 总是返回true
        /// </summary>
        public bool Place(KitchenObject kitchenObject)
        {
            Put(kitchenObject);
            return true;
        }

    }
}
