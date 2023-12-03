using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct FruitObject
{
    public string name;
    public Color color;
    public float size;
    public int mergeScore;
}

[CreateAssetMenu(fileName = "Fruit DataBase", menuName = "Custom/Fruit Database")]
public class FruitDatabase : ScriptableObject
{
    [SerializeField] private FruitObject[] fruits;


    public void CalculateSizes()
    {
        float smallest = fruits[0].size;
        float largest = fruits[fruits.Length - 1].size;
        float spacing = (largest - smallest) / (fruits.Length - 1);
        for (int i = 0; i < fruits.Length; i++)
        {
            fruits[i].size = smallest + spacing * i;
        }
        fruits[0].size = smallest;
        fruits[fruits.Length - 1].size = largest;
    }

    public void CalculateColors()
    {
        for (int i = 0; i < fruits.Length; i++)
        {
            float hue = i * (1f / fruits.Length);
            fruits[i].color = Color.HSVToRGB(hue, 1, 1);
        }
    }

    public FruitObject GetFruitObject(int index)
    {
        return fruits[index];
    }

    public FruitObject[] GetFruitsList()
    {
        return fruits;
    }
}
