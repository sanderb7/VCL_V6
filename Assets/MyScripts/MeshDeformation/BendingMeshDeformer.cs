using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter))]
public class BendingMeshDeformer : MonoBehaviour
{

    public enum Axis {X,Y,Z};
    [Tooltip("Axis (x or z) you want deform around")]
    public Axis BendingAroundAxis = Axis.Z;
    //[Tooltip("Use this curve to define the deformation")]
    //public AnimationCurve Refinecurve ;
    [Tooltip("Deformation Multiplier")]
    public float Multiplier = 1.0f;
	//[Tooltip("Allow unity to re-calculate the normals, sometimes its needed, others no")]
	//public bool RecalculateNormals = false;
	//[Tooltip("Defines whether the Deformer is static or Dynamic, if true, the deformer will only be calculated once at Start")]
	//public bool isStatic = false;

	Mesh deformingMesh;
	Vector3[] originalVertices, displacedVertices;

    float smallestX = 0.0f;
    float smallestY = 0.0f;
    float smallestZ = 0.0f;


    float largestX = 0.0f;
    float largestY = 0.0f;
    float largestZ = 0.0f;

    float length, thickness;
    float scaleFactor;

    void Start()
    {

        deformingMesh = GetComponent<MeshFilter>().mesh;
        originalVertices = deformingMesh.vertices;
        displacedVertices = new Vector3[originalVertices.Length];
        FindMeshBoundaries();
    }

    private void Update()
    {
        if (ThreePointBendTestController.loadChange)
        {
            scaleFactor = ThreePointBendTestController.scaleFactor;
            Bending();
        }
    }
    public void Bending()
    {
        length = (largestX - smallestX);
        thickness = (largestY - smallestY);

        float deltaX, deltaY;
        float theta;
        float centerlineDeflection;
        float new_x, new_y, new_z;

        for (int i = 0; i < originalVertices.Length; i++)
        {

            //set x,y,z to original location
            float x, y, z;
            x = originalVertices[i].x;
            y = originalVertices[i].y;
            z = originalVertices[i].z;

            new_x = x;
            new_y = y;
            new_z = z;


            ////determine the deflection of the centerline - calculation done in english units - and rotation 
            switch (BendingAroundAxis)
            {
                case Axis.X:
                    centerlineDeflection = CL_Bending(z);
                    new_y = y + centerlineDeflection * Multiplier/ this.transform.localScale.y; //deflects it in the y direction
                    //this routine does not show rotation
                    break;
                case Axis.Y:
                    //not defined for this analysis
                    break;
                case Axis.Z:
                    centerlineDeflection = CL_Bending(x);
                    //                    theta = CL_Rotation(x);
                    theta = 0; //rotation of a plane is not working correctly - becomes an issue when not a 1, 1, 1 scale - moving on for now
                    deltaX = (y) * Mathf.Sin(theta); //3*PL^2/48EI = PL^2/16EI
                    deltaY = scaleFactor * centerlineDeflection;

                    new_x = x + (deltaX) / this.transform.localScale.x;
                    new_y = y * Mathf.Cos(theta) + deltaY / this.transform.localScale.y; //deflects it in the y direction
                    break;
            }


            //Deform mesh         
            Vector3 newvertPos = new Vector3(new_x, new_y, new_z);
            displacedVertices[i] = newvertPos;
        }

        deformingMesh.vertices = displacedVertices;

    }
    private float CL_Rotation(float x)
    {
        float theta = 0.0f;
        float xLocation = GetXLocation(x);

        //compute centerline rotation
        //note that scaleFactor = (PL^2/48EI) so 3*scaleFactor = PL^2/16EI; which is the constant outside of the dirivative dw/dx or theta 
        if (x <= 0)
        {
            theta = Mathf.Clamp(3.0f* scaleFactor * (1.0f - 4.0f * Mathf.Pow(xLocation/ length, 2)), 0, Mathf.PI / 2.0f); 
        }
        else
        {
            theta = -Mathf.Clamp(3.0f*scaleFactor * (1.0f - 4.0f * Mathf.Pow(xLocation/ length, 2)), 0, Mathf.PI / 2.0f);
        }
        return theta;
    }

    //this method computes the vertical deflection around the an axis
    private float CL_Bending(float x)
    {
        float centerlineDeflection = 0.0f;
        float xLocation = GetXLocation(x);

        //determine the deflection of the centerline - calculation done in english units
        centerlineDeflection = 1.0f * (xLocation) * (3.0f - Mathf.Pow((2.0f * (xLocation) / (length)), 2));

        return centerlineDeflection;
    }

    private float GetXLocation(float x)
    {
        //find if the lengthwise location is to the left or right of 0
        float xLocation = 0.0f;

        if (x <= 0)
        {
            xLocation = (x - smallestX);
        }
        else
        {
            xLocation = (largestX - x);
        }
        return xLocation;
    }
    private void FindMeshBoundaries()
    {

        for (int i = 0; i < originalVertices.Length; i++)
        {
            displacedVertices[i] = originalVertices[i];

            if (displacedVertices[i].x < smallestX)
            {
                smallestX = displacedVertices[i].x;
            }


            if (displacedVertices[i].y < smallestY)
            {
                smallestY = displacedVertices[i].y;
            }


            if (displacedVertices[i].z < smallestZ)
            {
                smallestZ = displacedVertices[i].z;
            }


            if (displacedVertices[i].x > largestX)
            {
                largestX = displacedVertices[i].x;
            }

            if (displacedVertices[i].y > largestY)
            {
                largestY = displacedVertices[i].y;
            }

            if (displacedVertices[i].z > largestZ)
            {
                largestZ = displacedVertices[i].z;
            }
        }

    }

}
