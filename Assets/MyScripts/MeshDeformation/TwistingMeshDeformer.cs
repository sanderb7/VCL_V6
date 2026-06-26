using UnityEngine;
using System.Collections;

using UnityEngine.UI;

[RequireComponent(typeof(MeshFilter))]
public class TwistingMeshDeformer : MonoBehaviour
{
	
	public enum Axis {X,Y,Z};
	[Tooltip("Choose the Axis you want the twist around")]
	public Axis DeformAroundAxis = Axis.Z;
    [Tooltip("Use this curve to define the deformation")]
    public AnimationCurve Refinecurve;
    [Tooltip("Deformation Multiplier")]
	public float Multiplier = 1.0f;
	[Tooltip("Allow unity to re-calculate the normals, sometimes its needed, others no")]
	public bool RecalculateNormals = false;
	[Tooltip("Defines whether the Deformer is static or Dynamic, if true, the deformer will only be calculated once at Start")]
	public bool isStatic = false;

	Mesh deformingMesh;
	Vector3[] originalVertices, displacedVertices;

    //variables to store mesh boundaries
    float largestX = 0.0f;
    float largestY = 0.0f;
    float largestZ = 0.0f;

    float smallestX = 0.0f;
    float smallestY = 0.0f;
    float smallestZ = 0.0f;

    //public float curvature;

    void Awake () 
	{
        if (Refinecurve.length == 0)
        {
            Refinecurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 0.5f));
        }

        deformingMesh = GetComponent<MeshFilter>().mesh;
		originalVertices = deformingMesh.vertices;
		displacedVertices = new Vector3[originalVertices.Length];

        FindMeshBoundaries();

	}

    private void Update()
    {
        if (TensileTestController.loadChange)
        {
            float curvature = -TensileTestController.curvature;
            Twist(curvature);
        }
    }
    public void Twist (float curvature)
	{
        for (int i = 0; i < originalVertices.Length; i++)
		{

			float x, y, z;
			x = originalVertices [i].x;
			y = originalVertices [i].y;
			z = originalVertices [i].z;
			float normalized = (y - smallestY) / (largestY - smallestY);
            float curveValue = Refinecurve.Evaluate (normalized);

            float new_x = x;
			float new_y = y;
			float new_z = z;

            float meshTwist; 

            switch (DeformAroundAxis) 
			{
                case Axis.X:
                    //this would need some work to get right - maybe later
                    //meshTwist = TwistAround_Y(z, y, twist);
                    //new_z = z + meshTwist * Multiplier; //this moves the mesh point in the vertical direction like twisting, which is in the local x direction based on its z position
                    //not defined
                    break;

                case Axis.Y:
                    meshTwist = TwistAround_Y(x, y, curvature);
                    new_z = z + meshTwist * Multiplier; //this moves the mesh point in the vertical direction like twisting, which is in the local x direction based on its z position
                    break;

                case Axis.Z:
                    //not defined for this case
                    break;
            }

			Vector3 newvertPos = new Vector3 (new_x, new_y, new_z);
			displacedVertices [i] = newvertPos;
		}

		deformingMesh.vertices = displacedVertices;
        if (RecalculateNormals)
        {
            deformingMesh.RecalculateNormals();
        }
    }
    private float TwistAround_Y(float x, float y, float twist)
    {
        float meshTwist;

        meshTwist = twist * (x) * (y); //this version twist from the centner and around the vertical center line

        return meshTwist;
    }

    private void FindMeshBoundaries()
    {
        for (int i = 0; i < originalVertices.Length; i++)
        {
            displacedVertices[i] = originalVertices[i];


            //find smallest

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

            //find largest

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

        //Debug.Log(smallestX + "  " + largestX + " " + (largestX - smallestX));
        //Debug.Log(smallestY + "  " + largestY + " " + (largestY - smallestY));

    }

}
