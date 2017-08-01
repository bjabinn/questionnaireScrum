using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QuestionScrumVS2015
{
    class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Start: " + DateTime.Now);
            int numQuestionInserted = 0;
            int numQuestionsAlreadyInDB = 0;

            string urlWebService = "https://api.surveyanyplace.com/v1/surveys/30517?isMobile=true&noCache=20170705085110&expand=true&lang=1&c=false";
            string json = new System.Net.WebClient().DownloadString(urlWebService);
            //Console.WriteLine(json);

            JObject jsonParsed = JObject.Parse(json);
            //var NumOfQuestion = jsonParsed["questionblocks"][0]["questions"].Children().ToList().Count;

            //Question: "translation -> "1" -> 
            //                                "id"
            //                                "text"
            //Answers: ""answers" (que es un array) ->             
            //                                      "translations" -> "1" -> "text"
            //                                      "correct" (boolean)  

            ScrumDbContext context = new ScrumDbContext();

            var questions = new List<Question>();

            Console.WriteLine("Json Received: " + DateTime.Now);
            for (var i = 0; i < 20; i++)
            {
                numQuestionInserted = 0;
                numQuestionsAlreadyInDB = 0; 
                try
                {
                    var question = new Question();

                    var translation = jsonParsed["questionblocks"][0]["questions"][0]["translations"]["1"];
                    question.Id = (int)(translation["id"]);
                    question.Text = (string)(translation["text"]);

                    //TODO: check if that id already exists into the db
                    var questionObjectIfExist = context.Questions.FirstOrDefault(x => x.Id == question.Id);
                    if (questionObjectIfExist == null)
                    {
                        question.AllAnswers = new List<Answer>();
                        var numOfAnswers = (jsonParsed["questionblocks"][0]["questions"][0]["answers"]).Count();
                        for (var j = 0; j < numOfAnswers; j++)
                        {
                            var answer = new Answer()
                            {
                                Text = (string)jsonParsed["questionblocks"][0]["questions"][0]["answers"][j]["translations"]["1"]["text"],
                                IsCorrect = (bool)(jsonParsed["questionblocks"][0]["questions"][0]["answers"][j]["correct"])
                            };
                            question.AllAnswers.Add(answer);
                        };


                        context.Questions.Add(question);
                        context.SaveChanges();
                        numQuestionInserted++;
                    }
                    else
                    {
                        numQuestionsAlreadyInDB++;
                    }
                }
                catch (Exception ex) {
                    Console.WriteLine("Exception: " + ex.Message);
                }                 
            } //end for 20 received questions
            Console.WriteLine("Question Inserted: " + numQuestionInserted + ". Question already found in DB: " + numQuestionsAlreadyInDB);

        } //end of Main
    } //end of Class
} //end of namespace

