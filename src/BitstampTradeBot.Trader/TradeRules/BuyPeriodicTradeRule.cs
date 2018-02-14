﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BitstampTradeBot.Data.Models;
using BitstampTradeBot.Data.Repositories;
using BitstampTradeBot.Models;
using BitstampTradeBot.Trader.Helpers;
using BitstampTradeBot.Trader.TradeHolders;

namespace BitstampTradeBot.Trader.TradeRules
{
    public class BuyPeriodicTradeRule : TradeRuleBase
    {
        private readonly BitstampPairCode _pairCode;

        public BuyPeriodicTradeRule(BitstampPairCode pairCode, TradeSettings tradeSettings, params ITradeHolder[] tradeHolders) : base(tradeSettings, tradeHolders)
        {
            _pairCode = pairCode;
        }

        internal override async Task ExecuteAsync(BitstampTrader bitstampTrader)
        {
            if (TradeHolders != null)
            {
                foreach (var tradeHolder in TradeHolders)
                {
                    if (tradeHolder.Execute(this)) return;
                }
            }

            // get ticker
            var ticker = await bitstampTrader.GetTickerAsync(_pairCode);

            // get the pair code id from cache
            var pairCodeId = CacheHelper.GetFromCache<List<CurrencyPair>>("TradingPairsDb").First(c => c.PairCode == _pairCode.ToString()).Id;
            
            // buy currency on Bitstamp exchange
            var orderResult = await bitstampTrader.BuyLimitOrderAsync(_pairCode, TradeSettings.GetBaseAmount(ticker), TradeSettings.GetBasePrice(ticker));

            // update database
            var ordersRepo = new SqlRepository<Order>(new AppDbContext());
            ordersRepo.Add(new Order
            {
                BuyId = orderResult.Id,
                CurrencyPairId = pairCodeId,
                BuyAmount = orderResult.Amount,
                BuyPrice = orderResult.Price
            });
            ordersRepo.Save();

            LastBuyTimestamp = DateTime.Now;
        }
    }
}
