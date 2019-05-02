using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AID
{
    //currently can only be firing once simultaneously
    public class ResponseChain : MonoBehaviour
    {

        public List<ABResponse> responses = new List<ABResponse>();

        private float counter = 0;
        private bool isRunning = false;
        private int responseIndex = 0;
        private SensorResponseRouter lastFiringRouter;

        public void Fire(SensorResponseRouter r)
        {
            counter = 0;
            isRunning = true;
            responseIndex = 0;
            lastFiringRouter = r;
        }

        void FixedUpdate()
        {
            if (!isRunning)
                return;

            float prevCounter = counter;
            counter += Time.deltaTime;

            //while there are still things to execute and we have time left
            while (responseIndex < responses.Count)
            {
                if (counter < responses[responseIndex].delay)
                    break;

                //if counter was less than delay and is now great than it, 
                if (prevCounter <= responses[responseIndex].delay)
                {
                    //fire it
                    responses[responseIndex].Fire(lastFiringRouter);
                }

                if (counter >= responses[responseIndex].delay + responses[responseIndex].hold)
                {
                    //if counter is greater than hold
                    counter -= responses[responseIndex].delay + responses[responseIndex].hold;
                    responseIndex++;
                    continue;
                }

                //in a hold
                break;
            }

            if (responseIndex >= responses.Count)
                isRunning = false;
        }
    }
}