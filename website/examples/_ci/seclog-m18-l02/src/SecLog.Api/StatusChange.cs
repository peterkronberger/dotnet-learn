using System.ComponentModel.DataAnnotations;
using SecLog.Model;

namespace SecLog.Api;

/// <summary>The JSON body of <c>PUT /incidents/{id}/status</c>.</summary>
public sealed record StatusChange(
    [property: Required,
        AllowedValues(
            StatusNames.Open,
            StatusNames.Investigating,
            StatusNames.Closed)]
    string Status);
