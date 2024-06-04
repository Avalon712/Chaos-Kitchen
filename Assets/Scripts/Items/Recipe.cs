using System.Collections.Generic;
using UnityEngine;

namespace ChaosKitchen.Items
{
    [CreateAssetMenu]
    public sealed class Recipe : ScriptableObject
    {
        public string menuName;
        public List<KitchenObjectType> recipe;
    }
}
