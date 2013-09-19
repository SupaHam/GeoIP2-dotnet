﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using MaxMind.GeoIP2.Exceptions;
using MaxMind.GeoIP2.Model;
using MaxMind.GeoIP2.Responses;
using RestSharp;
using RestSharp.Deserializers;

namespace MaxMind.GeoIP2
{
    /// <summary>
    /// <para>
    /// This class provides a client API for all the GeoIP2 web service's end points.
    /// The end points are Country, City, City/ISP/Org, and Omni. Each end point
    /// returns a different set of data about an IP address, with Country returning
    /// the least data and Omni the most.
    /// </para>
    /// 
    /// <para>
    /// Each web service end point is represented by a different model class 
    /// which contains data about the IP address.
    /// </para>
    /// 
    /// <para>
    /// If the web service does not return a particular piece of data for an IP
    /// address, the associated property is not populated.
    /// </para>
    /// 
    /// <para>
    /// The web service may not return any information for an entire record, in which
    /// case all of the properties for that model class will be empty.
    /// </para>
    /// 
    /// <para>
    /// Usage
    /// </para>
    /// 
    /// <para>
    /// The basic API for this class is the same for all of the web service end
    /// points. First you create a web service object with your MaxMind
    /// userId and licenseKey, then you call the method corresponding
    /// to a specific end point, passing it the IP address you want to look up.
    /// </para>
    /// 
    /// <para>
    /// If the request succeeds, the method call will return a model class for the
    /// end point you called. This model in turn contains multiple record classes,
    /// each of which represents part of the data returned by the web service.
    /// </para>
    /// 
    /// <para>
    /// If the request fails, the client class throws an exception.
    /// </para>
    /// 
    /// <para>
    /// Exceptions
    /// </para>
    /// 
    /// <para>
    /// For details on the possible errors returned by the web service itself, see <a
    /// href="http://dev.maxmind.com/geoip2/geoip/web-services">the GeoIP2 web
    /// service documentation</a>.
    /// </para>
    /// 
    /// </summary>
    public class WebServiceClient : IGeoIP2Provider
    {
        private const string BASE_URL = "https://geoip.maxmind.com/geoip/v2.0";

        private readonly int _userId;

        private readonly string _licenseKey;

        private List<string> _languages;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebServiceClient"/> class.
        /// </summary>
        /// <param name="userId">Your MaxMind user ID.</param>
        /// <param name="licenseKey">Your MaxMind license key.</param>
        public WebServiceClient(int userId, string licenseKey)
            : this(userId, licenseKey, new List<string> { "en" })
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebServiceClient"/> class.
        /// </summary>
        /// <param name="userId">The user unique identifier.</param>
        /// <param name="licenseKey">The license key.</param>
        /// <param name="languages">List of language codes to use in name property from most preferred to least preferred.</param>
        public WebServiceClient(int userId, string licenseKey, List<string> languages)
        {
            _userId = userId;
            _licenseKey = licenseKey;
            _languages = languages;
        }

        private IRestClient CreateClient()
        {
            var restClient = new RestClient(BASE_URL);
            restClient.Authenticator = new HttpBasicAuthenticator(_userId.ToString(), _licenseKey);
            restClient.AddHandler("application/vnd.maxmind.com-omni+json", new JsonDeserializer());
            restClient.AddHandler("application/vnd.maxmind.com-country+json", new JsonDeserializer());
            restClient.AddHandler("application/vnd.maxmind.com-city+json", new JsonDeserializer());
            restClient.AddHandler("application/vnd.maxmind.com-city-isp-org+json", new JsonDeserializer());

            return restClient;
        }


        /// <summary>
        /// Returns an <see cref="OmniResponse"/> for the specified ip address.
        /// </summary>
        /// <param name="ipAddress">The ip address.</param>
        /// <returns>An <see cref="OmniResponse"/></returns>
        public OmniResponse Omni(string ipAddress)
        {
            return Omni(ipAddress, CreateClient());
        }

        /// <summary>
        /// Returns an <see cref="OmniResponse"/> for the specified ip address.
        /// </summary>
        /// <param name="ipAddress">The ip address.</param>
        /// <param name="restClient">The RestClient to use</param>
        /// <returns>An <see cref="OmniResponse"/></returns>
        internal OmniResponse Omni(string ipAddress, IRestClient restClient)
        {
            return Execute<OmniResponse>("omni/{ip}", ipAddress, restClient);
        }

        /// <summary>
        /// Returns an <see cref="CountryResponse"/> for the specified ip address.
        /// </summary>
        /// <param name="ipAddress">The ip address.</param>
        /// <returns>An <see cref="CountryResponse"/></returns>
        public CountryResponse Country(string ipAddress)
        {
            return Country(ipAddress, CreateClient());
        }

        /// <summary>
        /// Returns an <see cref="CountryResponse"/> for the specified ip address.
        /// </summary>
        /// <param name="ipAddress">The ip address.</param>
        /// <param name="restClient">The RestClient to use</param>
        /// <returns>An <see cref="CountryResponse"/></returns>
        internal CountryResponse Country(string ipAddress, IRestClient restClient)
        {
            return Execute<CountryResponse>("country/{ip}", ipAddress, restClient);
        }

        /// <summary>
        /// Returns an <see cref="CityResponse"/> for the specified ip address.
        /// </summary>
        /// <param name="ipAddress">The ip address.</param>
        /// <returns>An <see cref="CityResponse"/></returns>
        public CityResponse City(string ipAddress)
        {
            return City(ipAddress, CreateClient());
        }

        /// <summary>
        /// Returns an <see cref="CityResponse"/> for the specified ip address.
        /// </summary>
        /// <param name="ipAddress">The ip address.</param>
        /// <param name="restClient">The RestClient to use</param>
        /// <returns>An <see cref="CityResponse"/></returns>
        internal CityResponse City(string ipAddress, IRestClient restClient)
        {
            return Execute<CityResponse>("city/{ip}", ipAddress, restClient);
        }

        /// <summary>
        /// Returns an <see cref="CityIspOrgResponse"/> for the specified ip address.
        /// </summary>
        /// <param name="ipAddress">The ip address.</param>
        /// <returns>An <see cref="CityIspOrgResponse"/></returns>
        public CityIspOrgResponse CityIspOrg(string ipAddress)
        {
            return CityIspOrg(ipAddress, CreateClient());
        }

        /// <summary>
        /// Returns an <see cref="CityIspOrgResponse"/> for the specified ip address.
        /// </summary>
        /// <param name="ipAddress">The ip address.</param>
        /// <param name="restClient">The RestClient to use</param>
        /// <returns>An <see cref="CityIspOrgResponse"/></returns>
        internal CityIspOrgResponse CityIspOrg(string ipAddress, IRestClient restClient)
        {
            return Execute<CityIspOrgResponse>("city_isp_org/{ip}", ipAddress, restClient);
        }

        private T Execute<T>(string urlPattern, string ipAddress, IRestClient restClient) where T : CountryResponse, new()
        {
            var request = new RestRequest(urlPattern);
            request.AddUrlSegment("ip", ipAddress);

            var response = restClient.Execute<T>(request);

            var status = (int)response.StatusCode;
            if (status == 200)
            {
                if (response.ContentLength <= 0)
                    throw new GeoIP2HttpException(string.Format("Received a 200 response for {0} but there was no message body.", response.ResponseUri), response.StatusCode, response.ResponseUri);

                if(response.ContentType == null || !response.ContentType.Contains("json"))
                    throw new GeoIP2Exception(string.Format("Received a 200 response for {0} but it does not appear to be JSON:\n", response.ContentType));

                if(response.Data == null)
                    throw new GeoIP2Exception(string.Format("Received a 200 response but not decode it as JSON: {0}", response.Content));

                response.Data.SetLanguages(_languages);                
                return response.Data;
            }
            else if (status >= 400 && status < 500)
            {
                Handle4xxStatus(response);
            }
            else if (status >= 500 && status < 600)
            {
                throw new GeoIP2HttpException(string.Format("Received a server ({0}) error for {1}", (int)response.StatusCode, response.ResponseUri), response.StatusCode, response.ResponseUri);
            }

            throw new GeoIP2HttpException(
                string.Format("Received a very surprising HTTP status ({0}) for {1}", (int)response.StatusCode,
                    response.ResponseUri), response.StatusCode, response.ResponseUri);
        }

        private void Handle4xxStatus(IRestResponse response)
        {
            if (string.IsNullOrEmpty(response.Content))
            {
                throw new GeoIP2HttpException(string.Format("Received a {0} error for {1} with no body", response.StatusCode, response.ResponseUri), response.StatusCode, response.ResponseUri);
            }

            try
            {
                var d = new JsonDeserializer();
                var webServiceError = d.Deserialize<WebServiceError>(response);
                HandleErrorWithJSONBody(webServiceError, response);
            }
            catch (SerializationException ex)
            {
                throw new GeoIP2HttpException(string.Format("Received a {0} error for {1} but it did not include the expected JSON body: {2}", response.StatusCode, response.ResponseUri, response.Content), response.StatusCode, response.ResponseUri);
            }

        }

        private static void HandleErrorWithJSONBody(WebServiceError webServiceError, IRestResponse response)
        {

            if (webServiceError.Code == null || webServiceError.Error == null)
                throw new GeoIP2HttpException(
                    "Response contains JSON but does not specify code or error keys: " + response.Content, response.StatusCode,
                    response.ResponseUri);

            if(webServiceError.Code == "IP_ADDRESS_NOT_FOUND" || webServiceError.Code == "IP_ADDRESS_RESERVED")
                throw new GeoIP2AddressNotFoundException(webServiceError.Error);
            else if(webServiceError.Code == "AUTHORIZATION_INVALID" || webServiceError.Code == "LICENSE_KEY_REQUIRED" || webServiceError.Code == "USER_ID_REQUIRED")
                throw new GeoIP2AuthenticationException(webServiceError.Error);
            else if(webServiceError.Code == "OUT_OF_QUERIES")
                throw new GeoIP2OutOfQueriesException(webServiceError.Error);

            throw new GeoIP2InvalidRequestException(webServiceError.Error, webServiceError.Code, response.ResponseUri);
        }
    }
}