using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Asteroid : MonoBehaviour
{
    private Vector3 axisOfRotation;
    private Vector3 originalSize;
    private Rigidbody rb;
    private ParticleSystem particles;
    private AudioSource audioSource;


    // Start is called before the first frame update
    void Start()
    {
        axisOfRotation = Random.insideUnitSphere * 2;

        originalSize = transform.localScale;
        rb = GetComponent<Rigidbody>();
        particles = GetComponentInChildren<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(axisOfRotation);
    }

    private void OnCollisionEnter(Collision collision)
    {
        CrashEffect();
    }

    void CrashEffect()
    {
        StartCoroutine("Shrink");
        rb.isKinematic = true;
        particles.Play();
        audioSource.Play(0);
    }

    public void Reset()
    {
        transform.localScale = originalSize;
        rb.isKinematic = false;
        particles.Stop();
    }

    IEnumerator Shrink()
    {
        Debug.Log("Shrink");
        StartCoroutine("SlowMo");
        Vector3 targetScale = new Vector3(0, 0, 0);
        float elapsedTime = 0;
        float waitTime = 0.5f;


        while (elapsedTime < waitTime)
        {
            transform.localScale = Vector3.Lerp(originalSize, targetScale, (elapsedTime / waitTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        //make sure it getsd there if it's not quite there
        transform.localScale = targetScale;
        yield return null;
    }

    IEnumerator SlowMo()
    {
        Time.timeScale = 0.25f;
        yield return new WaitForSecondsRealtime(2);
        Time.timeScale = 1;
    }
}