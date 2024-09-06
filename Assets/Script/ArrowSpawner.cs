using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ArrowSpawner : MonoBehaviour
{

    public GameObject arrow;
    public GameObject notch;

    private XRGrabInteractable _bow;
    private bool _arrowNotched = false;
    private GameObject _currentArrow;
    void Start()
    {
        _bow = GetComponent<XRGrabInteractable>();
        PullInteraction.PullAction += NotchEmpty;
    }

    private void OnDestroy()
    {
        PullInteraction.PullAction -= NotchEmpty;

    }

    // Update is called once per frame
    void Update()
    {
        if (_bow.isSelected && !_arrowNotched)
        {
            Debug.Log("test");
            _arrowNotched = true;
            StartCoroutine("DelayedSpawn");
        }
        if (!_bow.isSelected) {
            Destroy(_currentArrow);
            NotchEmpty(1f);
        } 
    }

    private void NotchEmpty(float value)
    {
        _currentArrow = null;
        _arrowNotched = false;

    }

    IEnumerator DelayedSpawn()
    {
        yield return new WaitForSeconds(1f);
        _currentArrow = Instantiate(arrow, notch.transform); 
    }
}
