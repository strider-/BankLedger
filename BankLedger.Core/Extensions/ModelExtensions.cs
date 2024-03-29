﻿using System;

namespace BankLedger.Core.Extensions
{
    public static class ModelExtensions
    {
        public static string Place(this int i)
        {
            string suffix = string.Empty;

            int ones = i % 10;
            int tens = (int)Math.Floor(i / 10M) % 10;

            if (tens == 1)
            {
                suffix = "th";
            }
            else
            {
                switch (ones)
                {
                    case 1:
                        suffix = "st";
                        break;

                    case 2:
                        suffix = "nd";
                        break;

                    case 3:
                        suffix = "rd";
                        break;

                    default:
                        suffix = "th";
                        break;
                }
            }
            return $"{i}{suffix}";
        }
    }
}
