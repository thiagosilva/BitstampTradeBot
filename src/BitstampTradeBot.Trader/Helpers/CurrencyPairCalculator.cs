﻿using BitstampTradeBot.Models;

namespace BitstampTradeBot.Trader.Helpers
{
    class CurrencyPairCalculator
    {
        public static decimal AmountBase(decimal price, decimal amountCounter)
        {
            return amountCounter / price;
        }

        public static decimal AmountCounter(decimal price, decimal amountBase)
        {
            return amountBase * price;
        }
    }
}
