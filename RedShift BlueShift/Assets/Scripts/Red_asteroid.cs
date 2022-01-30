using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Red_asteroid : MonoBehaviour
{
    private Vector3 axisOfRotation;
    private Vector3 originalSize;
    private Rigidbody rb;
    private ParticleSystem particles;
    private AudioSource audioSource;

    [SerializeField]
    private Vector3 velocity = new Vector3();

    private PlayerController pc;
    [SerializeField]
    private Material asteroid_mat;

    // Start is called before the first frame update
    void Start()
    {
        axisOfRotation = Random.insideUnitSphere * 2;
        pc = GameObject.Find("Player").GetComponent<PlayerController>();

        originalSize = transform.localScale;
        rb = GetComponent<Rigidbody>();
        particles = GetComponentInChildren<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();
        velocity = new Vector3(velocity.x * Random.Range(0.6f, 1.4f), velocity.y * Random.Range(0.6f, 1.4f), 0f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(axisOfRotation);
        transform.position += velocity;

        if (pc.PlayerSpeed == SpeedClass.Slow)
        {
            asteroid_mat.color = new Color(asteroid_mat.color.r, asteroid_mat.color.g, asteroid_mat.color.b, Mathf.Clamp(asteroid_mat.color.a - 0.003f * Time.deltaTime, 0.05f, 1f));
        }
        else
        {
            asteroid_mat.color = new Color(asteroid_mat.color.r, asteroid_mat.color.g, asteroid_mat.color.b, Mathf.Clamp(asteroid_mat.color.a + 0.003f * Time.deltaTime, 0.05f, 1f));
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