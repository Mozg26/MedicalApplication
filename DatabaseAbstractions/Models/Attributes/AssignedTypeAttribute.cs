namespace DatabaseAbstractions.Models.Attributes
{
    [AttributeUsage(AttributeTargets.All)]
    public class AssignedTypeAttribute(Type type) : Attribute
    {
        public Type Type { get; set; } = type;
    }
}
