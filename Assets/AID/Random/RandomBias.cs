using UnityEngine;
using System.Collections;
using System.Linq;

namespace AID
{
	//to get these to show up in the inspector easily you need to inherit thusly,
	//  unity doesn't automatically handle templated types in the inspector
	//[System.Serializable]
	//public class RandomBiasInt : RandomBias<int>{}
	
	/**
     * Random result from a selection of items each with their own probability. The system then
     * updates their probabilites behind the scenes to skew results back towards the expected 
     * probablity without needing very large number of data points.
     * 
     * Essientially the system actively tries to break low probability streaks and make the period
     * for what the user expects to happen to happen sooner.
     */
	[System.Serializable]
	public class RandomBias<T> 
	{
		public T[] items;
		public float[] startingProbs;
		
		private float curTotalProb;
		private float[] scratchProbs;
		
		private int curTotalResults;
		private int[] runningResults;

        public void Init()
        {
            Init(items, startingProbs);
        }
		
		public void Init(T[] theItems, float[] theProbs)
		{
			items = theItems;
			startingProbs = theProbs;
			
			Reset();
		}
		
		public void Reset()
		{
			scratchProbs = (float[])startingProbs.Clone();
			
			curTotalProb = 0;
			
			for(int i = 0 ; i < scratchProbs.Length; i++)
			{
				curTotalProb += scratchProbs[i];
			}
			
			//fill the running res with a semi-stable system
			curTotalResults = 0;
			runningResults = new int[items.Length];
			for(int i = 0; i < runningResults.Length; i++)
			{
				runningResults[i] = Mathf.Max(1, (int) (scratchProbs[i] * 100));
				curTotalResults += runningResults[i];
			}
		}
		
		public T Next()
		{
			//error early out
			if(items == null || items.Length == 0)
			{
				Debug.LogError("No items to random between");
				return default(T);
			}
			
			//what is the capacity of the random
			float curTotal = curTotalProb;
			
			//get a random val in range
			float r = Random.Range(0.0f, curTotal);
			
			int chosen = -1;
			
			//find out which item that actually relates to
			for(int i = 0; i < scratchProbs.Length; i++)
			{
				if(r < scratchProbs[i])
				{
					chosen = i;
					break;
				}
				else
				{
					r -= scratchProbs[i];
				}
			}
			
			//handle rare case of none found, means it's the end val
			if(chosen == -1)
			{
				chosen = items.Length-1;
			}
			
			//update the odds given this result
			UpdateProbability(chosen);
			
			return items[chosen];
		}
		
		private void UpdateProbability(int chosen)
		{
			runningResults[chosen]++;
			curTotalResults++;
			
			curTotalProb = 0;
			
			//calc new total probs for each element, compare to ideal and update scratch
			//  such that we push back towards the ideal
			for(int i = 0; i < runningResults.Length; i++)
			{
				//calc disparity
				float actualOdds = (float) (runningResults[i] / (double)curTotalResults);
				float oddDif = startingProbs[i] - actualOdds;
				
				//adjust to force back to ideal
				scratchProbs[i] = startingProbs[i] + oddDif;
				
				curTotalProb += scratchProbs[i];
			}
		}
	}
}