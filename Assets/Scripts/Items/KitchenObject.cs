using UnityEngine;

namespace ChaosKitchen.Items
{
    public sealed class KitchenObject : MonoBehaviour
    {
        [SerializeField] private KitchenObjectType _type;
        [SerializeField] private Sprite _sprite;
        [SerializeField] private string _objName;

        public KitchenObjectType KitchenObjectType => _type;

        public Sprite Icon => _sprite;

        public string Name => _objName;

        private void OnDestroy()
        {
            _sprite = null;
        }
    }
}
