using System;
using System.IO;
using ImageResizer;

namespace Brewgr.Web.Core.Service
{
	public class DefaultStaticContentService : IStaticContentService
	{
		delegate void WriteImageDelegate(string physicalRoot, string imageRoot, Stream FileStream);

		/// <summary>
		/// Writes an image to disk
		/// </summary>
		public string SaveRecipeImage(Stream fileStream, string physicalRoot)
		{
			if (fileStream == null)
			{
				throw new ArgumentNullException("fileStream");
			}

			if (string.IsNullOrWhiteSpace(physicalRoot))
			{
				throw new ArgumentNullException("physicalRoot");
			}

			var imageRoot = this.GenerateRecipeUrlRoot();

			// Do the rest asynchronously
			//var asyncImageWriter = new WriteImageDelegate(this.ResizeAndSaveRecipeImages);
			//asyncImageWriter.BeginInvoke(physicalRoot, imageRoot, fileStream, null, null);

			// 2/14/2013 - Turn off Async Image Resizing -- Have had a few image issues, this might be why
			this.ResizeAndSaveRecipeImages(physicalRoot, imageRoot, fileStream);

			return imageRoot;
		}

		/// <summary>
		/// Deletes a Recipe Image
		/// </summary>
		public void DeleteRecipeImage(string physicalRoot, string oldImageUrlRoot)
		{
			if (string.IsNullOrWhiteSpace(physicalRoot))
			{
				throw new ArgumentNullException("physicalRoot");
			}

			if (string.IsNullOrWhiteSpace(oldImageUrlRoot))
			{
				throw new ArgumentNullException("oldImageUrlRoot");
			}

			var correctedImageRoot = oldImageUrlRoot.Replace("/", "\\");

			var targetDirectory = string.Concat(physicalRoot,
			                                    correctedImageRoot.Substring(0, correctedImageRoot.LastIndexOf("\\")));

			var detailImageDiskLocation = string.Format("{0}{1}_d.jpg", physicalRoot, correctedImageRoot);
			var thumbnailDiskLocation = string.Format("{0}{1}_t.jpg", physicalRoot, correctedImageRoot);

			if (File.Exists(detailImageDiskLocation))
			{
				File.Delete(detailImageDiskLocation);
			}

			if (File.Exists(thumbnailDiskLocation))
			{
				File.Delete(thumbnailDiskLocation);
			}
		}

		/// <summary>
		/// Resizes ans Saves Recipe Images
		/// </summary>
		void ResizeAndSaveRecipeImages(string physicalRoot, string imageRoot, Stream fileStream)
		{
			var correctedImageRoot = imageRoot.Replace("/","\\");

			var targetDirectory = string.Concat(physicalRoot, correctedImageRoot.Substring(0, correctedImageRoot.LastIndexOf("\\")));
			
			if(!Directory.Exists(targetDirectory))
			{
				Directory.CreateDirectory(targetDirectory);
			}

			var detailImageDiskLocation = string.Format("{0}{1}_d.jpg", physicalRoot,correctedImageRoot);
			var thumbnailDiskLocation = string.Format("{0}{1}_t.jpg", physicalRoot, correctedImageRoot);

			// Get Image Bytes
			var memoryStream = new MemoryStream();
			fileStream.CopyTo(memoryStream);
			var bytes = memoryStream.ToArray();

			// Resize and Save Detail
			using (var stream = new FileStream(detailImageDiskLocation, FileMode.OpenOrCreate))
			{
				ImageBuilder.Current.Build(new MemoryStream(bytes), stream, new ResizeSettings("format=jpg&quality=80&width=300&scale=both&height=300&crop=auto"));	
			}

			// Resize and Save Thumbnail
			using (var stream = new FileStream(thumbnailDiskLocation, FileMode.OpenOrCreate))
			{
				ImageBuilder.Current.Build(new MemoryStream(bytes), stream, new ResizeSettings("format=jpg&quality=80&width=80&scale=both&height=80&crop=auto"));
			}
		}

		/// <summary>
		/// Generates a Recipe Url Root
		/// </summary>
		string GenerateRecipeUrlRoot()
		{
			// This uses the first 2 characters of the Guid as the parent folder...
			// That gives us 1296 folders to throw images in.  That should keep us going for awhile.
			var guid = Guid.NewGuid();
			return string.Format("/img/r/{0}{1}/{2}", guid.ToString()[0], guid.ToString()[1], guid);
		}
	}
}