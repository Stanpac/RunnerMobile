using System;
using UnityEngine;

// Tool Selection of a weighted list of choices
public class WeightedSelection<T>
{
    [SerializeField] [HideInInspector] 
    public WeightedSelection<T>.ChoiceInfo[] Choices;
    
    [SerializeField] [HideInInspector] 
    private int _count;

    [SerializeField] [HideInInspector] 
    private float _totalWeight;

    private const int MinCapacity = 8;

    public int Count
    {
        get => _count;
        private set => _count = value;
    }

    public WeightedSelection(int capacity = 8) => Choices = new WeightedSelection<T>.ChoiceInfo[capacity];

    public int Capacity
    {
        get => Choices.Length;
        set
        {
            if (value < 8 || value < Count)
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }

            WeightedSelection<T>.ChoiceInfo[] choices1 = Choices;
            Choices = new WeightedSelection<T>.ChoiceInfo[value];
            WeightedSelection<T>.ChoiceInfo[] choices2 = Choices;
            int count = Count;
            Array.Copy((Array)choices1, (Array)choices2, count);
        }
    }

    public void AddChoice(T value, float weight) => AddChoice(new WeightedSelection<T>.ChoiceInfo()
    {
        value = value,
        weight = weight
    });

    public void AddChoice(WeightedSelection<T>.ChoiceInfo choice)
    {
        if (Count == Capacity)
            Capacity *= 2;
        Choices[Count++] = choice;
        _totalWeight += choice.weight;
    }

    public void RemoveChoice(int choiceIndex)
    {
        int index1 = choiceIndex >= 0 && Count > choiceIndex ? choiceIndex : throw new ArgumentOutOfRangeException(nameof(choiceIndex));
        for (int index2 = Count - 1; index1 < index2; ++index1) {
            Choices[index1] = Choices[index1 + 1];
        }
        Choices[--Count] = new WeightedSelection<T>.ChoiceInfo();
        RecalculateTotalWeight();
    }

    public void ModifyChoiceWeight(int choiceIndex, float newWeight)
    {
        Choices[choiceIndex].weight = newWeight;
        RecalculateTotalWeight();
    }

    public void Clear()
    {
        for (int index = 0; index < Count; ++index) {
            Choices[index] = new WeightedSelection<T>.ChoiceInfo();
        }
        Count = 0;
        _totalWeight = 0.0f;
    }

    private void RecalculateTotalWeight()
    {
        _totalWeight = 0.0f;
        for (int index = 0; index < Count; ++index)
            _totalWeight += Choices[index].weight;
    }
    
    
    public T Evaluate() => Choices[EvaluateToChoiceIndex()].value;

    public int EvaluateToChoiceIndex() => EvaluateToChoiceIndex(null);

    public int EvaluateToChoiceIndex(int[] ignoreIndices)
    {
        if (Count == 0)
            throw new InvalidOperationException("Cannot call Evaluate without available choices.");
        float totalWeight = _totalWeight;

        if (ignoreIndices != null) {
            foreach (int ignoreIndex in ignoreIndices)
                totalWeight -= Choices[ignoreIndex].weight;
        }

        float num1 = totalWeight;
        float num2 = 0.0f;

        for (int toChoiceIndex = 0; toChoiceIndex < Count; ++toChoiceIndex) {
            if (ignoreIndices == null || Array.IndexOf<int>(ignoreIndices, toChoiceIndex) == -1) {
                num2 += Choices[toChoiceIndex].weight;
                if (num1 < num2)
                    return toChoiceIndex;
            }
        }

        return Count - 1;
    }

    public WeightedSelection<T>.ChoiceInfo GetChoice(int i) => Choices[i];

    [Serializable]
    public struct ChoiceInfo
    {
        public T value;
        public float weight;
    }
}