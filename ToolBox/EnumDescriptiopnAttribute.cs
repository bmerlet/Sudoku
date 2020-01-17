//
// Copyright 2016 Benoit J. Merlet
//

using System;
using System.Collections.Generic;
using System.Reflection;

namespace Toolbox
{
    /// <summary>
    /// Field attribute holding a string description for an enum value
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class EnumDescriptionAttribute : Attribute
    {

        #region Constructors

        public EnumDescriptionAttribute()
        {
        }

        public EnumDescriptionAttribute(string description)
        {
            this.Description = description;
        }

        #endregion

        #region Properties

        public string Description
        {
            get;
            set;
        }

        #endregion

        #region Static public methods

        /// <summary>Returns the description of the specified enum value.</summary>
        /// <param name="value">The value of the enum for which to return the description.</param>
        /// <returns>A description of the enum, or the enum name if no description exists.</returns>
        public static string GetDescription(object value)
        {
            if (value == null)
            {
                return null;
            }

            Type type = value.GetType();

            // Make sure the object is an enum.
            if (!type.IsEnum)
            {
                throw new ApplicationException("Value parameter must be an enum.");
            }

            // Get field info
            FieldInfo fieldInfo = type.GetField(value.ToString());
            if (fieldInfo == null)
            {
                return null;
            }

            // Get EnumDescriptionAttribute attributes for this field
            object[] descriptionAttributes = fieldInfo.GetCustomAttributes(typeof(EnumDescriptionAttribute), false);

            // If no DescriptionAttribute exists for this enum value, check the DescriptiveEnumEnforcementAttribute and decide how to proceed.
            if (descriptionAttributes == null || descriptionAttributes.Length == 0)
            {
                return null;
            }
            else if (descriptionAttributes.Length > 1)
            {
                throw new ApplicationException("Too many EnumDescription attributes for enum '" + type.Name + "', value '" + value.ToString() + "'.");
            }

            // Return the value of the DescriptionAttribute.
            return ((EnumDescriptionAttribute)descriptionAttributes[0]).Description;
        }

        // Return all descriptions for an enum
        public static string[] GetDescriptions<T>()
        {
            return GetDescriptions(typeof(T));
        }

        public static string[] GetDescriptions(Type enumType)
        {
            var strings = new List<string>();
            foreach (var enumVal in Enum.GetValues(enumType))
            {
                var str = GetDescription(enumVal);
                if (str != null && !strings.Contains(str))
                {
                    strings.Add(str);
                }
            }

            return strings.ToArray();
        }

        // Match a description to an enum value
        public static T MatchDescription<T>(object val)
        {
            var value = val as string;
            if (value != null)
            {
                foreach (var enumVal in Enum.GetValues(typeof(T)))
                {
                    if (value == GetDescription(enumVal))
                    {
                        return (T)enumVal;
                    }
                }
            }

            return default(T);
        }

        #endregion
    }
}
