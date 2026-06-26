//this script contains  matrix algebra methods to:
// A[6x6] x B[1x6]
// A[6x6] x B[6x6]
// A[3x3] x B[1x3]
// inverse of a 3x3 and 6x6 matrix using Guass-Jordan Elimination
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatrixOperations
{
    
    // multiply a square 6x6 matrix by a 1x6 vector
    public float[] MatrixVectorMultiplication6x6(float[,] matrix, float[] columnVector, int order)
    {
        float[] result = new float[6];
        for (int i = 0; i < order; i++)
        {
            for (int j = 0; j < order; j++)
            {
                result[i] += matrix[i, j] * columnVector[j];
            }
        }
        return result;
    }
    //multiply two 6 by 6 matrices together - could make this more generic and just past a order
    public float[,] MatrixMultiplication(float[,] A, float[,] B)
    {
        float[,] C = new float[20, 20];
        int order = 6;

        for (int i = 0; i < order; i++)
        {
            for (int j = 0; j < order; j++)
            {
                for (int k = 0; k < order; k++)
                {
                    C[i, j] = C[i, j] + A[i, k] * B[k, j];
                }
            }
        }
        return C;
    }

    //multiply a 3x3 matrix by a 1x3 vector
    public float[] MatrixVectorMultiplication3x3(float[,] matrix, float[] columnVector, int order)
    {
        float[] result = new float[3];
        for (int i = 0; i < order; i++)
        {
            for (int j = 0; j < order; j++)
            {
                result[i] += matrix[i, j] * columnVector[j];
            }
        }
        return result;
    }
    // Function to perform the inverse operation on the matrix using the Gauss-Jordan Method
    //First step is to setup a matrix [M,I] where M is the matrix whose inverse is desired and I is the identity matrix
    //after this a series or row operations are performed that results in M becoming the identity matrix and I becoming the inverse of M
    public float[,] MatrixInverse3x3(float[,] matrixToInvert3x3, int order)
    {
        // Matrix Declaration.
        float[,] matrix = new float[3, 6];
        float[,] matrixInverse = new float[3, 3];
        float temp;

        //store inbound 3x3 matrix in 3x6 matrix

        for (int i = 0; i < order; i++)
        {
            for (int j = 0; j < order; j++)
            {
                matrix[i, j] = matrixToInvert3x3[i, j];
            }
        }


        // Create the augmented matrix
        // Add the identity matrix of order at the end of original matrix so [M I].
        for (int i = 0; i < order; i++)
        {
            for (int j = 0; j < 2 * order; j++)
            {
                // Add '1' at the diagonal places of the matrix to create a identity matirx
                if (j == (i + order))
                    matrix[i, j] = 1;
            }
        }

        // Interchange the row of matrix,
        // interchanging of row will start from the last row
        for (int i = order - 1; i > 0; i--)
        {
            // Swapping each and every element of the two rows
            if (matrix[i - 1, 0] < matrix[i, 0])
                for (int j = 0; j < 2 * order; j++)
                {
                    // Swapping of the row, if above condition satisfied.
                    temp = matrix[i, j];
                    matrix[i, j] = matrix[i - 1, j];
                    matrix[i - 1, j] = temp;
                }
        }

        // Replace a row by sum of itself and a constant multiple of another row of the matrix
        for (int i = 0; i < order; i++)
        {
            for (int j = 0; j < order; j++)
            {
                if (j != i)
                {

                    temp = matrix[j, i] / matrix[i, i];
                    for (int k = 0; k < 2 * order; k++)
                    {
                        matrix[j, k] -= matrix[i, k] * temp;
                    }
                }
            }
        }

        // Multiply each row by a nonzero integer. Divide row element by the diagonal element
        //this results in [I M^-1]
        for (int i = 0; i < order; i++)
        {

            temp = matrix[i, i];
            for (int j = 0; j < 2 * order; j++)
            {

                matrix[i, j] = matrix[i, j] / temp;
            }
        }

        //store the inverse in the matrixInverse variable
        for (int i = 0; i < order; i++)
        {
            for (int j = 0; j < order; j++)
            {
                matrixInverse[i, j] = matrix[i, j + order];
            }
        }

        return matrixInverse;
    }

    public float[,] MatrixInverse6x6(float[,] matrixToInvert6x6)
    {
        // Matrix Declaration.
        float[,] matrix = new float[6, 12];
        float[,] matrixInverse = new float[6, 6];
        float temp;
        int order = 6;

        //store inbound 3x3 matrix in 3x6 matrix

        for (int i = 0; i < order; i++)
        {
            for (int j = 0; j < order; j++)
            {
                matrix[i, j] = matrixToInvert6x6[i, j];
            }
        }

        // Create the augmented matrix
        // Add the identity matrix of order at the end of original matrix so [M I].
        for (int i = 0; i < order; i++)
        {
            for (int j = 0; j < 2 * order; j++)
            {
                // Add '1' at the diagonal places of the matrix to create a identity matirx
                if (j == (i + order))
                    matrix[i, j] = 1;
            }
        }

        //Removed this for the 6x6 in this case since it can be a sparsely populated matrix

        // Interchange the row of matrix,
        // interchanging of row will start from the last row
        //for (int i = order - 1; i > 0; i--)
        //{
        //    // Swapping each and every element of the two rows
        //    if (matrix[i - 1, 0] < matrix[i, 0])
        //        for (int j = 0; j < 2 * order; j++)
        //        {
        //            // Swapping of the row, if above condition satisfied.
        //            temp = matrix[i, j];
        //            matrix[i, j] = matrix[i - 1, j];
        //            matrix[i - 1, j] = temp;
        //        }
        //}


        // Replace a row by sum of itself and a constant multiple of another row of the matrix
        for (int i = 0; i < order; i++)
        {
            for (int j = 0; j < order; j++)
            {
                if (j != i)
                {

                    temp = matrix[j, i] / matrix[i, i];
                    for (int k = 0; k < 2 * order; k++)
                    {
                        matrix[j, k] -= matrix[i, k] * temp;
                    }
                }
            }
        }

        // Multiply each row by a nonzero integer. Divide row element by the diagonal element
        //this results in [I M^-1]
        for (int i = 0; i < order; i++)
        {

            temp = matrix[i, i];
            for (int j = 0; j < 2 * order; j++)
            {

                matrix[i, j] = matrix[i, j] / temp;
            }
        }

        //store the inverse in the matrixInverse variable
        for (int i = 0; i < order; i++)
        {
            for (int j = 0; j < order; j++)
            {
                matrixInverse[i, j] = matrix[i, j + order];
            }
        }

        return matrixInverse;
    }

    /// <summary>
    /// Methods to display various matrices 
    /// </summary>
    public void PrintMatrix3x3(float[,] ar, int order)
    {
        for (int i = 0; i < order; i++)
        {
            Debug.LogFormat("{0:E2}  {1:E2}  {2:E3}", ar[i, 0], ar[i, 1], ar[i, 2]);
        }
    }

    public void PrintMatrix3x6(float[,] ar, int order)
    {
        for (int i = 0; i < order; i++)
        {
            Debug.LogFormat("{0:E2}  {1:E2}  {2:E2}   {3:E2}  {4:E2}  {5:E2}", ar[i, 0], ar[i, 1], ar[i, 2], ar[i, 3], ar[i, 4], ar[i, 5]);
        }
    }

    public void PrintMatrix6x6(float[,] ar)
    {
        int order = 6;
        for (int i = 0; i < order; i++)
        {
            Debug.LogFormat("{0:E2}  {1:E2}  {2:E2}   {3:E2}  {4:E2}  {5:E2}", ar[i, 0], ar[i, 1], ar[i, 2], ar[i, 3], ar[i, 4], ar[i, 5]);
        }
    }

    public void PrintMatrix6x12(float[,] ar)
    {
        int order = 6;
        for (int i = 0; i < order; i++)
        {
            Debug.LogFormat("{0:E2}  {1:E2}  {2:E2}   {3:E2}  {4:E2}  {5:E2}   {6:E2}  {7:E2}  {8:E2}   {9:E2}  {10:E2}  {11:E2}", ar[i, 0], ar[i, 1], ar[i, 2], ar[i, 3], ar[i, 4], ar[i, 5], ar[i, 6], ar[i, 7], ar[i, 8], ar[i, 9], ar[i, 10], ar[i, 11]);
        }
    }

}