using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PistolSounds : MonoBehaviour
{
    [SerializeField] AudioSource pistolAudio;
    [SerializeField] AudioClip shot;
    [SerializeField] AudioClip emptyShot;
    [SerializeField] AudioClip hammerMoving;
    [SerializeField] AudioClip sliderGoingBack;
    [SerializeField] AudioClip sliderGoingForward;
    
    float defaultPitch;
    public BulletTime bulletTime;

    void Start()
    {
        bulletTime = Camera.main.GetComponent<BulletTime>();
    }

    public void PlayShotSound()
    {
        pistolAudio.pitch = RandomPitchInRange(0.1f);
        pistolAudio.PlayOneShot(shot);
    }

    public void PlayEmptySound()
    {
        pistolAudio.pitch = RandomPitchInRange(0.1f);
        pistolAudio.PlayOneShot(emptyShot);
    }

    float RandomPitchInRange(float range)
    {
        if (bulletTime.bulletTimeActive) {
            defaultPitch = 0.2f;
        } else {
            defaultPitch = 1f;
        }

        return Random.Range(defaultPitch - range, defaultPitch + range);
    }
}

