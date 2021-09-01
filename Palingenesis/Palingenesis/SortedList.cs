using System;
using System.Collections.Generic;
using System.Text;

namespace Palingenesis
{
    //sorted list class taken from sorted list PE
    class SortedList
    {
        // Implemented a single private field: a List of integers
        private List<int> listOfInts = new List<int>();

        // Count and IsEmpty properties
        public int Count
        {
            get
            {
                return listOfInts.Count;
            }
        }

        public bool IsEmpty
        {
            get
            {
                if (Count == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        // Methods galore:
        public void Add(int newData)
        {
            /* Create a local variable to track if the data has been added yet
             * Initialize this “added” flag to false
             * For every index, i, in the underlying List:
             *  If the newData to add is < the data at index i
             *      Insert the data at index i
             *      Set the added flag to true
             *      Break out of the loop
             * When the loop ends, if the data has not been added, add it to the end of the list
             */

            // lookig at every item in the list
            bool added = false;
            for (int i = 0; i < Count; i++)
            {
                if (newData < listOfInts[i])
                {
                    //putting the passed in integer into the spot in the list
                    listOfInts.Insert(i, newData);
                    added = true;
                    break;
                }
            }
            if (!added)
            {
                listOfInts.Add(newData);
            }
        }

        public bool Contains(int number)
        {
            bool isMatching = false;
            foreach (int item in listOfInts)
            {
                //seeing if the passed in int is the same as the int at that index
                if (number == item)
                {
                    isMatching = true;
                    break;
                }
                else
                {
                    isMatching = false;
                }
            }
            return isMatching;
        }

        public void Clear()
        {
            listOfInts.Clear();
        }

        public void Print()
        {
            //writing everything in the list
            foreach (int item in listOfInts)
            {
                Console.WriteLine(item);
            }
        }

        public int Min()
        {
            if (IsEmpty)
            {
                return int.MinValue;
            }
            else
            {
                return listOfInts[0];
            }
        }

        public int Max()
        {
            if (IsEmpty)
            {
                return int.MaxValue;
            }
            else
            {
                return listOfInts[Count - 1];
            }
        }
    }
}
