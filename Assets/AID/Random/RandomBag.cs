using UnityEngine;
using System.Collections;

namespace AID
{
    //to get these to show up in the inspector easily you need to inherit thusly,
    //  unity doesn't automatically handle templated types in the inspector
    //[System.Serializable]
    //public class RandomIntBag : RandomBag<int>{}

    /**
     * Random bag emulates drawing names out of a hat, when the hat is empty is it refilled
     * so if you fill it with 3 sets of d6 then in the total of 18 results before it refills you can 
     * only get 3 of each
     */
    [System.Serializable]
    public class RandomBag<T> 
    {
        public T[] items;
        public int sets = 0;
        private int curIndex = 0;
        private int[] bagOfIndicies;

        public void Init()
        {
            Init(items, sets);
        }

        public void Init(T[] theItems, int theSets)
        {
            items = theItems;
            sets = Mathf.Clamp(theSets, 1, int.MaxValue);
            curIndex = 0;
            bagOfIndicies = new int[sets * items.Length];

            FillBag();
            ShuffleBag();
        }

        public T Next()
        {
            if(items == null || items.Length == 0)
            {
                Debug.LogError("No Items in bag or not Init'ed");
                return default(T);
            }

            if(curIndex >= bagOfIndicies.Length)
            {
                curIndex = 0;
                ShuffleBag();
            }

            return items[ bagOfIndicies[curIndex++] ];
        }

        private void FillBag()
        {
            for(int i = 0; i < sets;i++ )
            {
                for(int j = 0; j < items.Length; j++)
                {
                    bagOfIndicies[i*items.Length + j] = j;
                }
            }
        }

        private void ShuffleBag()
        {
            AID.UTIL.ShuffleArray(bagOfIndicies);
        }
    }
}