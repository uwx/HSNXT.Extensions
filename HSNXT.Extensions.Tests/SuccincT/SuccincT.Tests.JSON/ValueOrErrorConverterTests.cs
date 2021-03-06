﻿using Newtonsoft.Json;
using NUnit.Framework;
using HSNXT.SuccincT.JSON;
using HSNXT.SuccincT.Options;
using static Newtonsoft.Json.JsonConvert;
using static NUnit.Framework.Assert;

namespace HSNXT.SuccincTTests.SuccincT.JSON
{
    [TestFixture]
    public class ValueOrErrorConverterTests
    {
        [Test]
        public void ConvertingValueToJsonAndBack_PreservesOptionState()
        {
            var settings = new JsonSerializerSettings();
            settings.Converters.Add(new ValueOrErrorConverter());
            var value = ValueOrError.WithValue("a");
            var json = SerializeObject(value, settings);
            var newValue = DeserializeObject<ValueOrError>(json, settings);

            IsTrue(newValue.HasValue);
            AreEqual("a", newValue.Value);
        }

        [Test]
        public void ConvertingErrorToJsonAndBack_PreservesOptionState()
        {
            var settings = new JsonSerializerSettings();
            settings.Converters.Add(new ValueOrErrorConverter());
            var value = ValueOrError.WithError("b");
            var json = SerializeObject(value, settings);
            var newValue = DeserializeObject<ValueOrError>(json, settings);

            IsFalse(newValue.HasValue);
            AreEqual("b", newValue.Error);
        }

        [Test]
        public void ConvertingJsonToValueOrError_FailsCleanlyIfSuccinctConverterNotUsed()
        {
            Throws<JsonSerializationException>(() => DeserializeObject<ValueOrError>("{\"value\":\"a\"}"));
        }

        [Test]
        public void ConvertingInvalidJsonToValueOrError_FailsCleanlyIfSuccinctConverterUsed()
        {
            var settings = new JsonSerializerSettings();
            settings.Converters.Add(new ValueOrErrorConverter());
            Throws<JsonSerializationException>(() => DeserializeObject<ValueOrError>("{}", settings));
        }
    }
}