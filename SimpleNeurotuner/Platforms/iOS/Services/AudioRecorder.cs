using System;
using AVFoundation;
using Foundation;
using System.Diagnostics;
using Microsoft.Maui.Storage;

namespace SimpleNeurotuner.Services;

public partial class AudioRecorder
{
    // declarations
    AVAudioRecorder recorder;
    NSDictionary settings;
    NSUrl audioFilePath;
    string folderRecordPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

    public partial void StartRecord(string audioFileName)
    {

        PrepareAudioRecording(folderRecordPath, audioFileName);
        recorder.Record();
    }

    public partial void StopRecord()
    {
        recorder.Stop();    
    }

    bool PrepareAudioRecording(string directoryname, string audioFileName)
    {
        // You must initialize an audio session before trying to record
        var audioSession = AVAudioSession.SharedInstance();
        var err = audioSession.SetCategory(AVAudioSessionCategory.PlayAndRecord);
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

        // Declare string for application temp path and tack on the file extension
        //string fileName = string.Format("Myfile{0}.wav", DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
        //string tempRecording = Path.Combine(Path.GetTempPath(), fileName);

        string fileName = audioFileName + ".wav";

        string newPathToWriteAudioFile = Path.Combine(directoryname, fileName);

        Console.WriteLine(newPathToWriteAudioFile);
        audioFilePath = NSUrl.FromFilename(newPathToWriteAudioFile);
        
        //set up the NSObject Array of values that will be combined with the keys to make the NSDictionary
        NSObject[] values = new NSObject[]
        {
                NSNumber.FromFloat(44100.0f),
                NSNumber.FromInt32((int)AudioToolbox.AudioFormatType.ALaw),
                NSNumber.FromInt32(1),
                NSNumber.FromInt32((int)AVAudioQuality.Max)
        };
        //Set up the NSObject Array of keys that will be combined with the values to make the NSDictionary
        NSObject[] keys = new NSObject[]
        {
                AVAudioSettings.AVSampleRateKey,
                AVAudioSettings.AVFormatIDKey,
                AVAudioSettings.AVNumberOfChannelsKey,
                AVAudioSettings.AVEncoderAudioQualityKey
        };
        //Set Settings with the Values and Keys to create the NSDictionary
        settings = NSDictionary.FromObjectsAndKeys(values, keys);

        //Set recorder parameters
        NSError error;
        recorder = AVAudioRecorder.Create(this.audioFilePath, new AudioSettings(settings), out error);
        if ((recorder == null) || (error != null))
        {
            Console.WriteLine(error);
            return false;
        }

        //Set Recorder to Prepare To Record
        if (!recorder.PrepareToRecord())
        {
            recorder.Dispose();
            recorder = null;
            return false;
        }

        recorder.FinishedRecording += delegate (object sender, AVStatusEventArgs e) {
            recorder.Dispose();
            recorder = null;
            Console.WriteLine("Done Recording (status: {0})", e.Status);
        };

        return true;
    }
}


