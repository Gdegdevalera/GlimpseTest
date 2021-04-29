using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace IntegrationTests
{
    [TestClass]
    public class FillDataTest
    {
        static readonly HttpClient http = new();

        [TestMethod]
        public async Task Publish_single_image()
        {
            var prev = await GetCategories();

            await PostImage(); // doesn't matter what image is

            await Task.Delay(TimeSpan.FromSeconds(10)); // wait until it is processed

            var actual = await GetCategories();

            actual.Categories.Count.Should().BeGreaterThan(prev.Categories.Count);
        }

        [TestMethod]
        public async Task Publish_many_images()
        {
            for (int i = 0; i < 10000; i++)
            {
                var tasks = new[] { 
                    PostImage(), 
                    PostImage(),
                    PostImage(),
                    PostImage(),
                    PostImage(),
                    PostImage() 
                };
                Task.WaitAny(tasks);
            }
        }

        private static async Task<CategoriesViewModel> GetCategories()
        {
            var response = await http.GetAsync("http://localhost:81/categories");
            response.IsSuccessStatusCode.Should().BeTrue();

            var body = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<CategoriesViewModel>(body);
        }

        private static async Task PostImage()
        {
            using var stream = new MemoryStream(new byte[] { 1, 2, 3, 4, 5, 6, 7 });
            var image = new StreamContent(stream);

            var response = await http.PostAsync("http://localhost:82", image);
            response.IsSuccessStatusCode.Should().BeTrue();
        }
    }
}
