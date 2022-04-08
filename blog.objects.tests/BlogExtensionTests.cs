using blog.objects.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace blog.objects.tests
{
    [TestClass]
    public class BlogExtensionTests
    {
        [TestMethod]
        public void ToBlogSetsTitle()
        {
            var expected = "foo";
            var target = new BlogEntry
            {
                title = expected
            };
            var result = target.ToBlog();
            Assert.AreEqual(expected, result.title);
        }

        [TestMethod]
        public void ToBlogSetsArticle()
        {
            var expected = "foo";
            var target = new BlogEntry
            {
                article = expected
            };
            var result = target.ToBlog();
            Assert.AreEqual(expected, result.article);

        }
    }
}