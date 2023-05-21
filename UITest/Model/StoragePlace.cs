using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UITest.Model
{
    class StoragePlace
    {
        public string Name { get; }
        public string Location { get; }

        public List<StoragePlaceProduct> StoragePlaceProducts { get; }

        public StoragePlace(string name, string location)
        {
            Name = name;
            Location = location;

            StoragePlaceProducts = new List<StoragePlaceProduct>();
        }

        public bool AddStoragePlaceProduct(StoragePlaceProduct product)
        {
            bool result = false;

            try
            {
                StoragePlaceProducts.Add(product);
                result = true;
            }
            catch
            {
                result = false;
            }

            return result;
        }

        public bool RemoveStoragePlaceProduct(StoragePlaceProduct product)
        {
            bool result = false;

            try
            {
                StoragePlaceProducts.Remove(product);
                result = true;
            }
            catch
            {
                result = false;
            }

            return result;
        }
    }
}
