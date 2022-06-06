using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PolarizedObjects : MonoBehaviour
{
    public static PolarizedObjects instance;

    [SerializeField]
    public float blinkTime;

    [SerializeField]
    public float outlineWidthFactor;

    float time;

    [SerializeField] float globalOutlineWidth;
    [System.Serializable]

    public class WaveWisePolarized
    {
        public GameObject[] polarizedObjects;
    }

    public bool canOutline = false;

    public WaveWisePolarized[] waveWisePolarised;

    List<Outline> outlines = new List<Outline>();

    private void Awake()
    {
        if (instance == null)
            instance = this;    
    }

    private void Start()
    {
        InitializeOutlinesInLevel();
    }

    public void ActivateOutlineByWave()
    {
        outlines.Clear();

        GameObject[] objects = waveWisePolarised[gameManager.instance.atWave].polarizedObjects;

        for (int i = 0; i < objects.Length; i++)
        {
            var temp = objects[i].GetComponent<Outline>();
            outlines.Add(temp);
        }

        canOutline = true;
    }

    public void InitializeOutlinesInLevel()
    {

        for (int i = 0; i < waveWisePolarised.Length; i++)
        {
            GameObject[] puzzlePolarisedObject = waveWisePolarised[i].polarizedObjects;

            for (int j = 0; j < puzzlePolarisedObject.Length; j++)
            {
                AddOutlineToObject(puzzlePolarisedObject[j]);
            }
        }
    }

    public void AddOutlineToObject(GameObject obj)
    {
        //Debug.Log("Working");
        var outline = obj.AddComponent<Outline>();
        //outlineObjects.Add(outline);
        outline.OutlineMode = Outline.Mode.OutlineAll;
        outline.OutlineColor = Color.white;
        outline.OutlineWidth = 0;

        // Preventing outline from overlapping other objects
        outline.OutlineMode = Outline.Mode.OutlineVisible;
    }

    public void StopOutliningObjects()
    {
        canOutline = false;
    }

    private void Update()
    {
        if (canOutline && outlines.Count > 0)
        {
            time += Time.deltaTime;

            foreach (Outline outline in outlines)
            {
                outline.outlineFillMaterial.SetFloat("_OutlineWidth", globalOutlineWidth + Mathf.Sin(time / blinkTime) * outlineWidthFactor);
            }
        }
        else if (outlines.Count > 0)
        {
            for( int i = 0; i < outlines.Count;)
            {
                outlines[i].outlineFillMaterial.SetFloat("_OutlineWidth", 0);
                outlines.RemoveAt(i);
            }
        }
    }
}
