﻿using ConsoleApp;
using MasterMemory.Annotations;
using MasterMemory;
using MessagePack;
using System.Collections.Generic;
using System.IO;
using System;

namespace ConsoleApp.Tables
{
   public sealed partial class PersonTable : TableBase<Person>
   {
        readonly Func<Person, int> primaryIndexSelector;

        readonly Person[] secondaryIndex0;
        readonly Func<Person, int> secondaryIndex0Selector;
        readonly Person[] secondaryIndex2;
        readonly Func<Person, (Gender Gender, int Age)> secondaryIndex2Selector;
        readonly Person[] secondaryIndex1;
        readonly Func<Person, Gender> secondaryIndex1Selector;

        public PersonTable(Person[] sortedData)
            : base(sortedData)
        {
            this.primaryIndexSelector = x => x.PersonId;
            this.secondaryIndex0Selector = x => x.Age;
            this.secondaryIndex0 = CloneAndSortBy(this.secondaryIndex0Selector, System.Collections.Generic.Comparer<int>.Default);
            this.secondaryIndex2Selector = x => (x.Gender, x.Age);
            this.secondaryIndex2 = CloneAndSortBy(this.secondaryIndex2Selector, System.Collections.Generic.Comparer<(Gender Gender, int Age)>.Default);
            this.secondaryIndex1Selector = x => x.Gender;
            this.secondaryIndex1 = CloneAndSortBy(this.secondaryIndex1Selector, System.Collections.Generic.Comparer<Gender>.Default);
        }

        public RangeView<Person> SortByAge => new RangeView<Person>(secondaryIndex0, 0, secondaryIndex0.Length, true);
        public RangeView<Person> SortByGenderAndAge => new RangeView<Person>(secondaryIndex2, 0, secondaryIndex2.Length, true);
        public RangeView<Person> SortByGender => new RangeView<Person>(secondaryIndex1, 0, secondaryIndex1.Length, true);

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public Person FindByPersonId(int key)
        {
            var lo = 0;
            var hi = data.Length - 1;
            while (lo <= hi)
            {
				var mid = (int)(((uint)hi + (uint)lo) >> 1);
                var selected = data[mid].PersonId;
                var found = (selected < key) ? -1 : (selected > key) ? 1 : 0;
                if (found == 0) { return data[mid]; }
                if (found < 0) { lo = mid + 1; }
                else { hi = mid - 1; }
            }
            return default;
        }

        public Person FindClosestByPersonId(int key, bool selectLower = true)
        {
            return FindUniqueClosestCore(data, primaryIndexSelector, System.Collections.Generic.Comparer<int>.Default, key, selectLower);
        }

        public RangeView<Person> FindRangeByPersonId(int min, int max, bool ascendant = true)
        {
            return FindUniqueRangeCore(data, primaryIndexSelector, System.Collections.Generic.Comparer<int>.Default, min, max, ascendant);
        }

        public RangeView<Person> FindByAge(int key)
        {
            return FindManyCore(secondaryIndex0, secondaryIndex0Selector, System.Collections.Generic.Comparer<int>.Default, key);
        }

        public RangeView<Person> FindClosestByAge(int key, bool selectLower = true)
        {
            return FindManyClosestCore(secondaryIndex0, secondaryIndex0Selector, System.Collections.Generic.Comparer<int>.Default, key, selectLower);
        }

        public RangeView<Person> FindRangeByAge(int min, int max, bool ascendant = true)
        {
            return FindManyRangeCore(secondaryIndex0, secondaryIndex0Selector, System.Collections.Generic.Comparer<int>.Default, min, max, ascendant);
        }

        public RangeView<Person> FindByGenderAndAge((Gender Gender, int Age) key)
        {
            return FindManyCore(secondaryIndex2, secondaryIndex2Selector, System.Collections.Generic.Comparer<(Gender Gender, int Age)>.Default, key);
        }

        public RangeView<Person> FindClosestByGenderAndAge((Gender Gender, int Age) key, bool selectLower = true)
        {
            return FindManyClosestCore(secondaryIndex2, secondaryIndex2Selector, System.Collections.Generic.Comparer<(Gender Gender, int Age)>.Default, key, selectLower);
        }

        public RangeView<Person> FindRangeByGenderAndAge((Gender Gender, int Age) min, (Gender Gender, int Age) max, bool ascendant = true)
        {
            return FindManyRangeCore(secondaryIndex2, secondaryIndex2Selector, System.Collections.Generic.Comparer<(Gender Gender, int Age)>.Default, min, max, ascendant);
        }

        public RangeView<Person> FindByGender(Gender key)
        {
            return FindManyCore(secondaryIndex1, secondaryIndex1Selector, System.Collections.Generic.Comparer<Gender>.Default, key);
        }

        public RangeView<Person> FindClosestByGender(Gender key, bool selectLower = true)
        {
            return FindManyClosestCore(secondaryIndex1, secondaryIndex1Selector, System.Collections.Generic.Comparer<Gender>.Default, key, selectLower);
        }

        public RangeView<Person> FindRangeByGender(Gender min, Gender max, bool ascendant = true)
        {
            return FindManyRangeCore(secondaryIndex1, secondaryIndex1Selector, System.Collections.Generic.Comparer<Gender>.Default, min, max, ascendant);
        }

    }
}