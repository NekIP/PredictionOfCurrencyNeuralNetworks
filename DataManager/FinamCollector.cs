﻿using DataBase;
using DataBase.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DataManager {
    public interface IFinamCollector : IDataCollector<Product> { }

	public abstract class FinamCollector : DataCollector<Product>, IFinamCollector {
		/*it's horrible*/
		protected static readonly Dictionary<string, string> FinamData = new Dictionary<string, string> {
            { "market", "" },		// market
			{ "em", "" },			// index, label paper
            { "code", "" },			// tool on the market
            { "apply", "0"},
            { "df", "" },
            { "mf", "" },			// mf - month from
            { "yf", "" },
            { "from", ""},			// dd.MM.yyyy
            { "dt", "" },
            { "mt", "" },
            { "yt", "" },
			{ "to", ""},			// dd.MM.yyyy
            { "p", "7" },           // the period of quotations (1 tick, 2 1 minute, 3 5 minutes, 4 10 minutes, 5 15 minutes, 6 30 minutes, 7 1 hour, 8 1 day, 9 1 week, 10 1 month)
            { "f", ""},				// output filename
            { "e", ".txt" },        // extension of the received file; possible options - .txt or .csv
            { "cn", ""},			// he name of the contract, the same as the code
            { "dtf", "4" },			// The format of the date (1 - yyyymmdd, 2 - yymmdd, 3 - ddmmgg, 4 - dd/mm/yy, 5 - mm/dd/yy)
			{ "tmf", "3" },			// time format (1 - hhmmss, 2 - hhmm, 3 - hh:mm:ss, 4 - hh:mm)
			{ "MSOR", "1" },		// give time (0 - the beginning of the candle, 1 - the end of the candle)
			{ "mstimever", "1" },	// type of time (0 - not Moscow, 1 - Moscow)
			{ "sep", "3" },			// parameter field separator (1 - comma (,), 2 - point (.), 3 - semicolon (;), 4 - tab (	), 5 - space ( ))
			{ "sep2", "1" },		// parameter separator of digits (1 - no, 2 - point (.), 3 - comma (,), 4 - space ( ), 5 - quotation mark ('))
			{ "datf", "5" },		/* List of received data (
										#1 — TICKER, PER, DATE, TIME, OPEN, HIGH, LOW, CLOSE, VOL; 
										#2 — TICKER, PER, DATE, TIME, OPEN, HIGH, LOW, CLOSE; 
										#3 — TICKER, PER, DATE, TIME, CLOSE, VOL; 
										#4 — TICKER, PER, DATE, TIME, CLOSE; 
										#5 — DATE, TIME, OPEN, HIGH, LOW, CLOSE, VOL; 
										#6 — DATE, TIME, LAST, VOL, ID, OPER
										, - delimeter).*/
			{ "at", "1" },			// add a title to the file (0 - no, 1 - yes)
		};

		protected string FinamCode { get; set; }
		protected int FinamEm { get; set; }
		protected int FinamMarket { get; set; }
        protected Log Log { get; set; }

		protected FinamCollector(IRepository<Product> repository, 
			string finamCode, int finamEm, int finamMarket) : base("http://export.finam.ru/", repository) {
			FinamCode = finamCode;
			FinamEm = finamEm;
			FinamMarket = finamMarket;
            Log = new Log($"log_{ FinamCode }.txt", true);
        }

		public override Task<List<Product>> List(DateTime from, DateTime to, TimeSpan step) {
			CheckConditionOnException(to - from < TimeSpan.FromDays(1),
                "The difference in time should be more than day");
            return base.List(from, to, step);
		}

        public override async Task DownloadMissingData(DateTime before, TimeSpan step) {
            var table = Repository.Table();
            if (table.Count() == 0) {
                await GetAndSaveData(GlobalFrom, before);
                return;
            }
			var minDeffRight = table.Min(x => new TimeSpan(Math.Abs(before.Ticks - x.Date.Ticks)));
			if (minDeffRight >= step) {
				var lastExistTime = table
                    .Where(x => new TimeSpan(Math.Abs(before.Ticks - x.Date.Ticks)) == minDeffRight).First().Date;
                await GetAndSaveData(lastExistTime, before);
            }
            var minDeffLeft = table.Min(x => new TimeSpan(Math.Abs(x.Date.Ticks - GlobalFrom.Ticks)));
            if (minDeffLeft >= step) {
                var firstExistTime = table
                    .Where(x => new TimeSpan(Math.Abs(GlobalFrom.Ticks - x.Date.Ticks)) == minDeffLeft).First().Date;
                await GetAndSaveData(GlobalFrom, firstExistTime);
            }
        }

        protected async Task GetAndSaveData(DateTime from, DateTime to) {
            var table = Repository.Table();
            var newData = await DownloadDataByStep(from, to);
            await table.AddRangeAsync(newData);
            await Repository.SaveChangesAsync();
            DeleteDuplicateEntries(new ProductComparer());
        }

		protected KeyValuePair<string, string>[] GetParameters(DateTime from, DateTime to) {
			var finamData = new Dictionary<string, string>(FinamData);
            finamData["code"] = FinamCode;
            finamData["em"] = FinamEm.ToString();
            finamData["market"] = FinamMarket.ToString();
            finamData["from"] = from.ToString("dd.MM.yyyy");
            finamData["to"] = to.ToString("dd.MM.yyyy");
            finamData["df"] = from.Day.ToString();
            finamData["mf"] = (from.Month - 1).ToString();
            finamData["yf"] = from.Year.ToString();
            finamData["dt"] = to.Day.ToString();
            finamData["mt"] = (to.Month - 1).ToString();
            finamData["yt"] = to.Year.ToString();
            finamData["cn"] = FinamCode;
            finamData["f"] = FinamCode + "_" + from.ToString("dd.MM.yyyy") + "_" + to.ToString("dd.MM.yyyy");
			return finamData.ToArray();
		}

		protected async Task<List<Product>> DownloadDataByStep(DateTime from, DateTime to) {
			var loader = new Loader();
			var result = new List<Product>();
			var step = TimeSpan.FromDays(10);
			for (var current = from; current <= to; current += step) {
				var next = current + step < to ? current + step : to + TimeSpan.FromDays(1);
				var entities = await loader.Get(GetUrl(current, next), Converter, GetParameters(current, next));
                await Log.Write($"AddedRecords={ entities.Count };From={ current.ToString("dd/MM/yyyy HH:mm:ss") };" +
                    $"To={ next.ToString("dd/MM/yyyy HH:mm:ss") };" +
                    $"DateOfFirstItemInArray={ entities.FirstOrDefault()?.Date.ToString("dd/MM/yyyy HH:mm:ss") }" +
                    $"DateOfLastItemInArray={ entities.LastOrDefault()?.Date.ToString("dd/MM/yyyy HH:mm:ss") }");
				result.AddRange(entities);
                Thread.Sleep(250);
			}
			return result;
		}

		// #5 — DATE, TIME, OPEN, HIGH, LOW, CLOSE, VOL;
		protected List<Product> Converter(string text) {
			var result = new List<Product>();
			var fieldDelimeter = ';';
			var lineDelimetr = '\n';
			var lines = text.Split(lineDelimetr).Skip(1);
			foreach (var line in lines) {
                if (string.IsNullOrWhiteSpace(line)) {
                    continue;
                }
				var fields = line.Split(fieldDelimeter);
				var date = DateTime.ParseExact($"{ fields[0] }-{ fields[1] }", "dd/MM/yy-HH:mm:ss",
					CultureInfo.InvariantCulture);
				var open = double.Parse(fields[2].Replace('.', ','));
				var high = double.Parse(fields[3].Replace('.', ','));
				var low = double.Parse(fields[4].Replace('.', ','));
				var close = double.Parse(fields[5].Replace('.', ','));
				result.Add(new Product {
					Date = date,
					Open = open,
					High = high,
					Low = low,
					Close = close,
					ChangeCloseOpen = close - open,
					ChangeHighLow = high - low
				});
			}
			return result;
		}

        protected string GetUrl(DateTime from, DateTime to) =>
            $"{ Source }{ FinamCode }_{ from.ToString("yyMMdd") }_{ to.ToString("yyMMdd") }{ FinamData["e"] }";

        protected class ProductComparer : IEqualityComparer<Product> {
            public bool Equals(Product x, Product y) => x.Date == y.Date;
            public int GetHashCode(Product obj) => obj.Date.GetHashCode();
        }
    }
}
