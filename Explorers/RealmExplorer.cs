﻿using System;
using System.Linq;
using System.Text;
using System.Net;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.IO;
using WowDotNetAPI.Explorers.Models;
using WowDotNetAPI.Explorers.Interfaces;
using WowDotNetAPI.Explorers.Extensions;
using WowDotNetAPI.Interfaces;

namespace WowDotNetAPI.Explorers
{
	public class RealmExplorer : IRealmExplorer
	{
		private const string baseRealmAPIurl = "http://{0}.battle.net/api/wow/realm/status{1}";

		private readonly IJsonSource jsonSource;
		private readonly string region;
		private readonly JavaScriptSerializer serializer;

		public static RealmExplorer Create(string region)
		{
			return new RealmExplorer(region, new JsonSource(), new JavaScriptSerializer());
		}

		public static RealmExplorer CreateWithProxy(string region, string proxyUser, string proxyPassword, string proxyUrl)
		{
			return new RealmExplorer(region, new ProxyJsonSource(proxyUser, proxyPassword, proxyUrl), new JavaScriptSerializer());
		}

		public RealmExplorer(IJsonSource jsonSource) : this("us", jsonSource, new JavaScriptSerializer()) { }

		public RealmExplorer(string region, IJsonSource jsonSource) : this(region, jsonSource, new JavaScriptSerializer()) { }

		public RealmExplorer(string region, IJsonSource jsonSource, JavaScriptSerializer serializer)
		{
			if (region == null) throw new ArgumentNullException("region");
			if (jsonSource == null) throw new ArgumentNullException("jsonSource");
			if (serializer == null) throw new ArgumentNullException("serializer");

			this.region = region;
			this.jsonSource = jsonSource;
			this.serializer = serializer;
		}

		public Realm GetSingleRealm(string name)
		{
			RealmList realmList = GetMultipleRealms(name);
			return realmList == null ? null : realmList.realms.FirstOrDefault();
		}

		public RealmList GetAllRealms()
		{
			string url = string.Format(baseRealmAPIurl, region, string.Empty);
			return this.GetRealmData(url);
		}

		public RealmList GetRealmsByType(string type)
		{
			RealmList realmList = GetRealmData(string.Format(baseRealmAPIurl, region, string.Empty));
			realmList = realmList.WithType(type);
			return realmList;
		}

		public RealmList GetRealmsByPopulation(string population)
		{
			RealmList realmList = GetRealmData(string.Format(baseRealmAPIurl, region, string.Empty));
			realmList = realmList.WithPopulation(population);
			return realmList;
		}

		public RealmList GetRealmsByStatus(bool status)
		{
			RealmList realmList = GetRealmData(string.Format(baseRealmAPIurl, region, string.Empty));
			if (status)
			{
				realmList = realmList.WhereUp();
			}
			else
			{
				realmList = realmList.WhereDown();
			}
			return realmList;
		}

		public RealmList GetRealmsByQueue(bool queue)
		{
			RealmList realmList = this.GetRealmData(string.Format(baseRealmAPIurl, region, string.Empty));
			if (queue)
			{
				realmList = realmList.WithQueue();
			}
			else
			{
				realmList = realmList.WithoutQueue();
			}

			return realmList;
		}

		public RealmList GetMultipleRealms(params string[] names)
		{
			if (names == null
				|| names.Length == 0
				|| names.Any(r => r == null))
			{
				return new RealmList();
			}

			string query = "?realms=" + String.Join(",", names);

			return GetMultipleRealmsViaQuery(query);
		}

		public RealmList GetMultipleRealmsViaQuery(string query)
		{
			if (string.IsNullOrEmpty(query)) throw new ArgumentException("Value must not be null or empty.", "query");

			try
			{
				string url = string.Format(baseRealmAPIurl, region, query);
				return GetRealmData(url);
			}
			catch
			{
				return null;
			}
		}

		public string GetRealmsByTypeAsJson(string type)
		{
			return ConvertRealmListToJson(GetRealmsByType(type));
		}

		public string GetRealmsByPopulationAsJson(string population)
		{
			return ConvertRealmListToJson(GetRealmsByPopulation(population));
		}

		public string GetRealmsByStatusAsJson(bool status)
		{
			return ConvertRealmListToJson(GetRealmsByStatus(status));
		}

		public string GetRealmsByQueueAsJson(bool queue)
		{
			return ConvertRealmListToJson(GetRealmsByQueue(queue));
		}

		public string GetAllRealmsAsJson()
		{
			string url = string.Format(baseRealmAPIurl, region, string.Empty);
			return jsonSource.GetJson(url);
		}

		public string GetSingleRealmAsJson(string name)
		{
			return ConvertRealmListToJson(GetMultipleRealms(name));
		}

		public string GetMultipleRealmsAsJson(params string[] mames)
		{
			return ConvertRealmListToJson(GetMultipleRealms(mames));
		}

		public string GetRealmsViaQueryAsJson(string query)
		{
			string url = string.Format(baseRealmAPIurl, region, query);
			return jsonSource.GetJson(url);
		}

		private string ConvertRealmListToJson(RealmList realmList)
		{
			return serializer.Serialize(realmList);
		}

		public RealmList GetRealmData(string url)
		{
			string json = jsonSource.GetJson(url);
			RealmList realmList = serializer.Deserialize<RealmList>(json);
			return realmList;
		}

	}
}
