/***********************************************************************
/*      Copyright Niugnep Software, LLC 2014 All Rights Reserved.      *
/*                                                                     *
/*     THIS WORK CONTAINS TRADE SECRET AND PROPRIETARY INFORMATION     *
/*     WHICH IS THE PROPERTY OF NIUGNEP SOFTWARE, LLC OR ITS           *
/*             LICENSORS AND IS SUBJECT TO LICENSE TERMS.              *
/**********************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// This MonoBehavior expects to be attached to the same game object
// that a UINgraph is attached to.  This object is called "graph" if
// you created it with the NGraph Graph Creation Wizard.

// added to the original script to include more graph configurations
namespace GraphMaster
{
    public class ScatterGraphScript : MonoBehaviour
    {

        UGuiGraph graph;

        //this is for the basic isotropic material
        UGuiDataSeriesXy series1;
        public List<Vector2> data1 = new List<Vector2>();


        //this is for the orthotropic material
        UGuiDataSeriesXy series21;
        UGuiDataSeriesXy series22;
        public List<Vector2> data21 = new List<Vector2>();
        public List<Vector2> data22 = new List<Vector2>();


        //this is for the multilayer case
        UGuiDataSeriesXy series31;
        UGuiDataSeriesXy series32;
        public List<Vector2> data31 = new List<Vector2>();
        public List<Vector2> data32 = new List<Vector2>();


        public TextMeshProUGUI xAxisTitle;
        public TextMeshProUGUI yAxisTitle;

        private DataController dataController;

        void Awake()
        {
            // Do not try to create run-time objects unless we are in play mode
            if (!Application.isPlaying)
            {
                return;
            }

            dataController = FindObjectOfType<DataController>();

            //setup the graph based on the test type
            switch ((int)dataController.testType)
            {
                case (int)TestType.isotropicMaterialTensileTest: IsotropicGraphSetup(); break;
                case (int)TestType.orthotropicMaterialTensileTest: OrthrotropicGraphSetup(); break;
                case (int)TestType.multilayerMaterialTensileTest: MultiLayerOrthrotropicGraphSetup(); break;
                default: break;
            }

        }
        private void IsotropicGraphSetup()
        {
            // Setup the graph
            graph = gameObject.GetComponent<UGuiGraph>();
            graph.setRanges(0, 0.03f, 0, 20000);
            series1 = graph.addDataSeries<UGuiDataSeriesXy>("1", Color.blue);
            series1.PlotThickness = 2.0f;
            series1.MarkerWeight = 10.0f;
            series1.Reveal = 0.5f;
            series1.MarkersStyle = DataSeriesXy.MarkerStyle.Triangle;
            data1.Add(new Vector2(0, 0));

            xAxisTitle.text = "Displacement (in)";
            yAxisTitle.text = "Load (Lbs)";
        }

        private void OrthrotropicGraphSetup()
        {
            graph = gameObject.GetComponent<UGuiGraph>();
            graph.setRanges(-0.005f, 0.005f, 0, 20000);

            series21 = graph.addDataSeries<UGuiDataSeriesXy>("21", Color.blue);
            series21.PlotThickness = 2.0f;
            series21.MarkerWeight = 10.0f;
            series21.Reveal = 0.5f;
            series21.MarkersStyle = DataSeriesXy.MarkerStyle.Triangle;

            series22 = graph.addDataSeries<UGuiDataSeriesXy>("22", Color.green);
            series22.PlotThickness = 2.0f;
            series22.MarkerWeight = 10.0f;
            series22.Reveal = 0.5f;
            series22.MarkersStyle = DataSeriesXy.MarkerStyle.Triangle;

            data21.Add(new Vector2(0, 0));
            data22.Add(new Vector2(0, 0));

            xAxisTitle.text = "Strain (in/in)";
            yAxisTitle.text = "Stress (Psi)";
        }

        private void MultiLayerOrthrotropicGraphSetup()
        {
            graph = gameObject.GetComponent<UGuiGraph>();
            graph.setRanges(-0.01f, 0.01f, 0, 20000);

            series31 = graph.addDataSeries<UGuiDataSeriesXy>("31", Color.blue);
            series31.PlotThickness = 2.0f;
            series31.MarkerWeight = 10.0f;
            series31.Reveal = 0.5f;
            series31.MarkersStyle = DataSeriesXy.MarkerStyle.Triangle;

            series32 = graph.addDataSeries<UGuiDataSeriesXy>("32", Color.green);
            series32.PlotThickness = 2.0f;
            series32.MarkerWeight = 10.0f;
            series32.Reveal = 0.5f;
            series32.MarkersStyle = DataSeriesXy.MarkerStyle.Triangle;

            data31.Add(new Vector2(0, 0));
            data32.Add(new Vector2(0, 0));

            xAxisTitle.text = "Strain";
            yAxisTitle.text = "N<sub>x</sub> (Psi)";

        }

        //these routines are called from the script that runs the tensile test
        public void UpdateGraphIsotropic(float x, float y)
        {
            data1.Add(new Vector2(x, y));
            series1.Data = data1;
        }

        public void UpdateGraphOrthotropic(float x1, float y1, float y2)
        {
            data21.Add(new Vector2(y1, x1));
            series21.Data = data21;

            data22.Add(new Vector2(y2, x1));
            series22.Data = data22;
        }

        public void UpdateGraphMultiLayerOrthotropic(float x1, float y1, float y2)
        {
            data31.Add(new Vector2(y1, x1));
            series31.Data = data31;

            data32.Add(new Vector2(y2, x1));
            series32.Data = data32;
        }
    }
}