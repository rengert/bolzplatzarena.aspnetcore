using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;

namespace Bolzplatzarena.AspNetCore.TagHelpers
{
	public class InlineScriptTagHelper : TagHelper
	{
		[HtmlAttributeName("href")]
		public string Href { get; set; } = null !;

		private IWebHostEnvironment HostingEnvironment { get; }
		private IMemoryCache Cache { get; }

		public InlineScriptTagHelper(IWebHostEnvironment hostingEnvironment, IMemoryCache cache)
		{
			HostingEnvironment = hostingEnvironment;
			Cache = cache;
		}

		public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
		{
			var path = Href;
			var cacheKey = "InlineScriptTagHelper-" + path;
			var fileContent = await Cache.GetOrCreateAsync(cacheKey, async entry => await Get(entry, path));

			if (string.IsNullOrEmpty(fileContent))
			{
				output.SuppressOutput();
				return;
			}

			output.TagName = "script";
			output.Attributes.RemoveAll("href");
			output.Content.AppendHtml(fileContent);
		}

		private async Task<string?> Get(ICacheEntry entry, string path)
		{
			IFileProvider fileProvider = HostingEnvironment.WebRootFileProvider;
			IChangeToken changeToken = fileProvider.Watch(path);

			entry.SetPriority(CacheItemPriority.NeverRemove);
			entry.AddExpirationToken(changeToken);

			IFileInfo file = fileProvider.GetFileInfo(path);
			if (!file.Exists)
			{
				return null;
			}

			return await ReadFileContent(file);
		}

		private static async Task<string> ReadFileContent(IFileInfo file)
		{
			await using var stream = file.CreateReadStream();
			using var textReader = new StreamReader(stream);
			return await textReader.ReadToEndAsync();
		}
	}
}