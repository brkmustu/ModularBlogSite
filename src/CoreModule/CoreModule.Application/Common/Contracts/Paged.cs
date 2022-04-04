using System.Runtime.Serialization;

namespace CoreModule.Application.Common.Contracts;

[DataContract(Name = nameof(Paged<T>) + "Of{0}")]
public class Paged<T>
{
    /// <summary>Information about the requested page.</summary>
    [DataMember] public PageInfo Paging { get; set; }

    /// <summary>The list of items for the given page.</summary>
    [DataMember] public T[] Items { get; set; }
}
