using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class audioMgr : MonoBehaviour
{
    public static audioMgr instance = null;
    public int free;
    public int playing;

    private List<AudioSource> playingAudioList = new List<AudioSource>();
    private List<AudioSource> freeAudioList = new List<AudioSource>();

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlaySound(AudioClip clip)
    {
        AudioSource audio;
        if (freeAudioList.Count != 0)
        {
            audio = freeAudioList[0];
            freeAudioList.RemoveAt(0);
        }
        else
        {
            audio = gameObject.AddComponent<AudioSource>();

        }
        playingAudioList.Add(audio);
        audio.clip = clip;
        audio.clip.LoadAudioData();
        audio.loop = false;
        audio.Play();
        StartCoroutine(WaitPlayEnd(audio));
    }

    IEnumerator WaitPlayEnd(AudioSource audio)
    {
        yield return new WaitUntil(() => {
            return !audio.isPlaying; 
        });

        freeAudioList.Add(audio);
        playingAudioList.Remove(audio);
    }
}