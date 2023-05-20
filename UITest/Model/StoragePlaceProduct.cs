using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UITest.Model
{
    class StoragePlaceProduct
    {
        public Product Product { get; }
        public int Amount { get; private set; }

        public StoragePlaceProduct(Product product, int amount)
        {
            Product = product;
            Amount = amount;
        }

        public void IncreaseProductAmount(int amount)
        {
            Amount += amount;
        }

        public bool DecreaseProductAmount()
        {
            bool result = false;

            if(Amount > 0)
            {
                Amount -= Amount;
                result = true;
            }
            
            return result;
        }
    }
}
