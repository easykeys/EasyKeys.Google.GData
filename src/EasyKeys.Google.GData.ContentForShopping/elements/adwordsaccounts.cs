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
using EasyKeys.Google.GData.Client;

namespace EasyKeys.Google.GData.ContentForShopping.Elements
{
    public class AdwordsAccounts : SimpleContainer
    {
        /// <summary>
        /// AdwordsAccount collection
        /// </summary>
        private ExtensionCollection<AdwordsAccount> _adwordsAccountList;

        /// <summary>
        /// default constructor for app:control in the Content for Shopping API
        /// </summary>
        public AdwordsAccounts()
            : base(
                ContentForShoppingNameTable.AdwordsAccounts,
                ContentForShoppingNameTable.scDataPrefix,
                ContentForShoppingNameTable.BaseNamespace)
        {
            ExtensionFactories.Add(new AdwordsAccount());
        }

        /// <summary>
        /// Returns the sc:adwords_account elements
        /// </summary>
        public ExtensionCollection<AdwordsAccount> AdwordsAccountList
        {
            get
            {
                if (_adwordsAccountList == null)
                {
                    _adwordsAccountList = new ExtensionCollection<AdwordsAccount>(this);
                }

                return _adwordsAccountList;
            }
        }
    }
}
