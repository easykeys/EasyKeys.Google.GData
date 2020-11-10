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
using EasyKeys.Google.GData.Extensions.AppControl;
using EasyKeys.Google.GData.Client;

namespace EasyKeys.Google.GData.ContentForShopping.Elements
{
    public class ProductControl : AppControl
    {
        /// <summary>
        /// Status collection
        /// </summary>
        private ExtensionCollection<Status> _statusList;

        /// <summary>
        /// Required destination collection
        /// </summary>
        private ExtensionCollection<RequiredDestination> _requiredDestinations;

        /// <summary>
        /// Validate destination collection
        /// </summary>
        private ExtensionCollection<ValidateDestination> _validateDestinations;

        /// <summary>
        /// Excluded destination collection
        /// </summary>
        private ExtensionCollection<ExcludedDestination> _excludedDestinations;

        /// <summary>
        /// default constructor for app:control in the Content for Shopping API
        /// </summary>
        public ProductControl()
            : base(BaseNameTable.NSAppPublishingFinal)
        {
            ExtensionFactories.Add(new RequiredDestination());
            ExtensionFactories.Add(new ValidateDestination());
            ExtensionFactories.Add(new ExcludedDestination());
            ExtensionFactories.Add(new Warnings());
            ExtensionFactories.Add(new Status());
        }

        /// <summary>
        /// returns the sc:warnings element
        /// </summary>
        public Warnings Warnings
        {
            get
            {
                return FindExtension(
                    ContentForShoppingNameTable.Warnings,
                    ContentForShoppingNameTable.BaseNamespace) as Warnings;
            }

            set
            {
                ReplaceExtension(
                    ContentForShoppingNameTable.Warnings,
                    ContentForShoppingNameTable.BaseNamespace,
                    value);
            }
        }

        /// <summary>
        /// Returns the sc:required_destination elements
        /// </summary>
        public ExtensionCollection<RequiredDestination> RequiredDestinations
        {
            get
            {
                if (_requiredDestinations == null)
                {
                    _requiredDestinations = new ExtensionCollection<RequiredDestination>(this);
                }

                return _requiredDestinations;
            }
        }

        /// <summary>
        /// Returns the sc:validate_destination elements
        /// </summary>
        public ExtensionCollection<ValidateDestination> ValidateDestinations
        {
            get
            {
                if (_validateDestinations == null)
                {
                    _validateDestinations = new ExtensionCollection<ValidateDestination>(this);
                }

                return _validateDestinations;
            }
        }

        /// <summary>
        /// Returns the sc:excluded_destination elements
        /// </summary>
        public ExtensionCollection<ExcludedDestination> ExcludedDestinations
        {
            get
            {
                if (_excludedDestinations == null)
                {
                    _excludedDestinations = new ExtensionCollection<ExcludedDestination>(this);
                }

                return _excludedDestinations;
            }
        }

        /// <summary>
        /// Status collection.
        /// </summary>
        public ExtensionCollection<Status> StatusList
        {
            get
            {
                if (_statusList == null)
                {
                    _statusList = new ExtensionCollection<Status>(this);
                }

                return _statusList;
            }
        }

        /// <summary>
        /// we need to ignore version changes because the namespace is hard-coded
        /// </summary>
        protected override void VersionInfoChanged() { }
    }
}
