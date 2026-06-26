using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter))]
public class MyCurveShapeDeformer : MonoBehaviour {
	
	public enum Axis {X,Y,Z};
	[Tooltip("Choose the Axis you want the deformer to work on")]
	public Axis DeformAxis = Axis.Y;
	[Tooltip("Use this curve to define the deformation")]
	public AnimationCurve Refinecurve ;
	[Tooltip("Deformation Multiplier")]
	public float Multiplier = 1.0f;
	[Tooltip("Allow unity to re-calculate the normals, sometimes its needed, others no")]
	public bool RecalculateNormals = false;
	[Tooltip("Defines whether the Deformer is static or Dynamic, if true, the deformer will only be calculated once at Start")]
	public bool isStatic = false;

	Mesh deformingMesh;
	Vector3[] originalVertices, displacedVertices;
	float smallestY = 0.0f;
	float largestY  = 0.0f;

    //I added these
    float largestX = 0.0f;
    float smallestX = 0.0f;
    float smallestZ = 0.0f;

    void Start () 
	{
		if (Refinecurve.length == 0)
        {
			Refinecurve = new AnimationCurve (new Keyframe (0, 0), new Keyframe (1, 0.5f));
		}

		deformingMesh = GetComponent<MeshFilter>().mesh;
		originalVertices = deformingMesh.vertices;
		displacedVertices = new Vector3[originalVertices.Length];

        //int count = 0;
        //float lastz = 0.2f;

        for (int i = 0; i < originalVertices.Length; i++)
		{
			displacedVertices[i] = originalVertices[i];

            //if (originalVertices[i].z != lastz)
            //{
            //    count += 1;
            //    Debug.Log(count + " " + i + " " + originalVertices[i]);
            //    //                Debug.Log(count);
            //    lastz = originalVertices[i].z;

            //}

          //  Debug.Log(count + " " + i + " " + originalVertices[i].z);

            if (displacedVertices[i].y < smallestY)
			{
				smallestY = displacedVertices[i].y;
			}

            //added these
            if (displacedVertices[i].x < smallestX)
            {
                smallestX = displacedVertices[i].x;
            }

            if (displacedVertices[i].z < smallestZ)
            {
                smallestX = displacedVertices[i].z;
            }

            //added these

            if (displacedVertices[i].x > largestX)
            {
                largestX = displacedVertices[i].x;
            }

            if (displacedVertices[i].y > largestY)
            {
                largestY = displacedVertices[i].y;
            }
        }

        Debug.Log(smallestY + "  " + largestY + " " + (largestY - smallestY));
		CurveUP ();
	}

	void FixedUpdate()
	{
		if (!isStatic) {
			CurveUP ();
		}
	}

	void CurveUP ()
	{
        float lastCenterLineDeflection = 0.0f;
        float lasty = -smallestY;
        float dx = (largestY - smallestY) / 36.0f;
 

        for (int i = 0; i < originalVertices.Length; i++)
		{

			float x, y, z;
			x = originalVertices [i].x;
			y = originalVertices [i].y;
			z = originalVertices [i].z;
			float normalized = (y - smallestY) / (largestY - smallestY);
            float curveValue = Refinecurve.Evaluate (normalized);

//            float centerlineDeflection = -1.0f * (y - smallestY) * (3.0f - Mathf.Pow((2.0f * (y - smallestY) / (2.0f * (largestY - smallestY))), 2));
            float centerlineDeflection = -1.0f * (x - smallestX) * (3.0f - Mathf.Pow((2.0f * (x - smallestX) / (2.0f * (largestX - smallestX))), 2));
            float dwdx = (centerlineDeflection - lastCenterLineDeflection)/dx;
            //            float meshBend = -1.0f*(y - smallestY) * (Mathf.Pow(3.0f - (2.0f * (y - smallestY)/(0.6f)), 2));
             float meshBendx = centerlineDeflection;
//            float meshBendx = centerlineDeflection - x * dwdx;

            float meshTwist = Mathf.Pow(5*(z - smallestZ), 1);

            float new_x = x;
			float new_y = y;
			float new_z = z;

            lastCenterLineDeflection = centerlineDeflection;


            switch (DeformAxis) 
			{
                case Axis.X:
                    //new_x = x + curveValue * Multiplier;
                    new_x = x + meshBendx * Multiplier; //this moves the mesh point in the vertical direction like bending, which is in the local x direction based on its y position
                    break;
                case Axis.Y:
                    //                    new_y = y + curveValue * Multiplier;
                    new_y = y + meshBendx * Multiplier; //this stretches it
                    //                    new_y = y + twistCurve * Multiplier;

                    break;
                case Axis.Z:
//                    new_z = z + curveValue * Multiplier;
                    new_x = x + meshTwist * Multiplier; //this moves the mesh point in the vertical direction like twisting, which is in the local x direction based on its z position

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

}
