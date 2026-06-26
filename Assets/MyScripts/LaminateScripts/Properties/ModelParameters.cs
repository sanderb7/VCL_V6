//I don't think this is used any more
using UnityEngine;

[System.Serializable]
public class ModelParameters
{
    //these are parameters for use with the Ramberg-Osgood Equation for simulating the nonlinear behavior of a material
    float yieldStress; //psi
    float ultimateTensileStress; //psi
    float failureStrain;
    float yieldStrain;
}
