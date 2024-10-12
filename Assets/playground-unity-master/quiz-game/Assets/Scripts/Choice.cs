using System;

namespace Assets.Scripts
{
    [Serializable]
    public class Choice
    {
        public string Statement;
        public bool   IsTrue;

        public Choice
        (
            string statement
        )
        {
            Statement = statement;
            IsTrue    = false;
        }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(Statement);
        }
    }
}
