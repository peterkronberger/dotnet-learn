using System.Collections;
using System.Globalization;
using System.Reflection;

namespace Playground;

/// <summary>A local variable at one step. <c>RefId</c> is set for reference-type values (points into the heap list).</summary>
public sealed record VariableInfo(string Name, string Text, int? RefId);

/// <summary>An object of the simplified heap model.</summary>
public sealed record HeapObjectInfo(int Id, string Type, string Text);

/// <summary>State before the statement in <c>Line</c> runs (<see cref="StepTraceRewriter.EndLine"/> = program end).</summary>
public sealed record StepInfo(int Line, IReadOnlyList<VariableInfo> Variables, IReadOnlyList<HeapObjectInfo> Heap, string Output);

public sealed record StepTrace(IReadOnlyList<StepInfo> Steps, bool Truncated);

/// <summary>
/// Receives the calls inserted by <see cref="StepTraceRewriter"/> and builds the step list (spike S5).
/// Reference-type values become numbered heap objects; strings and value types are shown inline.
/// Only collections and public members are read, never lazy sequences (enumerating them would run user code twice).
/// </summary>
public sealed class StepRecorder(Func<string> currentOutput)
{
    public const int MaxSteps = 500;
    private const int MaxElements = 8;
    private const int MaxHeapObjects = 20;
    private const int MaxTextLength = 40;

    private static readonly Dictionary<Type, string> Keywords = new()
    {
        [typeof(int)] = "int", [typeof(long)] = "long", [typeof(short)] = "short", [typeof(byte)] = "byte",
        [typeof(double)] = "double", [typeof(float)] = "float", [typeof(decimal)] = "decimal",
        [typeof(bool)] = "bool", [typeof(char)] = "char", [typeof(string)] = "string", [typeof(object)] = "object",
    };

    private readonly Dictionary<object, int> _ids = new(ReferenceEqualityComparer.Instance);
    private readonly List<StepInfo> _steps = [];

    public bool Truncated { get; private set; }

    public StepTrace ToTrace() => new(_steps, Truncated);

    public void Record(int line, string[] names, object?[] values)
    {
        if (_steps.Count >= MaxSteps)
        {
            Truncated = true;
            return;
        }

        var queue = new Queue<object>();
        var variables = new List<VariableInfo>();
        for (var i = 0; i < names.Length; i++)
        {
            variables.Add(DescribeVariable(names[i], values[i], queue));
        }

        var heap = new SortedDictionary<int, HeapObjectInfo>();
        while (queue.Count > 0 && heap.Count < MaxHeapObjects)
        {
            var item = queue.Dequeue();
            var id = _ids[item];
            if (!heap.ContainsKey(id))
            {
                heap[id] = new HeapObjectInfo(id, TypeName(item.GetType()), DescribeObject(item, queue));
            }
        }

        _steps.Add(new StepInfo(line, variables, heap.Values.ToList(), currentOutput()));
    }

    private VariableInfo DescribeVariable(string name, object? value, Queue<object> queue)
    {
        if (IsInline(value))
        {
            return new VariableInfo(name, FormatInline(value), null);
        }

        var id = IdOf(value!, queue);
        return new VariableInfo(name, $"#{id}", id);
    }

    private static bool IsInline(object? value) => value is null or string || value.GetType().IsValueType;

    private int IdOf(object value, Queue<object> queue)
    {
        if (!_ids.TryGetValue(value, out var id))
        {
            id = _ids.Count + 1;
            _ids[value] = id;
        }

        queue.Enqueue(value);
        return id;
    }

    private static string FormatInline(object? value) => value switch
    {
        null => "null",
        string s => $"\"{Shorten(s)}\"",
        char c => $"'{c}'",
        bool b => b ? "true" : "false",
        IFormattable f => f.ToString(null, CultureInfo.InvariantCulture),
        _ => Shorten(Safe(() => value.ToString()) ?? value.GetType().Name),
    };

    private string FormatElement(object? value, Queue<object> queue) =>
        IsInline(value) ? FormatInline(value) : $"#{IdOf(value!, queue)}";

    private string DescribeObject(object item, Queue<object> queue)
    {
        if (item is Delegate)
        {
            return "(delegate)";
        }

        if (IsCollection(item))
        {
            var parts = new List<string>();
            var count = 0;
            foreach (var element in (IEnumerable)item)
            {
                if (++count > MaxElements)
                {
                    parts.Add("…");
                    break;
                }

                parts.Add(FormatElement(element, queue));
            }

            return $"[{string.Join(", ", parts)}]";
        }

        var type = item.GetType();
        var members = new List<string>();
        foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.Instance))
        {
            members.Add($"{field.Name} = {FormatElement(Safe(() => field.GetValue(item)), queue)}");
        }

        foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                     .Where(p => p.CanRead && p.GetIndexParameters().Length == 0))
        {
            members.Add($"{property.Name} = {FormatElement(Safe(() => property.GetValue(item)), queue)}");
        }

        return members.Count == 0 ? "{ }" : $"{{ {string.Join(", ", members)} }}";
    }

    private static bool IsCollection(object item) =>
        item is Array or ICollection
        || item.GetType().GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICollection<>));

    private static T? Safe<T>(Func<T> read)
    {
        try
        {
            return read();
        }
        catch (Exception)
        {
            return default;
        }
    }

    private static string Shorten(string text) =>
        text.Length <= MaxTextLength ? text : text[..MaxTextLength] + "…";

    private static string TypeName(Type type)
    {
        if (Keywords.TryGetValue(type, out var keyword))
        {
            return keyword;
        }

        if (type.IsArray)
        {
            return TypeName(type.GetElementType()!) + "[]";
        }

        if (!type.IsGenericType)
        {
            return type.Name;
        }

        var name = type.Name[..type.Name.IndexOf('`')];
        return $"{name}<{string.Join(", ", type.GetGenericArguments().Select(TypeName))}>";
    }
}
