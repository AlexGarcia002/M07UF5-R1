using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class ActivateGrabRay : MonoBehaviour
{
    [SerializeField] InputActionProperty leftShowLineButton;
    [SerializeField] InputActionProperty rightShowLineButton;

    public GameObject leftGrabRay;
    public GameObject rightGrabRay;

    public XRDirectInteractor leftDirectGrab;
    public XRDirectInteractor rightDirectGrab;

    [SerializeField] XRInteractorLineVisual leftXRLine;
    [SerializeField] XRInteractorLineVisual rightXRLine;

    public Gradient gradientHide;   //viiva piiloon
    public Gradient gradientShow;   //viiva näkyvissä
    public Gradient gradientShowHilight;//viiva highlightattu
    

    void Start()
    {
        leftXRLine = leftGrabRay.GetComponent<XRInteractorLineVisual>();
        rightXRLine = rightGrabRay.GetComponent<XRInteractorLineVisual>();
        SetGrabRayGradients();
    }

    void Update()
    {
        GrabRayVisible();

        leftGrabRay.SetActive(leftDirectGrab.interactablesSelected.Count == 0);
        rightGrabRay.SetActive(rightDirectGrab.interactablesSelected.Count == 0);
    }

    void SetGrabRayGradients()
    {
        rightXRLine.invalidColorGradient = gradientHide;
        rightXRLine.validColorGradient = gradientShowHilight;
        leftXRLine.invalidColorGradient = gradientHide;
        leftXRLine.validColorGradient = gradientShowHilight;
    }

    public void ShowGrabRayWhenInvalid()
    {
        rightXRLine.invalidColorGradient = gradientShow;
        leftXRLine.invalidColorGradient = gradientShow;
    }

    public void HideGrabRayWhenInvalid()
    {
        rightXRLine.invalidColorGradient = gradientHide;
        leftXRLine.invalidColorGradient = gradientHide;
    }

    void GrabRayVisible()
    {
        if (leftShowLineButton.action.ReadValue<float>() == 1f || rightShowLineButton.action.ReadValue<float>() == 1f) {
            ShowGrabRayWhenInvalid();
        } else {
            HideGrabRayWhenInvalid();
        }
    }
}
