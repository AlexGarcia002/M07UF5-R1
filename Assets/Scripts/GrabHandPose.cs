using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class GrabHandPose : MonoBehaviour
{
    float poseTransitionDuration = 0.2f;
    public HandData rightHandPose;
    public HandData leftHandPose;

    Vector3 startingHandPosition;
    Vector3 finalHandPosition;
    Quaternion startingHandRotation;
    Quaternion finalHandRotation;
    Quaternion[] startingFingerRotations;
    Quaternion[] finalFingerRotations;

    void Start()
    {
        XRGrabInteractable grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(SetupPose);
        grabInteractable.selectExited.AddListener(UnSetPose);

        rightHandPose.gameObject.SetActive(false);
        leftHandPose.gameObject.SetActive(false);
    }

    public void SetupPose(BaseInteractionEventArgs arg)
    {

        if(arg.interactorObject is XRDirectInteractor) {
            
            HandData handData = arg.interactorObject.transform.GetComponentInChildren<HandData>();
            handData.animator.enabled = false;

            if(handData.handType == HandData.HandModelType.Right) {
                SetHandDataValues(handData, rightHandPose);
            } else {
                SetHandDataValues(handData, leftHandPose);
            }


            
            //SetHandData(handData, finalHandPosition, finalHandRotation, finalFingerRotations); -0.0413 -0.0717
            StartCoroutine(SetHandDataRoutine(handData, finalHandPosition, finalHandRotation, finalFingerRotations,
                                                startingHandPosition, startingHandRotation, startingFingerRotations));
        }
    }
    /*
     * aseessa k�si: Vector3(-0.0331999995,0.0715000033,0.0516999997) x:-15astetta
     * normi k�si: Vector3(-0.00100000005,0.00100000005,-0.0350000001)
     * 
     * 
     */

    public void UnSetPose(BaseInteractionEventArgs arg)
    {
        if (arg.interactorObject is XRDirectInteractor) {

            HandData handData = arg.interactorObject.transform.GetComponentInChildren<HandData>();
            handData.animator.enabled = true;

            //SetHandData(handData, startingHandPosition, startingHandRotation, startingFingerRotations);
            StartCoroutine(SetHandDataRoutine(handData, startingHandPosition, startingHandRotation, startingFingerRotations,
                                                finalHandPosition, finalHandRotation, finalFingerRotations));
        }
    }

    public void SetHandDataValues(HandData h1, HandData h2)
    {
        //pistoolin scale vaikuttaa k�den kokoon, siksi t�m� ei toimi oikein
        //startingHandPosition = h1.root.localPosition;
        //finalHandPosition = h2.root.localPosition;
        startingHandPosition = new Vector3(h1.root.localPosition.x / h1.root.localScale.x,
                                            h1.root.localPosition.y / h1.root.localScale.y,
                                            h1.root.localPosition.z / h1.root.localScale.z);
        finalHandPosition = new Vector3(h2.root.localPosition.x / h2.root.localScale.x,
                                        h2.root.localPosition.y / h2.root.localScale.y,
                                        h2.root.localPosition.z / h2.root.localScale.z);


        startingHandRotation = h1.root.localRotation;
        finalHandRotation = h2.root.localRotation;

        startingFingerRotations = new Quaternion[h1.fingerBones.Length];
        finalFingerRotations = new Quaternion[h1.fingerBones.Length];

        for(int i = 0; i < h1.fingerBones.Length; i++) {
            startingFingerRotations[i] = h1.fingerBones[i].localRotation;
            finalFingerRotations[i] = h2.fingerBones[i].localRotation;
        }
    }

    public void SetHandData(HandData h, Vector3 newPosition, Quaternion newRotation, Quaternion[] newBonesRotation)
    {
        h.root.localPosition = newPosition;
        h.root.localRotation = newRotation;

        for(int i = 0; i < newBonesRotation.Length; i++) {
            h.fingerBones[i].localRotation = newBonesRotation[i];
        }
    }

    public IEnumerator SetHandDataRoutine(HandData h, Vector3 newPosition, Quaternion newRotation, Quaternion[] newBonesRotation,
                       Vector3 startingPosition, Quaternion startingRotation, Quaternion[] startingBonesRotation)
    {
        float timer = 0;

        while(timer < poseTransitionDuration) {
            Vector3 p = Vector3.Lerp(startingPosition, newPosition, timer / poseTransitionDuration);
            Quaternion r = Quaternion.Lerp(startingRotation, newRotation, timer / poseTransitionDuration);

            h.root.localPosition = p;
            h.root.localRotation = r;

            for(int i = 0; i < newBonesRotation.Length; i++) {
                h.fingerBones[i].localRotation = Quaternion.Lerp(startingBonesRotation[i], newBonesRotation[i], timer / poseTransitionDuration);
            }

            timer += Time.deltaTime;
            yield return null;
        }
    }

#if UNITY_EDITOR

    [MenuItem("Tools/Mirror Selected Right Grab Pose")]
    public static void MirrorRightPose()
    {
        Debug.Log("MirrorRightPose");
        GrabHandPose handPose = Selection.activeGameObject.GetComponent<GrabHandPose>();
        handPose.MirrorPose(handPose.leftHandPose, handPose.rightHandPose);
    }
#endif

    public void MirrorPose(HandData poseToMirror, HandData poseUsedToMirror)
    {
        Vector3 mirroredPosition = poseUsedToMirror.root.localPosition;
        mirroredPosition.x *= -1;

        Quaternion mirroredQuaternion = poseUsedToMirror.root.localRotation;
        mirroredQuaternion.y *= -1;
        mirroredQuaternion.z *= -1;

        poseToMirror.root.localPosition = mirroredPosition;
        poseToMirror.root.localRotation = mirroredQuaternion;

        for(int i = 0; i < poseUsedToMirror.fingerBones.Length; i++) {
            poseToMirror.fingerBones[i].localRotation = poseUsedToMirror.fingerBones[i].localRotation;
        }
    }
}
