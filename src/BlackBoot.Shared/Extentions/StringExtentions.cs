namespace BlackBoot.Shared.Extentions;

public static class StringExtentions
{

    public static bool HasValue(this string value, bool ignoreWhiteSpace = true)
    {
        return ignoreWhiteSpace ? !string.IsNullOrWhiteSpace(value) : !string.IsNullOrEmpty(value);
    }
}

