using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockController : MonoBehaviour
{
    public GameObject ClockPendulum;
    public GameObject HourPointer;
    public GameObject MinutePointer;
    public float PendulumAngle = 5.0f;
    // Ile sekund w rzeczywistości trwa jedna sekunda w grze.
    public float TimeScale = 1.0f;
    private const float MINUTE_POINTER_FULL_ROTATION_TIME = 60.0f * 60.0f;
    private float elapsedTime;
    private float direction;

	// Use this for initialization
	void Start ()
    {
        elapsedTime = TimeScale * 0.5f;
        direction = 1.0f;
    }
	
	// Update is called once per frame
	void Update ()
    {
        float delta = Time.deltaTime;
        float scaledDelta = delta / TimeScale;
        float minuteZInc = scaledDelta * 360.0f / MINUTE_POINTER_FULL_ROTATION_TIME;
        float hourZInc = minuteZInc / 24.0f;
        float pendulumZInc = PendulumAngle * 2.0f * scaledDelta;
        Vector3 minuteRotation = MinutePointer.transform.localRotation.eulerAngles;
        Vector3 hourRotation = HourPointer.transform.localRotation.eulerAngles;
        Vector3 pendulumRotation = ClockPendulum.transform.localRotation.eulerAngles;
        float minuteZ = minuteRotation.z + minuteZInc;
        if (minuteZ > 360.0f)
        {
            minuteZ = minuteZ - 360.0f;
        }
        float hourZ = hourRotation.z + hourZInc;
        if (hourZ > 360.0f)
        {
            hourZ = hourZ - 360.0f;
        }
        float pendulumZ = pendulumRotation.z + (direction * pendulumZInc);
        elapsedTime += delta;
        if (elapsedTime >= TimeScale)
        {
            float oldDir = direction;
            direction *= (-1.0f);
            elapsedTime = elapsedTime - TimeScale;
            pendulumZ = (oldDir * PendulumAngle) + (direction * PendulumAngle * 2.0f * elapsedTime / TimeScale);
        }
        MinutePointer.transform.localRotation = Quaternion.Euler(minuteRotation.x, minuteRotation.y, minuteZ);
        HourPointer.transform.localRotation = Quaternion.Euler(hourRotation.x, hourRotation.y, hourZ);
        ClockPendulum.transform.localRotation = Quaternion.Euler(pendulumRotation.x, pendulumRotation.y, pendulumZ);
    }
}
