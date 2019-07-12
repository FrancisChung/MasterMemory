﻿using MasterMemory.Tests;
using MasterMemory;
using MessagePack;
using System.Collections.Generic;
using System;

namespace MasterMemory.Tests.Tables
{
   public sealed partial class SkillMasterTable : TableBase<SkillMaster>
   {
        readonly Func<SkillMaster, (int SkillId, int SkillLevel)> primaryIndexSelector;


        public SkillMasterTable(SkillMaster[] sortedData)
            : base(sortedData)
        {
            this.primaryIndexSelector = x => (x.SkillId, x.SkillLevel);
        }


        public SkillMaster FindBySkillIdAndSkillLevel((int SkillId, int SkillLevel) key)
        {
            return FindUniqueCore(data, primaryIndexSelector, System.Collections.Generic.Comparer<(int SkillId, int SkillLevel)>.Default, key);
        }

        public SkillMaster FindClosestBySkillIdAndSkillLevel((int SkillId, int SkillLevel) key, bool selectLower = true)
        {
            return FindUniqueClosestCore(data, primaryIndexSelector, System.Collections.Generic.Comparer<(int SkillId, int SkillLevel)>.Default, key, selectLower);
        }

        public RangeView<SkillMaster> FindRangeBySkillIdAndSkillLevel((int SkillId, int SkillLevel) min, (int SkillId, int SkillLevel) max, bool ascendant = true)
        {
            return FindUniqueRangeCore(data, primaryIndexSelector, System.Collections.Generic.Comparer<(int SkillId, int SkillLevel)>.Default, min, max, ascendant);
        }

    }
}