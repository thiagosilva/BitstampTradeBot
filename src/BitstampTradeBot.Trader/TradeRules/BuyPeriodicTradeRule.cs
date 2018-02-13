﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BitstampTradeBot.Data.Models;
using BitstampTradeBot.Data.Repositories;
using BitstampTradeBot.Models;
using BitstampTradeBot.Models.Helpers;
using BitstampTradeBot.Trader.Helpers;

namespace BitstampTradeBot.Trader.TradeRules
{
    public class BuyPeriodicTradeRule : ITradeRule
    {
        private readonly BitstampPairCode _pairCode;
        private readonly TimeSpan _period;
        private DateTime _lastBuyTimestamp;

        public BuyPeriodicTradeRule(BitstampPairCode pairCode, TimeSpan period)
        {
            _pairCode = pairCode;
            _period = period;
        }

        public async Task ExecuteAsync(BitstampTrader bitstampTrader)
        {
            if (_lastBuyTimestamp == DateTime.MinValue)
            {
                _lastBuyTimestamp = DateTime.Now;
            }

            if (DateTime.Now > _lastBuyTimestamp.Add(_period))
            {
                // get ticker
                var ticker = await bitstampTrader.GetTickerAsync(_pairCode);

                // get the pair code id from cache
                var pairCodeId = CacheHelper.GetFromCache<List<CurrencyPair>>("TradingPairsDb").First(c => c.PairCode == _pairCode.ToString()).Id;

                // calculate price and amount
                var tradingPairInfo = CacheHelper.GetFromCache<List<BitstampTradingPairInfo>>("TradingPairInfo").First(i => i.UrlSymbol == _pairCode.ToLower());
                var price = Math.Round(ticker.Last * 0.9M, tradingPairInfo.CounterDecimals);
                var amount = Math.Round(CurrencyPairCalculator.AmountBase(price, 10), tradingPairInfo.BaseDecimals);

                // buy currency on Bitstamp exchange
                var orderResult = await bitstampTrader.BuyLimitOrderAsync(_pairCode, amount, price);

                // update database
                var ordersRepo = new SqlRepository<Order>(new AppDbContext());
                ordersRepo.Add(new Order
                {
                    BuyId = orderResult.Id,
                    CurrencyPairId = pairCodeId,
                    BuyAmount =  orderResult.Amount,
                    BuyPrice = orderResult.Price
                });
                ordersRepo.Save();

                _lastBuyTimestamp = DateTime.Now;
            }
        }
    }
}
