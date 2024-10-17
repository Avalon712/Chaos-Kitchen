using ChaosKitchen.UI;
using System.Collections.Generic;
using UnityEngine;

namespace ChaosKitchen.Items
{
    public sealed class PlateContainer : MonoBehaviour, ITransfer<KitchenObject>
    {
        [SerializeField] private PlateUI _plateUI;
        [SerializeField] private List<KitchenObject> _foodTemplate;

        //当前玩家上的菜
        private List<KitchenObjectType> _foods = new(5);

        /// <summary>
        /// 添加食谱中指定的食物
        /// </summary>
        /// <returns>是否添加成功</returns>
        public void AddFood(KitchenObject food)
        {
            KitchenObject kitchenObject = _foodTemplate.Find(k => k.KitchenObjectType == food.KitchenObjectType);
            kitchenObject?.gameObject.SetActive(true);
            _plateUI.ShowIcon(food.Icon);
            _foods.Add(food.KitchenObjectType);
        }


        public List<KitchenObjectType> GetFoods() { return _foods; }

        public void ClearFoods()
        {
            _plateUI.Close();
            for (int i = 0; i < _foodTemplate.Count; i++)
            {
                _foodTemplate[i].gameObject.SetActive(false);
            }
            _foods.Clear();
        }

        private void OnDisable()
        {
            ClearFoods();
        }

        public bool TransferTo(ITransfer<KitchenObject> receiver)
        {
            return false;
        }

        public bool Place(KitchenObject transferObj)
        {
            if (transferObj.KitchenObjectType != KitchenObjectType.Plate)
            {
                KitchenManager.Instance.Put(transferObj);
                AddFood(transferObj);
                return true;
            }
            return false;
        }
    }
}
