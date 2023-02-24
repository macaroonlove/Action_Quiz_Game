using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    GameObject BackgroundMusic;
    public AudioSource backmusic;

    void Awake()
    {
        //BackgroundMusic = GameObject.Find("Background");//""안에 음악제목 기입
        
        //if (backmusic.isPlaying) return; //배경음악이 재생되고 있다면 패스
        //else
        //{
        //    backmusic.Play();
        //    //DontDestroyOnLoad(BackgroundMusic); //배경음악 계속 재생하게(이후 버튼매니저에서 조작)
        //}
    }
}