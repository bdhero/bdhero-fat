using System;
using System.Collections.Generic;
using System.Linq;
using BDHeroCore.BDROM;
using DotNetUtils;
using Newtonsoft.Json;

namespace BDHeroCore.Services
{
    public static class BDService
    {
        private const string BaseUri = "http://bd.andydvorak.net/api/v2/movie";

        /// <summary>
        /// Queries the BDHero Main Movie database for all discs that have the same volume label as the given disc,
        /// and then returns the first (and theoretically only) disc whose hash code matches the given disc's (or null if none match).
        /// </summary>
        /// <param name="scanned">Scan result from BDInfo</param>
        /// <returns>The first disc from the BDHero database that has the same volume label and hash code as the given disc; otherwise null if none match.</returns>
        /// <exception cref="ResponseError"></exception>
        public static Disc GetDisc(Disc scanned)
        {
            var uri = GetUri(scanned);
            var responseText = HttpRequest.Get(uri);
            var responseObject = JsonConvert.DeserializeObject<GetResponse>(responseText);
            responseObject.method = "GET";
            responseObject.uri = uri;

            CheckForErrors(responseObject);

            var discs = Disc.DeserializeJson(responseObject.discs);
            var scannedHashCode = scanned.GetHashCode();

            return discs.FirstOrDefault(disc => disc.GetHashCode() == scannedHashCode);
        }

        /// <summary>
        /// PUTs the given Disc to the BDHero Main Movie DB.
        /// </summary>
        /// <param name="disc">Disc that was scanned by BDInfo and customized by the user.</param>
        /// <param name="apiKey">User's API key</param>
        /// <returns>true if the PUT succeeded; false if there were errors</returns>
        /// <exception cref="ResponseError"></exception>
        public static bool PutDisc(Disc disc, string apiKey)
        {
            var uri = GetUri(disc, apiKey);
            var json = JsonConvert.SerializeObject(disc.ToJsonObject());
            var data = new Dictionary<string, string> { { "json", json } };
            var responseText = HttpRequest.Put(uri, data);
            var responseObject = JsonConvert.DeserializeObject<PutResponse>(responseText);
            responseObject.method = "PUT";
            responseObject.uri = uri;

            CheckForErrors(responseObject);

            return responseObject.success;
        }

        private static string GetUri(Disc disc, string apiKey = null)
        {
            return
                string.IsNullOrWhiteSpace(apiKey) ?
                string.Format("{0}/{1}.json", BaseUri, disc.VolumeLabel) :
                string.Format("{0}?api_key={1}", GetUri(disc), apiKey);
        }

        /// <exception cref="ResponseError"></exception>
        private static void CheckForErrors(BaseResponse response)
        {
            if (!response.error) return;
            foreach (var error in response.errors)
            {
                error.Source = string.Format("{0} {1}", response.method, response.uri);
            }
            var firstError = response.errors.FirstOrDefault();
            if (firstError != null) throw firstError;
        }
    }

    class BaseResponse
    {
        public string method;
        public string uri;

        public bool success = false;
        public bool error = true;
        public IList<ResponseError> errors = new List<ResponseError>();
    }

    class GetResponse : BaseResponse
    {
        public IList<Disc.Json> discs = new List<Disc.Json>();
    }

    class PutResponse : BaseResponse
    {
    }

    public class ResponseError : Exception
    {
        public string textStatus;
        public string errorMessage;

        public override string Message
        {
            get
            {
                var hasTextStatus = !string.IsNullOrWhiteSpace(textStatus);
                var hasErrorMessage = !string.IsNullOrWhiteSpace(errorMessage);

                if (hasTextStatus && hasErrorMessage)
                    return string.Format("{0}: {1}", textStatus, errorMessage);
                if (hasTextStatus)
                    return textStatus;
                if (hasErrorMessage)
                    return errorMessage;

                return base.Message;
            }
        }
    }
}
