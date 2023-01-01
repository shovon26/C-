using System;
using System.Linq;
using HtmlAgilityPack;

namespace WebScraper
{
    class Program
    {
        static void Main(string[] args)
        {
            // Load the HTML document
            var html = @"<html><body><div class='class-list'>
                <div class='class-item'>
                    <h3 class='class-name'>Childbirth Education</h3>
                    <p class='class-description'>Learn about the stages of labor, pain management techniques, and breastfeeding.</p>
                    <p class='class-location'>Los Angeles</p>
                </div>
                <div class='class-item'>
                    <h3 class='class-name'>Newborn Care</h3>
                    <p class='class-description'>Learn about newborn appearance, feeding, diapering, and bathing.</p>
                    <p class='class-location'>San Francisco</p>
                </div>
                </div></body></html>";
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            // Find all nodes with the class 'class-item'
            var classNodes = htmlDoc.DocumentNode.SelectNodes("//div[@class='class-item']");

            // Iterate through the class nodes and extract the information
            foreach (var classNode in classNodes)
            {
                var nameNode = classNode.SelectSingleNode(".//h3[@class='class-name']");
                var descriptionNode = classNode.SelectSingleNode(".//p[@class='class-description']");
                var locationNode = classNode.SelectSingleNode(".//p[@class='class-location']");
                var name = nameNode?.InnerText;
                var description = descriptionNode?.InnerText;
                var location = locationNode?.InnerText;
                Console.WriteLine($"Name: {name}");
                Console.WriteLine($"Description: {description}");
                Console.WriteLine($"Location: {location}");
            }
        }
    }
}
