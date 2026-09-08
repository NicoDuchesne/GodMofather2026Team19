using UnityEngine;

namespace BitDuc.EnhancedTimeline.Network
{
    public class Estimator
    {
        public int Hash { get; }
        float current;
        float previous;
        double currentArrivalTime;
        double previousArrivalTime;
        double approximationTime;

        public Estimator(int hash, float initialValue, double now)
        {
            Hash = hash;

            current = initialValue;
            previous = initialValue;
            currentArrivalTime = now;
            previousArrivalTime = now - 0.1f;
        }

        public float Approximate(float deltaTime)
        {
            var velocity = (current - previous) / (currentArrivalTime - previousArrivalTime);
            approximationTime += deltaTime;

            return previous + (float)(velocity * approximationTime);
        }

        public void NextValue(float newValue, double arrivalTime)
        {
            if (arrivalTime - currentArrivalTime < Time.fixedDeltaTime)
            {
                current = newValue; 
                currentArrivalTime = arrivalTime;
            }
            else
            {
                previousArrivalTime = currentArrivalTime;
                currentArrivalTime = arrivalTime;

                previous = current;
                current = newValue;

                approximationTime = 0f;
            }
        }
    }
}
