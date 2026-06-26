using UnityEngine;

[System.Serializable]
public class MaterialProperties
{
    public string materialName;
    public float E1;        //Longitidinal Modulus (Psi)
    public float E2;        //Transerse Modulus (Psi)
    public float G12;       //Shear Modulus (Psi)
    public float nu12;      //Poisson's Ratio dimensionless
    public float density;   //lb per cubic in

    //these are for a layer that has been rotated
    public float Ex;        //Longitidinal Modulus (Psi)
    public float Ey;        //Transerse Modulus (Psi)
    public float Gxy;       //Shear Modulus (Psi)
    public float nuxy;      //Poisson's Ratio dimensionless

    public Material material; //need to add texture to this too - orientation (in physical properties) visual needed 

    //these are parameters for use with the Ramberg-Osgood Equation for simulating the nonlinear behavior of a material
    public float yieldStress; //psi
    public float ultimateTensileStress; //psi
    public float failureStrain;
    public float yieldStrain;

    //for display control
    public bool displayProperties;
}
