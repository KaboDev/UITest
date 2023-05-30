using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UITest.Core;

namespace UITest.Model
{
    class Storage : ObservableObject
    {
        public string Name { get; }
        public string Location { get; }

        private float _size;

        public float Size
        {
            get { return _size; }
            set
            {
                _size = value;
                OnPropertyChanged();
            }
        }

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

        public ObservableCollection<StoragePlace> StoragePlaces { get; }

        public Storage(string name, string location, float maxSize)
        {
            Name = name;
            Location = location;
            MaximumSize = maxSize;

            StoragePlaces = new ObservableCollection<StoragePlace>();
        }

        public bool AddStoragePlace(StoragePlace storagePlace)
        {
            bool result = false;

            try
            {
                float size = storagePlace.MaximumSize;

                if (Size + size < MaximumSize)
                {
                    StoragePlaces.Add(storagePlace);

                    Size += size;
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

        public bool RemoveStoragePlace(StoragePlace storagePlace)
        {
            bool result = false;

            try
            {
                StoragePlaces.Remove(storagePlace);
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
            int percentage = (int)(Size / MaximumSize * 100);
            SpaceUsed = percentage > 100 ? 100 : percentage;
        }
    }
}
