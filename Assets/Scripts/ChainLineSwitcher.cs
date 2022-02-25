using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Platformer.Inputs{
    [RequireComponent(typeof(SliderJoint2D))]
    public class ChainLineSwitcher : MonoBehaviour
    {
        private SliderJoint2D sliderJoint2D;
        [SerializeField] private float startMotorSpeed = 1f;
        private JointMotor2D jointMotor2D;
        private void Start()
        {
            sliderJoint2D = GetComponent<SliderJoint2D>();
            sliderJoint2D.useMotor = true;
            jointMotor2D = sliderJoint2D.motor;
            jointMotor2D.motorSpeed = startMotorSpeed;
            sliderJoint2D.motor = jointMotor2D;
        }
        public void SwitchSliderMotor(float switchValue)
        {
            jointMotor2D.motorSpeed = switchValue;
            sliderJoint2D.motor = jointMotor2D;
        }
    }
}