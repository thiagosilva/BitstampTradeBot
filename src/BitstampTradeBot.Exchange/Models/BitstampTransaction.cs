﻿using System;
using System.Linq;
using BitstampTradeBot.Exchange.Models.Attributes;
using Newtonsoft.Json;

namespace BitstampTradeBot.Exchange.Models
{
    internal class BitstampTransaction
    {
        // Date and time
        [JsonProperty(PropertyName = "datetime")]
        public DateTime Timestamp { get; set; }

        // Transaction ID
        [JsonProperty(PropertyName = "id")]
        public long Id { get; set; }

        // Transaction type: 0 - deposit; 1 - withdrawal; 2 - market trade; 14 - sub account transfer
        [JsonProperty(PropertyName = "type")]
        public int Type { get; set; }

        // USD amount
        [BitstampTransactionMember("amount")]
        [JsonProperty(PropertyName = "usd")]
        public decimal Usd { get; set; }

        // EUR amount
        [BitstampTransactionMember("amount")]
        [JsonProperty(PropertyName = "eur")]
        public decimal Eur { get; set; }

        // BTC amount
        [BitstampTransactionMember("amount")]
        [JsonProperty(PropertyName = "btc")]
        public decimal Btc { get; set; }

        // XRP amount
        [BitstampTransactionMember("amount")]
        [JsonProperty(PropertyName = "xrp")]
        public decimal Xrp { get; set; }

        // LTC amount
        [BitstampTransactionMember("amount")]
        [JsonProperty(PropertyName = "ltc")]
        public decimal Ltc { get; set; }

        // ETH amount
        [BitstampTransactionMember("amount")]
        [JsonProperty(PropertyName = "eth")]
        public decimal Eth { get; set; }

        // BCH amount
        [BitstampTransactionMember("amount")]
        [JsonProperty(PropertyName = "bch")]
        public decimal Bch { get; set; }

        // Exchange rate
        //[ExchangeRate]
        [BitstampTransactionMember("exchange")]
        [JsonProperty(PropertyName = "btc_usd")]
        public decimal ExchangeRateBtcUsd { get; set; }

        // Exchange rate
        [BitstampTransactionMember("exchange")]
        [JsonProperty(PropertyName = "btc_eur")]
        public decimal ExchangeRateBtcEur { get; set; }

        // Exchange rate
        [BitstampTransactionMember("exchange")]
        [JsonProperty(PropertyName = "eur_usd")]
        public decimal ExchangeRateEurUsd { get; set; }

        // Exchange rate
        [BitstampTransactionMember("exchange")]
        [JsonProperty(PropertyName = "xrp_usd")]
        public decimal ExchangeRateXrpUsd { get; set; }

        // Exchange rate
        [BitstampTransactionMember("exchange")]
        [JsonProperty(PropertyName = "xrp_eur")]
        public decimal ExchangeRateXrpEur { get; set; }

        // Exchange rate
        [BitstampTransactionMember("exchange")]
        [JsonProperty(PropertyName = "xrp_btc")]
        public decimal ExchangeRateXrpBtc { get; set; }

        // Exchange rate
        [BitstampTransactionMember("exchange")]
        [JsonProperty(PropertyName = "ltc_usd")]
        public decimal ExchangeRateLtcUsd { get; set; }

        // Exchange rate
        [BitstampTransactionMember("exchange")]
        [JsonProperty(PropertyName = "ltc_eur")]
        public decimal ExchangeRateLtcEur { get; set; }

        // Exchange rate
        [BitstampTransactionMember("exchange")]
        [JsonProperty(PropertyName = "ltc_btc")]
        public decimal ExchangeRateLtcBtc { get; set; }

        // Exchange rate
        [BitstampTransactionMember("exchange")]
        [JsonProperty(PropertyName = "eth_usd")]
        public decimal ExchangeRateEthUsd { get; set; }

        // Exchange rate
        [BitstampTransactionMember("exchange")]
        [JsonProperty(PropertyName = "eth_eur")]
        public decimal ExchangeRateEthEur { get; set; }

        // Exchange rate
        [BitstampTransactionMember("exchange")]
        [JsonProperty(PropertyName = "eth_btc")]
        public decimal ExchangeRateEthBtc { get; set; }

        // Exchange rate
        [BitstampTransactionMember("exchange")]
        [JsonProperty(PropertyName = "bch_usd")]
        public decimal ExchangeRateBchUsd { get; set; }

        // Exchange rate
        [BitstampTransactionMember("exchange")]
        [JsonProperty(PropertyName = "bch_eur")]
        public decimal ExchangeRateBchEur { get; set; }

        // Exchange rate
        [BitstampTransactionMember("exchange")]
        [JsonProperty(PropertyName = "bch_btc")]
        public decimal ExchangeRateBchBtc { get; set; }

        // Transaction fee
        [JsonProperty(PropertyName = "fee")]
        public decimal Fee { get; set; }

        // Executed order ID.
        [JsonProperty(PropertyName = "order_id")]
        public long OrderId { get; set; }

        public decimal Price
        {
            get
            {
                decimal price = 0;

                var properties = GetType().GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(BitstampTransactionMemberAttribute)));
                foreach (var property in properties)
                {
                    var attr = (BitstampTransactionMemberAttribute)property.GetCustomAttributes(typeof(BitstampTransactionMemberAttribute), false).First();
                    if (attr.MemberType != "exchange") continue;

                    var propValue = (decimal)property.GetValue(this, null);
                    if (propValue > 0)
                    {
                        price += propValue;
                    }

                }
                return price;
            }
        }
    }
}
