﻿/* Copyright (c) 2006 Google Inc.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
*/
using System;
using EasyKeys.Google.GData.Extensions;

namespace EasyKeys.Google.GData.ContentForShopping.Elements
{
    public class Datapoint : SimpleElement
    {
        /// <summary>
        /// default constructor for sc:datapoint
        /// </summary>
        public Datapoint()
            : base(
                ContentForShoppingNameTable.Datapoint,
               ContentForShoppingNameTable.scDataPrefix,
               ContentForShoppingNameTable.BaseNamespace)
        {
        }

        /// <summary>
        /// Constructs a new Datapoint instance with the specified values.
        /// </summary>
        /// <param name="date">The datapoint's date.</param>
        /// <param name="clicks">The datapoint's clicks.</param>
        /// <param name="paidClicks">The datapoint's paid clicks.</param>
        public Datapoint(string date, string clicks, string paidClicks)
            : base(
                ContentForShoppingNameTable.Datapoint,
               ContentForShoppingNameTable.scDataPrefix,
               ContentForShoppingNameTable.BaseNamespace)
        {
            Date = date;
            Clicks = clicks;
            PaidClicks = paidClicks;
        }

        /// <summary>
        /// Date property accessor
        /// </summary>
        public string Date
        {
            get
            {
                return Convert.ToString(Attributes[ContentForShoppingNameTable.Date]);
            }

            set
            {
                Attributes[ContentForShoppingNameTable.Date] = value;
            }
        }

        /// <summary>
        /// Clicks property accessor
        /// </summary>
        public string Clicks
        {
            get
            {
                return Convert.ToString(Attributes[ContentForShoppingNameTable.Clicks]);
            }

            set
            {
                Attributes[ContentForShoppingNameTable.Clicks] = value;
            }
        }

        /// <summary>
        /// PaidClicks property accessor
        /// </summary>
        public string PaidClicks
        {
            get
            {
                return Convert.ToString(Attributes[ContentForShoppingNameTable.PaidClicks]);
            }

            set
            {
                Attributes[ContentForShoppingNameTable.PaidClicks] = value;
            }
        }
    }
}
