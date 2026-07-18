using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UIElements;

[System.Serializable]
public class LaminateProperties
{
    public string Name;
    public float length;
    public float width;
    public float thickness;
    public int numberOfLayers;
    public List<Lamina> lamina = new List<Lamina>();

    public void Reset()
    {
        // Default values
        Name = "Unnamed";
        length = 0;
        width = 0;
        thickness = 0f;
        //numberOfLayers = 0;
    }
}
