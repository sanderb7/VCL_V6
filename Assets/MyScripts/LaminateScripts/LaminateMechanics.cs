
//This script several functions related to determining material and structural properties of a composite layers (i.e., lamina)
//and multilayer composites.  Functions Include:
//  - Equivalent bending stiffness - based on summing ExIx for each layer
//  - Calculation for ABD matrix used in classical lamination theory
//  - The reduced stiffness matrix (Q) and Transformed reduced stiffness matrix (Qbar)
//  - Compliance (S) and transformed compliance (Sbar) matrices
//  - non priciple axis values
//  - sample weight

// Not all of the methods below need to be public since they are only used to support the public methods - future tasking
using UnityEngine;

[System.Serializable]
public class LaminateMechanics
{
    [System.NonSerialized]
    public float[,] ABDMatrix = new float[6,6];

    private float[,] Q = new float[3, 3];
    private float[,] Qbar = new float[3, 3];

    private float[,] S = new float[3, 3];
    private float[,] Sbar = new float[3, 3];

    public float EquivalentBendingStiffness(DataController dataController)
    {
        float z = dataController.testLaminate.thickness / 2.0f; //inches
        float Ixx = 0.0f;
        float totalIxx = 0;
        float bendingStiffness = 0.0f; //units MSI * In^4

        //find Ixx
        for (int i = 0; i < dataController.testLaminate.numberOfLayers; i++)
        {
            float layerBase = dataController.testLaminate.width;
            float layerHeight = dataController.testLaminate.lamina[i].physicalProperties.thickness;
            float znext = z - layerHeight;
            float d = znext + layerHeight / 2;

            float AD2 = layerBase * layerHeight * Mathf.Pow(d, 2);

            Ixx = AD2 + (1.0f / 12.0f) * (layerBase * Mathf.Pow(layerHeight, 3));
            bendingStiffness += dataController.testLaminate.lamina[i].materialProperties.Ex * Ixx;

            totalIxx += Ixx;

            z = znext;
        }
        return bendingStiffness;
    }

    public float[,] FindABDMatrix(DataController dataController)
    {
        int numberOfLayers = dataController.testLaminate.numberOfLayers;
        float[,] A = new float[3, 3];
        float[,] B = new float[3, 3];
        float[,] D = new float[3, 3];

        float[] z = new float[10];

        LayerGeometry(dataController, z);

        for (int i = 0; i < numberOfLayers; i++)
        {
            TransformedReducedStiffnessMatrix(i, dataController);

            ExtensionalStiffnessMatrix(A, z[i], z[i+1]);
            CouplingStiffnessMatrix   (B,  z[i], z[i + 1]);
            BendingStiffnessMatrix    (D,  z[i], z[i + 1]);
            //need to add elements of the the reduced stiffness matrix
        }
        //Display3x3Matrix(A);
        //Display3x3Matrix(B);
       // Display3x3Matrix(D);

        //combine matrices

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                ABDMatrix[i, j]         = A[i, j];
                ABDMatrix[i, j + 3]     = B[i, j];
                ABDMatrix[i + 3, j]     = B[i, j];
                ABDMatrix[i + 3, j + 3] = D[i, j];
            }
        }

        //Display6x6Matrix(ABDMatrix);

        return ABDMatrix;
    }

  
    private void LayerGeometry(DataController dataController, float[] z)
    {
        
        z[0] = dataController.testLaminate.thickness / 2.0f;

        for (int i = 0; i < dataController.testLaminate.numberOfLayers; i++)
        {
            z[i+1] = z[i] - dataController.testLaminate.lamina[i].physicalProperties.thickness;
        }
    }
    private void ExtensionalStiffnessMatrix(float[,] A, float ztop, float zbottom)
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                A[i, j] = A[i, j] + Qbar[i, j] * (ztop - zbottom);
            }
        }
    }

    private void CouplingStiffnessMatrix(float[,] B, float ztop, float zbottom)
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                B[i, j] = B[i, j] + 0.5f*Qbar[i, j] * (ztop*ztop - zbottom*zbottom);
            }
        }
    }

    private void BendingStiffnessMatrix(float[,] D, float ztop, float zbottom)
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                D[i, j] = D[i, j] + (1.0f/3.0f)*Qbar[i, j] * ((ztop * ztop *ztop) - (zbottom * zbottom * zbottom));
            }
        }
    }
    public void ReducedStiffnessMatrix(int i, DataController dataController)
    {

        float E1 = dataController.testLaminate.lamina[i].materialProperties.E1;
        float E2 = dataController.testLaminate.lamina[i].materialProperties.E2;
        float G12 = dataController.testLaminate.lamina[i].materialProperties.G12;
        float v12 = dataController.testLaminate.lamina[i].materialProperties.nu12;

        float v21 = v12 * (E2 / E1);

        //these comments help to aling notation in textbooks with array values since they start at 0
        //notation in text books
        //   Q11    Q12   0
        //   Q12    Q22   0
        //   0       0    Q66
        // equivalent notation in the code
        //   Q[0,0]  Q[0,1]   0 
        //   Q[1,0]  Q[1,1]   0
        //   0         0      Q[2,2]

        Q[0, 0] = E1 / (1 - v12 * v21);
        Q[0, 1] = v12 * E2 / (1 - v12 * v21);

        Q[1, 0] = Q[0, 1];
        Q[1, 1] = E2 / (1 - v12 * v21);

        Q[2, 2] = G12;

    }

    public float[,] TransformedReducedStiffnessMatrix(int i, DataController dataController)
    {
        ReducedStiffnessMatrix(i, dataController);

        float theta = dataController.testLaminate.lamina[i].physicalProperties.orientation * Mathf.Deg2Rad;
        float cosTheta = Mathf.Cos(theta);
        float sinTheta = Mathf.Sin(theta);

        //these comments help to aling notation in textbooks with array values since they start at 0
        //notation in text books
        //   Qbar11    Qbar12   Qbar16
        //   Qbar12    Qbar22   Qbar26
        //   Qbar16    Qbar26   Qbar66
        // equivalent notation in the code
        //   Qbar[0,0]  Qbar[0,1]   Qbar[0,2] 
        //   Qbar[1,0]  Qbar[1,1]   Qbar[1,2]
        //   Qbar[2,0]  Qbar[2,1]   Qbar[2,2]

        Qbar[0, 0] = Q[0, 0] * Mathf.Pow(cosTheta, 4) + 2 * (Q[0, 1] + 2 * Q[2, 2]) * (Mathf.Pow(sinTheta, 2) * Mathf.Pow(cosTheta, 2)) + Q[1, 1] * Mathf.Pow(sinTheta, 4);

        Qbar[0, 1] = (Q[0, 0] + Q[1, 1] - 4 * Q[2, 2]) * (Mathf.Pow(sinTheta, 2) * Mathf.Pow(cosTheta, 2)) + Q[0, 1] * (Mathf.Pow(sinTheta, 4) + Mathf.Pow(cosTheta, 4));

        Qbar[0, 2] = (Q[0, 0] - Q[0, 1] - 2 * Q[2, 2]) * (sinTheta * Mathf.Pow(cosTheta, 3)) + (Q[0, 1] - Q[1, 1] + 2 * Q[2, 2]) * (Mathf.Pow(sinTheta, 3) * cosTheta);

        Qbar[1, 0] = Qbar[0, 1];

        Qbar[1, 1] = Q[0, 0] * Mathf.Pow(sinTheta, 4) + 2 * (Q[0, 1] + 2 * Q[2, 2]) * (Mathf.Pow(sinTheta, 2) * Mathf.Pow(cosTheta, 2)) + Q[1, 1] * Mathf.Pow(cosTheta, 4);

        Qbar[1, 2] = (Q[0, 0] - Q[0, 1] - 2 * Q[2, 2]) * (Mathf.Pow(sinTheta, 3) * cosTheta) + (Q[0, 1] - Q[1, 1] + 2 * Q[2, 2]) * (sinTheta * Mathf.Pow(cosTheta, 3));

        Qbar[2, 0] = Qbar[0, 2];
        Qbar[2, 1] = Qbar[1, 2];
        Qbar[2, 2] = (Q[0, 0] + Q[1, 1] - 2 * Q[0, 1] - 2 * Q[2, 2]) * (Mathf.Pow(sinTheta, 2) * Mathf.Pow(cosTheta, 2)) + Q[2, 2] * (Mathf.Pow(sinTheta, 4) + Mathf.Pow(cosTheta, 4));

        return Qbar;

    }

    public float[,] ComplianceMatrix(int i, DataController dataController) 
    {
        //float[,] S = new float[3, 3];

        float E1 = dataController.testLaminate.lamina[i].materialProperties.E1;
        float E2 = dataController.testLaminate.lamina[i].materialProperties.E2;
        float G12 = dataController.testLaminate.lamina[i].materialProperties.G12;
        float v12 = dataController.testLaminate.lamina[i].materialProperties.nu12;

        float v21 = v12 * (E2 / E1);

        //these comments help to aling notation in textbooks with array values since they start at 0
        //notation in text books
        //   S11    S12   0
        //   S12    S22   0
        //   0       0    Q66
        // equivalent notation in the code
        //   S[0,0]  S[0,1]   0 
        //   S[1,0]  S[1,1]   0
        //   0         0      S[2,2]

        S[0, 0] = 1.0f/E1;
        S[0, 1] = -v12/E1;

        S[1, 0] = S[0, 1];
        S[1, 1] = 1.0f/E2;

        S[2, 2] = 1.0f/G12;

        return S;
    }

    public float[,] TransformedComplianceMatrix(int i, DataController dataController)
    {
        ComplianceMatrix(i, dataController);

        float theta = dataController.testLaminate.lamina[i].physicalProperties.orientation * Mathf.Deg2Rad;
        float cosTheta = Mathf.Cos(theta);
        float sinTheta = Mathf.Sin(theta);

        //these comments help to aling notation in textbooks with array values since they start at 0
        //notation in text books
        //   Sbar11    Sbar12   Sbar16
        //   Sbar12    Sbar22   Sbar26
        //   Sbar16    Sbar26   Sbar66
        // equivalent notation in the code
        //   Sbar[0,0]  Sbar[0,1]   Sbar[0,2] 
        //   Sbar[1,0]  Sbar[1,1]   Sbar[1,2]
        //   Sbar[2,0]  Sbar[2,1]   Sbar[2,2]

        //first rown
        Sbar[0, 0] = S[0, 0] * Mathf.Pow(cosTheta, 4) + (2.0f*S[0, 1] + S[2, 2]) * (Mathf.Pow(sinTheta, 2) * Mathf.Pow(cosTheta, 2)) + S[1, 1] * Mathf.Pow(sinTheta, 4);

        Sbar[0, 1] = S[0, 1] * (Mathf.Pow(sinTheta, 4) + Mathf.Pow(cosTheta, 4)) + (S[0, 0] + S[1, 1] - S[2, 2]) * (Mathf.Pow(sinTheta, 2) * Mathf.Pow(cosTheta, 2));

        Sbar[0, 2] = (2.0f*S[0, 0] - 2.0f*S[0, 1] - S[2, 2]) * (sinTheta * Mathf.Pow(cosTheta, 3)) - (2 * S[1, 1] - 2.0f*S[0, 1] - S[2, 2]) * (Mathf.Pow(sinTheta, 3) * cosTheta);

        //second row
        Sbar[1, 0] = Sbar[0, 1];

        Sbar[1, 1] = S[0, 0] * Mathf.Pow(sinTheta, 4) + (2.0f*S[0, 1] + S[2, 2]) * (Mathf.Pow(sinTheta, 2) * Mathf.Pow(cosTheta, 2)) + S[1, 1] * Mathf.Pow(cosTheta, 4);

        Sbar[1, 2] = (2.0f * S[0, 0] - 2.0f*S[0,1] - S[2,2]) * (Mathf.Pow(sinTheta, 3) * cosTheta) - (2.0f * S[1, 1] - 2.0f*S[0, 1] - S[2, 2]) * (sinTheta * Mathf.Pow(cosTheta, 3));

        //row three
        Sbar[2, 0] = Sbar[0, 2];

        Sbar[2, 1] = Sbar[1, 2];
        Sbar[2, 2] = 2.0f*(2*S[0, 0] + 2.0f*S[1, 1] - 4 * S[0, 1] - S[2, 2]) * (Mathf.Pow(sinTheta, 2) * Mathf.Pow(cosTheta, 2)) + S[2, 2] * (Mathf.Pow(sinTheta, 4) + Mathf.Pow(cosTheta, 4));

        return Sbar;

    }
    public void NonPrincipalProperties(int i, DataController dataController)
    {
        float theta = dataController.testLaminate.lamina[i].physicalProperties.orientation * Mathf.Deg2Rad;
        float cosTheta = Mathf.Cos(theta);
        float sinTheta = Mathf.Sin(theta);

        float E1   = dataController.testLaminate.lamina[i].materialProperties.E1;
        float E2   = dataController.testLaminate.lamina[i].materialProperties.E2;
        float G12  = dataController.testLaminate.lamina[i].materialProperties.G12;
        float nu12 = dataController.testLaminate.lamina[i].materialProperties.nu12;

        //Ex
        float Ex_Inverse = (1 / E1) * Mathf.Pow(cosTheta, 4) + (1 / G12 - 2 * nu12 / E1) * (Mathf.Pow(sinTheta, 2) * Mathf.Pow(cosTheta, 2)) + (1 / E2) * Mathf.Pow(sinTheta, 4);
        dataController.testLaminate.lamina[i].materialProperties.Ex = 1 / Ex_Inverse;

        //Ey
        float Ey_Inverse = (1.0f / E1) * Mathf.Pow(sinTheta, 4) + (1.0f / G12 - 2.0f * nu12 / E1) * (Mathf.Pow(sinTheta, 2) * Mathf.Pow(cosTheta, 2)) + (1 / E2) * Mathf.Pow(cosTheta, 4);
        dataController.testLaminate.lamina[i].materialProperties.Ey = 1 / Ey_Inverse;

        //Gxy
        float Gxy_Inverse = 2.0f * (2.0f / E1 + 2.0f / E2 + 4.0f * nu12 / E1 - 1 / G12) * (Mathf.Pow(sinTheta, 2) * Mathf.Pow(cosTheta, 2)) + (1 / G12) * (Mathf.Pow(sinTheta, 4) + Mathf.Pow(cosTheta, 4));
        dataController.testLaminate.lamina[i].materialProperties.Gxy = 1.0f / Gxy_Inverse;

        //nxy
        float Ex = dataController.testLaminate.lamina[i].materialProperties.Ex;
        dataController.testLaminate.lamina[i].materialProperties.nuxy = Ex * (nu12 / E1 * (Mathf.Pow(sinTheta, 4) + Mathf.Pow(cosTheta, 4)) - (1.0f / E1 + 1.0f / E2 - 1.0f / G12) * (Mathf.Pow(sinTheta, 2) * Mathf.Pow(cosTheta, 2)));


        float Ey = dataController.testLaminate.lamina[i].materialProperties.Ey;
        float Gxy = dataController.testLaminate.lamina[i].materialProperties.Gxy;
        float nuxy = dataController.testLaminate.lamina[i].materialProperties.nuxy;
    }

    public float LaminateWeight(DataController dataController)
    {
        float weight = 0.0f;

        for (int i = 0; i < dataController.testLaminate.numberOfLayers; i++)
        {
            float layerWidth = dataController.testLaminate.width;
            float layerLength = dataController.testLaminate.length;
            float layerHeight = dataController.testLaminate.lamina[i].physicalProperties.thickness;

            float volume = layerWidth * layerHeight * layerLength;
            weight += volume * dataController.testLaminate.lamina[i].materialProperties.density;
        }

        return weight;
    }

    public float SecondMomentOfInertia(DataController dataController)
    {
        float z = dataController.testLaminate.thickness / 2.0f; //inches
        float Ixx = 0.0f;
        float totalIxx = 0;

        //find Ixx
        for (int i = 0; i < dataController.testLaminate.numberOfLayers; i++)
        {
            float layerBase = dataController.testLaminate.width;
            float layerHeight = dataController.testLaminate.lamina[i].physicalProperties.thickness;
            float znext = z - layerHeight;
            float d = znext + layerHeight / 2;

            float AD2 = layerBase * layerHeight * Mathf.Pow(d, 2);

            Ixx = AD2 + (1.0f / 12.0f) * (layerBase * Mathf.Pow(layerHeight, 3));

            totalIxx += Ixx;

            z = znext;
        }

        return totalIxx;
    }


    //this are used to when verifying calculations
    private void Display6x6Matrix(float[,] matrix)
    {
        for (int i = 0; i < 6; i++)
        {
            Debug.LogFormat("{0:E2}  {1:E2}  {2:E2}   {3:E2}  {4:E2}  {5:E2}", matrix[i, 0], matrix[i, 1], matrix[i, 2], matrix[i, 3], matrix[i, 4], matrix[i, 5]);
        }
    }


    private void Display3x3Matrix(float[,] matrix)
    {
        int n = 3;
        for (int k = 0; k < n; k++)
        {
            Debug.LogFormat("{0:E3}  {1:E3}  {2:E3}", matrix[k, 0], matrix[k, 1], matrix[k, 2]);
        }

        Debug.Log(" ");
    }
}
