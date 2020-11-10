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
using System.Globalization;

using EasyKeys.Google.GData.Client;
using EasyKeys.Google.GData.ContentForShopping.Elements;
using EasyKeys.Google.GData.Extensions;
using EasyKeys.Google.GData.Extensions.AppControl;

namespace EasyKeys.Google.GData.ContentForShopping
{
    /// <summary>
    /// Feed API customization class for defining Product feed.
    /// </summary>
    public class ProductEntry : AbstractEntry
    {
        private const string dateFormat8601 = "yyyy-MM-ddTHH:mm:ss.fffzzz";

        /// <summary>
        /// AdditionalImageLink collection
        /// </summary>
        private ExtensionCollection<AdditionalImageLink> _additionalImageLinks;

        /// <summary>
        /// Tax collection
        /// </summary>
        private ExtensionCollection<Tax> _taxRules;

        /// <summary>
        /// Shipping collection
        /// </summary>
        private ExtensionCollection<Shipping> _shippingRules;

        /// <summary>
        /// Feature collection
        /// </summary>
        private ExtensionCollection<Feature> _features;

        /// <summary>
        /// Size collection
        /// </summary>
        private ExtensionCollection<Size> _sizes;

        /// <summary>
        /// AdwordsLabels collection
        /// </summary>
        private ExtensionCollection<AdwordsLabels> _adwordsLabelsCollection;

        /// <summary>
        /// AdwordsQueryParam collection
        /// </summary>
        private ExtensionCollection<AdwordsQueryParam> _adwordsQueryParams;

        /// <summary>
        /// Custom attributes collection
        /// </summary>
        private ExtensionCollection<CustomAttribute> _customAttributes;

        /// <summary>
        /// Custom groups collection
        /// </summary>
        private ExtensionCollection<CustomGroup> _customGroups;

        public ProductEntry()
            : base()
        {
            AddExtension(new ProductId());
            AddExtension(new TargetCountry());
            AddExtension(new ContentLanguage());
            AddExtension(new ExpirationDate());
            AddExtension(new AdditionalImageLink());
            AddExtension(new Adult());
            AddExtension(new AdwordsGrouping());
            AddExtension(new AdwordsLabels());
            AddExtension(new AdwordsQueryParam());
            AddExtension(new AdwordsRedirect());
            AddExtension(new AgeGroup());
            AddExtension(new Author());
            AddExtension(new Availability());
            AddExtension(new Brand());
            AddExtension(new Channel());
            AddExtension(new Color());
            AddExtension(new Condition());
            AddExtension(new CustomAttribute());
            AddExtension(new CustomGroup());
            AddExtension(new Edition());
            AddExtension(new Feature());
            AddExtension(new FeaturedProduct());
            AddExtension(new Gender());
            AddExtension(new Genre());
            AddExtension(new GoogleProductCategory());
            AddExtension(new Gtin());
            AddExtension(new ImageLink());
            AddExtension(new ItemGroupId());
            AddExtension(new Manufacturer());
            AddExtension(new Material());
            AddExtension(new ModelNumber());
            AddExtension(new Mpn());
            AddExtension(new Pages());
            AddExtension(new Pattern());
            AddExtension(new Performance());
            AddExtension(new Price());
            AddExtension(new ProductReviewAverage());
            AddExtension(new ProductReviewCount());
            AddExtension(new ProductType());
            AddExtension(new ProductWeight());
            AddExtension(new Publisher());
            AddExtension(new Quantity());
            AddExtension(new SalePrice());
            AddExtension(new SalePriceEffectiveDate());
            AddExtension(new Shipping());
            AddExtension(new ShippingWeight());
            AddExtension(new Size());
            AddExtension(new Tax());
            AddExtension(new Year());
            AddExtension(new IdentifierExists());
            AddExtension(new UnitPricingMeasure());
            AddExtension(new UnitPricingBaseMeasure());
            AddExtension(new EnergyEfficiencyClass());
            AddExtension(new Multipack());

            // replacing the default app:control extension with the API-specific one
            RemoveExtension(new AppControl());
            AddExtension(new ProductControl());
        }

        /// <summary>
        /// returns the Content for Shopping API-specific app:control element
        /// </summary>
        /// <returns></returns>
        public new ProductControl AppControl
        {
            get
            {
                return FindExtension(
                    BaseNameTable.XmlElementPubControl,
                    BaseNameTable.NSAppPublishingFinal) as ProductControl;
            }

            set
            {
                ReplaceExtension(
                    BaseNameTable.XmlElementPubControl,
                    BaseNameTable.NSAppPublishingFinal,
                    value);
            }
        }

        /// <summary>
        /// Product Identifier.
        /// </summary>
        public string ProductId
        {
            get
            {
                return GetStringValue<ProductId>(
                    ContentForShoppingNameTable.ProductId,
                    ContentForShoppingNameTable.BaseNamespace);
            }

            set
            {
                SetStringValue<ProductId>(
                    value,
                    ContentForShoppingNameTable.ProductId,
                    ContentForShoppingNameTable.BaseNamespace);
            }
        }

        /// <summary>
        /// Target Country.
        /// </summary>
        public string TargetCountry
        {
            get
            {
                return GetStringValue<TargetCountry>(
                    ContentForShoppingNameTable.TargetCountry,
                    ContentForShoppingNameTable.BaseNamespace);
            }

            set
            {
                SetStringValue<TargetCountry>(
                    value,
                    ContentForShoppingNameTable.TargetCountry,
                    ContentForShoppingNameTable.BaseNamespace);
            }
        }

        /// <summary>
        /// Content Language.
        /// </summary>
        public string ContentLanguage
        {
            get
            {
                return GetStringValue<ContentLanguage>(
                    ContentForShoppingNameTable.ContentLanguage,
                    ContentForShoppingNameTable.BaseNamespace);
            }

            set
            {
                SetStringValue<ContentLanguage>(
                    value,
                    ContentForShoppingNameTable.ContentLanguage,
                    ContentForShoppingNameTable.BaseNamespace);
            }
        }

        /// <summary>
        /// Expiration Date.
        /// </summary>
        public DateTime ExpirationDate
        {
            get
            {
                string date = GetStringValue<ExpirationDate>(
                    ContentForShoppingNameTable.ExpirationDate,
                    ContentForShoppingNameTable.BaseNamespace);
                return DateTime.ParseExact(date, dateFormat8601, CultureInfo.InvariantCulture);
            }

            set
            {
                SetStringValue<ExpirationDate>(
                    value.ToString(dateFormat8601),
                    ContentForShoppingNameTable.ExpirationDate,
                    ContentForShoppingNameTable.BaseNamespace);
            }
        }

        /// <summary>
        /// Adult.
        /// </summary>
        public bool Adult
        {
            get
            {
                bool value;
                if (!bool.TryParse(
                    GetStringValue<Adult>(
                    ContentForShoppingNameTable.Adult,
                    ContentForShoppingNameTable.BaseNamespace), out value))
                {
                    value = false;
                }

                return value;
            }

            set
            {
                SetStringValue<Adult>(
                    value.ToString(),
                    ContentForShoppingNameTable.Adult,
                    ContentForShoppingNameTable.BaseNamespace);
            }
        }

        /// <summary>
        /// Adwords Grouping
        /// </summary>
        public string AdwordsGrouping
        {
            get
            {
                return GetStringValue<AdwordsGrouping>(
                    ContentForShoppingNameTable.AdwordsGrouping,
                    ContentForShoppingNameTable.ProductsNamespace);
            }

            set
            {
                SetStringValue<AdwordsGrouping>(
                    value,
                    ContentForShoppingNameTable.AdwordsGrouping,
                    ContentForShoppingNameTable.ProductsNamespace);
            }
        }

        /// <summary>
        /// Adwords Redirect
        /// </summary>
        public string AdwordsRedirect
        {
            get
            {
                return GetStringValue<AdwordsRedirect>(
                    ContentForShoppingNameTable.AdwordsRedirect,
                    ContentForShoppingNameTable.ProductsNamespace);
            }

            set
            {
                SetStringValue<AdwordsRedirect>(
                    value,
                    ContentForShoppingNameTable.AdwordsRedirect,
                    ContentForShoppingNameTable.ProductsNamespace);
            }
        }

        /// <summary>
        /// AgeGroup.
        /// </summary>
        public string AgeGroup
        {
            get
            {
                return GetStringValue<AgeGroup>(
                    ContentForShoppingNameTable.AgeGroup,
                    ContentForShoppingNameTable.ProductsNamespace);
            }

            set
            {
                SetStringValue<AgeGroup>(
                    value,
                    ContentForShoppingNameTable.AgeGroup,
                    ContentForShoppingNameTable.ProductsNamespace);
            }
        }

        /// <summary>
        /// Author.
        /// </summary>
        public string Author
        {
            get
            {
                return GetStringValue<AgeGroup>(
                    ContentForShoppingNameTable.Author,
                    ContentForShoppingNameTable.ProductsNamespace);
            }

            set
            {
                SetStringValue<Author>(
                    value,
                    ContentForShoppingNameTable.Author,
                    ContentForShoppingNameTable.ProductsNamespace);
            }
        }

        /// <summary>
        /// Availability.
        /// </summary>
        public string Availability
        {
            get
            {
                return GetStringValue<Availability>(
                    ContentForShoppingNameTable.Availability,
                    ContentForShoppingNameTable.ProductsNamespace);
            }

            set
            {
                SetStringValue<Availability>(
                    value,
                    ContentForShoppingNameTable.Availability,
                    ContentForShoppingNameTable.ProductsNamespace);
            }
        }

        /// <summary>
        /// FeaturedProduct.
        /// </summary>
        public bool FeaturedProduct
        {
            get
            {
                return (GetStringValue<FeaturedProduct>(
                    ContentForShoppingNameTable.FeaturedProduct,
                    ContentForShoppingNameTable.BaseNamespace) == "y");
            }

            set
            {
                string stringValue = value ? "y" : "n";
                SetStringValue<FeaturedProduct>(
                    stringValue,
                    ContentForShoppingNameTable.FeaturedProduct,
                    ContentForShoppingNameTable.BaseNamespace);
            }
        }

        /// <summary>
        /// Brand.
        /// </summary>
        public string Brand
        {
            get
            {
                return GetStringValue<Brand>(
                    ContentForShoppingNameTable.Brand,
                    ContentForShoppingNameTable.ProductsNamespace);
            }

            set
            {
                SetStringValue<Brand>(
                    value,
                    ContentForShoppingNameTable.Brand,
                    ContentForShoppingNameTable.ProductsNamespace);
            }
        }

        /// <summary>
        /// Channel.
        /// </summary>
        public string Channel
        {
            get
            {
                return GetStringValue<Channel>(
                    ContentForShoppingNameTable.Channel,
                    ContentForShoppingNameTable.ProductsNamespace);
            }

            set
            {
                SetStringValue<Channel>(
                    value,
                    ContentForShoppingNameTable.Channel,
                    ContentForShoppingNameTable.ProductsNamespace);
            }
        }

        /// <summary>
        /// Color.
        /// </summary>
        public string Color
        {
            get
            {
                return GetStringValue<Color>(
                    ContentForShoppingNameTable.Color,
                    ContentForShoppingNameTable.ProductsNamespace);
            }

            set
            {
                SetStringValue<Color>(
                    value,
                    ContentForShoppingNameTable.Color,
                    ContentForShoppingNameTable.ProductsNamespace);
            }
        }

        /// <summary>
        /// Condition.
        /// </summary>
        public string Condition
        {
            get
            {
                return GetStringValue<Condition>(
                    ContentForShoppingNameTable.Condition,
                    ContentForShoppingNameTable.ProductsNamespace);
            }

            set
            {
                SetStringValue<Condition>(
                    value,
                    ContentForShoppingNameTable.Condition,
                    ContentForShoppingNameTable.ProductsNamespace);
            }
        }

        /// <summary>
        /// Gtin.
        /// </summary>
        public string Gtin
        {
            get
            {
                return GetStringValue<Gtin>(
                    ContentForShoppingNameTable.Gtin,
                    ContentForShoppingNameTable.ProductsNamespace);
            }

            set
            {
                SetStringValue<Gtin>(
                    value,
                    ContentForShoppingNameTable.Gtin,
                    ContentForShoppingNameTable.ProductsNamespace);
            }
        }

        /// <summary>
        /// ItemGroupId.
        /// </summary>
        public string ItemGroupId
        {
            get
            {
                return GetStringValue<ItemGroupId>(
                    ContentForShoppingNameTable.ItemGroupId,
                    ContentForShoppingNameTable.ProductsNamespace);
            }

            set
            {
                SetStringValue<ItemGroupId>(
                    value,
                    ContentForShoppingNameTable.ItemGroupId,
                    ContentForShoppingNameTable.ProductsNamespace);
            }
        }

        /// <summary>
        /// Product Type.
        /// </summary>
        public string ProductType
        {
            get
            {
                return GetStringValue<ProductType>(
                    ContentForShoppingNameTable.ProductType,
                    ContentForShoppingNameTable.ProductsNamespace);
            }

            set
            {
                SetStringValue<ProductType>(
                    value,
                    ContentForShoppingNameTable.ProductType,
                    ContentForShoppingNameTable.ProductsNamespace);
            }
        }

        /// <summary>
        /// Edition.
        /// </summary>
        public string Edition
        {
            get
            {
                return GetStringValue<Edition>(
                    ContentForShoppingNameTable.Edition,
                    ContentForShoppingNameTable.ProductsNamespace);
            }

            set
            {
                SetStringValue<Edition>(
                    value,
                    ContentForShoppingNameTable.Edition,
                    ContentForShoppingNameTable.ProductsNamespace);
            }
        }

        /// <summary>
        /// Gender.
        /// </summary>
        public string Gender
        {
            get
            {
                return GetStringValue<Gender>(
                    ContentForShoppingNameTable.Gender,
                    ContentForShoppingNameTable.ProductsNamespace);
            }

            set
            {
                SetStringValue<Gender>(
                    value,
                    ContentForShoppingNameTable.Gender,
                    ContentForShoppingNameTable.ProductsNamespace);
            }
        }

        /// <summary>
        /// Genre.
        /// </summary>
        public string Genre
        {
            get
            {
                return GetStringValue<Genre>(
                    ContentForShoppingNameTable.Genre,
                    ContentForShoppingNameTable.ProductsNamespace);
            }

            set
            {
                SetStringValue<Genre>(
                    value,
                    ContentForShoppingNameTable.Genre,
                    ContentForShoppingNameTable.ProductsNamespace);
            }
        }

        /// <summary>
        /// GoogleProductCategory.
        /// </summary>
        public string GoogleProductCategory
        {
            get
            {
                return GetStringValue<GoogleProductCategory>(
                    ContentForShoppingNameTable.GoogleProductCategory,
                    ContentForShoppingNameTable.ProductsNamespace);
            }

            set
            {
                SetStringValue<GoogleProductCategory>(
                    value,
                    ContentForShoppingNameTable.GoogleProductCategory,
                    ContentForShoppingNameTable.ProductsNamespace);
            }
        }

        /// <summary>
        /// Manufacturer.
        /// </summary>
        public string Manufacturer
        {
            get
            {
                return GetStringValue<Manufacturer>(
                    ContentForShoppingNameTable.Manufacturer,
                    ContentForShoppingNameTable.ProductsNamespace);
            }

            set
            {
                SetStringValue<Manufacturer>(
                    value,
                    ContentForShoppingNameTable.Manufacturer,
                    ContentForShoppingNameTable.ProductsNamespace);
            }
        }

        /// <summary>
        /// Material.
        /// </summary>
        public string Material
        {
            get
            {
                return GetStringValue<Material>(
                    ContentForShoppingNameTable.Material,
                    ContentForShoppingNameTable.ProductsNamespace);
            }

            set
            {
                SetStringValue<Material>(
                    value,
                    ContentForShoppingNameTable.Material,
                    ContentForShoppingNameTable.ProductsNamespace);
            }
        }

        /// <summary>
        /// Model Number.
        /// </summary>
        public string ModelNumber
        {
            get
            {
                return GetStringValue<ModelNumber>(
                    ContentForShoppingNameTable.ModelNumber,
                    ContentForShoppingNameTable.ProductsNamespace);
            }

            set
            {
                SetStringValue<ModelNumber>(
                    value,
                    ContentForShoppingNameTable.ModelNumber,
                    ContentForShoppingNameTable.ProductsNamespace);
            }
        }

        /// <summary>
        /// Manufacturer's Part Number.
        /// </summary>
        public string Mpn
        {
            get
            {
                return GetStringValue<Mpn>(
                    ContentForShoppingNameTable.Mpn,
                    ContentForShoppingNameTable.ProductsNamespace);
            }

            set
            {
                SetStringValue<Mpn>(
                    value,
                    ContentForShoppingNameTable.Mpn,
                    ContentForShoppingNameTable.ProductsNamespace);
            }
        }

        /// <summary>
        /// Publisher.
        /// </summary>
        public string Publisher
        {
            get
            {
                return GetStringValue<Publisher>(
                    ContentForShoppingNameTable.Publisher,
                    ContentForShoppingNameTable.ProductsNamespace);
            }

            set
            {
                SetStringValue<Publisher>(
                    value,
                    ContentForShoppingNameTable.Publisher,
                    ContentForShoppingNameTable.ProductsNamespace);
            }
        }

        /// <summary>
        /// Pages.
        /// </summary>
        public int Pages
        {
            get
            {
                return Convert.ToInt32(GetStringValue<Pages>(
                    ContentForShoppingNameTable.Pages,
                    ContentForShoppingNameTable.ProductsNamespace));
            }

            set
            {
                SetStringValue<Pages>(
                    value.ToString(),
                    ContentForShoppingNameTable.Pages,
                    ContentForShoppingNameTable.ProductsNamespace);
            }
        }

        /// <summary>
        /// Pattern.
        /// </summary>
        public string Pattern
        {
            get
            {
                return GetStringValue<Pattern>(
                    ContentForShoppingNameTable.Pattern,
                    ContentForShoppingNameTable.ProductsNamespace);
            }

            set
            {
                SetStringValue<Pattern>(
                    value,
                    ContentForShoppingNameTable.Pattern,
                    ContentForShoppingNameTable.ProductsNamespace);
            }
        }

        /// <summary>
        /// ProductReviewAverage.
        /// </summary>
        // TODO: use a float instead of a string
        public string ProductReviewAverage
        {
            get
            {
                return GetStringValue<ProductReviewAverage>(
                    ContentForShoppingNameTable.ProductReviewAverage,
                    ContentForShoppingNameTable.ProductsNamespace);
            }

            set
            {
                SetStringValue<ProductReviewAverage>(
                    value,
                    ContentForShoppingNameTable.ProductReviewAverage,
                    ContentForShoppingNameTable.ProductsNamespace);
            }
        }

        /// <summary>
        /// ProductReviewCount.
        /// </summary>
        public int ProductReviewCount
        {
            get
            {
                return Convert.ToInt32(GetStringValue<ProductReviewCount>(
                    ContentForShoppingNameTable.ProductReviewCount,
                    ContentForShoppingNameTable.ProductsNamespace));
            }

            set
            {
                SetStringValue<ProductReviewCount>(
                    value.ToString(),
                    ContentForShoppingNameTable.ProductReviewCount,
                    ContentForShoppingNameTable.ProductsNamespace);
            }
        }

        /// <summary>
        /// Year.
        /// </summary>
        public int Year
        {
            get
            {
                return Convert.ToInt32(GetStringValue<Year>(
                    ContentForShoppingNameTable.Year,
                    ContentForShoppingNameTable.ProductsNamespace));
            }

            set
            {
                SetStringValue<Year>(
                    value.ToString(),
                    ContentForShoppingNameTable.Year,
                    ContentForShoppingNameTable.ProductsNamespace);
            }
        }

        /// <summary>
        /// Quantity.
        /// </summary>
        public int Quantity
        {
            get
            {
                return Convert.ToInt32(GetStringValue<Quantity>(
                    ContentForShoppingNameTable.Quantity,
                    ContentForShoppingNameTable.ProductsNamespace));
            }

            set
            {
                SetStringValue<Quantity>(
                    value.ToString(),
                    ContentForShoppingNameTable.Quantity,
                    ContentForShoppingNameTable.ProductsNamespace);
            }
        }

        /// <summary>
        /// Price.
        /// </summary>
        public Price Price
        {
            get
            {
                return FindExtension(
                    ContentForShoppingNameTable.Price,
                    ContentForShoppingNameTable.ProductsNamespace) as Price;
            }

            set
            {
                ReplaceExtension(
                    ContentForShoppingNameTable.Price,
                    ContentForShoppingNameTable.ProductsNamespace,
                    value);
            }
        }

        /// <summary>
        /// Performance.
        /// </summary>
        public Performance Performance
        {
            get
            {
                return FindExtension(
                    ContentForShoppingNameTable.Performance,
                    ContentForShoppingNameTable.BaseNamespace) as Performance;
            }

            set
            {
                ReplaceExtension(
                    ContentForShoppingNameTable.Performance,
                    ContentForShoppingNameTable.BaseNamespace,
                    value);
            }
        }

        /// <summary>
        /// Sale Price.
        /// </summary>
        public SalePrice SalePrice
        {
            get
            {
                return FindExtension(
                    ContentForShoppingNameTable.SalePrice,
                    ContentForShoppingNameTable.ProductsNamespace) as SalePrice;
            }

            set
            {
                ReplaceExtension(
                    ContentForShoppingNameTable.SalePrice,
                    ContentForShoppingNameTable.ProductsNamespace,
                    value);
            }
        }

        /// <summary>
        /// Sale Price Effective Date.
        /// </summary>
        public string SalePriceEffectiveDate
        {
            get
            {
                return GetStringValue<SalePriceEffectiveDate>(
                    ContentForShoppingNameTable.SalePriceEffectiveDate,
                    ContentForShoppingNameTable.ProductsNamespace);
            }

            set
            {
                SetStringValue<SalePriceEffectiveDate>(
                    value,
                    ContentForShoppingNameTable.SalePriceEffectiveDate,
                    ContentForShoppingNameTable.ProductsNamespace);
            }
        }

        /// <summary>
        /// Shipping Weight.
        /// </summary>
        public ShippingWeight ShippingWeight
        {
            get
            {
                return FindExtension(
                    ContentForShoppingNameTable.ShippingWeight,
                    ContentForShoppingNameTable.ProductsNamespace) as ShippingWeight;
            }

            set
            {
                ReplaceExtension(
                    ContentForShoppingNameTable.ShippingWeight,
                    ContentForShoppingNameTable.ProductsNamespace,
                    value);
            }
        }

        /// <summary>
        /// Product Weight.
        /// </summary>
        public ProductWeight ProductWeight
        {
            get
            {
                return FindExtension(
                    ContentForShoppingNameTable.ProductWeight,
                    ContentForShoppingNameTable.ProductsNamespace) as ProductWeight;
            }

            set
            {
                ReplaceExtension(
                    ContentForShoppingNameTable.ProductWeight,
                    ContentForShoppingNameTable.ProductsNamespace,
                    value);
            }
        }

        /// <summary>
        /// ImageLink.
        /// </summary>
        public ImageLink ImageLink
        {
            get
            {
                return FindExtension(
                    ContentForShoppingNameTable.ImageLink,
                    ContentForShoppingNameTable.BaseNamespace) as ImageLink;
            }

            set
            {
                ReplaceExtension(
                    ContentForShoppingNameTable.ImageLink,
                    ContentForShoppingNameTable.BaseNamespace,
                    value);
            }
        }

        /// <summary>
        /// Identifier exist.
        /// </summary>
        public bool IdentifierExists
        {
            get
            {
                bool value;
                if (!bool.TryParse(
                    GetStringValue<IdentifierExists>(
                    ContentForShoppingNameTable.IdentifierExists,
                    ContentForShoppingNameTable.ProductsNamespace), out value))
                {
                    value = false;
                }

                return value;
            }

            set
            {
                SetStringValue<IdentifierExists>(
                    value.ToString(),
                    ContentForShoppingNameTable.IdentifierExists,
                    ContentForShoppingNameTable.ProductsNamespace);
            }
        }

        /// <summary>
        /// Gets or sets the unit pricing measure.
        /// </summary>
        /// <value>
        /// The unit pricing measure.
        /// </value>
        public UnitPricingMeasure UnitPricingMeasure
        {
            get
            {
                return FindExtension(
                    ContentForShoppingNameTable.UnitPricingMeasure,
                    ContentForShoppingNameTable.ProductsNamespace) as UnitPricingMeasure;
            }

            set
            {
                ReplaceExtension(
                    ContentForShoppingNameTable.UnitPricingMeasure,
                    ContentForShoppingNameTable.ProductsNamespace,
                    value);
            }
        }

        /// <summary>
        /// Gets or sets the unit pricing base measure.
        /// </summary>
        /// <value>
        /// The unit pricing base measure.
        /// </value>
        public UnitPricingBaseMeasure UnitPricingBaseMeasure
        {
            get
            {
                return FindExtension(
                    ContentForShoppingNameTable.UnitPricingBaseMeasure,
                    ContentForShoppingNameTable.ProductsNamespace) as UnitPricingBaseMeasure;
            }

            set
            {
                ReplaceExtension(
                    ContentForShoppingNameTable.UnitPricingBaseMeasure,
                    ContentForShoppingNameTable.ProductsNamespace,
                    value);
            }
        }

        /// <summary>
        /// Gets or sets the energy efficiency class.
        /// </summary>
        /// <value>
        /// The energy efficiency class.
        /// </value>
        public string EnergyEfficiencyClass
        {
            get
            {
                return GetStringValue<EnergyEfficiencyClass>(
                    ContentForShoppingNameTable.EnergyEfficiencyClass,
                    ContentForShoppingNameTable.ProductsNamespace);
            }

            set
            {
                SetStringValue<EnergyEfficiencyClass>(
                    value,
                    ContentForShoppingNameTable.EnergyEfficiencyClass,
                    ContentForShoppingNameTable.ProductsNamespace);
            }
        }

        /// <summary>
        /// Gets or sets the merchant multipack quantity.
        /// </summary>
        /// <value>
        /// The merchant multipack quantity.
        /// </value>
        public string Multipack
        {
            get
            {
                return GetStringValue<Multipack>(
                    ContentForShoppingNameTable.Multipack,
                    ContentForShoppingNameTable.ProductsNamespace);
            }

            set
            {
                SetStringValue<Multipack>(
                    value,
                    ContentForShoppingNameTable.Multipack,
                    ContentForShoppingNameTable.ProductsNamespace);
            }
        }

        /// <summary>
        /// AdditionalImageLink collection.
        /// </summary>
        public ExtensionCollection<AdditionalImageLink> AdditionalImageLinks
        {
            get
            {
                if (_additionalImageLinks == null)
                {
                    _additionalImageLinks = new ExtensionCollection<AdditionalImageLink>(this);
                }

                return _additionalImageLinks;
            }
        }

        /// <summary>
        /// Tax collection.
        /// </summary>
        public ExtensionCollection<Tax> TaxRules
        {
            get
            {
                if (_taxRules == null)
                {
                    _taxRules = new ExtensionCollection<Tax>(this);
                }

                return _taxRules;
            }
        }

        /// <summary>
        /// Shipping collection.
        /// </summary>
        public ExtensionCollection<Shipping> ShippingRules
        {
            get
            {
                if (_shippingRules == null)
                {
                    _shippingRules = new ExtensionCollection<Shipping>(this);
                }

                return _shippingRules;
            }
        }

        /// <summary>
        /// Feature collection.
        /// </summary>
        public ExtensionCollection<Feature> Features
        {
            get
            {
                if (_features == null)
                {
                    _features = new ExtensionCollection<Feature>(this);
                }

                return _features;
            }
        }

        /// <summary>
        /// Size collection.
        /// </summary>
        public ExtensionCollection<Size> Sizes
        {
            get
            {
                if (_sizes == null)
                {
                    _sizes = new ExtensionCollection<Size>(this);
                }

                return _sizes;
            }
        }

        /// <summary>
        /// AdwordsLabels collection
        /// </summary>
        public ExtensionCollection<AdwordsLabels> AdwordsLabelsCollection
        {
            get
            {
                if (_adwordsLabelsCollection == null)
                {
                    _adwordsLabelsCollection = new ExtensionCollection<AdwordsLabels>(this);
                }

                return _adwordsLabelsCollection;
            }
        }

        /// <summary>
        /// AdwordsQueryParam collection
        /// </summary>
        public ExtensionCollection<AdwordsQueryParam> AdwordsQueryParams
        {
            get
            {
                if (_adwordsQueryParams == null)
                {
                    _adwordsQueryParams = new ExtensionCollection<AdwordsQueryParam>(this);
                }

                return _adwordsQueryParams;
            }
        }

        /// <summary>
        /// Custom attribute collection.
        /// </summary>
        public ExtensionCollection<CustomAttribute> CustomAttributes
        {
            get
            {
                if (_customAttributes == null)
                {
                    _customAttributes = new ExtensionCollection<CustomAttribute>(this);
                }

                return _customAttributes;
            }
        }

        /// <summary>
        /// Custom group collection.
        /// </summary>
        public ExtensionCollection<CustomGroup> Groups
        {
            get
            {
                if (_customGroups == null)
                {
                    _customGroups = new ExtensionCollection<CustomGroup>(this);
                }

                return _customGroups;
            }
        }

        /// <summary>
        /// gd:errors element
        /// </summary>
        /// <returns></returns>
        public ExtensionCollection<BatchError> BatchErrors
        {
            get
            {
                return Content.BatchErrors.Errors;
            }
        }
    }
}
