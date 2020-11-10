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
using System.Globalization;

using EasyKeys.Google.GData.Client;

namespace EasyKeys.Google.GData.Extensions
{
    /// <summary>
    /// The gd:rating tag specifies the rating that you are assigning to a resource (in a request to add a rating) 
    /// or the current average rating of the resource based on aggregated user ratings
    /// </summary>
    public class Rating : SimpleAttribute
    {
        /// <summary>
        /// default constructor for gd:rating. This will set min and max
        /// to 1 and 5 respectively
        /// </summary>
        public Rating()
        : base(
            GDataParserNameTable.XmlRatingElement,
               GDataParserNameTable.gDataPrefix,
               GDataParserNameTable.gNamespace)
        {
            Attributes.Add(GDataParserNameTable.XmlAttributeMin, "1");
            Attributes.Add(GDataParserNameTable.XmlAttributeMax, "5");
            Attributes.Add(GDataParserNameTable.XmlAttributeNumRaters, null);
            Attributes.Add(GDataParserNameTable.XmlAttributeAverage, null);
        }

        /// <summary>
        /// The min attribute specifies the minimum rating that can be assigned to a resource. This value must be 1.
        /// </summary>
        /// <returns></returns>
        public int Min
        {
            get
            {
                return Int16.Parse(Attributes[GDataParserNameTable.XmlAttributeMin] as string);
            }

            set
            {
                if (value != 1)
                    throw new ArgumentOutOfRangeException("Min must be 1");

                Attributes[GDataParserNameTable.XmlAttributeMin] = value.ToString(CultureInfo.InvariantCulture);
            }
        }

        /// <summary>
        /// The max attribute specifies the maximum rating that can be assigned to a resource. This value must be 5.
        /// </summary>
        /// <returns></returns>
        public int Max
        {
            get
            {
                return Int16.Parse(Attributes[GDataParserNameTable.XmlAttributeMax] as string);
            }

            set
            {
                if (value != 5)
                    throw new ArgumentOutOfRangeException("Max must be 5");

                Attributes[GDataParserNameTable.XmlAttributeMax] = value.ToString(CultureInfo.InvariantCulture);
            }
        }

        /// <summary>
        /// The numRaters attribute indicates how many people have rated the resource. This attribute is not used 
        /// in a request to add a rating
        /// </summary>
        /// <returns></returns>
        public int NumRaters
        {
            get
            {
                return Int32.Parse(Attributes[GDataParserNameTable.XmlAttributeNumRaters] as string);
            }

            set
            {
                Attributes[GDataParserNameTable.XmlAttributeNumRaters] = value.ToString(CultureInfo.InvariantCulture);
            }
        }

        /// <summary>
        /// The average attribute indicates the average rating given to the resource.
        /// This attribute is not used in a request to add a rating.
        /// </summary>
        /// <returns></returns>
        public double Average
        {
            get
            {
                return double.Parse(Attributes[GDataParserNameTable.XmlAttributeAverage] as string, CultureInfo.InvariantCulture);
            }

            set
            {
                Attributes[GDataParserNameTable.XmlAttributeAverage] = value.ToString(CultureInfo.InvariantCulture);
            }
        }

        //////////////////////////////////////////////////////////////////////

        /// <summary>Accessor for "value" attribute.</summary> 
        /// <returns> </returns>
        //////////////////////////////////////////////////////////////////////
        public new int Value
        {
            get
            {
                return Int16.Parse(Attributes[BaseNameTable.XmlValue] as string);
            }

            set
            {
                if (value < 1 || value > 5)
                    throw new ArgumentOutOfRangeException("value must be between 1 and 5");

                Attributes[BaseNameTable.XmlValue] = value.ToString(CultureInfo.InvariantCulture);
            }
        }

        // end of accessor public string Value
    }
}
