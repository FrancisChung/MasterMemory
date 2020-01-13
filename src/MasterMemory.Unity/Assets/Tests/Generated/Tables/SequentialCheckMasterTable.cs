// <auto-generated />
using MasterMemory.Tests.TestStructures;
using MasterMemory.Validation;
using MasterMemory;
using MessagePack;
using System.Collections.Generic;
using System.Text;
using System;

namespace MasterMemory.Tests.Tables
{
   public sealed partial class SequentialCheckMasterTable : TableBase<SequentialCheckMaster>, ITableUniqueValidate
   {
        readonly Func<SequentialCheckMaster, int> primaryIndexSelector;


        public SequentialCheckMasterTable(SequentialCheckMaster[] sortedData)
            : base(sortedData)
        {
            this.primaryIndexSelector = x => x.Id;
        }


        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public SequentialCheckMaster FindById(int key)
        {
            var lo = 0;
            var hi = data.Length - 1;
            while (lo <= hi)
            {
                var mid = (int)(((uint)hi + (uint)lo) >> 1);
                var selected = data[mid].Id;
                var found = (selected < key) ? -1 : (selected > key) ? 1 : 0;
                if (found == 0) { return data[mid]; }
                if (found < 0) { lo = mid + 1; }
                else { hi = mid - 1; }
            }
            return ThrowKeyNotFound(key);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool TryFindById(int key, out SequentialCheckMaster result)
        {
            var lo = 0;
            var hi = data.Length - 1;
            while (lo <= hi)
            {
                var mid = (int)(((uint)hi + (uint)lo) >> 1);
                var selected = data[mid].Id;
                var found = (selected < key) ? -1 : (selected > key) ? 1 : 0;
                if (found == 0) { result = data[mid]; return true; }
                if (found < 0) { lo = mid + 1; }
                else { hi = mid - 1; }
            }
            result = default;
            return false;
        }

        public SequentialCheckMaster FindClosestById(int key, bool selectLower = true)
        {
            return FindUniqueClosestCore(data, primaryIndexSelector, System.Collections.Generic.Comparer<int>.Default, key, selectLower);
        }

        public RangeView<SequentialCheckMaster> FindRangeById(int min, int max, bool ascendant = true)
        {
            return FindUniqueRangeCore(data, primaryIndexSelector, System.Collections.Generic.Comparer<int>.Default, min, max, ascendant);
        }


        void ITableUniqueValidate.ValidateUnique(ValidateResult resultSet)
        {
            ValidateUniqueCore(data, primaryIndexSelector, "Id", resultSet);       
        }

        public static MasterMemory.Meta.MetaTable CreateMetaTable()
        {
            return new MasterMemory.Meta.MetaTable(typeof(SequentialCheckMaster), typeof(SequentialCheckMasterTable), "sequantial_master",
                new MasterMemory.Meta.MetaProperty[]
                {
                    new MasterMemory.Meta.MetaProperty(typeof(SequentialCheckMaster).GetProperty("Id")),
                    new MasterMemory.Meta.MetaProperty(typeof(SequentialCheckMaster).GetProperty("Cost")),
                },
                new MasterMemory.Meta.MetaIndex[]{
                    new MasterMemory.Meta.MetaIndex(new System.Reflection.PropertyInfo[] {
                        typeof(SequentialCheckMaster).GetProperty("Id"),
                    }, true, true, System.Collections.Generic.Comparer<int>.Default),
                });
        }
    }
}