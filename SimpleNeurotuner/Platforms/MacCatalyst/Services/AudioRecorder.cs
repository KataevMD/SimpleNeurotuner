using System;
using AVFoundation;
using Foundation;
using System.Diagnostics;
using Microsoft.Maui.Storage;
using CoreAudioKit;

namespace SimpleNeurotuner.Services;

public partial class AudioRecorder
{
    enum RecordingState
    {
    recording, paused, stopped
    }
    // declarations
    private AVAudioRecorder recorder;
    private NSDictionary settings;
    private NSUrl audioFilePath;
    private Services.AudioFiles audioFiles;
    string folderRecordPath = "Contents/Record";

    private AVAudioEngine audioEngine = new AVAudioEngine();
    private AVAudioMixerNode mixerNode;
    private RecordingState state = RecordingState.stopped;

    public partial void StartRecord(string audioFileName)
    {
       
        if (!Directory.Exists(folderRecordPath))
        {
            Directory.CreateDirectory(folderRecordPath);
        }

        // Declare string for application temp path and tack on the file extension
        //string fileName = string.Format("Myfile{0}.wav", DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));

        string fileName = audioFileName + ".wav";
        //string tempRecording = Path.Combine(Path.GetTempPath(), fileName);

        audioFiles = new Services.AudioFiles();
        string newPathToWriteAudioFile = Path.Combine(folderRecordPath, fileName);

        Console.WriteLine(newPathToWriteAudioFile);
        audioFilePath = NSUrl.FromFilename(newPathToWriteAudioFile);

        PrepareAudioEngine();
        NSError nSError = new NSError();

        //var tapNode = (AVAudioNode)mixerNode;
        //var format = tapNode.GetBusOutputFormat(0);

        var inputNode = audioEngine.InputNode;
        
        var audioFile = new AVAudioFile(audioFilePath, settings: inputNode.GetBusInputFormat(0).Settings, AVAudioCommonFormat.PCMFloat32, false, out nSError);


        AVAudioPlayerNode playerNode = new AVAudioPlayerNode();
        var changePitchEffect = new AVAudioUnitTimePitch();
       
        changePitchEffect.Pitch = 200;
        //changePitchEffect.Overlap = 7;

        audioEngine.AttachNode(playerNode);
        audioEngine.AttachNode(changePitchEffect);


        audioEngine.Connect(playerNode, changePitchEffect, null);
        audioEngine.Connect(changePitchEffect, audioEngine.OutputNode, null);

        //var buffer = new AVAudioPcmBuffer(format, 1);

        audioEngine.InputNode.InstallTapOnBus(0, 4096, inputNode.GetBusInputFormat(0), tapBlock: (AVAudioPcmBuffer buffer, AVAudioTime when) =>


        playerNode.ScheduleBuffer(buffer, AVAudioPlayerNodeCompletionCallbackType.PlayedBack, null)



        );
        audioEngine.StartAndReturnError(out nSError);
        state = RecordingState.recording;

        playerNode.Play();
        // recorder.Record();

    }

    public partial void StopRecord()
    {
        // recorder.Stop();
        audioEngine.InputNode.RemoveTapOnBus(0);
        audioEngine.Stop();
       
        state = RecordingState.stopped;
    }

    bool PrepareAudioEngine()
    {
        mixerNode = new AVAudioMixerNode();
        // You must initialize an audio session before trying to record
        var audioSession = AVAudioSession.SharedInstance();
        var err = audioSession.SetCategory(AVAudioSessionCategory.PlayAndRecord);

        var f = audioSession.AvailableInputs;
        for (int i = 0; i < f.Length; i++)
        {
            Console.WriteLine(f[i].ToString()+"\n");
        }
        

        var sampleRate = audioSession.SampleRate;
        if (err != null)
        {
            Console.WriteLine("audioSession: {0}", err);
            return false;
        }
        err = audioSession.SetActive(true);
        if (err != null)
        {
            Console.WriteLine("audioSession: {0}", err);
            return false;
        }

        mixerNode.Volume = 0;
        //audioEngine.AttachNode(mixerNode);

        var inputNode = audioEngine.InputNode;
        var inputFormat = inputNode.GetBusInputFormat(0);

       // audioEngine.Connect(inputNode, mixerNode, inputFormat);

        var mainMixerNode = audioEngine.MainMixerNode;

        //var mixerFormat = new AVAudioFormat(AVAudioCommonFormat.PCMFloat32, inputFormat.SampleRate, 1, interleaved: false);

        audioEngine.Connect(inputNode, mainMixerNode, inputFormat);
        audioEngine.Connect(mainMixerNode, audioEngine.OutputNode, audioEngine.OutputNode.GetBusOutputFormat(0));
        audioEngine.Prepare();

        return true;
    }



    //bool PrepareAudioRecording(string audioFileName)
    //{
    //    // You must initialize an audio session before trying to record
    //    var audioSession = AVAudioSession.SharedInstance();
    //    var err = audioSession.SetCategory(AVAudioSessionCategory.Record);
    //    var sampleRate = audioSession.SampleRate;
    //    if (err != null)
    //    {
    //        Console.WriteLine("audioSession: {0}", err);
    //        return false;
    //    }
    //    err = audioSession.SetActive(true);
    //    if (err != null)
    //    {
    //        Console.WriteLine("audioSession: {0}", err);
    //        return false;
    //    }

    //    // Declare string for application temp path and tack on the file extension
    //    //string fileName = string.Format("Myfile{0}.wav", DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));

    //    string fileName = audioFileName+".wav";
    //    //string tempRecording = Path.Combine(Path.GetTempPath(), fileName);

    //    audioFiles = new Services.AudioFiles();
    //    string newPathToWriteAudioFile = Path.Combine(folderRecordPath, fileName);

    //    Console.WriteLine(newPathToWriteAudioFile);
    //    audioFilePath = NSUrl.FromFilename(newPathToWriteAudioFile);

    //    //set up the NSObject Array of values that will be combined with the keys to make the NSDictionary
    //    NSObject[] values = new NSObject[]
    //    {
    //            NSNumber.FromDouble(sampleRate),
    //            NSNumber.FromInt32((int)AudioToolbox.AudioFormatType.ALaw),
    //            NSNumber.FromInt32(1),
    //            NSNumber.FromInt32((int)AVAudioQuality.Min),
    //            NSNumber.FromInt32(12800),
    //            NSNumber.FromInt32(16)
    //    };
    //    //Set up the NSObject Array of keys that will be combined with the values to make the NSDictionary
    //    NSObject[] keys = new NSObject[]
    //    {
    //            AVAudioSettings.AVSampleRateKey,
    //            AVAudioSettings.AVFormatIDKey,
    //            AVAudioSettings.AVNumberOfChannelsKey,
    //            AVAudioSettings.AVEncoderAudioQualityKey,
    //            AVAudioSettings.AVEncoderBitRateKey,
    //            AVAudioSettings.AVLinearPCMBitDepthKey

    //    };
    //    //Set Settings with the Values and Keys to create the NSDictionary
    //    settings = NSDictionary.FromObjectsAndKeys(values, keys);

    //    //Set recorder parameters
    //    NSError error;
    //    recorder = AVAudioRecorder.Create(this.audioFilePath, new AudioSettings(settings), out error);
    //    if ((recorder == null) || (error != null))
    //    {
    //        Console.WriteLine(error);
    //        return false;
    //    }

    //    //Set Recorder to Prepare To Record
    //    if (!recorder.PrepareToRecord())
    //    {
    //        recorder.Dispose();
    //        recorder = null;
    //        return false;
    //    }

    //    recorder.FinishedRecording += delegate (object sender, AVStatusEventArgs e) {
    //        recorder.Dispose();
    //        recorder = null;
    //        Console.WriteLine("Done Recording (status: {0})", e.Status);
    //    };

    //    return true;
    //}


}


