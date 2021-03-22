using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;

public class MvcHelpers
{
    public IEnumerable<AreaRegistration> GetAllAreasRegistered()
    {
        var assembly = this.GetType().Assembly;
        var areaTypes = assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(AreaRegistration)));
        var areas = new List<AreaRegistration>();
        foreach (var type in areaTypes)
        {

            areas.Add((AreaRegistration)Activator.CreateInstance(type));
        }
        return areas;
    }
    public object[] GetAreas()
    {
        var areaNames = RouteTable.Routes.OfType<Route>()
  .Where(d => d.DataTokens != null && d.DataTokens.ContainsKey("area"))
  .Select(r => r.DataTokens["area"]).ToArray();
        return areaNames;
    }

    private static List<Type> GetSubClasses<T>()
    {
        return Assembly.GetCallingAssembly().GetTypes().Where(
            type => type.IsSubclassOf(typeof(T))).ToList();
    }

    public List<string> GetControllerNames()
    {
        List<string> controllerNames = new List<string>();
        GetSubClasses<Controller>().ForEach(
            type => controllerNames.Add(type.Name.Replace("Controller", "")));
        return controllerNames;
    }

    public List<string> GetActionName(string controllerName)
    {
        var types =
            from a in AppDomain.CurrentDomain.GetAssemblies()
            from t in a.GetTypes()
            where typeof(IController).IsAssignableFrom(t) &&
                    string.Equals(controllerName + "Controller", t.Name, StringComparison.OrdinalIgnoreCase)
            select t;

        var controllerType = types.FirstOrDefault();

        if (controllerType == null)
        {
            return Enumerable.Empty<string>().ToList();
        }
        return new ReflectedControllerDescriptor(controllerType)
            .GetCanonicalActions().Select(x => x.ActionName)
            .ToList();
    }


}

public static class Helper
{
    public static object GetPropertyValue(this object T, string propName)
    {
        return (T.GetType().GetProperty(propName) == null) ? null : T.GetType().GetProperty(propName).GetValue(T, null);
    }

    public static object GetPropertyValue(this object T, string propName, string lang)
    {
        if (string.IsNullOrEmpty(lang))
        {
            return T.GetPropertyValue(propName);
        }
        else
        {
            return T.GetPropertyValue(propName + lang);
        }
    }

    //Update Not Null Field From Other Object
    public static void UpdateDataFrom<TD, TS>(this TD dest, TS source)
    {
        var sourceProps = typeof(TS).GetProperties().Where(x => x.CanRead).ToList();
        var destProps = typeof(TD).GetProperties()
            .Where(x => x.CanWrite)
            .ToList();

        foreach (var sourceProp in sourceProps)
        {
            if (destProps.Any(x => x.Name == sourceProp.Name) && sourceProp.GetValue(source, null) != null)
            {
                var p = destProps.First(x => x.Name == sourceProp.Name);
                if (p.CanWrite)
                { // check if the property can be set or no.
                    p.SetValue(dest, sourceProp.GetValue(source, null), null);
                }
            }
        }
    }
}
