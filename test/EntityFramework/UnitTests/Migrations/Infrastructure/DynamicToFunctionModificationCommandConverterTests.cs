﻿// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

namespace System.Data.Entity.Migrations.Infrastructure
{
    using System.Data.Entity.Core.Metadata.Edm;
    using System.Data.Entity.Migrations.Infrastructure.FunctionsModel;
    using System.Data.SqlClient;
    using System.Linq;
    using Xunit;
    using Console = System.Console;

    public class DynamicToFunctionModificationCommandConverterTests
    {
        [Fact]
        public void Can_convert_insert_command_trees()
        {
            var model = TestContext.CreateDynamicUpdateModel();

            var modificationFunctionMapping
                = TestContext.GetModificationFunctionMapping("ExtraSpecialOrder");

            var converter
                = new DynamicToFunctionModificationCommandConverter(
                    modificationFunctionMapping.Item1, modificationFunctionMapping.Item2);

            var updateCommandTreeGenerator
                = new ModificationCommandTreeGenerator(model);

            var commandTrees
                = updateCommandTreeGenerator
                    .GenerateInsert(modificationFunctionMapping.Item1.EntityType.FullName);

            Assert.Equal(3, commandTrees.Count());

            var resultTrees = converter.Convert(commandTrees);

            Assert.Equal(3, resultTrees.Count());

            var firstCommandTree = resultTrees.First();

            Assert.Equal(8, firstCommandTree.Parameters.Count());
            Assert.Equal(8, firstCommandTree.SetClauses.Count());

            Assert.Equal("teh_codez", firstCommandTree.Parameters.ElementAt(0).Key);
            Assert.Equal("Signature", firstCommandTree.Parameters.ElementAt(1).Key);
            Assert.Equal("the_name", firstCommandTree.Parameters.ElementAt(2).Key);
            Assert.Equal("Address_Street", firstCommandTree.Parameters.ElementAt(3).Key);
            Assert.Equal("Address_City", firstCommandTree.Parameters.ElementAt(4).Key);
            Assert.Equal("Address_Country_Name", firstCommandTree.Parameters.ElementAt(5).Key);
            Assert.Equal("OrderGroupId", firstCommandTree.Parameters.ElementAt(6).Key);
            Assert.Equal("Customer_CustomerId", firstCommandTree.Parameters.ElementAt(7).Key);

            var properties = ((RowType)firstCommandTree.Returning.ResultType.EdmType).Properties;

            Assert.Equal(4, properties.Count);
            Assert.Equal("key_result", properties[1].Name);
        }

        [Fact]
        public void Can_convert_update_command_trees()
        {
            var model = TestContext.CreateDynamicUpdateModel();

            var modificationFunctionMapping
                = TestContext.GetModificationFunctionMapping("ExtraSpecialOrder");

            var converter
                = new DynamicToFunctionModificationCommandConverter(
                    modificationFunctionMapping.Item1, modificationFunctionMapping.Item2);

            var updateCommandTreeGenerator
                = new ModificationCommandTreeGenerator(model);

            var commandTrees
                = updateCommandTreeGenerator
                    .GenerateUpdate(modificationFunctionMapping.Item1.EntityType.FullName);

            Assert.Equal(3, commandTrees.Count());

            var resultTrees = converter.Convert(commandTrees);

            Assert.Equal(3, resultTrees.Count());

            var firstCommandTree = resultTrees.First();

            Assert.Equal(12, firstCommandTree.Parameters.Count());
            Assert.Equal(6, firstCommandTree.SetClauses.Count());

            Assert.Equal("Name", firstCommandTree.Parameters.ElementAt(0).Key);
            Assert.Equal("Address_Street", firstCommandTree.Parameters.ElementAt(1).Key);
            Assert.Equal("Address_City", firstCommandTree.Parameters.ElementAt(2).Key);
            Assert.Equal("Address_Country_Name", firstCommandTree.Parameters.ElementAt(3).Key);
            Assert.Equal("OrderGroupId", firstCommandTree.Parameters.ElementAt(4).Key);
            Assert.Equal("Customer_CustomerId", firstCommandTree.Parameters.ElementAt(5).Key);
            Assert.Equal("xid", firstCommandTree.Parameters.ElementAt(6).Key);
            Assert.Equal("key_for_update", firstCommandTree.Parameters.ElementAt(7).Key);
            Assert.Equal("Code", firstCommandTree.Parameters.ElementAt(8).Key);
            Assert.Equal("Signature", firstCommandTree.Parameters.ElementAt(9).Key);
            Assert.Equal("Name_Original", firstCommandTree.Parameters.ElementAt(10).Key);
            Assert.Equal("RowVersion_Original", firstCommandTree.Parameters.ElementAt(11).Key);

            var properties = ((RowType)firstCommandTree.Returning.ResultType.EdmType).Properties;

            Assert.Equal(2, properties.Count);
            Assert.Equal("order_fu", properties[0].Name);
        }

        [Fact]
        public void Can_convert_delete_command_trees()
        {
            var model = TestContext.CreateDynamicUpdateModel();

            var modificationFunctionMapping
                = TestContext.GetModificationFunctionMapping("ExtraSpecialOrder");

            var converter
                = new DynamicToFunctionModificationCommandConverter(
                    modificationFunctionMapping.Item1, modificationFunctionMapping.Item2);

            var updateCommandTreeGenerator
                = new ModificationCommandTreeGenerator(model);

            var commandTrees
                = updateCommandTreeGenerator
                    .GenerateDelete(modificationFunctionMapping.Item1.EntityType.FullName);

            Assert.Equal(3, commandTrees.Count());

            var resultTrees = converter.Convert(commandTrees);

            Assert.Equal(3, resultTrees.Count());

            var lastCommandTree = resultTrees.Last();

            Assert.Equal(7, lastCommandTree.Parameters.Count());

            Assert.Equal("xid", lastCommandTree.Parameters.ElementAt(0).Key);
            Assert.Equal("key_for_delete", lastCommandTree.Parameters.ElementAt(1).Key);
            Assert.Equal("Code", lastCommandTree.Parameters.ElementAt(2).Key);
            Assert.Equal("Signature", lastCommandTree.Parameters.ElementAt(3).Key);
            Assert.Equal("Name_Original", lastCommandTree.Parameters.ElementAt(4).Key);
            Assert.Equal("RowVersion_Original", lastCommandTree.Parameters.ElementAt(5).Key);
            Assert.Equal("Customer_CustomerId", lastCommandTree.Parameters.ElementAt(6).Key);
        }
    }
}