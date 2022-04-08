using blog.objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace blog.data.Extensions
{
    internal static class BlogExtensions
    {
        public static string ToFragment(this string? content)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                return content ?? string.Empty;
            }
            var frag = new StringBuilder(512);
            using (var sgml = new Sgml.SgmlReader())
            {
                sgml.DocType = "HTML";
                using (var reader = new StringReader($"<body>{content}</body>"))
                {
                    sgml.InputStream = reader;
                    var doc = new XmlDocument();
                    doc.Load(sgml);
                    if (doc.DocumentElement == null) return content;
                    var node = doc.DocumentElement?.FirstChild?.FirstChild ?? doc.DocumentElement?.FirstChild;
                    while (frag.Length < 256 && node != null)
                    {
                        var text = node.InnerText
                            .Substring(0, node.InnerText.Length > 256 ? 256 : node.InnerText.Length)
                            .Replace("&nbsp;", " ");
                        frag.Append(text);
                        frag.Append(" ");
                        var prev = node;
                        node = node.NextSibling;
                        if (node == null)
                        {
                            node = prev.ParentNode?.NextSibling?.FirstChild;
                        }
                    }
                }
            }
            return frag.ToString().Trim();
        }

        public static Title ToTitle(this Blog blog)
        {
            return new Title
            {
                fragment = blog.fragment ?? string.Empty,
                id = blog.id ?? string.Empty,
                title = blog.title ?? string.Empty,
                publishedAt = blog.publishedAt
            };
        }
    }
}
