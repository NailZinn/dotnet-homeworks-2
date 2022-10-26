using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Hw7.MyHtmlServices;

public static class HtmlHelperExtensions
{
    public static IHtmlContent MyEditorForModel(this IHtmlHelper helper)
    {
        var model = helper.ViewData.Model;
        var properties = helper.ViewData.ModelMetadata.ModelType.GetProperties();

        return model is null ? ProcessGetRequest(properties) : ProcessPostRequest(properties, model);
    }

    private static IHtmlContent ProcessGetRequest(PropertyInfo[] properties)
    {
        var builder = new HtmlContentBuilder();

        foreach (var property in properties)
        {
            var name = property.Name;
            var type = property.PropertyType;
            var displayAttribute = property.GetCustomAttribute<DisplayAttribute>();
            var labelContent = GetLabelContent(property.Name, displayAttribute);

            if (type.IsEnum)
            {
                var enumNames = type.GetEnumNames();
                var selectTag = GetSelectTag(enumNames);

                builder.AppendHtmlLine(
                    $"<div>{GetLabelTag($"{name}", labelContent)}" +
                    $"{selectTag}<br></div>");
            }
            else
            {
                var contentType = type == typeof(string) ? "text" : "number";

                builder.AppendHtmlLine(
                    $"<div>{GetLabelTag($"{name}", labelContent)}" +
                    $"<input id=\"{name}\" type=\"{contentType}\"><br></div>");
            }
        }

        return builder;
    }

    private static IHtmlContent ProcessPostRequest(PropertyInfo[] properties, object? model)
    {
        var builder = new HtmlContentBuilder();

        foreach (var property in properties)
        {
            var validationAttrs = property.GetCustomAttributes<ValidationAttribute>();
            var propertyVal = property.GetValue(model);

            foreach (var validator in validationAttrs)
            {
                if (!validator.IsValid(propertyVal))
                {
                    builder.AppendHtmlLine(
                        $"<div>{GetLabelTag($"{property.Name}", string.Empty)}" +
                        $"<span>{validator.ErrorMessage}</span><br></div>");
                }
            }
        }

        return builder;
    }

    private static string GetLabelTag(string forAttribute, string content)
        => $"<label for=\"{forAttribute}\">{content}</label><br>";

    private static string GetSelectTag(string[] data)
        => $"<select>{string.Join("", GetOptionTags(data))}</select>";
    
    private static string[] GetOptionTags(string[] data)
        => data.Select(value => $"<option value=\"{value}\">{value}</option>").ToArray();

    private static string GetLabelContent(string propertyName, DisplayAttribute? attribute)
        => attribute is null ? SplitStringByUpperCase(propertyName) : attribute.Name;

    private static string SplitStringByUpperCase(string entry)
        => string.Join(' ', Regex.Split(entry, @"(?<!^)(?=[A-Z])"));
} 