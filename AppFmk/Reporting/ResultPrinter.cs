
using System.Linq;
using HtmlAgilityPack;
using System.IO;
using AppFmk.Interfaces;

namespace AppFmk.Reporting
{
    public class ResultPrinter : IResultPrinter
    {
        public string FilePath { get; set; }
        public string TestName { get; set; }

        public void PrepareTemplate()
        {
            HtmlDocument resultFile = new HtmlDocument();
            resultFile.Load(FilePath);
            if (resultFile.GetElementbyId("Results") == null)
            {
                HtmlNode htmlNode = resultFile.CreateElement("html");
                HtmlNode headNode = resultFile.CreateElement("head");
                HtmlNode bodyNode = resultFile.CreateElement("body");
                htmlNode.AppendChild(headNode);
                htmlNode.AppendChild(bodyNode);
                resultFile.DocumentNode.AppendChild(htmlNode);

                resultFile.DocumentNode.SelectSingleNode("//head").AppendChild(HtmlNode.CreateNode("<style></style>"));
                resultFile.DocumentNode.SelectSingleNode("//head/style").InnerHtml = HtmlDocument.HtmlEncode("table { width: 100 %; }"
                                                                                                             + "table, th, td {border: 1px solid black; border-collapse: collapse; }"
                                                                                                             + "th, td { padding: 5px; text-align: left; }"
                                                                                                             + "table#Results tr:nth-child(even) { background-color: #eee;}"
                                                                                                             + "table#Results tr:nth-child(odd) { background-color:#fff;}"
                                                                                                             + "table#Results th {background-color: black; color: white;}");
                resultFile.DocumentNode.SelectSingleNode("//body").AppendChild(HtmlNode.CreateNode("<table></table>"));
                resultFile.DocumentNode.SelectSingleNode("//body/table").SetAttributeValue("id", "Results");
                resultFile.DocumentNode.SelectSingleNode("//body/table").AppendChild(HtmlNode.CreateNode("<tr></tr>"));
                resultFile.DocumentNode.SelectSingleNode("//body/table").FirstChild.AppendChild(HtmlNode.CreateNode("<th>TestName</th>"));
                resultFile.DocumentNode.SelectSingleNode("//body/table").FirstChild.AppendChild(HtmlNode.CreateNode("<th>Results</th>"));
                resultFile.DocumentNode.SelectSingleNode("//body/table").FirstChild.AppendChild(HtmlNode.CreateNode("<th>Actual</th>"));
                resultFile.DocumentNode.SelectSingleNode("//body/table").FirstChild.AppendChild(HtmlNode.CreateNode("<th>Expected</th>"));
                resultFile.DocumentNode.SelectSingleNode("//body/table").FirstChild.AppendChild(HtmlNode.CreateNode("<th>Comments</th>"));
                resultFile.DocumentNode.SelectSingleNode("//body/table").FirstChild.AppendChild(HtmlNode.CreateNode("<th>Screenshots</th>"));
                StreamWriter stWriter = new StreamWriter(FilePath);
                resultFile.Save(stWriter);
                stWriter.Close();
            }
        }

        public void UpdateResult(bool result, string actual, string expected, string message, string screenshotPath)
        {
            HtmlDocument resultFile = new HtmlDocument();
            resultFile.Load(FilePath);
            resultFile.GetElementbyId("Results").AppendChild(HtmlNode.CreateNode("<tr></tr>"));
            resultFile.GetElementbyId("Results").ChildNodes.Last().AppendChild(HtmlNode.CreateNode("<td>" + TestName + "</td>"));

            if (result)
                resultFile.GetElementbyId("Results").ChildNodes.Last().AppendChild(HtmlNode.CreateNode("<td>" + "Pass" + "</td>"));
            else
                resultFile.GetElementbyId("Results").ChildNodes.Last().AppendChild(HtmlNode.CreateNode("<td><p style='color: red'>" + "Fail" + "</p></td>"));

            resultFile.GetElementbyId("Results").ChildNodes.Last().AppendChild(HtmlNode.CreateNode("<td>" + actual + "</td>"));
            resultFile.GetElementbyId("Results").ChildNodes.Last().AppendChild(HtmlNode.CreateNode("<td>" + expected + "</td>"));
            resultFile.GetElementbyId("Results").ChildNodes.Last().AppendChild(HtmlNode.CreateNode("<td>" + message + "</td>"));
            if(screenshotPath != "")
                 resultFile.GetElementbyId("Results").ChildNodes.Last().AppendChild(HtmlNode.CreateNode("<td>" + "<a href=\"" + screenshotPath + "\">" +"View"+ "</a>"+ "</td>"));
            else
                resultFile.GetElementbyId("Results").ChildNodes.Last().AppendChild(HtmlNode.CreateNode("<td>" + screenshotPath + "</td>"));

            StreamWriter stWriter = new StreamWriter(FilePath);
            resultFile.Save(stWriter);
            stWriter.Close();
        }

    }
}
