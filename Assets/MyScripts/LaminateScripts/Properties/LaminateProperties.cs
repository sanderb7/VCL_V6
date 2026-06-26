using System.Collections.Generic;

[System.Serializable]
public class LaminateProperties
{
    public string Name;
    public float length;
    public float width;
    public float thickness;
    public int numberOfLayers;
    public List<Lamina> lamina = new List<Lamina>();
}