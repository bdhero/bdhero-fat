using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using DotNetUtils;
using Newtonsoft.Json;

namespace Hasher
{
    class Input
    {
        [JsonProperty(PropertyName = "name")]
        public readonly string Name;

        [JsonProperty(PropertyName = "size")]
        public readonly long Size;

        [JsonProperty(PropertyName = "humanSize")]
        public string HumanSize { get { return FileUtils.HumanFriendlyFileSize(Size); } }

        [JsonProperty(PropertyName = "algorithms")]
        public readonly IDictionary<string, string> Algorithms;

        public Input(string path, IEnumerable<Algorithm> algorithms) :
            this(Path.GetFileName(path), File.ReadAllBytes(path), algorithms)
        {
        }

        public Input(string name, Stream stream, IEnumerable<Algorithm> algorithms) :
            this(name, FileUtils.ReadStream(stream), algorithms)
        {
        }

        public Input(string name, byte[] buffer, IEnumerable<Algorithm> algorithms)
        {
            Name = name;
            Size = buffer.Length;
            Algorithms = new Dictionary<string, string>();

            foreach (var algorithm in algorithms)
            {
                Algorithms[algorithm.MachineName] = algorithm.ComputeBytes(buffer);
            }
        }
    }
}
