using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioSource[] sfx;
    [SerializeField] public AudioSource[] bgm;
    private int bgmIndex;
    public int BGMCount => bgm.Length;
    private void Awake() {
        Instance = this;
    }

    private void Update() {
        
    }

    public void PlaySFX(int i) {
        if(i < sfx.Length) {
            sfx[i].pitch = Random.Range(.85f, 1);
            sfx[i].Play();
        }
    }


    public void StopSFX(int i) {
        sfx[i].Stop();
    }

    public void PlayBGM(int i) {
        StopBGM();
        bgm[i].loop = true;
        bgm[i].Play();
    }

    public void PlayRandomBGM() {
        bgmIndex = Random.Range(0, bgm.Length);
        PlayBGM(bgmIndex);
    }

    public void StopBGM() {
        for (int j = 0; j < bgm.Length; j++) {
            bgm[j].Stop();
        }
    }

}
