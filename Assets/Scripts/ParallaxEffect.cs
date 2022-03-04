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
    public bool overrideUseFixedUpdate
    {
        get{ return _useFixedUpdate;}
        set
        {
            _useFixedUpdate = value;
            if(!value)
            {
                CancelInvoke(nameof(CheckFPS));
            }
            else
            {
                InvokeRepeating(nameof(CheckFPS), 1, 1);
            }
        }
    }
    public bool _useFixedUpdate = true;
    void Start()
    {
        size = backgrounds.Length;
        initialCameraPosition = transform.position;
        initialPositions = new Vector3[size];
        for (int i = 0; i < size; i++)
        {
            initialPositions[i] = backgrounds[i].position;
        }
        InvokeRepeating(nameof(CheckFPS), 1, 1);
    }

    private void CheckFPS()
    {
        float fps = (int)(1f / Time.unscaledDeltaTime);
        if (fps < 40)
        {
            _useFixedUpdate = false;
        }
        else
        {
            _useFixedUpdate = true;
        }
    }
    private void FixedUpdate()
    {
        if(_useFixedUpdate)
        {
            for (int i = 0; i < size; i++)
            {
                backgrounds[i].position = initialPositions[i] + (transform.position - initialCameraPosition) * coefficients[i];
            }
        }
    }

    private void LateUpdate()
    {
        if(!_useFixedUpdate)
        {
            for (int i = 0; i < size; i++)
            {
                backgrounds[i].position = initialPositions[i] + (transform.position - initialCameraPosition) * coefficients[i];
            }
        }
    }
}
