using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    GameObject BackgroundMusic;
    public AudioSource backmusic;

    void Awake()
    {
        //BackgroundMusic = GameObject.Find("Background");//""�ȿ� �������� ����
        
        //if (backmusic.isPlaying) return; //��������� ����ǰ� �ִٸ� �н�
        //else
        //{
        //    backmusic.Play();
        //    //DontDestroyOnLoad(BackgroundMusic); //������� ��� ����ϰ�(���� ��ư�Ŵ������� ����)
        //}
    }
}