using System;
using System.Text;
using AVFoundation;
using Foundation;

namespace SimpleNeurotuner.Services;

public partial class AudioPlayer
{
    NSUrl audioFilePath;
    AVAudioEngine audioEngine;
    AVAudioPlayerNode playerNode;
    public partial void StartPlayer(string pathToAudioFile)
    {

        audioFilePath = NSUrl.FromFilename(pathToAudioFile);

        NSError nSError = new NSError();
        var audioFile = new AVAudioFile(audioFilePath, AVAudioCommonFormat.PCMFloat32, true, out nSError);

        audioEngine = new AVAudioEngine();
        playerNode = new AVAudioPlayerNode();

        audioEngine.AttachNode(playerNode);
        audioEngine.Connect(playerNode, audioEngine.OutputNode, audioFile.ProcessingFormat);

        playerNode.ScheduleFile(audioFile, null, AVAudioPlayerNodeCompletionCallbackType.PlayedBack, null);

        audioEngine.StartAndReturnError(out nSError);
        playerNode.Play();


    }

    public partial void StopPlayer()
    {
        playerNode.Stop();
        audioEngine.Stop();

    }
}


