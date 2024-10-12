using System;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    [Serializable]
    public class Question {
        public string Text;

        [SerializeField]
        public Choice[] Choices;

        public Question
        (
            string   text, 
            string[] choiceStatements, 
            int      correctChoiceLocation
        )
        {
            Text = text;
            Choices = choiceStatements.Select(statement => new Choice(statement)).ToArray();
            Choices[correctChoiceLocation - 1].IsTrue = true;
        }

        public bool IsValid()
        {
            if (string.IsNullOrEmpty(Text))
            {
                return false;
            }
            return Choices.Length == Constants.NumberOfChoices
                   && Choices.Any(choice => choice.IsTrue)
                   && Choices.Count(choice => choice.IsTrue) <= Constants.MaximumCorrectChoicesInSingleQuestion
                   && Choices.All(choice => choice.IsValid())
                   && Choices.GroupBy(choice => choice.Statement).Count() == Choices.Length;
        }

        public Choice[] HalfTheChoices()
        {
            var random = new System.Random();
            var correctChoice = Choices.Where(choice => choice.IsTrue);

            return Choices
                .Where(choice => !choice.IsTrue)
                .OrderBy(_ => random.Next())
                .Take(Constants.NumberOfChoices/2 - 1)
                .Concat(correctChoice)
                .ToArray();
        }
    }

    public static class QuestionExtensions
    {
        public static Question[] ConcatSeed(this Question[] questions)
        {
            return questions.Concat(new[]
            {
                new Question("Mengapa kerentanan dunia maya tidak akan pernah hilang?",           new [] { "Pemerintah tidak akan mengizinkan orang untuk memperbaikinya.", "Penjahat membutuhkan mereka untuk mencuri identitas.", "Mereka adalah efek samping dari kebebasan dan kemudahan berkomunikasi online.", "Mereka dilindungi di pangkalan rahasia di bulan." }, 3),
                new Question("Manakah dari kelompok-kelompok ini yang mengeksploitasi kerentanan dunia maya?",                       new [] { "Penjahat", "Pemerintah", "Peretas", "Semua hal di atas"                }, 4),
                new Question("Mengapa peretas meretas?", new [] { "Untuk membuat penemuan", "Untuk melindungi informasi", "Untuk mencuri informasi dan uang", "Semua hal di atas"                    }, 3),
                new Question("Manakah dari ini yang secara teratur digunakan untuk komunikasi online yang aman?",                   new [] { "Sandi Caesar", "Kriptografi kunci publik", "Kode morse", "Kode teka-teki"    }, 2),
                new Question("Bagaimana kita bisa tahu jika sebuah situs web telah terenkripsi?",       new [] { "Google itu.", "Cari simbol gembok di URL.", "Semua situs web mengenkripsi lalu lintas mereka.", "Situs terenkripsi membutuhkan waktu lebih lama untuk dimuat."                      }, 2),
            }).ToArray();
        }

        public static Question[] GetValid(this Question[] questions)
        {
            return questions.Where(question => question.IsValid()).ToArray();
        }

        public static Question[] RandomizeChoicesOrder(this Question[] questions)
        {
            var random = new System.Random();
            foreach (var question in questions)
            {
                question.Choices = question.Choices.OrderBy(_ => random.Next()).ToArray();
            }
            return questions;
        }
    }
}
