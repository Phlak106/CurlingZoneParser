using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;

namespace CurlingZoneParser
{
    class Program
    {
        static void Main(string[] args)
        {
            var htmlWeb = new HtmlWeb();
            var eventId = args[0];
            var numDraws = args[1];
            Dictionary<int, int> scores = new Dictionary<int, int>();
            for(int j = 1; j <= Convert.ToInt32(numDraws); ++j)
            {
                var htmlDoc = htmlWeb.Load($"https://www.curlingzone.com/event.php?eventid={eventId}&view=Scores&showdrawid={j}");
                var nodes = htmlDoc.DocumentNode.SelectNodes("//table[@class='linescorebox']");

                foreach(var linescore in nodes)
                {
                    var rows = linescore.SelectNodes(".//tr");
                    var hammer = rows[1].SelectSingleNode(".//td[@class='linescorehammer']").InnerText != "&nbsp;";
                    var team1 = rows[1].SelectNodes(".//td[@class='linescoreend']").Select(x => x.InnerText.Replace("&nbsp;", String.Empty)).Where(x => (!String.IsNullOrEmpty(x)) && Char.IsDigit(x[0])).Select(x => Int32.Parse(x)).ToArray();
                    var team2 = rows[2].SelectNodes(".//td[@class='linescoreend']").Select(x => x.InnerText.Replace("&nbsp;", String.Empty)).Where(x => (!String.IsNullOrEmpty(x)) && Char.IsDigit(x[0])).Select(x => Int32.Parse(x)).ToArray();
                    for(int i = 0; i < team1.Length; i++)
                    {
                        var result = team1[i] - team2[i];
                        if(!hammer)
                        {
                            result = -result;
                        }
                        if(scores.ContainsKey(result))
                        {
                            scores[result]++;
                        }
                        else
                        {
                            scores.Add(result, 1);
                        }

                        if(team1[i] > team2[i] && hammer)
                        {
                            hammer = false;
                        }
                        else if(team1[i] < team2[i] && !hammer)
                        {
                            hammer = true;
                        }
                    }
                }
            }
            var totalEnds = scores.Sum(x => x.Value);
            foreach(var score in scores.Keys.OrderBy(x => x))
            {
                Console.WriteLine($"{score} : {scores[score]} | {(double)scores[score] / totalEnds:P2}");
            }
        }
    }
}