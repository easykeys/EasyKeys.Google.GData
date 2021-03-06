/* Copyright (c) 2006 Google Inc.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
*/
#region Using directives

#define USE_TRACING

using System;
using System.IO;

using EasyKeys.EasyKeys.Google.GData.Client.Internal;

#endregion

// <summary>Contains the MediaSources currently implemented</summary>
namespace EasyKeys.Google.GData.Client
{
    /// <summary>
    /// placeholder for a media object to be uploaded
    /// the base class only defines some primitives like content type
    /// </summary>
    public abstract class MediaSource
    {
        private string _contentType;
        private string _contentName;

        /// <summary>
        /// constructs a media source based on a contenttype
        /// </summary>
        /// <param name="contenttype">the contenttype of the file</param>
        /// <returns></returns>
        public MediaSource(string contenttype)
        {
            ContentType = contenttype;
        }

        /// <summary>
        /// constructs a media source based on a contenttype and a name
        /// </summary>
        /// <param name="name">the name of the content</param>
        /// <param name="contenttype">the contenttype of the file</param>
        /// <returns></returns>
        public MediaSource(string name, string contenttype)
        {
            Name = name;
            ContentType = contenttype;
        }

        /// <summary>
        /// returns the length of the content of the media source
        /// </summary>
        /// <returns></returns>
        public abstract long ContentLength
        {
            get;
        }

        /// <summary>
        /// the name value of the content influence directly the slug
        /// header send
        /// </summary>
        /// <returns></returns>
        public string Name
        {
            get
            {
                return _contentName;
            }

            set
            {
                _contentName = value;
            }
        }

        /// <summary>
        /// returns the contenttype of the media source, like img/jpg
        /// </summary>
        /// <returns></returns>
        public string ContentType
        {
            get
            {
                return _contentType;
            }

            set
            {
                _contentType = value;
            }
        }

        /// <summary>
        /// returns a stream of the actual content that is base64 encoded
        /// </summary>
        /// <returns></returns>
        [Obsolete("That name was misleading. Use GetDataStream() instead")]
        public abstract Stream Data
        {
            get;
        }

        /// <summary>
        /// returns a stream of the actual content that is base64 encoded
        /// </summary>
        /// <returns></returns>
        public abstract Stream GetDataStream();
    }

    /// <summary>
    /// a file based implementation. Takes a filename as it's base working mode
    /// </summary>
    /// <returns></returns>
    public class MediaFileSource : MediaSource
    {
        private string _file;
        private Stream _stream;

        /// <summary>
        /// constructor. note that you can override the slug header without influencing the filename
        /// </summary>
        /// <param name="fileName">the file to be used, this will be the default slug header</param>
        /// <param name="contentType">the content type to be used</param>
        /// <returns></returns>
        public MediaFileSource(string fileName, string contentType)
            : base(fileName, contentType)
        {
            _file = fileName;

            // strip out the path from the Slug header
            FileInfo fileInfo = new FileInfo(fileName);
            Name = fileInfo.Name;
        }

        /// <summary>
        /// constructor. note that you can override the slug header without influencing the filename
        /// </summary>
        /// <param name="data">The stream for the file. If this constructor is used, the filename is only
        /// used for descriptive purposes, the data will be read from the passed stream</param>
        /// <param name="fileName">the file to be used, this will be the default slug header</param>
        /// <param name="contentType">the content type to be used</param>
        /// <returns></returns>
        public MediaFileSource(Stream data, string fileName, string contentType)
            : base(fileName, contentType)
        {
            _stream = data;
        }

        /// <summary>
        /// Gets Content Type based on the file extension.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns>NULL or the registered contenttype</returns>
        public static string GetContentTypeForFileName(string fileName)
        {
            return MimeTypeLookup.GetMimeType(fileName.ToLower());
        }

        /// <summary>
        /// returns the content length of the file
        /// </summary>
        /// <returns></returns>
        public override long ContentLength
        {
            get
            {
                long result;

                try
                {
                    Stream s = GetDataStream();
                    result = s.Length;
                    s.Close();
                }
                catch (NotSupportedException)
                {
                    result = -1;
                }

                return result;
            }
        }

        /// <summary>
        /// returns the stream for the file. The file will be opened in readonly mode
        /// note, the caller has to release the resource
        /// </summary>
        /// <returns></returns>
        [Obsolete("That name was misleading. Use GetDataStream() instead")]
        public override Stream Data
        {
            get
            {
                return GetDataStream();
            }
        }

        /// <summary>
        /// returns the stream for the file. The file will be opened in readonly mode
        /// note, the caller has to release the resource
        /// </summary>
        /// <returns></returns>
        public override Stream GetDataStream()
        {
            if (!String.IsNullOrEmpty(_file))
            {
                return File.OpenRead(_file);
            }

            var newStream = new MemoryStream();
            _stream.Position = 0;
            CopyStream(_stream, newStream);
            newStream.Position = 0;
            return newStream;
        }

        private void CopyStream(Stream input, Stream output)
        {
            byte[] buffer = new byte[32768];
            int read;
            while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, read);
            }
        }
    }
}

