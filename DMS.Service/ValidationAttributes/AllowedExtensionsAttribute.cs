using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

public class AllowedExtensionsAttribute : ValidationAttribute
{
    private readonly string[] _extensions;
    public AllowedExtensionsAttribute(string[] extensions)
    {
        _extensions = extensions;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var file = value as IFormFile;
        if (file != null)
        {
            var extension = Path.GetExtension(file.FileName).ToLower();
            if (!_extensions.Contains(extension))
            {
                return new ValidationResult($"This file extension is not allowed. Allowed extensions: {string.Join(", ", _extensions)}");
            }
        }

        return ValidationResult.Success;
    }
}
