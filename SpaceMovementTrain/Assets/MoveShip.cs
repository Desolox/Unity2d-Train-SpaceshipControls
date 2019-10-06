using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class MoveShip : MonoBehaviour{

    Rigidbody2D rb2d;
    EdgeCollider2D edgeCol2d;

    List<Thruster> Thrusters;
    List<Thruster> thrustersForMoveForward = new List<Thruster>();
    List<Thruster> thrustersForMoveBackwards = new List<Thruster>();
    List<Thruster> thrustersForRotateClockwise = new List<Thruster>();
    List<Thruster> thrustersForRotateCounterClockwise = new List<Thruster>();
    List<Thruster> thrustersForStrafeToRight = new List<Thruster>();
    List<Thruster> thrustersForStrafeToLeft = new List<Thruster>();

    bool needRotate, needStrafe, needMove;
    ThrusterOrientationEnum dirToMove, dirToRotate, dirToStrafe;

    Vector2 pivotCenter;


    // Start is called before the first frame update
    void Start(){
        rb2d = GetComponent<Rigidbody2D>();
        edgeCol2d = GetComponent<EdgeCollider2D>();
        pivotCenter = edgeCol2d.bounds.center;

        AssignThrusterToPosition();
    }

    private void FixedUpdate() {
        if(needMove) {
            Move();
        }

        if(needRotate) {
            RotateShip();
        }else if(needStrafe) {
            Strafe();
        }
    }

    // Update is called once per frame
    void Update() {

        float moveDir = Input.GetAxisRaw("Vertical");
        if(moveDir > 0) {
            needMove = true;
            dirToMove = ThrusterOrientationEnum.Back;
        }else if(moveDir < 0) {
            needMove = true;
            dirToMove = ThrusterOrientationEnum.Front;
        }
        else {
            needMove = false;
            dirToMove = ThrusterOrientationEnum.None;
        }

        float rotDir = Input.GetAxisRaw("Horizontal");
        float strafeDir = Input.GetAxisRaw("Strafe");

        if(rotDir > 0) {
            needRotate = true;
            dirToRotate = ThrusterOrientationEnum.Left;
        }else if (rotDir < 0) {
            needRotate = true;
            dirToRotate = ThrusterOrientationEnum.Right;
        }
        else {
            needRotate = false;
            dirToRotate = ThrusterOrientationEnum.None;

            if(strafeDir > 0) {
                needStrafe = true;
                dirToStrafe = ThrusterOrientationEnum.Left;
            }
            else if(strafeDir < 0) {
                needStrafe = true;
                dirToStrafe = ThrusterOrientationEnum.Right;
            }
            else {
                needStrafe = false;
                dirToStrafe = ThrusterOrientationEnum.None;
            }
        }
    }

    void RotateShip() {
        switch(dirToRotate) {
            case ThrusterOrientationEnum.Right:
                for(int i = 0; i < thrustersForRotateCounterClockwise.Count; i++) {
                    float angle = thrustersForRotateCounterClockwise[i].transform.rotation.eulerAngles.z;
                    if(angle > 180) {
                        angle -= 90;
                    }else if(angle < -180) {
                        angle += 90;
                    }
                    Debug.Log($"{thrustersForRotateCounterClockwise[i].name} is at {angle} degre.");
                    rb2d.AddForceAtPosition(
                        GetVecByAngle(angle,
                                        thrustersForRotateCounterClockwise[i].ThrustPower),
                                    thrustersForRotateCounterClockwise[i].transform.position);
                }
                break;
            case ThrusterOrientationEnum.Left:
                for(int i = 0; i < thrustersForRotateClockwise.Count; i++) {
                    float angle = thrustersForRotateClockwise[i].transform.rotation.eulerAngles.z;
                    if(angle > 180) {
                        angle -= 90;
                    }
                    else if(angle < -180) {
                        angle += 90;
                    }
                    Debug.Log($"{thrustersForRotateClockwise[i].name} is at {angle} degre.");
                    rb2d.AddForceAtPosition(
                        GetVecByAngle(angle,
                                        thrustersForRotateClockwise[i].ThrustPower),
                                    thrustersForRotateClockwise[i].transform.position);
                }
                break;
            default:
                break;
        }
    }

    void Strafe() {
        switch(dirToStrafe) {
            case ThrusterOrientationEnum.Right:
                for(int i = 0; i < thrustersForStrafeToLeft.Count; i++) {
                    float angle = thrustersForStrafeToLeft[i].transform.rotation.eulerAngles.z;
                    if(angle > 180) {
                        angle -= 90;
                    }
                    else if(angle < -180) {
                        angle += 90;
                    }
                    rb2d.AddForceAtPosition(
                        GetVecByAngle(angle,
                                        thrustersForStrafeToLeft[i].ThrustPower),
                                    thrustersForStrafeToLeft[i].transform.position);
                }
                break;
            case ThrusterOrientationEnum.Left:
                for(int i = 0; i < thrustersForStrafeToRight.Count; i++) {
                    float angle = thrustersForStrafeToRight[i].transform.rotation.eulerAngles.z;
                    if(angle > 180) {
                        angle -= 90;
                    }
                    else if(angle < -180) {
                        angle += 90;
                    }
                    rb2d.AddForceAtPosition(
                        GetVecByAngle(angle,
                                        thrustersForStrafeToRight[i].ThrustPower),
                                    thrustersForStrafeToRight[i].transform.position);
                }
                break;
            default:
                break;
        }
    }

    void Move() {
        switch(dirToMove) {
            case ThrusterOrientationEnum.Back:
                for(int i = 0; i < thrustersForMoveForward.Count; i++) {
                    rb2d.AddForceAtPosition(
                        GetVecByAngle(thrustersForMoveForward[i].transform.rotation.eulerAngles.z - 90,
                                        thrustersForMoveForward[i].ThrustPower),
                                    thrustersForMoveForward[i].transform.position);
                }
                break;
            case ThrusterOrientationEnum.Front:
                for(int i = 0; i < thrustersForMoveBackwards.Count; i++) {
                    rb2d.AddForceAtPosition(
                        GetVecByAngle(thrustersForMoveBackwards[i].transform.rotation.eulerAngles.z - 90,
                                        thrustersForMoveBackwards[i].ThrustPower),
                                    thrustersForMoveBackwards[i].transform.position);
                }
                break;
            default:
                break;
        }
    }

    Vector2 GetVecByAngle(float a, float length) {
        return FromAngle(a / 360 * Mathf.PI * 2, length);
    }

    Vector2 FromAngle(float angle, float length) {
        return new Vector2(length * Mathf.Cos(angle), length * Mathf.Sin(angle));
    }

    void AssignThrusterToPosition() {
        Thrusters = GameObject.FindObjectsOfType<Thruster>().ToList();

        for(int i = 0; i < Thrusters.Count; i++) {

            switch(Thrusters[i].ThrusterOrientation) {
                case ThrusterOrientationEnum.Back:
                    thrustersForMoveForward.Add(Thrusters[i]);
                    break;
                case ThrusterOrientationEnum.Front:
                    thrustersForMoveBackwards.Add(Thrusters[i]);
                    break;
                case ThrusterOrientationEnum.Right:
                    thrustersForStrafeToLeft.Add(Thrusters[i]);
                    if(Thrusters[i].transform.position.y > pivotCenter.y) {
                        thrustersForRotateCounterClockwise.Add(Thrusters[i]);
                    }
                    else if(Thrusters[i].transform.position.y < pivotCenter.y) {
                        thrustersForRotateClockwise.Add(Thrusters[i]);
                    }
                    break;
                case ThrusterOrientationEnum.Left:
                    thrustersForStrafeToRight.Add(Thrusters[i]);
                    if(Thrusters[i].transform.position.y > pivotCenter.y) {
                        thrustersForRotateClockwise.Add(Thrusters[i]);
                    }
                    else if(Thrusters[i].transform.position.y < pivotCenter.y) {
                        thrustersForRotateCounterClockwise.Add(Thrusters[i]);
                    }
                    break;
                default:
                    Debug.LogError($"ThrusterOrientation is not assigned for [{Thrusters[i].name}]");
                    break;
            }
        }
    }
}
