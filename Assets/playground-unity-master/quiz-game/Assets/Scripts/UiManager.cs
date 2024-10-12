using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public class UiManager : MonoBehaviour
    {
        private ControlsHook _controls;

        private Color _positiveColor;
        private Color _negativeColor;

        private void Start()
        {
            _controls = GetComponent<ControlsHook>();

            _positiveColor = new Color(21 / 255f, 150 / 255f, 86 / 255f);
            _negativeColor = new Color(135 / 255f, 35 / 255f, 13 / 255f);
        }

        public void DrawScore(int score)
        {
            _controls.Score.text = "Score: " + score;
        }

        public void DrawTime(TimeSpan time)
        {
            _controls.Timer.text = "Time: " + time;
        }

        public void DrawQuestion(Question question)
        {
            _controls.Question.text = question.Text;
            for (var i = 0; i < question.Choices.Length; i++)
            {
                _controls.ChoiceButtons[i].Statement.text = question.Choices[i].Statement;
            }
        }

        public void ShowOnlyButtonsOfChoices(Choice[] choices)
        {
            foreach (var choiceButton in _controls.ChoiceButtons)
            {
                if (choices.All(choice => choice.Statement != choiceButton.Statement.text))
                {
                    choiceButton.Button.SetActive(false);
                }
            }
        }

        public void PlayJoker()
        {
            _controls.Joker.SetActive(false);
        }

        public void ResetQuestionButtons()
        {
            foreach (var choiceButton in _controls.ChoiceButtons)
            {
                choiceButton.Button.SetActive(true);
            }
        }

        public void DrawMessage(string message, bool isPositive = false)
        {
            _controls.Choices.SetActive(false);
            _controls.Message.color = isPositive ? _positiveColor : _negativeColor;
            _controls.Message.text = message;
        }

        public void DrawDisappearingMessage(string message, bool isPositive = false)
        {
            DrawMessage(message, isPositive);
            StartCoroutine(DelayAction(RemoveMessage));
        }

        private void RemoveMessage()
        {
            _controls.Message.text = "";
            _controls.Choices.SetActive(true);
        }

        private IEnumerator DelayAction(Action action)
        {
            yield return new WaitForSeconds(Constants.WaitTimeForDelayedAction);
            action();
        }
    }
}
