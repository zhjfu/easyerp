﻿namespace EasyErp.Core.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    public interface ITypeFinder
    {
        IList<Assembly> GetAssemblies();
        IEnumerable<Type> FindClassesOfType(Type assignTypeFrom, bool onlyConcreteClasses = true);

        IEnumerable<Type> FindClassesOfType(
            Type assignTypeFrom,
            IEnumerable<Assembly> assemblies,
            bool onlyConcreteClasses = true);

        IEnumerable<Type> FindClassesOfType<T>(bool onlyConcreteClasses = true);
        IEnumerable<Type> FindClassesOfType<T>(IEnumerable<Assembly> assemblies, bool onlyConcreteClasses = true);
    }
}