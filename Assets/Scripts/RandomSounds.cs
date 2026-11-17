using UnityEngine;
using System.Collections;

public class RandomSounds : MonoBehaviour
{
    public PlayerMovement player;
    public AudioSource[] soundSources;
    private float soundChance = 0.4f;
    private int lastSound = -1;

    void Start()
    {
        StartCoroutine(PlayRandomSounds());
    }
    IEnumerator PlayRandomSounds()
    {
    while (true) {
        yield return new WaitForSeconds(15f);
        float sanity = player.sanityValue;
        if (sanity >= 80f)
            continue;
        float chance = Mathf.InverseLerp(80f, 20f, sanity) * soundChance;
        if (Random.value > chance){
        continue;
        }
        if (soundSources.Length == 0){
        continue;
        }

        int soundIndex;
        do
        {
            soundIndex = Random.Range(0, soundSources.Length);
        }
        while (soundIndex == lastSound && soundSources.Length > 1);

        lastSound = soundIndex;
        soundSources[soundIndex].Play();
        }
    }
}
