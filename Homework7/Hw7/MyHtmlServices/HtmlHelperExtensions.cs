using Hw7.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Linq;
using System.Text.RegularExpressions;

namespace Hw7.MyHtmlServices;

public static class HtmlHelperExtensions
{
    public static IHtmlContent MyEditorForModel(this IHtmlHelper helper)
    {
        var model = helper.ViewData.Model;
        var properties = helper.ViewData.ModelMetadata.ModelType.GetProperties();

        return model == null ? ProcessGetRequest(properties) : ProcessPostRequest(properties, model);
    }

    private static IHtmlContent ProcessGetRequest(PropertyInfo[] properties)
    {
        var builder = new HtmlContentBuilder();
        var displayAttrs = properties.Select(prop => prop.GetCustomAttribute<DisplayAttribute>());
        var zip = properties.Zip(displayAttrs);

        foreach (var (property, displayAttribute) in zip)
        {
            var name = property.Name;
            var type = property.PropertyType;
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
        var validationAttrs = properties.Select(prop => prop.GetCustomAttributes<ValidationAttribute>());
        var zip = properties.Zip(validationAttrs);

        foreach (var (property, validationAttributes) in zip)
        {
            var propertyVal = property.GetValue(model);

            foreach (var validator in validationAttributes)
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
    {
        return
            "<select>" +
                string.Join("", GetOptionTags(data)) +
            "</select>";
    }

    private static string[] GetOptionTags(string[] data)
    {
        return Enumerable
            .Range(0, data.Length)
            .Select(i => $"<option value=\"{data[i]}\">{data[i]}</option>")
            .ToArray();
    }

    private static string GetLabelContent(string propertyName, DisplayAttribute? attribute)
        => attribute == null ? SplitStringByUpperCase(propertyName) : attribute.Name;

    private static string SplitStringByUpperCase(string entry)
        => string.Join(' ', Regex.Split(entry, @"(?<!^)(?=[A-Z])"));
} 