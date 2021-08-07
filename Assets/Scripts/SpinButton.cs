using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using System;

public class SpinButton : MonoBehaviour
{
    [SerializeField] private float spinTime = 0.5f; // Amount of time needed for a full spin
    [SerializeField] private AnimationCurve curve = AnimationCurve.EaseInOut(0,0,1,1); // Controls the animation pacing
    
    private IEnumerator StartSpinning()
    {
        if (spinTime <= 0) {
            yield break; // Don't do any spinning when spinTime <=0
        }

        float elapsedTime = 0.0f; // Keep track of how long it's been spinning for

        while (elapsedTime < spinTime) {
            elapsedTime += Time.deltaTime;
            var timeElapsedPercentage = elapsedTime / spinTime; // Calculate how far along the animation it is (0 - 1)
            
            // Use this value to figure out how many degrees should be rotated at on this frame.
            // Using an animation curve for this instead of incrementing the rotation every frame is a much more powerful and flexible way to animate.
            // It removes much of the manual work to focus on the desired movement of the UI, rather than coding complex animations.
            var rotationAngle = curve.Evaluate(timeElapsedPercentage) * 360f; // Sampling from the curve at a requested time to tell what's going on
            
            // Calculate the rotation by rotating this many angles around the x-axis. TODO: convert axis to property
            transform.localRotation = Quaternion.AngleAxis(rotationAngle, Vector3.right);
            yield return null; // Wait a new frame
        }
        
        // After the animation is complete: reset the rotation to normal
        transform.localRotation = quaternion.identity;
    }

    public void Spin()
    {
        StartCoroutine(StartSpinning());
    }
}
