using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public class Quiz
    {
        public int Score;

        private readonly IList<Question> _questions;

        private Quiz(Question[] questions)
        {
            Score = 0;
            _questions = questions
                .ConcatSeed()
                .GetValid()
                .RandomizeChoicesOrder()
                .ToList();
        }

        public IMaybe<Question> GetNextQuestion()
        {
            if (!_questions.Any())
            {
                return None<Question>.Exists();
            }
            var nextQuestion = _questions[Random.Range(0, _questions.Count)];
            _questions.Remove(nextQuestion);
            return Some<Question>.Exists(nextQuestion);
        }
        
        public bool SubmitAnswer(Question question, string answer)
        {
            var selectedChoice = question
                .Choices
                .First(choice => choice.Statement == answer);

            if (selectedChoice.IsTrue)
            {
                Score++;
            }
            return selectedChoice.IsTrue;
        }

        public static Quiz CreateFrom(Question[] userDefinedQuestions)
        {
            return new Quiz(userDefinedQuestions);
        }
    }
}
