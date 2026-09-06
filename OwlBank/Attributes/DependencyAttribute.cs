[AttributeUsage(AttributeTargets.Class)]
public class DependencyAttribute : Attribute
{
    public Type ServiceType { get; }
    public ServiceLifetime Lifetime { get; }

    public DependencyAttribute(Type serviceType, ServiceLifetime lifetime = ServiceLifetime.Scoped)
    {
        ServiceType = serviceType;
        Lifetime = lifetime;
    }
}