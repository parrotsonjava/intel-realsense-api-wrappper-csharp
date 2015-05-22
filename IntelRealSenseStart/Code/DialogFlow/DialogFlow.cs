using System;
using System.Collections.Generic;
using IntelRealSenseStart.Code.DialogFlow.Decision;
using IntelRealSenseStart.Code.RealSense;
using IntelRealSenseStart.Code.RealSense.Event;

namespace IntelRealSenseStart.Code.DialogFlow
{
    public class DialogFlow<STATE>
    {
        private readonly RealSenseManager manager;

        private STATE currentState;

        

        public DialogFlow(RealSenseManager manager, Dictionary<STATE, List<DialogDecision<STATE>> )
        {
            this.manager = manager;

            manager.SpeechOutput += manager_SpeechOutput;
            manager.SpeechRecognized += manager_SpeechRecognized;
        }
        
        private void manager_SpeechOutput(SpeechOutputStatusEventArgs eventArgs)
        {
             
        }

        private void manager_SpeechRecognized(SpeechRecognitionEventArgs eventArgs)
        {

        }

        private void Start(STATE startState)
        {


            currentState = startState;
        }



    }
}
