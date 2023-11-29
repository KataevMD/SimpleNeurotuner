using System;
using AVFoundation;
using Foundation;
using System.Diagnostics;
using Microsoft.Maui.Storage;
using CoreAudioKit;
using AudioToolbox;



namespace SimpleNeurotuner.Services;

public partial class AudioPlayer
{
    private NSUrl audioFilePath;
    private AVAudioEngine audioEngine;
    private AVAudioPlayerNode playerNode;
    
    public partial void StartPlayer(string pathToAudioFile)
    {
        var audioSession = AVAudioSession.SharedInstance();
        var err = audioSession.SetCategory(AVAudioSessionCategory.PlayAndRecord);
        err = audioSession.SetActive(true);

        audioEngine = new AVAudioEngine();
        playerNode = new AVAudioPlayerNode();

        audioFilePath = NSUrl.FromFilename(pathToAudioFile);

        NSError nSError = new NSError();
        var audioFile = new AVAudioFile(audioFilePath, AVAudioCommonFormat.PCMFloat32, false, out nSError);
        var audioFormat = audioFile.ProcessingFormat;
        var audioFrameCount = ((uint)audioFile.Length);

        var audioFileBuffer = new AVAudioPcmBuffer(audioFormat, audioFrameCount);

        audioFile.ReadIntoBuffer(audioFileBuffer, out nSError);

        //var mainMixer = audioEngine.MainMixerNode;

        var changePitchEffect = new AVAudioUnitTimePitch();
        changePitchEffect.Pitch = 200;

        

        audioEngine.AttachNode(playerNode);
        audioEngine.AttachNode(changePitchEffect);

        audioEngine.Connect(playerNode, changePitchEffect, audioFileBuffer.Format);
        audioEngine.Connect(changePitchEffect, audioEngine.OutputNode, audioFileBuffer.Format);

        audioEngine.StartAndReturnError(out nSError);

        

        
        playerNode.Play();

        playerNode.ScheduleBuffer(audioFileBuffer, AVAudioPlayerNodeCompletionCallbackType.PlayedBack, null);

    }

    public partial void StopPlayer()
    {
        playerNode.Stop();
        audioEngine.Stop();
        
    }    
}


