
namespace ChaosKitchen
{
    public interface ITransfer<T>
    {
        /// <summary>
        /// 将当前持有的KitchenObject传递给另外一个可接受的对象
        /// </summary>
        /// <param name="receiver"></param>
        /// <returns>是否传递成功</returns>
        public bool TransferTo(ITransfer<T> receiver);

        /// <summary>
        /// 放置KitchenObject
        /// </summary>
        /// <param name="transferObj"></param>
        /// <returns>是否放置成功</returns>
        public bool Place(T transferObj);
    }
}
