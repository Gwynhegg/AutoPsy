using AutoPsy.Database.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoPsy.Logic
{
    public static class PoolModelling
    {
        public static List<float> CreatePoolModel(List<float> baseState)
        {
            List<float> rightHandStepping = new List<float>();
            List<float> leftHandStepping = new List<float>();

            float leftCurrentState = baseState[0], rightCurrentState = baseState[baseState.Count - 1];
           
            for (int i = 0; i < baseState.Count; i++)
            {
                if (leftCurrentState < baseState[i])
                    leftCurrentState = baseState[i];
                if (leftCurrentState - 1 >= 1) leftHandStepping.Add(leftCurrentState--); else leftHandStepping.Add(leftCurrentState);
            }

            for (int i = baseState.Count - 1; i >= 0; i--)
            {
                if (rightCurrentState < leftHandStepping[i])
                    rightCurrentState = leftHandStepping[i];
                if (rightCurrentState - 1 >= 1) rightHandStepping.Insert(0, rightCurrentState--); else rightHandStepping.Insert(0, rightCurrentState);
            }

            var result = GetAverageValues(baseState, leftHandStepping, rightHandStepping);
            
            return result;
        }

        private static List<float> GetAverageValues(List<float> baseState, List<float> leftState, List<float> rightState)
        {
            List<float> result = new List<float>();
            for (int i = 0; i < baseState.Count; i++)
                result.Add((float)(Math.Ceiling((baseState[i] + leftState[i] + rightState[i]) / 3)));
            return result;
        }

    }
}
