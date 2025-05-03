using System.Collections;
using UnityEngine;

public class PistolBulletCasing : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] AudioSource casingAudio;
    [SerializeField] AudioClip[] casingClip = new AudioClip[3];

    float defaultPitch;
    float defaultMass;
    BulletTime bulletTime;

    void Start()
    {
        bulletTime = Camera.main.GetComponent<BulletTime>();        

        Destroy(gameObject, 10f);
    }

    void OnCollisionEnter(Collision collision)
    {
        HandleBulletTime();

        casingAudio.pitch = RandomPitchInRange(0.1f);
        casingAudio.PlayOneShot(casingClip[Random.Range(0,2)]);
    }

    float RandomPitchInRange(float range)
    {
        if (bulletTime.bulletTimeActive) {
            defaultPitch = 0.5f;            
        } else {
            defaultPitch = 1f;            
        }
        return Random.Range(defaultPitch - range, defaultPitch + range);
    }

    void HandleBulletTime()
    {
        if (bulletTime.bulletTimeActive) {
            //rb.useGravity = false;
            //JustWaitAndDoAfter(2f);
            defaultMass = 0.1f;
        } else {
            //rb.useGravity = true;
            defaultMass = 1f;
        }
        rb.mass = defaultMass;
    }

    IEnumerator JustWaitAndDoAfter(float delay)
    {
        yield return new WaitForSeconds(delay);
        rb.useGravity = true;               //joo t‰m‰ ei toimi, se ei laita painovoimaa p‰‰lle
    }
}