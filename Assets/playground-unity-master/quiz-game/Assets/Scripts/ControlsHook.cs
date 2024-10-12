using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    [Serializable]
    public class ChoiceButton
    {
        public GameObject Button;
        public Text Statement;
    }

    public class ControlsHook : MonoBehaviour
    {
        public Text Question;

        [SerializeField]
        public ChoiceButton[] ChoiceButtons;

        public GameObject Choices;

        public GameObject Joker;

        public Text Score;

        public Text Timer;

        public Text Message;
    }
}
