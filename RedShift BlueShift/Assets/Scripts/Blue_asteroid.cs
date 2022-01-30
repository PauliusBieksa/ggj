using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Blue_asteroid : MonoBehaviour
{
    private Vector3 axisOfRotation;
    private Vector3 originalSize;
    private Rigidbody rb;
    private ParticleSystem particles;
    private AudioSource audioSource;

    private PlayerController pc;
    [SerializeField]
    private Material asteroid_mat;

    private Vector3 previousPosition;
    private Transform pos;

    // Start is called before the first frame update
    void Start()
    {
        pos = gameObject.transform;
        axisOfRotation = Random.insideUnitSphere * 2;
        pc = GameObject.Find("Player").GetComponent<PlayerController>();
        //asteroid_mat = GetComponent<Material>();

        originalSize = transform.localScale;
        rb = GetComponent<Rigidbody>();
        particles = GetComponentInChildren<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

        if(Mathf.Abs(previousPosition.z - pos.position.z) > 20)
        {
            Reset();
        }
        transform.Rotate(axisOfRotation);

        if (pc.PlayerSpeed == SpeedClass.Fast)
        {
            asteroid_mat.color = new Color(asteroid_mat.color.r, asteroid_mat.color.g, asteroid_mat.color.b, Mathf.Clamp(asteroid_mat.color.a - 0.001f * Time.deltaTime, 0.05f, 1f));
        }
        else
        {
            asteroid_mat.color = new Color(asteroid_mat.color.r, asteroid_mat.color.g, asteroid_mat.color.b, Mathf.Clamp(asteroid_mat.color.a + 0.001f * Time.deltaTime, 0.05f, 1f));
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.ToLower().Contains("asteroid"))
            return;

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