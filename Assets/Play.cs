using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using TMPro;

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
    
    public float[] scale = new float[8];
    public float[] usedSounds = new float[8];
    private int[] minorProgression = new int[7];
    private int[] majorProgression = new int[7];
    public Text pitchText;

    private float trackedTime = 0f;
    private float mainCounter = 0f;
    private bool play = false;
    private bool played = false;
    private int bpmInt;
    private double secondsPerBeat;
    private int rand;
    private int previousRand;
    private int tickCounter = 0;
    private int replayCounter = 0;
    private int gap;
    private int chanceToSkip = 0;
    private int maxInaRow = 3;
    private int maxJump;
    private int trackNumber = -1;
    private bool modalOpened = false;
    private int sampleNote = 0;
    private int trackNote = 0;
    private bool isMajor = true;
    private int differenceBetween;
    
    private string soundPath;
    private AudioClip clip;
    private AudioClip generatedClip;
    
    public AudioSource generated;
    public AudioSource drums;
    public AudioSource background;
    public AudioSource custom;

    public Button setBackground;
    public Slider backgroundVolumeSlider;
    public Toggle muteBackground;
    public Toggle loopBackground;
    public InputField backgroundStartOffset;
    
    public Button setDrums;
    public Slider drumsVolumeSlider;
    public Toggle muteDrums;
    public Toggle loopDrums;
    public InputField drumsStartOffset;
    
    public Button setCustom;
    public Slider customVolumeSlider;
    public Toggle muteCustom;
    public Toggle loopCustom;
    public InputField customStartOffset;
    
    public Button setGenerated;
    public Slider generatedVolumeSlider;
    public Toggle muteGenerated;
    public Toggle loopGenerated;
    public InputField generatedStartOffset;

    public Dropdown sampleNoteDropdown;
    public Dropdown trackNoteDropdown;
    public Dropdown trackScaleDropdown;

    void Start()
    {

        changeBPM();
        setChanceToSkip();
        setMaxInRow();
        setMaxJump();
        setSampleNote();
        setTrackNote();

        minorProgression[0] = 2;
        minorProgression[1] = 1;
        minorProgression[2] = 2;
        minorProgression[3] = 2;
        minorProgression[4] = 1;
        minorProgression[5] = 2;
        minorProgression[6] = 2;

        majorProgression[0] = 2;
        majorProgression[1] = 2;
        majorProgression[2] = 1;
        majorProgression[3] = 2;
        majorProgression[4] = 2;
        majorProgression[5] = 2;
        majorProgression[6] = 1;
        
        fillScale();
        setUsedSounds();

        
    }

    public void setSampleNote()
    {
        sampleNote = sampleNoteDropdown.value;
        findDifference();
        fillScale();
        setUsedSounds();
    }
    
    public void setTrackNote()
    {
        trackNote = trackNoteDropdown.value;
        findDifference();
        fillScale();
        setUsedSounds();
    }
    
    public void setTrackScale()
    {
        if (trackScaleDropdown.value == 0)
            isMajor = true;
        else
            isMajor = false;
        
        fillScale();
        setUsedSounds();
    }

    private void findDifference()
    {
        int seekRight;
        int seekLeft;
        if (trackNote < sampleNote)
        {
            seekRight = sampleNote - trackNote;
            seekLeft = trackNote + Math.Abs(sampleNote - 12);

            if (Math.Abs(seekRight) <= Math.Abs(seekLeft))
            {
                differenceBetween = seekRight * -1;
            } else
            {
                differenceBetween =  seekLeft;
            }
                
        }
        else
        {
            seekRight = trackNote - sampleNote;
            seekLeft = sampleNote + Math.Abs(trackNote - 12);

            if (Math.Abs(seekRight) <= Math.Abs(seekLeft))
            {
                differenceBetween = seekRight;
            } else
            {
                differenceBetween =  seekLeft * -1;
            }
        }
    }

    public void changeVolume(int track)
    {
        switch (track)
        {
            case 0:
                background.volume = backgroundVolumeSlider.value;
                break;
            case 1:
                drums.volume = drumsVolumeSlider.value;
                break;
            case 2:
                custom.volume = customVolumeSlider.value;
                break;
            case 3:
                generated.volume = generatedVolumeSlider.value;
                break;
            
        }
    }

    public void muteTrack(int track)
    {
        switch (track)
        {
            case 0:
                background.mute = muteBackground.isOn;
                break;
            case 1:
                drums.mute = muteDrums.isOn;
                break;
            case 2:
                custom.mute = muteCustom.isOn;
                break;
            case 3:
                generated.mute = muteGenerated.isOn;
                break;
        }
    }

    public void loopTrack(int track)
    {
        switch (track)
        {
            case 0:
                background.loop = loopBackground.isOn;
                break;
            case 1:
                drums.loop = loopDrums.isOn;
                break;
            case 2:
                custom.loop = loopCustom.isOn;
                break;
            case 3:
                generated.loop = loopGenerated.isOn;
                break;
        }
    }

    public void changeStartOffset(int track)
    {
        switch (track)
        {
            case 0:
                try
                {
                    backgroundStartOffset.text = float.Parse(backgroundStartOffset.text.Replace(".", ",")).ToString("0.00");
                    background.time = float.Parse(backgroundStartOffset.text);
                }
                catch (FormatException e)
                {
                    backgroundStartOffset.text = "0,00";
                    background.time = float.Parse(backgroundStartOffset.text);
                }
                break;
            case 1:
                try
                {
                    drumsStartOffset.text = float.Parse(drumsStartOffset.text.Replace(".", ",")).ToString("0.00");
                    drums.time = float.Parse(drumsStartOffset.text);
                }
                catch (FormatException e)
                {
                    drumsStartOffset.text = "0,00";
                    drums.time = float.Parse(drumsStartOffset.text);
                }
                break;
            case 2:
                try
                {
                    customStartOffset.text = float.Parse(customStartOffset.text.Replace(".", ",")).ToString("0.00");
                    custom.time = float.Parse(customStartOffset.text);
                }
                catch (FormatException e)
                {
                    customStartOffset.text = "0,00";
                    custom.time = float.Parse(customStartOffset.text);
                }
                break;
            case 3:
                try
                {
                    generatedStartOffset.text = float.Parse(generatedStartOffset.text.Replace(".", ",")).ToString("0.00");
                    generated.time = float.Parse(generatedStartOffset.text);
                }
                catch (FormatException e)
                {
                    generatedStartOffset.text = "0,00";
                    generated.time = float.Parse(generatedStartOffset.text);
                }
                break;
        }
    }

    public void resetSettings(int track)
    {
        switch (track)
        {
            case 0:
                backgroundStartOffset.text = "0,00";
                background.time = float.Parse(backgroundStartOffset.text);
                background.loop = true;
                loopBackground.isOn = true;
                background.mute = false;
                muteBackground.isOn = false;
                background.volume = 0.5f;
                backgroundVolumeSlider.value = 0.5f;
                break;

            case 1:
                drumsStartOffset.text = "0,00";
                drums.time = float.Parse(backgroundStartOffset.text);
                drums.loop = true;
                loopDrums.isOn = true;
                drums.mute = false;
                muteDrums.isOn = false;
                drums.volume = 0.5f;
                drumsVolumeSlider.value = 0.5f;
                break;
            case 2:
                customStartOffset.text = "0,00";
                custom.time = float.Parse(backgroundStartOffset.text);
                custom.loop = true;
                loopCustom.isOn = true;
                custom.mute = false;
                muteCustom.isOn = false;
                custom.volume = 0.5f;
                customVolumeSlider.value = 0.5f;
                break;
            case 3:
                generatedStartOffset.text = "0,00";
                generated.time = float.Parse(backgroundStartOffset.text);
                generated.loop = true;
                loopGenerated.isOn = true;
                generated.mute = false;
                muteGenerated.isOn = false;
                generated.volume = 0.5f;
                generatedVolumeSlider.value = 0.5f;
                break;
        }
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
        switch (trackNumber)
        {
            case 0:
                background.clip = generatedClip;
                setBackground.GetComponentInChildren<Text>().text = Path.GetFileName(soundPath);
                break;
            case 1:
                drums.clip = generatedClip;
                setDrums.GetComponentInChildren<Text>().text = Path.GetFileName(soundPath);
                break;
            case 2:
                custom.clip = generatedClip;
                setCustom.GetComponentInChildren<Text>().text = Path.GetFileName(soundPath);
                break;
            case 3:
                generated.clip = generatedClip;
                setGenerated.GetComponentInChildren<Text>().text = Path.GetFileName(soundPath);
                break;
        }
    }

    private WWW GetAudioFromFile(string path)
    {
        string audioToLoad = string.Format(path);
        WWW request = new WWW(audioToLoad);
        return request;
    }

    public void setClip(int track)
    {
        if (!modalOpened)
        {
            FileSelector.GetFile(GotFile, ".wav");
            trackNumber = track;
            modalOpened = true;
        }
    }
    
    void GotFile(FileSelector.Status status, string path){
        Debug.Log("File Status : "+status+", Path : "+path);
        if(status.ToString() == "Successful")
        {
            resetSettings(trackNumber);
            soundPath = path;
            StartCoroutine(LoadAudio());
        }
        modalOpened = false;
    }

    public void fillScale()
    {
        if (isMajor)
        {
            for (int i = 0; i < 8; i++)
            {
                if (i == 0)
                {
                    scale[i] = differenceBetween;
                }
                else
                {
                    scale[i] = scale[i - 1] + majorProgression[i - 1];
                }
            }
        }
        else
        {
            for (int i = 0; i < 8; i++)
            {
                if (i == 0)
                {
                    scale[i] = differenceBetween;
                }
                else
                {
                    scale[i] = scale[i - 1] + minorProgression[i - 1];
                }
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
        generated.pitch = 1;
        generated.Play();
    }
    
    public void pitch1()
    {
        generated.pitch = (float)Math.Pow(2, 1f / 12f);//(float)Math.Pow(1.0594630943592953, 1);
        generated.Play();
    }

    public void pitch2()
    {
        generated.pitch = (float)Math.Pow(2, 2f / 12f);//(float)Math.Pow(1.0594630943592953, 2);
        generated.Play();
    }
    
    public void pitch3()
    {
        generated.pitch = (float)Math.Pow(2, 3f / 12f);//(float)Math.Pow(1.0594630943592953, 3);
        generated.Play();
    }
    
    public void pitch4()
    {
        generated.pitch = (float)Math.Pow(2, 4f / 12f);//(float)Math.Pow(1.0594630943592953, 4);
        generated.Play();
    }
    
    public void pitch5()
    {
        generated.pitch = (float)Math.Pow(2, 5f / 12f);//(float)Math.Pow(1.0594630943592953, 5);
        generated.Play();
    }
    public void pitch6()
    {
        generated.pitch = (float)Math.Pow(2, 6f / 12f);//(float)Math.Pow(1.0594630943592953, 6);
        generated.Play();
    }
    public void pitch7()
    {
        generated.pitch = (float)Math.Pow(2, 7f / 12f);//(float)Math.Pow(1.0594630943592953, 7);
        generated.Play();
    }
    public void pitch8()
    {
        generated.pitch = (float)Math.Pow(2, 8f / 12f);//(float)Math.Pow(1.0594630943592953, 8);
        generated.Play();
    }
    public void pitch9()
    {
        generated.pitch = (float)Math.Pow(2, 9f / 12f);//(float)Math.Pow(1.0594630943592953, 9);
        generated.Play();
    }
    public void pitch10()
    {
        generated.pitch = (float)Math.Pow(2, 10f / 12f);//(float)Math.Pow(1.0594630943592953, 10);
        generated.Play();
    }
    public void pitch11()
    {
        generated.pitch = (float)Math.Pow(2, 11f / 12f);//(float)Math.Pow(1.0594630943592953, 11);
        generated.Play();
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
                
               
               
               gap = Random.Range(1, 100);

               if (gap < chanceToSkip)
               {
                   generated.pitch = usedSounds[rand];

                   if (previousRand == rand)
                       replayCounter++;
                   else
                       replayCounter = 0;
                   
                   generated.Play();
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
