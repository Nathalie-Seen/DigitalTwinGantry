using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Oculus.Interaction;

public class LeverMeter : MonoBehaviour
{
    [SerializeField] private Grabbable m_grabbable;
    [SerializeField] private OneGrabRotateTransformer m_transformer;
    [SerializeField] private OneGrabRotateTransformer.Axis m_axis;
    [SerializeField] private UnityEvent<float> m_onUpdate;

    private float m_value;
    public float Value { get => m_value; }

    private void Start() {
        m_grabbable.WhenGrabbableUpdated += WhenGrabbableUpdated;
    }

    public static float Remap (float from, float fromMin, float fromMax, float toMin,  float toMax)
    {
        var fromAbs  =  from - fromMin;
        var fromMaxAbs = fromMax - fromMin;      
       
        var normal = fromAbs / fromMaxAbs;
 
        var toMaxAbs = toMax - toMin;
        var toAbs = toMaxAbs * normal;
 
        var to = toAbs + toMin;
       
        return to;
    }

    private void WhenGrabbableUpdated(GrabbableArgs args) {
        if (args.GrabbableEvent == GrabbableEvent.Update) {

            float rot = 0;
            switch (m_axis) {
                case OneGrabRotateTransformer.Axis.Right: rot = m_grabbable.transform.eulerAngles.x; break;
                case OneGrabRotateTransformer.Axis.Up: rot = m_grabbable.transform.eulerAngles.y; break;
                case OneGrabRotateTransformer.Axis.Forward: rot = m_grabbable.transform.eulerAngles.z; break;
            }

            if (rot > 180) {
                rot = -(360 - rot);
            }

            m_value = Remap(rot, m_transformer.Constraints.MinAngle.Value, m_transformer.Constraints.MaxAngle.Value, 0, 1);

            m_onUpdate.Invoke(Value);
        }
    }

    private void OnDestroy() {
        m_grabbable.WhenGrabbableUpdated -= WhenGrabbableUpdated;
    }
}
