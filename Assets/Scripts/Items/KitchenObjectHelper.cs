
namespace ChaosKitchen.Items
{
    public static class KitchenObjectHelper
    {
        /// <summary>
        /// 是否能进行切割
        /// </summary>
        /// <returns>true:能进行切割</returns>
        public static bool CanCutting(KitchenObjectType kitchenObjectType)
        {
            switch (kitchenObjectType)
            {
                case KitchenObjectType.Cabbage: return true;
                case KitchenObjectType.CheeseBlock: return true;
                case KitchenObjectType.Tomato: return true;
            }

            return false;
        }

        /// <summary>
        /// 获取指定类型的物品需要进行切割的刀数
        /// </summary>
        public static int GetNeedCuttingCount(KitchenObjectType kitchenObjectType)
        {
            switch (kitchenObjectType)
            {
                case KitchenObjectType.Cabbage: return 4;
                case KitchenObjectType.CheeseBlock: return 6;
                case KitchenObjectType.Tomato: return 5;
            }
            return 1;
        }

        /// <summary>
        /// 获取指定类型的物品的切割后的物品
        /// </summary>
        public static KitchenObjectType GetCuttedKitchenObjectType(KitchenObjectType kitchenObjectType)
        {
            switch (kitchenObjectType)
            {
                case KitchenObjectType.Cabbage: return KitchenObjectType.CabbageSlice;
                case KitchenObjectType.CheeseBlock: return KitchenObjectType.CheeseSlice;
                case KitchenObjectType.Tomato: return KitchenObjectType.TomatoSlice;
            }
            return KitchenObjectType.None;
        }

        /// <summary>
        /// 判断一个物品是否能进行烹饪
        /// </summary>
        public static bool CanCook(KitchenObjectType kitchenObjectType)
        {
            switch (kitchenObjectType)
            {
                case KitchenObjectType.MeatPattyBurned: return true;
                case KitchenObjectType.MeatPattyCooked: return true;
                case KitchenObjectType.MeatPattyUncooked: return true;
            }
            return false;
        }

        /// <summary>
        /// 获取食物的烹饪状态
        /// </summary>
        public static CookState GetCookState(KitchenObjectType kitchenObjectType)
        {
            switch (kitchenObjectType)
            {
                case KitchenObjectType.MeatPattyBurned: return CookState.Burned;
                case KitchenObjectType.MeatPattyCooked: return CookState.Cooked;
                case KitchenObjectType.MeatPattyUncooked: return CookState.Uncooked;
            }

            return CookState.Uncooked;
        }

        /// <summary>
        /// 获取每种可以进行烹饪的食物的烹饪时间
        /// </summary>
        public static float GetCookTime(KitchenObjectType kitchenObjectType)
        {
            switch (kitchenObjectType)
            {
                case KitchenObjectType.MeatPattyCooked: return 2;
                case KitchenObjectType.MeatPattyUncooked: return 3;
            }
            return 0;
        }

        /// <summary>
        /// 获取当前物品的下一个烹饪状态的物品类型
        /// </summary>
        /// <param name="kitchenObjectType">当前物品类型</param>
        /// <returns>当前物品的下一个烹饪状态的物品类型</returns>
        public static KitchenObjectType GetNextCookStateKitchenObjectType(KitchenObjectType kitchenObjectType)
        {
            switch (kitchenObjectType)
            {
                case KitchenObjectType.MeatPattyCooked: return KitchenObjectType.MeatPattyBurned;
                case KitchenObjectType.MeatPattyUncooked: return KitchenObjectType.MeatPattyCooked;
            }

            return KitchenObjectType.None;
        }
    }
}
