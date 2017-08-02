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
            int numExceptions = 0;

            string urlWebService = "https://api.surveyanyplace.com/v1/surveys/30517?isMobile=true&noCache=20170705085110&expand=true&lang=1&c=false";
            string json = new System.Net.WebClient().DownloadString(urlWebService);
            //Console.WriteLine(json);
            //http://www.entityframeworktutorial.net/code-first/configure-one-to-many-relationship-in-code-first.aspx
            JObject jsonParsed = JObject.Parse(json);

            ScrumDbContext context = new ScrumDbContext();

            var questions = new List<Question>();

            Console.WriteLine("Json Received: " + DateTime.Now);

            for (var i = 0; i < 20; i++)
            {                 
                try
                {
                    var question = new Question();

                    var translation = jsonParsed["questionblocks"][0]["questions"][i]["translations"]["1"];
                    question.Id = (int)(translation["id"]);
                    question.Text = (string)(translation["text"]);

                    var questionObjectIfExist = context.Questions.FirstOrDefault(x => x.Id == question.Id);
                    if (questionObjectIfExist == null)
                    {
                        var numOfAnswers = (jsonParsed["questionblocks"][0]["questions"][i]["answers"]).Count();
                        for (var j = 0; j < numOfAnswers; j++)
                        {
                            var answer = new Answer()
                            {
                                Text = (string)jsonParsed["questionblocks"][0]["questions"][i]["answers"][j]["translations"]["1"]["text"],
                                IsCorrect = (bool)(jsonParsed["questionblocks"][0]["questions"][i]["answers"][j]["correct"])
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
                    numExceptions++;
                    Console.WriteLine("Exception: " + ex.Message);
                    Console.WriteLine("Press a key to continue");
                    Console.Read();
                }                 
            } //end for 20 received questions
            Console.WriteLine("Question Inserted: " + numQuestionInserted + 
                             ". Question already found in DB: " + numQuestionsAlreadyInDB + 
                             ". Num of Errors: " + numExceptions);

        } //end of Main
    } //end of Class
} //end of namespace

