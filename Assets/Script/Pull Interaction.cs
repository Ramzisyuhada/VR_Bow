using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System;
public class PullInteraction : XRBaseInteractable

{
    public static event Action <float> PullAction;
    public Transform start, end;
    public GameObject notch;

    public float pullAmount {  get; private set; }
    private LineRenderer lineRenderer;
    private IXRSelectInteractor pullingInteractor = null;

    protected override void Awake()
    {
        base.Awake();
        lineRenderer = GetComponent<LineRenderer>();
    }

    public void SetPullInteractor(SelectEnterEventArgs args)
    {
        pullingInteractor = args.interactorObject;
    }
    public void Release()
    {
        PullAction?.Invoke(pullAmount);
        pullingInteractor = null;
        pullAmount = 0f;
        notch.transform.localPosition = new Vector3(notch.transform.localPosition.x, notch.transform.localPosition.y, 0f);
        UpdateString();
    }
  
    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        base.ProcessInteractable(updatePhase);
        if(updatePhase == XRInteractionUpdateOrder.UpdatePhase.Dynamic)
        {
            if (isSelected)
            {
                Vector3 pullPosisi = pullingInteractor.transform.position;
                pullAmount = CalculatePull(pullPosisi);
                UpdateString();
            }
        }
    }
    private float CalculatePull(Vector3 pullPosisition)
    {
        Vector3 pullDirection = pullPosisition - start.position;
        Vector3 target = end.position - start.position;
        float maxleng = target.magnitude;
        target.Normalize();
        float pullValue = Vector3.Dot(pullDirection, target) / maxleng;
        return Mathf.Clamp(pullValue, 0, 1);
    }
    private void UpdateString()
    {
        Vector3 linePosisition = Vector3.forward * Mathf.Lerp(start.transform.localPosition.z, end.transform.localPosition.z, pullAmount);
        notch.transform.localPosition = new Vector3(notch.transform.localPosition.x, notch.transform.localPosition.y, linePosisition.z + 2f);
        lineRenderer.SetPosition(1,linePosisition);
    }
   
}
