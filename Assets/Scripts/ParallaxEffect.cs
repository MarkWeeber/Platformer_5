using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    [SerializeField] private Transform [] backgrounds = null;
    [SerializeField] private float[] coefficients = null;
    private int size = 0;
    void Start()
    {
        size = backgrounds.Length;
    }

    void FixedUpdate()
    {
        for (int i = 0; i < size; i++)
        {
            backgrounds[i].position = transform.position * coefficients[i];
        }
    }
}
