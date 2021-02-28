using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Caching.Memory;

namespace Bolzplatzarena.AspNetCore.TagHelpers
{
	public class InlineImageTagHelper : InlineTagHelper
	{
		private static readonly FileExtensionContentTypeProvider _contentTypeProvider = new();

		[HtmlAttributeName("src")]
		public string Src { get; set; } = null !;

		public InlineImageTagHelper(IWebHostEnvironment hostingEnvironment, IMemoryCache cache)
			: base(hostingEnvironment, cache)
		{
		}

		public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
		{
			var fileContent = await GetFileContentBase64Async(Src);
			if (string.IsNullOrEmpty(fileContent))
			{
				output.SuppressOutput();
				return;
			}

			if (!_contentTypeProvider.TryGetContentType(Src, out var contentType))
			{
				contentType = "application/octet-stream";
			}

			output.TagName = "img";
			var srcAttribute = $"data:{contentType};base64,{fileContent}";

			output.Attributes.RemoveAll("src");
			output.Attributes.Add("src", srcAttribute);
			output.TagMode = TagMode.SelfClosing;
			output.Content.AppendHtml(fileContent);
		}
	}
}
