/* Copyright (c) 2006 Google Inc.
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
    public class Warnings : SimpleContainer
    {
        private ExtensionCollection<Warning> _warningList;

        /// <summary>
        /// default constructor for sc:warnings
        /// </summary>
        public Warnings()
            : base(
                ContentForShoppingNameTable.Warnings,
            ContentForShoppingNameTable.scDataPrefix,
            ContentForShoppingNameTable.BaseNamespace)
        {
            ExtensionFactories.Add(new Warning());
        }

        /// <summary>
        /// Warning collection.
        /// </summary>
        public ExtensionCollection<Warning> Entries
        {
            get
            {
                if (_warningList == null)
                {
                    _warningList = new ExtensionCollection<Warning>(this);
                }

                return _warningList;
            }
        }
    }
}
