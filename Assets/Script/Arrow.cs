using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float speed = 10f;
    public Transform tip;

    private Rigidbody rb;
    private bool _inAir = false;    
    private Vector3 _LastPosition = Vector3.zero;

    public ParticleSystem _particle;
    public TrailRenderer _trail;
     
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
     /*   _particle = GetComponent<ParticleSystem>();
        _trail = GetComponent<TrailRenderer>();*/
        PullInteraction.PullAction += Release;
        Stop();

    }
    private void OnDestroy()
    {
        PullInteraction.PullAction -= Release;

    }
    private void Release(float value)
    {
        PullInteraction.PullAction -= Release;
        gameObject.transform.parent = null;
        _inAir = true;
        SetPhysics(true);
        Vector3 force = transform.forward * value * speed;
        rb.AddForce(force,ForceMode.Impulse);
        StartCoroutine(Rotasidenganvelocity());

        _particle.Play();
        _trail.emitting = true;


    }

    private IEnumerator Rotasidenganvelocity()
    {
        yield return new WaitForFixedUpdate();
        while (_inAir)
        {
            Quaternion rotasi = Quaternion.LookRotation(rb.velocity,transform.up);
            transform.rotation = rotasi;
            yield return null;
        }
    }
    // Update is called once per frame
    private void FixedUpdate()
    {
        if (_inAir)
        {
            CheckCollision();
            _LastPosition = tip.position;
        }
    }

    private void CheckCollision()
    {
        if (Physics.Linecast(_LastPosition,tip.position,out RaycastHit hitInfo))
        {
            if(hitInfo.transform.gameObject.layer != 8)
            {
                if(hitInfo.transform.TryGetComponent(out Rigidbody body))
                {
                    rb.interpolation = RigidbodyInterpolation.None;
                    transform.parent = hitInfo.transform;
                    body.AddForce(rb.velocity, ForceMode.Impulse);
                }
                Stop();
            }
        }
    }
    private void Stop()
    {
        _inAir = false;
        SetPhysics(false);

        _particle.Stop();
        _trail.emitting = false;


    }

    private void SetPhysics(bool usePhysics)
    {
        rb.useGravity = usePhysics;
        rb.isKinematic = !usePhysics;
    }
}
