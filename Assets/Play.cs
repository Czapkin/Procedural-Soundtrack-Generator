using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Play : MonoBehaviour
{
    
    public InputField bpm;
    public InputField skipChance;
    public InputField maxInRow;
    public InputField maxJumpInput;
    public InputField clipName;
    
    public float[] scale = new float[8];
    public float[] usedSounds = new float[8];
    private int[] minorProgression = new int[7];
    public Text pitchText;

    private float trackedTime = 0f;
    private float mainCounter = 0f;
    private bool play = false;
    private bool played = false;
    private int bpmInt;
    private double secondsPerBeat;
    private int sampleNote = 0;
    private int rand;
    private int previousRand;
    private int tickCounter = 0;
    private int replayCounter = 0;
    private int gap;
    private int chanceToSkip = 0;
    private int maxInaRow = 3;
    private int maxJump;
    
    private string soundPath;
    private AudioClip clip;
    private AudioClip generatedClip;
    
    public AudioSource test;
    public AudioSource drums;
    public AudioSource background;
    public AudioSource secondbackground;

    private Button button;
    private TextMeshPro backgroundText;
    
    void Start()
    {
        changeBPM();
        setChanceToSkip();
        setMaxInRow();
        setMaxJump();

        minorProgression[0] = 2;
        minorProgression[1] = 1;
        minorProgression[2] = 2;
        minorProgression[3] = 2;
        minorProgression[4] = 1;
        minorProgression[5] = 2;
        minorProgression[6] = 2;
        
        fillAMinorScale();
        setUsedSounds();

        button = GameObject.FindGameObjectWithTag("Background").GetComponent<Button>();
        backgroundText = GameObject.Find("BackgroundText").GetComponent<TextMeshPro>();
    }
    
    private IEnumerator LoadAudio()
    {
        WWW request = GetAudioFromFile(soundPath);
        yield return request;

        generatedClip = request.GetAudioClip();

        AssignAudioFile();
    }

    private void AssignAudioFile()
    {
        background.clip = generatedClip;
    }

    private WWW GetAudioFromFile(string path)
    {
        string audioToLoad = string.Format(path);
        WWW request = new WWW(audioToLoad);
        return request;
    }

    public void changeClip(int track)
    {
        Debug.Log(track);
        FileSelector.GetFile(GotFile, ".wav");
    }
    
    void GotFile(FileSelector.Status status, string path){
        Debug.Log("File Status : "+status+", Path : "+path);
        soundPath = path;
        backgroundText.text = path;
        StartCoroutine(LoadAudio());
        pitchText.text = soundPath;
    }

    public void fillAMinorScale()
    {
        for (int i = 0; i < 8; i++)
        {
            if (i == 0)
            {
                scale[i] = -3;
            }
            else
            {
                scale[i] = scale[i - 1] + minorProgression[i - 1];
            }
        }
    }

    public void setUsedSounds()
    {
        for (int i = 0; i < 8; i++)
        {
            usedSounds[i] = (float)Math.Pow(2, scale[i]/12f);//(float)Math.Pow(1.0594630943592953, scale[i]);
        }
    }
    
    public void changeBPM()
    {
        bpmInt = int.Parse(bpm.text);
        secondsPerBeat = 60d / bpmInt;
    }

    public void setChanceToSkip()
    {
        chanceToSkip = 100 - (int.Parse(skipChance.text));
    }

    public void setMaxInRow()
    {
        if(int.Parse(maxInRow.text) > 0)
            maxInaRow = int.Parse(maxInRow.text);
    }

    public void setMaxJump()
    {
        if(int.Parse(maxJumpInput.text) > 0)
            maxJump = int.Parse(maxJumpInput.text);
    }
    
    public void playAll()
    {
        drums.time = 4.15f;
        play = true;
        mainCounter = 0f;
        
        drums.Play();
        background.Play();
    }

    public void playBackground()
    {
        drums.time = 4.15f;
        mainCounter = 0f;
        drums.Play();
        background.Play();
    }

    public void playGenerated()
    {
        play = true;
    }
    
    public void buttonStop()
    {
        play = false;
        drums.Stop();
        background.Stop();
    }

    public void stopBackground()
    {
        drums.Stop();
        background.Stop();
    }

    public void stopGenerated()
    {
        play = false;
    }
    
    
    /*IEnumerator SoundOut()
    {
        while (true)
        {
            test.pitch = table2[Random.Range(0, 7)];
            test.Play();
            yield return new WaitForSeconds(0.375f);
            
        }
    }*/
    

    public void normalPitch()
    {
        test.pitch = 1;
        test.Play();
    }
    
    public void pitch1()
    {
        test.pitch = (float)Math.Pow(2, 1f / 12f);//(float)Math.Pow(1.0594630943592953, 1);
        test.Play();
    }

    public void pitch2()
    {
        test.pitch = (float)Math.Pow(2, 2f / 12f);//(float)Math.Pow(1.0594630943592953, 2);
        test.Play();
    }
    
    public void pitch3()
    {
        test.pitch = (float)Math.Pow(2, 3f / 12f);//(float)Math.Pow(1.0594630943592953, 3);
        test.Play();
    }
    
    public void pitch4()
    {
        test.pitch = (float)Math.Pow(2, 4f / 12f);//(float)Math.Pow(1.0594630943592953, 4);
        test.Play();
    }
    
    public void pitch5()
    {
        test.pitch = (float)Math.Pow(2, 5f / 12f);//(float)Math.Pow(1.0594630943592953, 5);
        test.Play();
    }
    public void pitch6()
    {
        test.pitch = (float)Math.Pow(2, 6f / 12f);//(float)Math.Pow(1.0594630943592953, 6);
        test.Play();
    }
    public void pitch7()
    {
        test.pitch = (float)Math.Pow(2, 7f / 12f);//(float)Math.Pow(1.0594630943592953, 7);
        test.Play();
    }
    public void pitch8()
    {
        test.pitch = (float)Math.Pow(2, 8f / 12f);//(float)Math.Pow(1.0594630943592953, 8);
        test.Play();
    }
    public void pitch9()
    {
        test.pitch = (float)Math.Pow(2, 9f / 12f);//(float)Math.Pow(1.0594630943592953, 9);
        test.Play();
    }
    public void pitch10()
    {
        test.pitch = (float)Math.Pow(2, 10f / 12f);//(float)Math.Pow(1.0594630943592953, 10);
        test.Play();
    }
    public void pitch11()
    {
        test.pitch = (float)Math.Pow(2, 11f / 12f);//(float)Math.Pow(1.0594630943592953, 11);
        test.Play();
    }
    

    // Update is called once per frame
    void Update()
    {

        if (play)
        {

            mainCounter += Time.deltaTime;
           
           if (mainCounter % secondsPerBeat >= 0 && mainCounter % secondsPerBeat < 0.1f && played == false && play)
           {
               previousRand = rand;
               
               rand = Random.Range(0, 8);
               
               
               while (tickCounter > 1 && (previousRand - rand > maxJump || previousRand - rand < (maxJump*-1)) || replayCounter >= maxInaRow)
               {
                   rand = Random.Range(0, 8);
               }
                
               test.pitch = usedSounds[rand];

               if (previousRand == rand)
                   replayCounter++;
               else
                   replayCounter = 0;
               
               gap = Random.Range(1, 100);

               if (gap < chanceToSkip)
               {
                   test.Play();
                   pitchText.text = rand.ToString();
                   
               }
               played = true;
               tickCounter++;
               Invoke("resetPlayed", 0.2f);
           }
        }

    }

    public void resetPlayed()
    {
        played = false;
    }
}
