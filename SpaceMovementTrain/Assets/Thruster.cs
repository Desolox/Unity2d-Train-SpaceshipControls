using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Thruster : MonoBehaviour {

    public float ThrustPower { get; private set; } = 1;
    public ThrusterOrientationEnum ThrusterOrientation;
}
