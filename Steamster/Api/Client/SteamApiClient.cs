﻿using Steamster.Api.Api.Models;
using Steamster.Api.Api.Queries;
using Steamster.Api.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Steamster.Api.Api.Interfaces;

namespace Steamster.Api.Api.Client
{
    public class SteamApiClient : ISteamApiClient
    {
        static HttpClient client;
        private readonly ISteamUserApi _steamUserApi;
        private readonly IPlayerServiceApi _playerServiceApi;
            public string ApiKey { get; }

        public SteamApiClient(string apiKey)
        {
            ApiKey = apiKey;

            client = new HttpClient();

            client.BaseAddress = new Uri("http://api.steampowered.com/");//TODO: May need to alter this

            //To update uri
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            _steamUserApi = new SteamUserApi(client,ApiKey);
            _playerServiceApi = new PlayerServiceApi(client,ApiKey);
            //_steamUserStatsApi = new SteamUserApi(client);
        }

        public async Task<GameData> GetGameData(int appId)
        {
            try
            {
                //Create lookup item
                var query = new GetGameDataQuery(client, ApiKey, appId);//Extend or implement

                var results = await query.ExecuteAsync().ConfigureAwait(false);

                return results;

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new GameData()
                {
                };
            }
        }

        public async Task<UserGameListData> GetUsersGames(string userSteamId)
        {
            try
            {
                //Create lookup item
                var query = new GetGamesByUserQuery(client, ApiKey, userSteamId);//Extend or implement

                var results = await query.ExecuteAsync().ConfigureAwait(false);

                return results;

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new UserGameListData()
                {
                };
            }
        }

        //public async Task<UserData> GetUserInformationBySteamId(string userSteamId)
        //{
        //    try
        //    {
        //        //Create lookup item
        //        var query = new GetUserDataQuery(client, ApiKey, userSteamId);//Extend or implement

        //        var results = await query.ExecuteAsync().ConfigureAwait(false);

        //        return results;

        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.Message);
        //        return new UserData()
        //        {
        //        };
        //    }
        //}
        public async Task<bool> SignIn(string userId)
        {
            //var games = await GetUsersGames(userId).ConfigureAwait(false);

            //return games.response.games.Any();
            var userInfo = await _steamUserApi.GetPlayerSummaries(ApiKey, new List<string> { userId });

            return userInfo.Any();
        }
    }
}
