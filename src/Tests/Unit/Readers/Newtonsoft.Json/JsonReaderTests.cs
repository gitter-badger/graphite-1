﻿using System.Threading.Tasks;
using Bender.Collections;
using Graphite.Extensions;
using Graphite.Http;
using Graphite.Readers;
using Newtonsoft.Json;
using NUnit.Framework;
using Should;
using Tests.Common;

namespace Tests.Unit.Readers.Newtonsoft.Json
{
    [TestFixture]
    public class JsonReaderTests
    {
        public class InputModel
        {
            public string Value { get; set; }
        }

        public class Handler
        {
            public void Post(InputModel request, string param) { }
        }

        [Test]
        public void Should_only_apply_if_the_content_type_is_application_json(
            [Values(true, false)] bool isJson)
        {
            var requestGraph = RequestGraph
                .CreateFor<Handler>(h => h.Post(null, null))
                    .WithRequestData("{}")
                    .WithRequestParameter("request")
                    .AddQuerystringParameter("param");

            if (isJson)
            {
                requestGraph.WithContentType(MimeTypes.ApplicationJson);
            }

            new Graphite.Newtonsoft.Json.JsonReader(new JsonSerializerSettings())
                .AppliesTo(requestGraph.GetRequestReaderContext())
                .ShouldEqual(isJson);
        }

        [Test]
        public void Should_only_apply_if_the_action_has_a_request_parameter(
            [Values(true, false)] bool hasRequest)
        {
            var requestGraph = RequestGraph
                .CreateFor<Handler>(h => h.Post(null, null))
                    .WithRequestData("{}")
                    .WithContentType(MimeTypes.ApplicationJson)
                    .AddQuerystringParameter("param");

            if (hasRequest)
            {
                requestGraph.WithRequestParameter("request");
            }

            new Graphite.Newtonsoft.Json.JsonReader(new JsonSerializerSettings())
                .AppliesTo(requestGraph.GetRequestReaderContext())
                .ShouldEqual(hasRequest);
        }

        [Test]
        public async Task Should_read_json()
        {
            var requestGraph = RequestGraph
                .CreateFor<Handler>(h => h.Post(null, null))
                    .WithRequestData("{\"Value\":\"fark\"}")
                    .WithRequestParameter("request")
                    .WithContentType(MimeTypes.ApplicationJson)
                    .AddQuerystringParameter("param");

            var result = await new Graphite.Newtonsoft.Json
                .JsonReader(new JsonSerializerSettings())
                .Read(requestGraph.GetRequestBinderContext());

            result.ShouldNotBeNull();
            result.ShouldBeType<InputModel>();
            result.CastTo<InputModel>().Value.ShouldEqual("fark");
        }
    }
}
