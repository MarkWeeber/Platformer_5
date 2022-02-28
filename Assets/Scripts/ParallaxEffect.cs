using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    [SerializeField] private Transform [] backgrounds = null;
    [SerializeField] private float[] coefficients = null;
    private Vector3 [] initialPositions = null;
    private Vector3 initialCameraPosition = Vector3.zero;
    private int size = 0;
    public bool useFixedUpdate = true;
    void Start()
    {
        size = backgrounds.Length;
        initialCameraPosition = transform.position;
        initialPositions = new Vector3[size];
        for (int i = 0; i < size; i++)
        {
            initialPositions[i] = backgrounds[i].position;
        }
    }

    void FixedUpdate()
    {
        if(useFixedUpdate)
        {
            for (int i = 0; i < size; i++)
            {
                backgrounds[i].position = initialPositions[i] + (transform.position - initialCameraPosition) * coefficients[i];
            }
        }
    }

    void LateUpdate()
    {
        if(!useFixedUpdate)
        {
            for (int i = 0; i < size; i++)
            {
                backgrounds[i].position = initialPositions[i] + (transform.position - initialCameraPosition) * coefficients[i];
            }
        }
    }
}
