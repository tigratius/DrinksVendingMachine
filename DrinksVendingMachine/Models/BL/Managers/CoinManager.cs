/*
using DrinksVendingMachine.Models.Entities;

namespace DrinksVendingMachine.Models.BL.Managers
{
    public class CoinManager
    {
        public void ChangeCoinCount(CoinEntity coinEntity, int count)
        {
            coinEntity.Count = count;
        }

        public void AddCoin(CoinEntity coinEntity, CurrentStateEntity currentState)
        {
            coinEntity.Count++;
            currentState.Deposit += (int)coinEntity.Value;
        }

        public void Block(CoinEntity coinEntity)
        {
            coinEntity.IsBlocking = true;
        }

        public void UnBlock(CoinEntity coinEntity)
        {
            coinEntity.IsBlocking = false;
        }
    }

    
}
*/
