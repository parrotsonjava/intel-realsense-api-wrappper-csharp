using System;
using IntelRealSenseStart.Code.RealSense.Component.Determiner;
using IntelRealSenseStart.Code.RealSense.Component.Output;
using IntelRealSenseStart.Code.RealSense.Config.RealSense;
using IntelRealSenseStart.Code.RealSense.Event;

namespace IntelRealSenseStart.Code.RealSense.Manager
{
    public static class RealSenseComponentsManagerSpeechExtensions
    {
        public static void ConfigureRecognition(this RealSenseComponentsManager manager,
            SpeechRecognitionConfiguration configuration)
        {
            manager.CheckIfReady();
            manager.GetComponent<SpeechRecognitionDeterminerComponent>().UpdateConfiguration(configuration);
        }

        public static void StartRecognition(this RealSenseComponentsManager manager)
        {
            manager.CheckIfReady();
            manager.GetComponent<SpeechRecognitionDeterminerComponent>().StartRecognition();
        }

        public static void StopRecognition(this RealSenseComponentsManager manager)
        {
            manager.CheckIfReady();
            manager.GetComponent<SpeechRecognitionDeterminerComponent>().StopRecognition();
        }

        public static void OnSpeechRecognized(this RealSenseComponentsManager manager, SpeechRecognitionEventListener listener)
        {
            if (manager.IsComponentActive(typeof(SpeechRecognitionDeterminerComponent)))
            {
                manager.GetComponent<SpeechRecognitionDeterminerComponent>().Speech += listener;
            }
        }

        public static void Speak(this RealSenseComponentsManager manager, String sentence)
        {
            manager.CheckIfReady();
            manager.GetComponent<SpeechSynthesisOutputComponent>().Speak(sentence);
        }

        public static void OnSpeechOutput(this RealSenseComponentsManager manager, SpeechOutputStatusListener listener)
        {
            if (manager.IsComponentActive(typeof(SpeechRecognitionDeterminerComponent)))
            {
                manager.GetComponent<SpeechSynthesisOutputComponent>().Speech += listener;
            }
        }


    }
}