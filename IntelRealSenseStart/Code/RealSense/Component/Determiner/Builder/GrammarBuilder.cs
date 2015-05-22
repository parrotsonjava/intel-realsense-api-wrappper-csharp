using System;
using System.Collections.Generic;
using IntelRealSenseStart.Code.RealSense.Helper;

namespace IntelRealSenseStart.Code.RealSense.Component.Determiner.Builder
{
    public class GrammarBuilder
    {
        private PXCMSpeechRecognition speechRecognition;

        private readonly Dictionary<String, int> grammarMap;
        private int currentGrammarId;

        public GrammarBuilder()
        {
            currentGrammarId = 1;
            grammarMap = new Dictionary<String, int>();
        }

        public int GetGrammarId(String grammar)
        {
            speechRecognition.Check(Preconditions.IsNotNull, "Speech recognition was not set for the grammar builder");
            return grammarMap.ContainsKey(grammar) ? GetGrammarIdFor(grammar) : CreateGrammarIdFor(grammar);
        }

        private int GetGrammarIdFor(string grammar)
        {
            return grammarMap[grammar];
        }

        private int CreateGrammarIdFor(string grammar)
        {
            var grammarBytes = new byte[grammar.Length * sizeof(char)];
            Buffer.BlockCopy(grammar.ToCharArray(), 0, grammarBytes, 0, grammarBytes.Length);
            speechRecognition.BuildGrammarFromMemory(currentGrammarId,
                PXCMSpeechRecognition.GrammarFileType.GFT_JSGF, grammarBytes);
            var currentId = currentGrammarId;

            grammarMap[grammar] = currentId;
            currentGrammarId++;

            return currentId;
        }

        public void UseSpeechRecognition(PXCMSpeechRecognition speechRecognition)
        {
            this.speechRecognition = speechRecognition;
        }
    }
}