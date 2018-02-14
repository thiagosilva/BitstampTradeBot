﻿using System;
using Newtonsoft.Json;

namespace BitstampTradeBot.Trader.Models.Exchange
{
    public enum BitstampOrderType { Buy = 0, Sell = 1 }

    public class BitstampOrder
    {
        [JsonIgnore]
        public BitstampPairCode PairCode { get; set; }

        // transaction ID
        [JsonProperty(PropertyName = "id")]
        public long Id { get; set; }

        // date and time
        [JsonProperty(PropertyName = "datetime")]
        public DateTime Timestamp { get; set; }

        // type: 0 - buy; 1 - sell
        [JsonProperty(PropertyName = "type")]
        public BitstampOrderType Type { get; set; }

        // price
        [JsonProperty(PropertyName = "price")]
        public decimal Price { get; set; }

        // amount
        [JsonProperty(PropertyName = "amount")]
        public decimal Amount { get; set; }
    }
}
