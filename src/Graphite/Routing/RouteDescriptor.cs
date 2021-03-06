﻿using Graphite.Extensions;
using Graphite.Reflection;

namespace Graphite.Routing
{
    public class RouteDescriptor
    {
        public RouteDescriptor(
            string method, string url,
            ParameterDescriptor[] urlParameters,
            ParameterDescriptor[] wildcardParameters,
            ParameterDescriptor[] querystringParameters,
            ParameterDescriptor requestParameter,
            TypeDescriptor responseType)
        {
            Id = $"{method}:{url}";
            Method = method;
            Url = url;
            UrlParameters = urlParameters;
            WildcardParameters = wildcardParameters;
            QuerystringParameters = querystringParameters;
            RequestParameter = requestParameter;
            HasRequest = requestParameter != null;
            ResponseType = responseType;
            HasResponse = responseType != null;
        }

        public virtual string Id { get; }
        public virtual string Method { get; }
        public virtual string Url { get; }
        public virtual ParameterDescriptor[] UrlParameters { get; }
        public virtual ParameterDescriptor[] WildcardParameters { get; }
        public virtual ParameterDescriptor[] QuerystringParameters { get; }
        public virtual bool HasRequest { get; }
        public virtual bool HasResponse { get; }
        public virtual ParameterDescriptor RequestParameter { get; }
        public virtual TypeDescriptor ResponseType { get; }

        public override int GetHashCode()
        {
            return this.GetHashCode(Id);
        }

        public override bool Equals(object obj)
        {
            return GetHashCode() == obj?.GetHashCode();
        }

        public override string ToString()
        {
            return Id;
        }
    }
}