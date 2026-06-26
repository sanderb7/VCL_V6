//the current approach uses prefabs to create a material property component, so this is attached to it.  Hence it needs to be based on MonoBehavior
//will chew on this some more to see if there is another method for creating the material data base.

using UnityEngine;

[System.Serializable]
public enum MaterialType
{
    isotropicMaterial   = 1,
    orthotropicMaterial = 2,
}
public class PrefabMaterialProperties : MonoBehaviour
{
    public string materialName;
    public float E1;        //Longitidinal Modulus (Psi)
    public float E2;        //Transerse Modulus (Psi)
    public float G12;       //Shear Modulus (Psi)
    public float nu12;      //Poisson's Ratio dimensionless
    public float density;   //lbm per cubic inch

    //these are for a layer that has been rotated
    public float Ex;        //Longitidinal Modulus (Psi)
    public float Ey;        //Transerse Modulus (Psi)
    public float Gxy;       //Shear Modulus (Psi)
    public float nuxy;      //Poisson's Ratio dimensionless

    public Material material; //need to add texture to this too - orientation (in physical properties) visual needed 
    public MaterialType materialType;

    //these are parameters for use with the Ramberg-Osgood Equation for simulating the nonlinear behavior of a material
    public float yieldStress; //psi
    public float ultimateTensileStress; //psi
    public float failureStrain;
    public float yieldStrain;

    public bool displayMaterial;
}
