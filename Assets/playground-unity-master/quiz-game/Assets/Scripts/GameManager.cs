using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class GameManager : MonoBehaviour {
        public Question[] Questions;
        
        private UiManager _uiManager;

        private Quiz _quiz;
        private Question _currentQuestion;
        private TimeSpan _playingTime;
        private bool _isTimerIsActive;

        private void Start()
        {
            _uiManager = GetComponent<UiManager>();
            _quiz = Quiz.CreateFrom(Questions);
            SetNewQuestion();
        }

        private void Update()
        {
            StartCoroutine(IncreaseTimer());
        }

        private void SetNewQuestion()
        {
            var maybeQuestion = _quiz.GetNextQuestion();

            maybeQuestion.Case(
                some: question =>
                {
                    _isTimerIsActive = true;
                    _currentQuestion = question;
                    _uiManager.DrawQuestion(_currentQuestion);
                },
                none: () => _uiManager.DrawMessage("No more questions")
            );
        }

        public void SubmitAnswer(Text answer)
        {
            _isTimerIsActive = false;

            var isCorrect = _quiz.SubmitAnswer(_currentQuestion, answer.text);
            _uiManager.DrawScore(_quiz.Score);

            _uiManager.ResetQuestionButtons();
            _uiManager.DrawDisappearingMessage(isCorrect ? "Correct!" : "Sorry :(", isCorrect);

            StartCoroutine(DelayAction(SetNewQuestion));
        }

        public void PlayJoker()
        {
            _uiManager.PlayJoker();
            var newChoices = _currentQuestion.HalfTheChoices();
            _uiManager.ShowOnlyButtonsOfChoices(newChoices);
        }

        private IEnumerator DelayAction(Action action)
        {
            yield return new WaitForSeconds(Constants.WaitTimeForDelayedAction);
            action();
        }

        private IEnumerator IncreaseTimer()
        {
            yield return new WaitForSeconds(1);
            if (_isTimerIsActive)
            {
                _playingTime = _playingTime.Add(new TimeSpan(0, 0, 1));
                _uiManager.DrawTime(_playingTime);
            }
        }
    }
}
