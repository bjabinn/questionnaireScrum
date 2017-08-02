using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;

namespace QuestionScrumVS2015
{
    class Program
    {
        public static void getDataAndSaveToDB()
        {
            System.IO.StreamWriter file = new System.IO.StreamWriter("RunningLog.txt", true);
            file.WriteLine("--------------> Start: " + DateTime.Now);
            int numQuestionInserted = 0;
            int numQuestionsAlreadyInDB = 0;
            int numExceptions = 0;

            string urlWebService = ConfigurationManager.AppSettings["urlApiQuestions"]; ;
            string json = new System.Net.WebClient().DownloadString(urlWebService);
            JObject jsonParsed = JObject.Parse(json);

            ScrumDbContext context = new ScrumDbContext();

            var questions = new List<Question>();

            file.WriteLine("Json Received: " + DateTime.Now);

            for (var i = 0; i < 20; i++)
            {
                try
                {
                    var question = new Question();

                    var translation = jsonParsed["questionblocks"][0]["questions"][i]["translations"]["1"];
                    question.IdFromExternalDB = (int)(translation["id"]);
                    question.Text = (string)(translation["text"]);

                    var questionObjectIfExist = context.Questions.FirstOrDefault(x => x.IdFromExternalDB == question.IdFromExternalDB);
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
                catch (Exception ex)
                {
                    numExceptions++;
                    file.WriteLine("Exception: " + ex.Message);
                    Console.WriteLine("");
                }
            } //end for 20 received questions
            file.WriteLine("Question Inserted: " + numQuestionInserted +
                             ". Question already found in DB: " + numQuestionsAlreadyInDB +
                             ". Num of Errors: " + numExceptions);
            file.WriteLine("");
            file.Close();
        } //end of getDataAndSaveToDB

        public static void Main(string[] args)
        {
            getDataAndSaveToDB();
        } //end of Main

    } //end of Class

} //end of namespace

