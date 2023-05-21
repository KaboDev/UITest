using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UITest.Model
{
    class Storage
    {
        public string Name { get; }
        public string Location { get; }
        public float Size { get; }
        public bool IsFull { get; }

        public List<StoragePlace> StoragePlaces { get; }

        public Storage(string name, string location, float size)
        {
            Name = name;
            Location = location;
            Size = size;
        
            StoragePlaces = new List<StoragePlace>();
        }

        public bool AddStoragePlace(StoragePlace storagePlace)
        {
            bool result = false;

            try
            {
                StoragePlaces.Add(storagePlace);
                result = true;
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
    }
}
