// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Diagnostics;
using System.Reflection;

// ReSharper disable once CheckNamespace

namespace System
{
    [DebuggerStepThrough]
    internal static class SharedTypeExtensions
    {
        public static Type UnwrapNullableType(this Type type)
        {
            return Nullable.GetUnderlyingType(type) ?? type;
        }

        public static bool IsNullableType(this Type type)
        {
            var typeInfo = type.GetTypeInfo();

            return !typeInfo.IsValueType
                   || (typeInfo.IsGenericType
                       && typeInfo.GetGenericTypeDefinition() == typeof(Nullable<>));
        }

        public static Type MakeNullable(this Type type)
        {
            if (type.IsNullableType())
            {
                return type;
            }

            return typeof(Nullable<>).MakeGenericType(type);
        }

        public static bool IsInteger(this Type type)
        {
            type = type.UnwrapNullableType();

            return type == typeof(int)
                   || type == typeof(long)
                   || type == typeof(short)
                   || type == typeof(byte)
                   || type == typeof(uint)
                   || type == typeof(ulong)
                   || type == typeof(ushort)
                   || type == typeof(sbyte);
        }

        private static bool IsNonIntegerPrimitive(this Type type)
        {
            type = type.UnwrapNullableType();

            return type == typeof(bool)
                   || type == typeof(byte[])
                   || type == typeof(char)
                   || type == typeof(DateTime)
                   || type == typeof(DateTimeOffset)
                   || type == typeof(decimal)
                   || type == typeof(double)
                   || type == typeof(float)
                   || type == typeof(Guid)
                   || type == typeof(string)
                   || type == typeof(TimeSpan)
                   || type.GetTypeInfo().IsEnum;
        }

        public static bool IsPrimitive(this Type type)
        {
            return type.IsInteger()
                   || type.IsNonIntegerPrimitive();
        }
    }
}
