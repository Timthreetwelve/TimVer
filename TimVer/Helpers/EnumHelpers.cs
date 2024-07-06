// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace TimVer.Helpers;

internal static class EnumHelpers
{
    internal static string GetEnumDescription(Enum enumObj)
    {
        FieldInfo? field = enumObj.GetType().GetField(enumObj.ToString());
        object[] attrArray = field!.GetCustomAttributes(false);

        if (attrArray.Length > 0)
        {
            DescriptionAttribute? attribute = attrArray[0] as DescriptionAttribute;
            return attribute!.Description;
        }
        else
        {
            return enumObj.ToString();
        }
    }
}
