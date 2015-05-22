using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntelRealSenseStart.Code.DialogFlow.Decision
{
    public class DialogDecision<STATE>
    {
        private readonly STATE followUpState;
        private readonly MatchesPredicate predicate;
        private readonly Action<String> action;

        public delegate bool MatchesPredicate(STATE currentState, String recognizedSentence);

        public DialogDecision(STATE followUpState, MatchesPredicate predicate, Action<String> action)
        {
            this.followUpState = followUpState;
            this.predicate = predicate;
            this.action = action;
        }

        public bool matches(STATE currentState, String recognizedSentence)
        {
            return predicate.Invoke(currentState, recognizedSentence);
        }

        public STATE FollowUpState
        {
            get { return followUpState; }
        }

        public Action<String> Action
        {
            get { return action; }
        }
    }
}