using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UITest.Core;

namespace UITest.Model
{
    class StoragePlace : ObservableObject
    {
        public string Name { get; }
        public string Location { get; }

        private float _weight;
        public float Weight
        {
            get { return _weight; }
            private set
            {
                _weight = value;
                OnPropertyChanged();
            }
        }

        private float _size;
        public float Size
        {
            get { return _size; }
            private set
            {
                _size = value;
                OnPropertyChanged();
            }
        }

        public float MaximumWeight { get; }
        public float MaximumSize { get; }

        private int _spaceUsed;

        public int SpaceUsed
        {
            get { return _spaceUsed; }
            set
            {
                _spaceUsed = value;
                OnPropertyChanged();
            }
        }

        public string ImagePath { get; }

        public ObservableCollection<StoragePlaceProduct> StoragePlaceProducts { get; }

        public StoragePlace(string name, string location, float maxWeight, float maxSize, string imagePath = "")
        {
            Name = name;
            Location = location;
            MaximumWeight = maxWeight;
            MaximumSize = maxSize;
            ImagePath = imagePath;

            StoragePlaceProducts = new ObservableCollection<StoragePlaceProduct>();
        }

        public bool AddStoragePlaceProduct(StoragePlaceProduct product)
        {
            bool result = false;

            try
            {
                float totalSize = 0;
                float totalWeight = 0;

                for (int i = 0; i < product.Amount; i++)
                {
                    //Product size is in cm³, whereas the size of storagePlace is in m³ => so multiply by 0.01
                    totalSize += product.Product.Size * 0.01f;
                    totalWeight += product.Product.Weight;
                }

                if (Weight + totalWeight < MaximumWeight && Size + totalSize < MaximumSize)
                {
                    StoragePlaceProducts.Add(product);

                    Weight += totalWeight;
                    Size += totalSize;
                    CalculateUsedPercentage();

                    result = true;
                }
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

        private void CalculateUsedPercentage()
        {
            int percentage = (int)Math.Ceiling(Size / MaximumSize * 100);
            SpaceUsed = percentage > 100 ? 100 : percentage;
        }
    }
}
